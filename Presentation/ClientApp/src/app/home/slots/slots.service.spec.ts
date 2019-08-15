import { TestBed, inject } from '@angular/core/testing';

import { SlotsService } from './slots.service';

describe('SlotsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SlotsService]
    });
  });

  it('should be created', inject([SlotsService], (service: SlotsService) => {
    expect(service).toBeTruthy();
  }));
});
