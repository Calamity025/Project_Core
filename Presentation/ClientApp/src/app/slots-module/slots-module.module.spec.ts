import { SlotsModuleModule } from './slots-module.module';

describe('SlotsModuleModule', () => {
  let slotsModuleModule: SlotsModuleModule;

  beforeEach(() => {
    slotsModuleModule = new SlotsModuleModule();
  });

  it('should create an instance', () => {
    expect(slotsModuleModule).toBeTruthy();
  });
});
