import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PaginationBtn } from './pagination-btn';

describe('PaginationBtn', () => {
  let component: PaginationBtn;
  let fixture: ComponentFixture<PaginationBtn>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaginationBtn],
    }).compileComponents();

    fixture = TestBed.createComponent(PaginationBtn);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
