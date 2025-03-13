import { TestBed } from '@angular/core/testing';

import { FlagBasedPreloadingStrategyService } from './flag-based-preloading-strategy.service';

describe('FlagBasedPreloadingStrategyService', () => {
  let service: FlagBasedPreloadingStrategyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FlagBasedPreloadingStrategyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
