import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  ViewChild,
  effect,
  input,
} from '@angular/core';
import { Chart, registerables } from 'chart.js/auto';
import annotationPlugin, { AnnotationOptions } from 'chartjs-plugin-annotation';
import 'chartjs-adapter-date-fns';
import { HappeningSummary } from '@models/happening-summary';

Chart.register(...registerables, annotationPlugin);


export interface DataPoint {
  date: string; //ISO 8601 string
  value: number;
}

@Component({
  selector: 'app-timeline-graph',
  standalone: true,
  imports: [],
  templateUrl: './graph.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TimelineGraphComponent implements AfterViewInit {
  @ViewChild('canvas', { static: true })
  canvas!: ElementRef<HTMLCanvasElement>;

  label = input<string>('');
  metrics = input<DataPoint[]>([]);
  happenings = input<HappeningSummary[]>([]);

  private chart!: Chart<'line', { x: Date; y: number }[], string>;

  constructor() {
    effect(() => {
      if (!this.chart) return;

      this.draw();
    });
  }

  ngAfterViewInit() {
    const ctx = this.canvas.nativeElement.getContext('2d')!;

    this.chart = new Chart(ctx, {
      type: 'line',
      data: {
        datasets: [
          {
            label: this.label(),
            data: [],
            fill: false,
            borderWidth: 2,
            tension: 0.5,
            pointRadius: 0.4,        
          },
        ],
      },
      options: {
        responsive: true,
        animation: false,
        scales: {
          x: {
            type: 'time',
            time: {
              parser: 'iso',      // parse ISO strings
              unit: 'day',        // bucket & tick by day
              displayFormats: {
                day: 'd MMM' 
              },
            },
          },
          y: {
            title: { display: true, text: this.label() },
          },
        },
        plugins: {
          legend: {
            display: false,
          },
          annotation: {
            common: { drawTime: 'afterDatasetsDraw' },
            annotations: this.happeningAnnotations(),
          }
        }
      },
    });
    this.draw();
  }

  private happeningAnnotations(): Record<string, AnnotationOptions> {
    return this.happenings().reduce((acc, h, i) => {
      acc[i] = {
        type: 'line',
        scaleID: 'x',
        value: new Date(h.startDate).getTime(),
        borderColor: 'rgba(200,0,0,0.5)',
        borderWidth: 2,
        adjustScaleRange: false,
        hitTolerance: 12,           //sligthly wider 'hoverarea'
        label: {
          display: false,
          content: h.name,     
          position: 'start',
          backgroundColor: 'rgba(200,0,0,0.8)',
          color: '#fff',
          font: { size: 10 },
          drawTime: 'afterDatasetsDraw',
        },
        enter({element }) {
          element.label!.options.display = true;
          return true;
        },
        leave({ element }) {
          element.label!.options.display = false;
          return true;
        },
      };
      return acc;
    }, {} as Record<string, AnnotationOptions>);
  }

  draw() {
    const pts = this.metrics().map(p => ({
      x: new Date(p.date),
      y: p.value,
    }));

    this.chart.data.datasets[0].data = pts;
    this.chart.update();
  }
}
