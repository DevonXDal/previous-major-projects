import { Component, OnDestroy, OnInit } from '@angular/core';
import { HostStaffApiService } from '../host-staff-api.service';
import { HostStaffStatus } from '../data/host-staff-status';
import { NgForm } from '@angular/forms';
import { MessageService, OverlayService } from 'primeng/api';
import { TimsOverlayService } from 'src/app/shared/tims-overlay.service';

@Component({
  selector: 'app-add-to-wait-queue',
  templateUrl: './add-to-wait-queue.component.html',
  styleUrls: ['./add-to-wait-queue.component.scss'],
})
export class AddToWaitQueueComponent implements OnInit {
  isLoading = false;
  numOfCustomers: number = 1;

  constructor(private apiService: HostStaffApiService, private overlayService: TimsOverlayService) { }

  ngOnInit(): void {
  }


  async submit(f: NgForm): Promise<void> {
    this.isLoading = true;

    if (this.numOfCustomers < 1 || this.numOfCustomers > 6) return;

    let result = await this.apiService.insertPartyIntoQueue(this.numOfCustomers);

    if (result) { // It failed
      this.overlayService.addSingleToast('Failed', result, 'error');
    } else { // No message in result is indicative of success
      this.overlayService.addSingleToast('Success', 'The customer group has been added to the wait queue', 'success');
    }
    

    this.isLoading = false;
  }

  
}
