import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StartCookingOrderComponent } from './start-cooking-order.component';

describe('StartCookingOrderComponent', () => {
  let component: StartCookingOrderComponent;
  let fixture: ComponentFixture<StartCookingOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StartCookingOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StartCookingOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
