import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CurrencyService {
  readonly currency = signal('EGP');

  setCurrency(currency: string): void {
    this.currency.set(currency);
  }

  getCurrency(): string {
    return this.currency();
  }
}
