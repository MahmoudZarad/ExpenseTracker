import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Budget,
  BudgetSummary,
  CreateBudget,
  UpdateBudget,
} from '../../../core/models/budget.model';
import { environment } from '../../../../environments/environment';
import { PaginatedResult } from '../../../core/models/paginated-result.model';

@Injectable({
  providedIn: 'root',
})
export class BudgetsService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Budgets`;

  getAll(pageNumber = 1, pageSize = 100): Observable<PaginatedResult<Budget>> {
    return this.http.get<PaginatedResult<Budget>>(this.apiUrl, {
      params: {
        PageNumber: pageNumber,
        PageSize: pageSize,
      },
    });
  }

  getById(id: number): Observable<Budget> {
    return this.http.get<Budget>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateBudget): Observable<number> {
    return this.http.post<number>(this.apiUrl, request);
  }

  update(request: UpdateBudget): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${request.id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getSummary(): Observable<BudgetSummary[]> {
    return this.http.get<BudgetSummary[]>(`${this.apiUrl}/summary`);
  }
}
