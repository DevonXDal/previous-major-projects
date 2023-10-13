import { TestBed } from '@angular/core/testing';

import { HostStaffApiService } from './host-staff-api.service';

describe('HostStaffApiService', () => {
  let service: HostStaffApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HostStaffApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
