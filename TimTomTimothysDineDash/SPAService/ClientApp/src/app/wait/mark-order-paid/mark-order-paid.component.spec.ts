import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MarkOrderPaidComponent } from './mark-order-paid.component';

describe('MarkOrderPaidComponent', () => {
  let component: MarkOrderPaidComponent;
  let fixture: ComponentFixture<MarkOrderPaidComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MarkOrderPaidComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MarkOrderPaidComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
