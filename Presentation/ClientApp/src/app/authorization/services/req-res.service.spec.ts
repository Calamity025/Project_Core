import { TestBed, inject } from '@angular/core/testing';

import { ReqResService } from './req-res.service';

describe('ReqResService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ReqResService]
    });
  });

  it('should be created', inject([ReqResService], (service: ReqResService) => {
    expect(service).toBeTruthy();
  }));
});
