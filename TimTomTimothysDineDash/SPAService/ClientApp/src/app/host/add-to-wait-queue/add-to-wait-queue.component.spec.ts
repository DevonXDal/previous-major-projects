import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddToWaitQueueComponent } from './add-to-wait-queue.component';

describe('AddToWaitQueueComponent', () => {
  let component: AddToWaitQueueComponent;
  let fixture: ComponentFixture<AddToWaitQueueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddToWaitQueueComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddToWaitQueueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
