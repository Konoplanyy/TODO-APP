import { Component, effect, input, output } from '@angular/core';

@Component({
  selector: 'app-color-picker',
  templateUrl: './color-picker.html',
  styleUrl: './color-picker.css',
})
export class ColorPicker {
  value = input('#378add');
  valueChange = output<string>();

  readonly presets = [
    { name: 'Sky', color: '#4F8EF7' },
    { name: 'Teal', color: '#2CBFA5' },
    { name: 'Green', color: '#6ABF69' },
    { name: 'Amber', color: '#F1AD4E' },
    { name: 'Coral', color: '#F47C6A' },
    { name: 'Rose', color: '#DF6FA5' },
    { name: 'Violet', color: '#9B7EE8' },
    { name: 'Slate', color: '#7892A6' },
  ];

  showCustom = false;
  hue = 210;
  saturation = 0.75;
  brightness = 0.87;
  private dragging: 'square' | 'hue' | null = null;

  constructor() {
    effect(() => {
      const color = this.value();
      this.setFromHex(color);
      if (!this.presets.some(preset => preset.color.toLowerCase() === color.toLowerCase()))
        this.showCustom = true;
    });
  }

  get hueColor(): string {
    return this.hsvToHex(this.hue, 1, 1);
  }

  selectPreset(color: string): void {
    this.setFromHex(color);
    this.valueChange.emit(color);
  }

  startDrag(event: PointerEvent, area: 'square' | 'hue'): void {
    this.dragging = area;
    (event.currentTarget as HTMLElement).setPointerCapture(event.pointerId);
    this.updateColor(event, area);
  }

  continueDrag(event: PointerEvent, area: 'square' | 'hue'): void {
    if (this.dragging === area) this.updateColor(event, area);
  }

  endDrag(): void {
    this.dragging = null;
  }

  onKeyDown(event: KeyboardEvent, area: 'square' | 'hue'): void {
    if (area === 'hue') {
      if (event.key === 'ArrowRight' || event.key === 'ArrowUp') this.hue = (this.hue + 5) % 360;
      else if (event.key === 'ArrowLeft' || event.key === 'ArrowDown') this.hue = (this.hue + 355) % 360;
      else return;
    } else {
      if (event.key === 'ArrowRight') this.saturation = this.clamp(this.saturation + 0.02);
      else if (event.key === 'ArrowLeft') this.saturation = this.clamp(this.saturation - 0.02);
      else if (event.key === 'ArrowUp') this.brightness = this.clamp(this.brightness + 0.02);
      else if (event.key === 'ArrowDown') this.brightness = this.clamp(this.brightness - 0.02);
      else return;
    }

    event.preventDefault();
    this.valueChange.emit(this.hsvToHex(this.hue, this.saturation, this.brightness));
  }

  onHexChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    const hex = input.value.trim();
    if (/^#[0-9a-fA-F]{6}$/.test(hex)) {
      this.setFromHex(hex);
      this.valueChange.emit(hex.toUpperCase());
    } else {
      input.value = this.value().toUpperCase();
    }
  }

  private updateColor(event: PointerEvent, area: 'square' | 'hue'): void {
    const rect = (event.currentTarget as HTMLElement).getBoundingClientRect();
    if (!rect.width || !rect.height) return;

    if (area === 'hue') {
      this.hue = this.clamp((event.clientX - rect.left) / rect.width) * 360;
    } else {
      this.saturation = this.clamp((event.clientX - rect.left) / rect.width);
      this.brightness = 1 - this.clamp((event.clientY - rect.top) / rect.height);
    }

    this.valueChange.emit(this.hsvToHex(this.hue, this.saturation, this.brightness));
  }

  private setFromHex(hex: string): void {
    if (!/^#[0-9a-fA-F]{6}$/.test(hex)) return;

    const red = parseInt(hex.slice(1, 3), 16) / 255;
    const green = parseInt(hex.slice(3, 5), 16) / 255;
    const blue = parseInt(hex.slice(5, 7), 16) / 255;
    const max = Math.max(red, green, blue);
    const min = Math.min(red, green, blue);
    const difference = max - min;

    if (difference === 0) this.hue = 0;
    else if (max === red) this.hue = 60 * (((green - blue) / difference) % 6);
    else if (max === green) this.hue = 60 * ((blue - red) / difference + 2);
    else this.hue = 60 * ((red - green) / difference + 4);

    if (this.hue < 0) this.hue += 360;
    this.saturation = max === 0 ? 0 : difference / max;
    this.brightness = max;
  }

  private hsvToHex(hue: number, saturation: number, brightness: number): string {
    const chroma = brightness * saturation;
    const second = chroma * (1 - Math.abs((hue / 60) % 2 - 1));
    const match = brightness - chroma;
    let red = 0, green = 0, blue = 0;

    if (hue < 60) { red = chroma; green = second; }
    else if (hue < 120) { red = second; green = chroma; }
    else if (hue < 180) { green = chroma; blue = second; }
    else if (hue < 240) { green = second; blue = chroma; }
    else if (hue < 300) { red = second; blue = chroma; }
    else { red = chroma; blue = second; }

    const toHex = (value: number) => Math.round((value + match) * 255).toString(16).padStart(2, '0');
    return `#${toHex(red)}${toHex(green)}${toHex(blue)}`.toUpperCase();
  }

  private clamp(value: number): number {
    return Math.min(1, Math.max(0, value));
  }
}
