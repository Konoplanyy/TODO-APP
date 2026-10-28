import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CategoryBtn } from './category-btn';

describe('CategoryBtn', () => {
  let component: CategoryBtn;
  let fixture: ComponentFixture<CategoryBtn>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryBtn],
    }).compileComponents();

    fixture = TestBed.createComponent(CategoryBtn);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
