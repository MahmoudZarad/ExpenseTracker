import { Component, inject, Input } from '@angular/core';
import { TransactionsService } from '../../../transactions/services/transactions.service';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { Dashboard } from '../../../../core/models/Dashboard.model';
import { number } from 'echarts';
import { CurrencyService } from '../../../../core/services/CurrencyService.service';

@Component({
  selector: 'app-balance-overview',
  standalone: true,
  imports: [DecimalPipe],
  templateUrl: './balance-overview.component.html',
  styleUrl: './balance-overview.component.css',
})
export class BalanceOverviewComponent {
  protected readonly currencyService = inject(CurrencyService);

  @Input()
  data: Dashboard | null = null;

  get balanceChangePercentage(): number {
    if (!this.data) {
      return 0;
    }
    return this.data.balanceChangePercentage;
  }
}
