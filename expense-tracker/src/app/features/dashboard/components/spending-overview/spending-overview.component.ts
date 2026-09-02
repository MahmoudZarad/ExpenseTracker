import { Component, computed, Input } from '@angular/core';
import { EChartsOption } from 'echarts';
import { NgxEchartsDirective } from 'ngx-echarts';
import { TransactionsService } from '../../../transactions/services/transactions.service';
import { SpendingSummary } from '../../../../core/models/spending-summary.model';

@Component({
  selector: 'app-spending-overview',
  standalone: true,
  imports: [NgxEchartsDirective],
  templateUrl: './spending-overview.component.html',
  styleUrl: './spending-overview.component.css',
})
export class SpendingOverviewComponent {
  @Input()
  data: SpendingSummary[] = [];

  get chartOptions(): EChartsOption {
    return {
      tooltip: {
        trigger: 'axis',

        valueFormatter: (value) => `EGP ${value}`,
      },

      grid: {
        left: 10,
        right: 10,
        top: 20,
        bottom: 20,
        containLabel: true,
      },

      xAxis: {
        type: 'category',

        data: this.data.map((item) => item.label ?? 'Unknown'),

        axisLabel: {
          color: '#94a3b8',
        },

        axisLine: {
          lineStyle: {
            color: '#334155',
          },
        },
      },

      yAxis: {
        type: 'value',

        axisLabel: {
          color: '#94a3b8',
        },

        splitLine: {
          lineStyle: {
            color: '#1e293b',
          },
        },
      },

      series: [
        {
          type: 'line',

          data: this.data.map((item) => item.amount),

          smooth: true,

          areaStyle: {},

          symbol: 'circle',

          symbolSize: 7,
        },
      ],
    };
  }
}
