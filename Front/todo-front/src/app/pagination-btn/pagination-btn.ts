import { Component, input, output } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-pagination-btn',
  styleUrl: './pagination-btn.css',
  templateUrl: './pagination-btn.html',
})
export class PaginationBtn {
  selected = input();
  Clicked = output<void>();

  testClick() {
      console.log('CLICK');
    }
}
