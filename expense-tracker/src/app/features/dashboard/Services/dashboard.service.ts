import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Dashboard } from '../../../core/models/Dashboard.model';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Dashboard`;

  getDashboard() {
    return this.http.get<Dashboard>(this.apiUrl);
  }
}
