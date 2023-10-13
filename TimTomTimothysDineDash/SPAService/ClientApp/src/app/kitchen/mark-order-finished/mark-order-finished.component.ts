import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { TimsOverlayService } from 'src/app/shared/tims-overlay.service';
import { KitchenStaffStatus } from '../data/kitchen-staff-status';
import { KitchenStaffApiService } from '../kitchen-staff-api.service';

@Component({
  selector: 'app-mark-order-finished',
  templateUrl: './mark-order-finished.component.html',
  styleUrls: ['./mark-order-finished.component.scss']
})
export class MarkOrderFinishedComponent implements OnInit, OnDestroy {

  tableId = 1;
  orderGuid = '';
  isLoading = false;
  tablesWithInProgressOrders = false;
  refreshInterval?: NodeJS.Timer;
  currStatus?: KitchenStaffStatus;


  constructor(private apiService: KitchenStaffApiService, private overlayService: TimsOverlayService) { }

  ngOnInit(): void {
    this.refreshInterval = setInterval(() => {
      this.currStatus = this.apiService.currStatus;
      this.checkForTablesWithInProgressOrders();
    }, 1000); // 1 seconds, heavy refresh rate

    this.currStatus = this.apiService.currStatus;
  }

  ngOnDestroy(): void {
      clearInterval(this.refreshInterval);
  }

  async submit(f: NgForm): Promise<void> {
    this.isLoading = true;

    if (this.currStatus?.Tables.find(x => x.orderGuid == this.orderGuid)?.isOrderStarted != true) {
      this.overlayService.addSingleToast('Not Allowed', 'The order must be started before declaring it finished', 'warn');
      this.isLoading = false;
      return;
    }
    
    let result = await this.apiService.markOrderAsFinished(this.orderGuid);

    if (result) { // It failed
      this.overlayService.addSingleToast('Failed', result, 'error');
    } else { // No message in result is indicative of success
      this.overlayService.addSingleToast('Success', `Table ${this.tableId}'s order is ready to be served`, 'success');
    }

    this.isLoading = false;
  }

  checkForTablesWithInProgressOrders(): void {
      this.tablesWithInProgressOrders = !!(this.currStatus && this.currStatus.Tables.filter(x => x.orderGuid && x.isOrderStarted && !x.isOrderFinished).length !== 0);
  }

}
