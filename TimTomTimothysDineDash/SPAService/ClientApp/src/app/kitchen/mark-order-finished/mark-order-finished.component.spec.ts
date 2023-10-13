import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MarkOrderFinishedComponent } from './mark-order-finished.component';

describe('MarkOrderFinishedComponent', () => {
  let component: MarkOrderFinishedComponent;
  let fixture: ComponentFixture<MarkOrderFinishedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MarkOrderFinishedComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MarkOrderFinishedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
