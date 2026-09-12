import { BudgetSummary } from './../../../../core/models/budget.model';
import { Component, inject, Input, input } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BudgetsService } from '../../../budgets/services/budgets.service';
import { CurrencyService } from '../../../../core/services/CurrencyService.service';

@Component({
  selector: 'app-budget-overview',
  standalone: true,
  imports: [DecimalPipe, RouterLink],
  templateUrl: './budget-overview.component.html',
  styleUrl: './budget-overview.component.css',
})
export class BudgetOverviewComponent {
  protected readonly currencyService = inject(CurrencyService);
  @Input()
  data: BudgetSummary[] = [];
}
