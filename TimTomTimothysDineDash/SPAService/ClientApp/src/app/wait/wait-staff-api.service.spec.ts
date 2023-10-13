import { TestBed } from '@angular/core/testing';

import { WaitStaffApiService } from './wait-staff-api.service';

describe('WaitStaffApiService', () => {
  let service: WaitStaffApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WaitStaffApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
