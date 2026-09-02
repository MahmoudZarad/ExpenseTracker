import { Component, inject, OnInit, signal } from '@angular/core';
import { BalanceOverviewComponent } from '../../components/balance-overview/balance-overview.component';
import { BudgetOverviewComponent } from '../../components/budget-overview/budget-overview.component';
import { RecentTransactionsComponent } from '../../components/recent-transactions/recent-transactions.component';
import { SpendingOverviewComponent } from '../../components/spending-overview/spending-overview.component';
import { DashboardService } from '../../Services/dashboard.service';
import { Dashboard } from '../../../../core/models/Dashboard.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    BalanceOverviewComponent,
    BudgetOverviewComponent,
    RecentTransactionsComponent,
    SpendingOverviewComponent,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  private readonly dashboardService = inject(DashboardService);

  dashboard = signal<Dashboard | null>(null);

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.dashboardService.getDashboard().subscribe({
      next: (response) => {
        this.dashboard.set(response);
      },

      error: (error) => {
        console.error(error);
      },
    });
  }
}
