import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SeatATableComponent } from './seat-atable.component';

describe('SeatATableComponent', () => {
  let component: SeatATableComponent;
  let fixture: ComponentFixture<SeatATableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SeatATableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SeatATableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
