import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InsertTakenOrderComponent } from './insert-taken-order.component';

describe('InsertTakenOrderComponent', () => {
  let component: InsertTakenOrderComponent;
  let fixture: ComponentFixture<InsertTakenOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InsertTakenOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InsertTakenOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
