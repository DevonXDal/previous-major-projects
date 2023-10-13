import { Component, OnDestroy, OnInit } from '@angular/core';
import { WaitStaffStatus } from './data/wait-staff-status';
import { WaitStaffApiService } from './wait-staff-api.service';

@Component({
  selector: 'app-wait',
  templateUrl: './wait.component.html',
  styleUrls: ['./wait.component.scss']
})
export class WaitComponent implements OnInit, OnDestroy {

  status?: WaitStaffStatus; // This will not be null at any point after the first time it gets data.
  showData = true;
  private interval?: NodeJS.Timer;
  pageTitle = 'Wait Staff';

  constructor(private apiService: WaitStaffApiService) { }

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
