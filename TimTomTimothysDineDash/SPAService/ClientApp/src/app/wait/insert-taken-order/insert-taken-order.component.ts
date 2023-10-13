import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { WaitStaffStatus } from '../data/wait-staff-status';
import { TimsOverlayService } from 'src/app/shared/tims-overlay.service';
import { WaitStaffApiService } from '../wait-staff-api.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-insert-taken-order',
  templateUrl: './insert-taken-order.component.html',
  styleUrls: ['./insert-taken-order.component.scss']
})
export class InsertTakenOrderComponent implements OnInit, OnDestroy {
  tableId = 1;
  orderDescription = '';
  isLoading = false;
  availableTables = false;
  refreshInterval?: NodeJS.Timer;
  currStatus?: WaitStaffStatus;
  @ViewChild('insertTakenOrder') private ngFormRef!: NgForm;

  constructor(private apiService: WaitStaffApiService, private overlayService: TimsOverlayService) { }

  ngOnInit(): void {
    this.refreshInterval = setInterval(() => {
      this.currStatus = this.apiService.currStatus;
      this.checkTablesForAvailability();
    }, 1000); // 1 seconds, heavy refresh rate

    this.currStatus = this.apiService.currStatus;
  }

  ngOnDestroy(): void {
      clearInterval(this.refreshInterval);
  }

  isFormDirty(): boolean {
    return this.ngFormRef.dirty ?? false;
  }

  async submit(f: NgForm): Promise<void> {
    this.isLoading = true;

    let result = await this.apiService.insertTakenOrder(this.tableId, this.orderDescription);

    if (result) { // It failed
      this.overlayService.addSingleToast('Failed', result, 'error');
    } else { // No message in result is indicative of success
      this.overlayService.addSingleToast('Success', `Order taken for table ${this.tableId}`, 'success');
    }

    this.isLoading = false;
  }

  checkTablesForAvailability(): void {
      this.availableTables = !!(this.currStatus && this.currStatus.Tables.filter(x => x.seatsUsed > 0 && x.orderGuid?.length === 0));
  }

}
