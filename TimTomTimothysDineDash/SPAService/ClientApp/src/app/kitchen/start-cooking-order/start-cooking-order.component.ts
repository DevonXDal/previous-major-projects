import { Component, OnDestroy, OnInit } from '@angular/core';
import { KitchenStaffStatus } from '../data/kitchen-staff-status';
import { KitchenStaffApiService } from '../kitchen-staff-api.service';
import { TimsOverlayService } from 'src/app/shared/tims-overlay.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-start-cooking-order',
  templateUrl: './start-cooking-order.component.html',
  styleUrls: ['./start-cooking-order.component.scss']
})
export class StartCookingOrderComponent implements OnInit, OnDestroy {

  tableId = 1;
  orderGuid = '';
  isLoading = false;
  tablesWithUnstartedOrders = false;
  refreshInterval?: NodeJS.Timer;
  currStatus?: KitchenStaffStatus;


  constructor(private apiService: KitchenStaffApiService, private overlayService: TimsOverlayService) { }

  ngOnInit(): void {
    this.refreshInterval = setInterval(() => {
      this.currStatus = this.apiService.currStatus;
      this.checkForTablesWithUnstartedOrders();
    }, 1000); // 1 seconds, heavy refresh rate

    this.currStatus = this.apiService.currStatus;
  }

  ngOnDestroy(): void {
      clearInterval(this.refreshInterval);
  }

  async submit(f: NgForm): Promise<void> {
    this.isLoading = true;

    let result = await this.apiService.markOrderAsStarted(this.orderGuid);

    if (result) { // It failed
      this.overlayService.addSingleToast('Failed', result, 'error');
    } else { // No message in result is indicative of success
      this.overlayService.addSingleToast('Success', `Seated a group of customers at table ${this.tableId}`, 'success');
    }

    this.isLoading = false;
  }

  checkForTablesWithUnstartedOrders(): void {
      this.tablesWithUnstartedOrders = !!(this.currStatus && this.currStatus.Tables.filter(x => x.orderGuid && !x.isOrderStarted).length !== 0);
  }

}
