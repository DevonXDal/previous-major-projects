import { TestBed } from '@angular/core/testing';

import { PreventOrderLossGuard } from './prevent-order-loss.guard';

describe('PreventOrderLossGuard', () => {
  let guard: PreventOrderLossGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(PreventOrderLossGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
