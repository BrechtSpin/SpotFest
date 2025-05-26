import { Component, computed, effect, inject, resource, signal } from '@angular/core';
import { HappeningService } from '@services/happening.service';
import { HappeningSummary } from '@models/happening-summary';
import { lastValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

export interface SearchParameters {
  Year: number;
  Month: number;  // 1 = January, 12 = December
  Index: number;
}

@Component({
  selector: 'app-happening-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './happening-list.component.html',
//  styleUrls: ['./happening-list.component.css']
})
export class HappeningListComponent {
  private happeningService = inject(HappeningService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  dateInput = signal<string>(this.getInitialMonthYear());
  index = signal<number>(0);
  readonly itemsPerPage = 10;

  constructor() {
    // getting params in url on page load
    this.route.queryParamMap.subscribe(map => {
      const dateParam = map.get('date');
      const indexParam = map.get('index');
      this.dateInput.set(dateParam ?? this.getInitialMonthYear());
      this.index.set(indexParam !== null && !isNaN(Number(indexParam))
        ? Number(indexParam) : 0
      );
    });
    //setting params in url on change
    effect(() => {
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: {
          date: this.dateInput(),
          index: this.index(),
        }
      });
    });
  }

  private getInitialMonthYear(): string {
    const d = new Date();
    return `${d.getFullYear()}-${(d.getMonth() + 1).toString().padStart(2, '0')}`;
  }

  selectMonthDate(value: string) {
    this.dateInput.set(value);
    this.index.set(0);
  }

  happeningsResource = resource<HappeningSummary[], SearchParameters>({
    request: () => {
      const [year, month] = this.dateInput().split('-').map(Number);
      return { Year: year, Month: month, Index: this.index() };
    },
    loader: async ({ request }) => {
      if (request.Month < 1 || request.Month > 12 || request.Index < 0) {
        return [];
      }
      return await lastValueFrom(
        this.happeningService.getHappeningsByYearMonth(
          request.Year,
          request.Month,
          request.Index
        )
      );
    },
    defaultValue: [] as HappeningSummary[],
  });

  happenings = computed(() => this.happeningsResource.value());
  isNextPage = computed(() => this.happeningsResource.value().length === this.itemsPerPage);

  nextPage() {
    this.index.set(this.index() + this.itemsPerPage);
  }

  prevPage() {
    let id = this.index() - this.itemsPerPage;
    if (id < 0) id = 0;
    this.index.set(id);
  }
}
