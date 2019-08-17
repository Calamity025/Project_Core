import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SlotEditComponent } from './slot-edit.component';

describe('SlotEditComponent', () => {
  let component: SlotEditComponent;
  let fixture: ComponentFixture<SlotEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SlotEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SlotEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
