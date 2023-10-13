import { TestBed } from '@angular/core/testing';

import { HealthCheckApiService } from './health-check-api.service';

describe('HealthCheckApiService', () => {
  let service: HealthCheckApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HealthCheckApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
