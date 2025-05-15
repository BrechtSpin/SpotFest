import { Component, inject, resource, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { lastValueFrom } from 'rxjs';
import { RouterModule } from '@angular/router';
import { HappeningService } from '@services/happening.service';
import { HappeningSummary } from '@models/happening-summary';

@Component({
  selector: 'app-happening-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './happening-sidebar.component.html',
})
export class HappeningSidebarComponent{
  private happeningService = inject(HappeningService) ;

  happeningResource = resource<HappeningSummary[], void >({
    loader: async () =>
      await lastValueFrom(this.happeningService.getCurrentAndUpcomingHappenings()),
    defaultValue: [] as HappeningSummary[],
  });

  currentHappenings = computed(() =>
    this.happeningResource.value().filter(h => new Date(h.startDate) <= new Date())
  );
  futureHappenings = computed(() =>
    this.happeningResource.value().filter(h => new Date(h.startDate) > new Date())
  );

  loading = computed(() => this.happeningResource.isLoading());
  error = computed(() => this.happeningResource.error());

}
