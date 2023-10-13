import { TestBed } from '@angular/core/testing';

import { TimsOverlayService } from './tims-overlay.service';

describe('TimsOverlayService', () => {
  let service: TimsOverlayService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TimsOverlayService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
