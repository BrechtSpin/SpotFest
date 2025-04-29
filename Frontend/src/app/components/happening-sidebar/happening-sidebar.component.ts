import { Component, signal } from '@angular/core';
import { CommonModule, NgForOf } from '@angular/common';
//import { CommonModule, NgIf, ForDirective } from '@angular/common';
import { HappeningService } from '@services/happening.service';
import { HappeningSummary } from '@models/happening-summary';
import { trigger, transition, style, animate, query, stagger } from '@angular/animations';

@Component({
  selector: 'app-happening-sidebar',
  standalone: true,
  imports: [CommonModule, NgForOf],//, NgIf, ForDirective],
  templateUrl: './happening-sidebar.component.html',
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        query('a', [
          style({ opacity: 0, transform: 'translateY(10px)' }),
          stagger(100, [
            animate('400ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
          ])
        ], { optional: true })
      ])
    ])
  ]
})
export class HappeningSidebarComponent {
  currentHappenings = signal<HappeningSummary[]>([]);
  futureHappenings = signal<HappeningSummary[]>([]);
  loading = signal(true);

  constructor(private happeningService: HappeningService) { }

  private getEvents() {
    this.happeningService.getCurrentAndUpcomingHappenings().subscribe(happenings => {
      const now = new Date();
      this.currentHappenings.set(happenings.filter(e => new Date(e.startDate) <= now));
      this.futureHappenings.set(happenings.filter(e => new Date(e.startDate) > now));
      this.loading.set(false);
    });
  }
}
