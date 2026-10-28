import { Component, input, output } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-search-field',
  styleUrl: './search-field.css',
  templateUrl: './search-field.html',
})
export class SearchField {
  value = input('');
  valueChange = output<string>();
}
