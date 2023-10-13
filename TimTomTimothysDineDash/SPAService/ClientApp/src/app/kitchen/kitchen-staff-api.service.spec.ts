import { TestBed } from '@angular/core/testing';

import { KitchenStaffApiService } from './kitchen-staff-api.service';

describe('KitchenStaffApiService', () => {
  let service: KitchenStaffApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(KitchenStaffApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
