import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MarkTableAsCleanedComponent } from './mark-table-as-cleaned.component';

describe('MarkTableAsCleanedComponent', () => {
  let component: MarkTableAsCleanedComponent;
  let fixture: ComponentFixture<MarkTableAsCleanedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MarkTableAsCleanedComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MarkTableAsCleanedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
