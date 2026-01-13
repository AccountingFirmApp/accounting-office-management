import { Component, OnInit, AfterViewInit, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { LoginRequestDto, GoogleLoginRequestDto } from '../../models/auth';
import { environment } from '../../environments/environment';

declare const google: any;

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements OnInit, AfterViewInit {
  email: string = '';
  password: string = '';
  errorMessage: string = '';
  isLoading: boolean = false;
  showPassword: boolean = false;

  private googleClientId: string = environment.googleClientId;

  constructor(
    private authService: AuthService,
    private router: Router,
    private ngZone: NgZone
  ) {
    console.log('🔑 Google Client ID:', this.googleClientId);
  }

  ngOnInit(): void {
    this.loadGoogleSignIn();
  }

  ngAfterViewInit(): void {
    // נחכה רגע שה-DOM יהיה מוכן
    setTimeout(() => {
      this.renderGoogleButton();
    }, 100);
  }

  loadGoogleSignIn(): void {
    if (document.querySelector('script[src="https://accounts.google.com/gsi/client"]')) {
      console.log('📌 Google SDK כבר טעון');
      return;
    }

    const script = document.createElement('script');
    script.src = 'https://accounts.google.com/gsi/client';
    script.async = true;
    script.defer = true;
    script.onload = () => {
      console.log('✅ Google Sign-In SDK טעון בהצלחה');
    };
    script.onerror = () => {
      console.error('❌ שגיאה בטעינת Google Sign-In SDK');
      this.errorMessage = 'לא ניתן לטעון את Google Sign-In';
    };
    document.head.appendChild(script);
  }

  renderGoogleButton(): void {
    if (typeof google === 'undefined') {
      console.warn('⚠️ Google SDK עדיין לא טעון');
      setTimeout(() => this.renderGoogleButton(), 500);
      return;
    }

    const buttonContainer = document.getElementById('google-signin-button');
    if (!buttonContainer) {
      console.error('❌ לא נמצא אלמנט google-signin-button');
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

      console.log('✅ כפתור Google נוצר בהצלחה');
    } catch (error) {
      console.error('❌ שגיאה ביצירת כפתור Google:', error);
      this.errorMessage = 'שגיאה באתחול Google Sign-In';
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  handleGoogleCallback(response: any): void {
    console.log('📥 התקבל token מ-Google');
    this.isLoading = true;
    this.errorMessage = '';

    const request: GoogleLoginRequestDto = {
      googleToken: response.credential
    };

    this.authService.googleLogin(request).subscribe({
      next: (result) => {
        console.log('✅ התחברות הצליחה:', result.worker.email);
        this.authService.saveToken(result.token);
        this.authService.saveWorkerInfo(result.worker);
        this.isLoading = false;
        this.router.navigate(['/home']);
      },
      error: (error) => {
        console.error('❌ שגיאה בהתחברות:', error);
        this.isLoading = false;
        if (error.status === 401) {
          this.errorMessage = error.error?.message || 'אימות Google נכשל';
        } else if (error.status === 500) {
          this.errorMessage = error.error?.message || 'שגיאה פנימית בשרת';
        } else {
          this.errorMessage = 'שגיאה בהתחברות עם Google. אנא נסה שוב';
        }
      }
    });
  }

  onSubmit(): void {
    if (!this.email || !this.password) {
      this.errorMessage = 'נא למלא את כל השדות';
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
        this.authService.saveWorkerInfo(response.worker);
        this.isLoading = false;
        this.router.navigate(['/home']);
      },
      error: (error) => {
        this.isLoading = false;
        if (error.status === 401) {
          this.errorMessage = error.error?.message || 'אימייל או סיסמה שגויים';
        } else if (error.status === 500) {
          this.errorMessage = error.error?.message || 'שגיאה פנימית בשרת';
        } else {
          this.errorMessage = 'שגיאה בהתחברות. אנא נסה שוב';
        }
      }
    });
  }
}
