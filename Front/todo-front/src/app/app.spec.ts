import { TestBed } from '@angular/core/testing';
import { App } from './app';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';

describe('App', () => {
  beforeEach(async () => {
    localStorage.clear();
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    })
      .compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should show the login form', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h2')?.textContent).toContain('Welcome back');
  });

  it('shows the error and enables login again after a 401 response', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    const app = fixture.componentInstance;
    app.email = 'missing@example.com';
    app.password = 'wrong-password';

    app.submitAuth();
    const request = TestBed.inject(HttpTestingController).expectOne('/api/Users/login');
    request.flush({ message: 'Invalid email or password.' }, { status: 401, statusText: 'Unauthorized' });
    await fixture.whenStable();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('[role=alert]')?.textContent).toContain('Invalid email or password.');
    expect((compiled.querySelector('button[type=submit]') as HTMLButtonElement).disabled).toBe(false);
    expect(compiled.querySelector('button[type=submit]')?.textContent).toContain('Log in');
    TestBed.inject(HttpTestingController).verify();
  });
});
