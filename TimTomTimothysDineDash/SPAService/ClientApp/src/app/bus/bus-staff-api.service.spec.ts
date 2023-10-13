import { TestBed } from '@angular/core/testing';

import { BusStaffApiService } from './bus-staff-api.service';

describe('BusStaffApiService', () => {
  let service: BusStaffApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BusStaffApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
