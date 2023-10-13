import { Component, OnDestroy, OnInit } from '@angular/core';
import { KitchenStaffApiService } from './kitchen-staff-api.service';
import { KitchenStaffStatus } from './data/kitchen-staff-status';

@Component({
  selector: 'app-kitchen',
  templateUrl: './kitchen.component.html',
  styleUrls: ['./kitchen.component.scss']
})
export class KitchenComponent implements OnInit, OnDestroy {

  status?: KitchenStaffStatus; // This will not be null at any point after the first time it gets data.
  showData = true;
  private interval?: NodeJS.Timer;
  pageTitle = 'Kitchen Staff';

  constructor(private apiService: KitchenStaffApiService) { }

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
