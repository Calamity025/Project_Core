import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SlotCreationComponent } from './slot-creation.component';

describe('SlotCreationComponent', () => {
  let component: SlotCreationComponent;
  let fixture: ComponentFixture<SlotCreationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SlotCreationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SlotCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
