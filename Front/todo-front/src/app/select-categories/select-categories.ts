import { Component, input, output } from '@angular/core';
import { Category } from '../../models/Category.model';

@Component({
  imports: [],
  selector: 'app-select-categories',
  styleUrl: './select-categories.css',
  templateUrl: './select-categories.html',
})
export class SelectCategories {
  categories = input<Category[]>([]);
  selectedId = input('');
  selectedIdChange = output<string>();
}
