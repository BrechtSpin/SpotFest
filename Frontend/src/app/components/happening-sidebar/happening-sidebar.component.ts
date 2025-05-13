import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HappeningService } from '@services/happening.service';
import { HappeningSummary } from '@models/happening-summary';

@Component({
  selector: 'app-happening-sidebar',
  standalone: true,
  imports: [CommonModule,  RouterModule],
  templateUrl: './happening-sidebar.component.html',
})
export class HappeningSidebarComponent {
  currentHappenings = signal<HappeningSummary[]>([]);
  futureHappenings = signal<HappeningSummary[]>([]);
  loading = signal(true);

  constructor(private happeningService: HappeningService) {
  this.getHappenings()}

  private getHappenings() {
    this.happeningService.getCurrentAndUpcomingHappenings().subscribe((happenings) => {
      const now = new Date().toISOString().split('T')[0];
      this.currentHappenings.set(happenings.filter(e => e.startDate <= now));
      this.futureHappenings.set(happenings.filter(e => e.startDate > now));
      this.loading.set(false);
    });
  }
}
