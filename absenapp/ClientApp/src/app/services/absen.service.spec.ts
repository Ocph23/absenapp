import { TestBed } from '@angular/core/testing';

import { AbsenService } from './absen.service';

describe('AbsenService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AbsenService = TestBed.get(AbsenService);
    expect(service).toBeTruthy();
  });
});
