import { Component, OnDestroy, OnInit } from '@angular/core';
import { HostStaffStatus } from './data/host-staff-status';
import { HostStaffApiService } from './host-staff-api.service';
import { tap } from 'rxjs';
import { error } from 'console';

@Component({
  selector: 'app-host',
  templateUrl: './host.component.html',
  styleUrls: ['./host.component.scss']
})
export class HostComponent implements OnInit, OnDestroy {
  status?: HostStaffStatus; // This will not be null at any point after the first time it gets data.
  showData = true;
  private interval?: NodeJS.Timer;
  pageTitle = 'Host Staff';

  constructor(private apiService: HostStaffApiService) { }

  ngOnInit(): void {
    this.interval = setInterval(() => {
      this.updatePage();
    }, 15000); // 15 seconds

    this.updatePage();
  }

  ngOnDestroy(): void {
    
      clearInterval(this.interval);
  }

  addSixToQueue(): void {
    this.apiService.insertPartyIntoQueue(6);
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
