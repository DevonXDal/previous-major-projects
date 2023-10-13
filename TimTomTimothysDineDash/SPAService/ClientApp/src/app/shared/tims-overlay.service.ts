import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root',
  
})
export class TimsOverlayService {

  constructor(private messageService: MessageService) { }

  addSingleToast(title: string, message: string, severity: 'success' | 'info' | 'warn' | 'error'): void {
    this.messageService.add({key: 'main', severity: severity, summary: title, detail: message});
  }
}
