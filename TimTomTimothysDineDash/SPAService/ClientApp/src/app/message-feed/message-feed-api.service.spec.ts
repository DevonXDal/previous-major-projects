import { TestBed } from '@angular/core/testing';

import { MessageFeedApiService } from './message-feed-api.service';

describe('MessageFeedApiService', () => {
  let service: MessageFeedApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MessageFeedApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
