import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import {
  UpdateUserSettingsRequest,
  UserProfileResponse,
} from '../../../core/models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/Users`;

  getMe(): Observable<UserProfileResponse> {
    return this.http.get<UserProfileResponse>(`${this.apiUrl}/me`);
  }

  updateMe(
    request: UpdateUserSettingsRequest,
  ): Observable<UserProfileResponse> {
    return this.http.put<UserProfileResponse>(`${this.apiUrl}/me`, request);
  }
}
export { UpdateUserSettingsRequest };
