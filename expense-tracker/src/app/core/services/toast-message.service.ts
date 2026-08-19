import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ToastMessageService {
  message = signal('');

  showMessage(message: string): void {
    this.message.set(message);

    setTimeout(() => {
      this.message.set('');
    }, 4000);
  }

  constructor() {}
}
