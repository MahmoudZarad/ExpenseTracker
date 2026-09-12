export interface UserProfile {
  id: number;
  name: string;
  email: string;
  currency: string;
  language: string;
}

export interface UpdateUserSettingsRequest {
  name: string;
  email: string;
  currency: string;
  language: string;
}

export interface UserProfileResponse {
  isSuccess: boolean;
  statusCode: number;
  value: UserProfile | null;
  error?: string;
}
