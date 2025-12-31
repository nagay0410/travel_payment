import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { RegisterComponent } from './register.component';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of, throwError } from 'rxjs';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let router: Router;
  let snackBarSpy: jasmine.SpyObj<MatSnackBar>;

  beforeEach(async () => {
    // スパイオブジェクトを作成
    authServiceSpy = jasmine.createSpyObj('AuthService', ['register']);
    authServiceSpy.register.and.returnValue(of(void 0));
    
    snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);

    await TestBed.configureTestingModule({
      imports: [
        NoopAnimationsModule,
        RouterTestingModule.withRoutes([]),
        HttpClientTestingModule,
        MatSnackBarModule
      ],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: MatSnackBar, useValue: snackBarSpy }
      ]
    })
    .overrideComponent(RegisterComponent, {
      add: {
        providers: [
          { provide: AuthService, useValue: authServiceSpy },
          { provide: MatSnackBar, useValue: snackBarSpy }
        ]
      }
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have invalid form when empty', () => {
    expect(component.registerForm.valid).toBeFalsy();
  });

  it('should have valid form when all fields are filled correctly', () => {
    component.registerForm.setValue({
      username: 'TestUser',
      email: 'test@example.com',
      password: 'password123',
      confirmPassword: 'password123'
    });
    expect(component.registerForm.valid).toBeTruthy();
  });

  it('should have invalid form when passwords do not match', () => {
    component.registerForm.setValue({
      username: 'TestUser',
      email: 'test@example.com',
      password: 'password123',
      confirmPassword: 'differentPassword'
    });
    expect(component.registerForm.valid).toBeFalsy();
    expect(component.registerForm.hasError('passwordMismatch')).toBeTruthy();
  });

  it('should call authService.register on valid form submit', () => {
    const validData = {
      username: 'TestUser',
      email: 'test@example.com',
      password: 'password123',
      confirmPassword: 'password123'
    };
    component.registerForm.setValue(validData);

    component.onSubmit();

    expect(authServiceSpy.register).toHaveBeenCalledWith({
      username: validData.username,
      email: validData.email,
      password: validData.password
    });
  });

  it('should navigate to login on successful registration', fakeAsync(() => {
    authServiceSpy.register.and.returnValue(of(void 0));
    const validData = {
      username: 'TestUser',
      email: 'test@example.com',
      password: 'password123',
      confirmPassword: 'password123'
    };
    component.registerForm.setValue(validData);

    component.onSubmit();
    tick();

    expect(snackBarSpy.open).toHaveBeenCalledWith(
      'ユーザー登録が完了しました。ログインしてください。',
      '閉じる',
      { duration: 3000 }
    );
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  }));

  it('should show error message on registration failure', fakeAsync(() => {
    authServiceSpy.register.and.returnValue(throwError(() => new Error('Registration failed')));
    const validData = {
      username: 'TestUser',
      email: 'test@example.com',
      password: 'password123',
      confirmPassword: 'password123'
    };
    component.registerForm.setValue(validData);

    component.onSubmit();
    tick();

    expect(snackBarSpy.open).toHaveBeenCalledWith(
      '登録に失敗しました。もう一度お試しください。',
      '閉じる',
      { duration: 3000 }
    );
    expect(router.navigate).not.toHaveBeenCalled();
  }));

  it('should not call authService.register when form is invalid', () => {
    component.registerForm.setValue({
      username: '',
      email: 'invalid-email',
      password: '123',
      confirmPassword: '456'
    });

    component.onSubmit();

    expect(authServiceSpy.register).not.toHaveBeenCalled();
  });
});
