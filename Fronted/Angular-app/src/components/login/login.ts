import { Component, OnInit, AfterViewInit, NgZone, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { LoginRequestDto, GoogleLoginRequestDto } from '../../models/auth';
import { environment } from '../../environments/environment';
import { WorkerService } from '../../services/worker';

declare const google: any;

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements OnInit, AfterViewInit,OnDestroy {
  email: string = '';
  password: string = '';
  errorMessage: string = '';
  isLoading: boolean = false;
  showPassword: boolean = false;

  private googleClientId: string = environment.googleClientId;

  constructor(
    private authService: AuthService,
    private router: Router,
    private ngZone: NgZone,
    private workerServise:WorkerService
  ) { }

  ngOnInit(): void {
    this.loadGoogleSignIn();
  }
 ngOnDestroy(): void {
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.renderGoogleButton();
    }, 100);
  }

  loadGoogleSignIn(): void {
    if (document.querySelector('script[src="https://accounts.google.com/gsi/client"]')) {
      return;
    }

    const script = document.createElement('script');
    script.src = 'https://accounts.google.com/gsi/client';
    script.async = true;
    script.defer = true;
    script.onerror = () => {
      this.errorMessage = 'לא ניתן לטעון את שירות Google Sign-In. אנא בדוק את החיבור לאינטרנט';
    };
    document.head.appendChild(script);
  }

  renderGoogleButton(): void {
    if (typeof google === 'undefined') {
      setTimeout(() => this.renderGoogleButton(), 500);
      return;
    }

    const buttonContainer = document.getElementById('google-signin-button');
    if (!buttonContainer) {
      return;
    }

    try {
      google.accounts.id.initialize({
        client_id: this.googleClientId,
        callback: (response: any) => {
          this.ngZone.run(() => {
            this.handleGoogleCallback(response);
          });
        }
      });

      google.accounts.id.renderButton(
        buttonContainer,
        {
          theme: 'outline',
          size: 'large',
          width: buttonContainer.offsetWidth,
          text: 'signin_with',
          shape: 'rectangular',
          logo_alignment: 'left'
        }
      );
    } catch (error) {
      this.errorMessage = 'שגיאה באתחול Google Sign-In. אנא נסה לרענן את הדף';
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  handleGoogleCallback(response: any): void {
    this.isLoading = true;
    this.errorMessage = '';

    const request: GoogleLoginRequestDto = {
      googleToken: response.credential
    };

    this.authService.googleLogin(request).subscribe({
      next: (result) => {
        this.authService.saveToken(result.token);
        this.workerServise.currentWorker=result.worker;
        this.isLoading = false;
        this.router.navigate(['/home']);
      },
      error: (error) => {
        this.isLoading = false;

        if (error.status === 0) {
          this.errorMessage = 'לא ניתן להתחבר לשרת. אנא בדוק את החיבור לאינטרנט או פנה לתמיכה';
        } else if (error.status === 401) {
          this.errorMessage = error.error?.message || 'אימות Google נכשל. המשתמש אינו רשום במערכת';
        } else if (error.status === 500) {
          this.errorMessage = error.error?.message || 'שגיאה פנימית בשרת. אנא נסה שוב מאוחר יותר';
        } else {
          this.errorMessage = `שגיאה בהתחברות עם Google (קוד: ${error.status}). אנא נסה שוב או פנה לתמיכה`;
        }
      }
    });
  }

  onSubmit(): void {
    if (!this.email || !this.password) {
      this.errorMessage = 'נא למלא את כל השדות';
      return;
    }

    if (!this.isValidEmail(this.email)) {
      this.errorMessage = 'כתובת אימייל אינה תקינה';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const loginRequest: LoginRequestDto = {
      email: this.email,
      password: this.password
    };

    this.authService.login(loginRequest).subscribe({
      next: (response) => {
        this.authService.saveToken(response.token);
        this.workerServise.currentWorker=response.worker;
        this.isLoading = false;
        this.router.navigate(['/home']);
      },
      error: (error) => {
        this.isLoading = false;

        // הצג הודעת שגיאה מפורטת ללקוח
        if (error.status === 0) {
          this.errorMessage = 'לא ניתן להתחבר לשרת. אנא בדוק את החיבור לאינטרנט או פנה לתמיכה';
        } else if (error.status === 401) {
          this.errorMessage = error.error?.message || 'אימייל או סיסמה שגויים. אנא נסה שוב';
        } else if (error.status === 500) {
          this.errorMessage = error.error?.message || 'שגיאה פנימית בשרת. אנא נסה שוב מאוחר יותר';
        } else {
          this.errorMessage = `שגיאה בהתחברות (קוד: ${error.status}). אנא נסה שוב או פנה לתמיכה`;
        }
      }
    });
  }

  private isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }
}
