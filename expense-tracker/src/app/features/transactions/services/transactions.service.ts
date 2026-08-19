import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { PaginatedResult } from '../../../core/models/paginated-result.model';
import {
  CreateTransaction,
  Transaction,
  UpdateTransaction,
} from '../../../core/models/transaction.module';
import { TransactionType } from '../../../core/models/category.model';

@Injectable({
  providedIn: 'root',
})
export class TransactionsService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Transactions`;

  getAll(
    pageNumber = 1,
    pageSize = 10,
    search?: string,
    type?: TransactionType,
    categoryId?: number,
  ): Observable<PaginatedResult<Transaction>> {
    return this.http.get<PaginatedResult<Transaction>>(this.apiUrl, {
      params: {
        PageNumber: pageNumber,
        PageSize: pageSize,
        Search: search ?? '',
        Type: type ?? '',
        CategoryId: categoryId ?? '',
      },
    });
  }

  getById(id: number): Observable<Transaction> {
    return this.http.get<Transaction>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateTransaction): Observable<number> {
    return this.http.post<number>(this.apiUrl, request);
  }

  update(request: UpdateTransaction): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${request.id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
