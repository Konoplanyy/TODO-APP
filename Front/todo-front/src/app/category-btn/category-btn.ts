import { Component, input, output } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-category-btn',
  styleUrl: './category-btn.css',
  templateUrl: './category-btn.html',
})
export class CategoryBtn {
  main_text = input<string>();
  task_color = input<string>();
  selected = input(false);
  clicked = output<void>();
}
