import { Component, OnDestroy, OnInit } from '@angular/core';
import { HealthCheckApiService } from './health-check-api.service';
import { TimsOverlayService } from './shared/tims-overlay.service';
import { slideInAnimation } from './animations';

/// https://dev.to/rodrigokamada/adding-the-progressive-web-application-pwa-to-an-angular-application-4g1e - PWA
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [
    slideInAnimation
  ]
})
export class AppComponent implements OnInit, OnDestroy {
  private isAliveInterval?: NodeJS.Timer;
  isOnline: boolean;
  title = 'app';

  constructor(private apiService: HealthCheckApiService, private overlayService: TimsOverlayService) {
    this.isOnline = false;
  }

  public ngOnInit(): void {
    this.updateOnlineStatus();

    this.isAliveInterval = setInterval(() => {
      this.updateOnlineStatus();
    }, 10000); // 10 seconds
  }

  ngOnDestroy(): void {
    
    clearInterval(this.isAliveInterval);
}

  private async updateOnlineStatus(): Promise<void> {
    let prevStatus = this.isOnline;
    this.isOnline = await this.apiService.checkApiReachable();

    if (this.isOnline && this.isOnline != prevStatus) {
      this.overlayService.addSingleToast('Connection Reestablished', 'The system can now communicate with the API', 'info');
    } else if (!this.isOnline && this.isOnline != prevStatus) {
      this.overlayService.addSingleToast('Connection Lost', 'The system is currently unable to communicate with the API', 'warn');
    }
  }

}
