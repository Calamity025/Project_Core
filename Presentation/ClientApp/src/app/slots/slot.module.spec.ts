import { SlotModule } from './slot.module';

describe('SlotModule', () => {
  let slotModule: SlotModule;

  beforeEach(() => {
    slotModule = new SlotModule();
  });

  it('should create an instance', () => {
    expect(slotModule).toBeTruthy();
  });
});
