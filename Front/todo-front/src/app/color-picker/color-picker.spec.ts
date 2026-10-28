import { TestBed } from '@angular/core/testing';
import { ColorPicker } from './color-picker';

describe('ColorPicker', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({ imports: [ColorPicker] }).compileComponents();
  });

  it('emits a preset color', async () => {
    const fixture = TestBed.createComponent(ColorPicker);
    fixture.componentRef.setInput('value', '#4F8EF7');
    await fixture.whenStable();

    const colors: string[] = [];
    fixture.componentInstance.valueChange.subscribe(color => colors.push(color));
    (fixture.nativeElement.querySelector('[aria-label="Select Rose"]') as HTMLButtonElement).click();

    expect(colors).toEqual(['#DF6FA5']);
  });

  it('accepts a custom HEX color', async () => {
    const fixture = TestBed.createComponent(ColorPicker);
    fixture.componentRef.setInput('value', '#4F8EF7');
    await fixture.whenStable();

    const colors: string[] = [];
    fixture.componentInstance.valueChange.subscribe(color => colors.push(color));
    (fixture.nativeElement.querySelector('.custom-toggle') as HTMLButtonElement).click();
    await fixture.whenStable();
    const input = fixture.nativeElement.querySelector('[aria-label="HEX color"]') as HTMLInputElement;
    input.value = '#12ab34';
    input.dispatchEvent(new Event('change'));

    expect(colors).toEqual(['#12AB34']);
  });
});
