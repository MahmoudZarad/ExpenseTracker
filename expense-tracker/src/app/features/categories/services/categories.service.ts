import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Category,
  CreateCategory,
  UpdateCategory,
} from '../../../core/models/category.model';
import { environment } from '../../../../environments/environment';
import { PaginatedResult } from '../../../core/models/paginated-result.model';

@Injectable({
  providedIn: 'root',
})
export class CategoriesService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Categories`;

  getAll(
    pageNumber = 1,
    pageSize = 100,
  ): Observable<PaginatedResult<Category>> {
    return this.http.get<PaginatedResult<Category>>(this.apiUrl, {
      params: {
        PageNumber: pageNumber,
        PageSize: pageSize,
      },
    });
  }

  getById(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateCategory): Observable<number> {
    return this.http.post<number>(this.apiUrl, request);
  }

  update(request: UpdateCategory): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${request.id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
