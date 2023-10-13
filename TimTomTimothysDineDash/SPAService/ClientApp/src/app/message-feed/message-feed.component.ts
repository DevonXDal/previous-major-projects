import { Component, OnInit } from '@angular/core';
import { MessageFeedApiService } from './message-feed-api.service';
import { Messages } from './messages';
import { animate, style, transition, trigger } from '@angular/animations';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-message-feed',
  templateUrl: './message-feed.component.html',
  styleUrls: ['./message-feed.component.scss'],
  animations: [
    // Animation from: https://indepth.dev/posts/1285/in-depth-guide-into-animations-in-angular
    trigger('fadeSlideInOut', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(1.25rem)' }),
        animate('500ms', style({ opacity: 1, transform: 'translateY(0)' })),
      ]),
      transition(':leave', [
        animate('500ms', style({ opacity: 0, transform: 'translateY(1.25rem)' })),
      ]),
    ]),
  ]
})
export class MessageFeedComponent implements OnInit {
  private interval?: NodeJS.Timer;
  pageTitle = 'Message Feed';
  status?: Messages;

  constructor(private apiService: MessageFeedApiService) { }

  ngOnInit(): void {
    this.interval = setInterval(() => {
      this.retrieveMessagesForFeed();
    }, 15000); // 15 seconds

    this.retrieveMessagesForFeed();
  }

  ngOnDestroy(): void {
    clearInterval(this.interval);
  }

  retrieveMessagesForFeed(): void {
    this.apiService.fetchStatus().subscribe(
      data => {
        // Set the value if the new data is different
        if (this.status !== data) {
          this.status = data;
          this.status.Messages.sort((a,b) => (b.created.toString()).localeCompare(a.created.toString()));
        }
      },
      error => {
        console.log(error);
      }
    )
  }

}
