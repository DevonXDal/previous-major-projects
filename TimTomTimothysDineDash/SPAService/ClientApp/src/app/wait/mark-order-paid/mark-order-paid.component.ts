import { Component, OnDestroy, OnInit } from '@angular/core';
import { WaitStaffApiService } from '../wait-staff-api.service';
import { TimsOverlayService } from 'src/app/shared/tims-overlay.service';
import { WaitStaffStatus } from '../data/wait-staff-status';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-mark-order-paid',
  templateUrl: './mark-order-paid.component.html',
  styleUrls: ['./mark-order-paid.component.scss']
})
export class MarkOrderPaidComponent implements OnInit, OnDestroy {
  tableId = 1;
  isLoading = false;
  tablesWithFood = false;
  refreshInterval?: NodeJS.Timer;
  currStatus?: WaitStaffStatus;


  constructor(private apiService: WaitStaffApiService, private overlayService: TimsOverlayService) { }

  ngOnInit(): void {
    this.refreshInterval = setInterval(() => {
      this.currStatus = this.apiService.currStatus;
      this.checkForTablesThatHaveReceivedFood();
    }, 1000); // 1 seconds, heavy refresh rate

    this.currStatus = this.apiService.currStatus;
  }

  ngOnDestroy(): void {
      clearInterval(this.refreshInterval);
  }

  async submit(f: NgForm): Promise<void> {
    this.isLoading = true;

    let result = await this.apiService.markTableAsHavingPaid(this.tableId);

    if (result) { // It failed
      this.overlayService.addSingleToast('Failed', result, 'error');
    } else { // No message in result is indicative of success
      this.overlayService.addSingleToast('Success', `Seated a group of customers at table ${this.tableId}`, 'success');
    }

    this.isLoading = false;
  }

  checkForTablesThatHaveReceivedFood(): void {
      this.tablesWithFood = !!(this.currStatus && this.currStatus.Tables.filter(x => x.isOrderFinished).length !== 0);
  }

}
