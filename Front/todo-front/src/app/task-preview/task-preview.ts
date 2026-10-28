import { Component, input, output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Task } from '../../models/Task.model';
import { Category } from '../../models/Category.model';

@Component({
  imports: [DatePipe],
  selector: 'app-task-preview',
  styleUrl: './task-preview.css',
  templateUrl: './task-preview.html',
})
export class TaskPreview {
  task = input.required<Task>();
  category = input<Category | undefined>();
  completedChange = output<Task>();
  edit = output<Task>();
  delete = output<Task>();

  isActive = false;

  toggleCompleted(): void {
    this.completedChange.emit({ ...this.task(), isCompleted: !this.task().isCompleted });
  }

  getDarkColor(hexColor: string): string {
    const hex = hexColor.replace('#', '');
    const r = parseInt(hex.substring(0, 2), 16) / 255;
    const g = parseInt(hex.substring(2, 4), 16) / 255;
    const b = parseInt(hex.substring(4, 6), 16) / 255;

    const max = Math.max(r, g, b);
    const min = Math.min(r, g, b);
    const delta = max - min;
    const l = (max + min) / 2;

    let h = 0;
    if (delta !== 0) {
      if (max === r) h = ((g - b) / delta) % 6;
      else if (max === g) h = (b - r) / delta + 2;
      else h = (r - g) / delta + 4;
      h *= 60;
      if (h < 0) h += 360;
    }

    let s = 0;
    if (delta !== 0) {
      s = delta / (1 - Math.abs(2 * l - 1));
    }

    const newLightness = l * 100 * 0.53;
    const newSaturation = Math.min(s * 100 * 1.15, 100);

    return this.hslToHex(h, newSaturation, newLightness);
  }


  getLightBackgroundColor(hexColor: string, lightness = 92): string {
    const hex = hexColor.replace('#', '');
    const r = parseInt(hex.substring(0, 2), 16) / 255;
    const g = parseInt(hex.substring(2, 4), 16) / 255;
    const b = parseInt(hex.substring(4, 6), 16) / 255;

    const max = Math.max(r, g, b);
    const min = Math.min(r, g, b);
    const delta = max - min;

    let hue = 0;
    if (delta !== 0) {
      if (max === r) hue = ((g - b) / delta) % 6;
      else if (max === g) hue = (b - r) / delta + 2;
      else hue = (r - g) / delta + 4;
      hue *= 60;
      if (hue < 0) hue += 360;
    }

    return this.hslToHex(hue, 70, lightness);
  }

  hslToHex(h: number, s: number, l: number): string {
    s /= 100;
    l /= 100;

    const c = (1 - Math.abs(2 * l - 1)) * s;
    const x = c * (1 - Math.abs(((h / 60) % 2) - 1));
    const m = l - c / 2;

    let r = 0, g = 0, b = 0;

    if (h < 60) { r = c; g = x; b = 0; }
    else if (h < 120) { r = x; g = c; b = 0; }
    else if (h < 180) { r = 0; g = c; b = x; }
    else if (h < 240) { r = 0; g = x; b = c; }
    else if (h < 300) { r = x; g = 0; b = c; }
    else { r = c; g = 0; b = x; }

    const toHex = (value: number) =>
      Math.round((value + m) * 255).toString(16).padStart(2, '0');

    return `#${toHex(r)}${toHex(g)}${toHex(b)}`;
  }
}
