import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { KitchenStaffStatus } from 'src/app/kitchen/data/kitchen-staff-status';
import { KitchenStaffApiService } from 'src/app/kitchen/kitchen-staff-api.service';
import { TimsOverlayService } from 'src/app/shared/tims-overlay.service';
import { WaitStaffStatus } from 'src/app/wait/data/wait-staff-status';
import { WaitStaffApiService } from 'src/app/wait/wait-staff-api.service';
import { BusStaffStatus } from '../data/bus-staff-status';
import { BusStaffApiService } from '../bus-staff-api.service';

/**
 * This MarkTableAsCleanedComponent represents the form used by bus staff to indicate that a table has been cleaned
 * and is ready for reuse.
 */
@Component({
  selector: 'app-mark-table-as-cleaned',
  templateUrl: './mark-table-as-cleaned.component.html',
  styleUrls: ['./mark-table-as-cleaned.component.scss']
})
export class MarkTableAsCleanedComponent implements OnInit, OnDestroy {

  tableId = 1;
  isLoading = false;
  emptyUncleanTables = false;
  refreshInterval?: NodeJS.Timer;
  currStatus?: BusStaffStatus;


  constructor(private apiService: BusStaffApiService, private overlayService: TimsOverlayService) { }

  ngOnInit(): void {
    this.refreshInterval = setInterval(() => {
      this.currStatus = this.apiService.currStatus;
      this.checkForEmptyUncleanTables();
    }, 1000); // 1 seconds, heavy refresh rate

    this.currStatus = this.apiService.currStatus;
  }

  ngOnDestroy(): void {
      clearInterval(this.refreshInterval);
  }

  /**
   * Submits the form if the state is correct and sends the table id off to the API to mark
   * the table as clean
   * @param f The submitting form
   */
  async submit(f: NgForm): Promise<void> {
    this.isLoading = true;

    let result = await this.apiService.markTableAsCleanAndReady(this.tableId);

    if (result) { // It failed
      this.overlayService.addSingleToast('Failed', result, 'error');
    } else { // No message in result is indicative of success
      this.overlayService.addSingleToast('Success', `Marked table ${this.tableId} as having been clean and readied for customers`, 'success');
    }

    this.isLoading = false;
  }

  checkForEmptyUncleanTables(): void {
      this.emptyUncleanTables = !!(this.currStatus && this.currStatus.Tables.filter(x => x.hasPaid && x.isNeedingCleaned).length !== 0);
  }


}
