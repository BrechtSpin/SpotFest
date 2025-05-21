import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  ViewChild,
  effect,
  input,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js/auto';
import 'chartjs-adapter-date-fns';

Chart.register(...registerables);


export interface DataPoint {
  date: string; //ISO 8601 string
  value: number;
}

@Component({
  selector: 'app-timeline-graph',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './graph.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TimelineGraphComponent implements AfterViewInit {
  @ViewChild('canvas', { static: true })
  canvas!: ElementRef<HTMLCanvasElement>;

  label = input<string>('');
  data = input<DataPoint[]>([]);

  private chart!: Chart<'line', { x: number; y: number }[], string>;

  constructor() {
    effect(() => {
      if (!this.chart) return;

      this.draw();
    });
  }

  ngAfterViewInit() {
    const ctx = this.canvas.nativeElement.getContext('2d')!;

    this.chart = new Chart<'line', { x: number; y: number }[], string>(ctx, {
      type: 'line',
      data: {
        datasets: [
          {
            label: this.label(),
            data: [] as { x: number; y: number }[],
            fill: false,
            borderWidth: 2,
            tension: 0.3,
          },
        ],
      },
      options: {
        scales: {
          x: {
            type: 'time',
            time: {
              parser: 'iso',      // parse ISO strings
              unit: 'day',        // bucket & tick by day
              displayFormats: {
                day: 'd MMM' // e.g. "May 8, 2025"
              },
            },
          },
          y: {
            title: { display: true, text: this.label() },
          },
        },
        responsive: true,
        animation: false,
      },
    });
    this.draw();
  }

  draw() {
    const pts = this.data().map(p => ({
      x: new Date(p.date).getTime(),
      y: p.value,
    }));

    this.chart.data.datasets[0].data = pts;
    this.chart.update('none');
  }

}
