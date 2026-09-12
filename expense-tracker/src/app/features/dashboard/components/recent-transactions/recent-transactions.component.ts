import { Component, inject, Input } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { TransactionsService } from '../../../transactions/services/transactions.service';
import { RouterLink } from '@angular/router';
import { Transaction } from '../../../../core/models/transaction.module';
import { CurrencyService } from '../../../../core/services/CurrencyService.service';

@Component({
  selector: 'app-recent-transactions',
  standalone: true,
  imports: [DecimalPipe, RouterLink, DatePipe],
  templateUrl: './recent-transactions.component.html',
  styleUrl: './recent-transactions.component.css',
})
export class RecentTransactionsComponent {
  protected readonly currencyService = inject(CurrencyService);
  @Input()
  data: Transaction[] = [];
}
