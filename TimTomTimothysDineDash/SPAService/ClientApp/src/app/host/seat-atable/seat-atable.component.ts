import { Component, OnDestroy, OnInit } from '@angular/core';
import { HostStaffStatus } from '../data/host-staff-status';
import { NgForm } from '@angular/forms';
import { HostStaffApiService } from '../host-staff-api.service';
import { TimsOverlayService } from 'src/app/shared/tims-overlay.service';

@Component({
  selector: 'app-seat-atable',
  templateUrl: './seat-atable.component.html',
  styleUrls: ['./seat-atable.component.scss']
})
export class SeatATableComponent implements OnInit, OnDestroy {
  tableId = 1;
  isLoading = false;
  availableTables = false;
  refreshInterval?: NodeJS.Timer;
  currStatus?: HostStaffStatus;


  constructor(private apiService: HostStaffApiService, private overlayService: TimsOverlayService) { }

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

  async submit(f: NgForm): Promise<void> {
    this.isLoading = true;

    let result = await this.apiService.seatAtTable(this.tableId);

    if (result) { // It failed
      this.overlayService.addSingleToast('Failed', result, 'error');
    } else { // No message in result is indicative of success
      this.overlayService.addSingleToast('Success', `Seated a group of customers at table ${this.tableId}`, 'success');
    }

    this.isLoading = false;
  }

  checkTablesForAvailability(): void {
      this.availableTables = !!(this.currStatus && this.currStatus.Tables.filter(x => x.isAvailable).length !== 0);
  }

}
