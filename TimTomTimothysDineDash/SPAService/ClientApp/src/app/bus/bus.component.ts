import { Component, OnDestroy, OnInit } from '@angular/core';
import { BusStaffStatus } from './data/bus-staff-status';
import { BusStaffApiService } from './bus-staff-api.service';

@Component({
  selector: 'app-bus',
  templateUrl: './bus.component.html',
  styleUrls: ['./bus.component.scss']
})
export class BusComponent implements OnInit, OnDestroy {

  status?: BusStaffStatus; // This will not be null at any point after the first time it gets data.
  showData = true;
  private interval?: NodeJS.Timer;
  pageTitle = 'Bus Staff';

  constructor(private apiService: BusStaffApiService) { }

  ngOnInit(): void {
    this.interval = setInterval(() => {
      this.updatePage();
    }, 15000); // 15 seconds

    this.updatePage();
  }

  ngOnDestroy(): void {
    
      clearInterval(this.interval);
  }

  toggleData(): void {
    this.showData = !this.showData;
  }

  private updatePage() {
    this.apiService.fetchStatus().subscribe(
      data => {
        this.status = data;
      },
      error => {
        console.log(error);
      }
    )
  }

}
