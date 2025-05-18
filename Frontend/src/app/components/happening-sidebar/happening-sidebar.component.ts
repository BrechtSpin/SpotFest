import { Component, inject, resource, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { happeningHub } from '@services/happeningHub.service';
import { HappeningSummary } from '@models/happening-summary';
import { NotFound404Component } from '@components/404/404-not-found';

@Component({
  selector: 'app-happening-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule, NotFound404Component ],
  templateUrl: './happening-sidebar.component.html',
})
export class HappeningSidebarComponent implements OnInit {
  private hub = inject(happeningHub);

  happeningResource = resource<HappeningSummary[], void>({    
    loader: async () =>
    {
      await this.hub.startConnection();
      return await this.hub.invoke<HappeningSummary[]>('LoadData');
    },
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


  async ngOnInit() {

    await this.hub.startConnection();
    this.hub.on<HappeningSummary[]>('HappeningsUpdated', () => {
      this.happeningResource.reload();
    });
    this.happeningResource.reload();
  }
}
