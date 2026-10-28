import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TaskPreview } from './task-preview';
import { Task } from '../../models/Task.model';

describe('TaskPreview', () => {
  let component: TaskPreview;
  let fixture: ComponentFixture<TaskPreview>;
  const task: Task = {
    id: '1', name: 'Test task', description: '', dueDate: '2026-10-14T10:00:00Z',
    isCompleted: false, categoryId: '1', createdAt: '', lastUpdatedAt: '',
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskPreview],
    }).compileComponents();

    fixture = TestBed.createComponent(TaskPreview);
    fixture.componentRef.setInput('task', task);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('emits a completed task when the checkbox changes', () => {
    const completed: Task[] = [];
    component.completedChange.subscribe(value => completed.push(value));
    fixture.nativeElement.querySelector('input[type=checkbox]').click();
    expect(completed[0].isCompleted).toBe(true);
  });
});
