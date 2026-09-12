import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CurrencyService {
  readonly currency = signal<string>(
    localStorage.getItem('selected_currency') || 'EGP',
  );

  setCurrency(currency: string): void {
    this.currency.set(currency);
    localStorage.setItem('selected_currency', currency);
  }

  getCurrency(): string {
    return this.currency();
  }
}
