import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  console.log('🔥 INTERCEPTOR IS RUNNING!'); // 🔥 לוג חובה לבדיקה
  
  const authService = inject(AuthService);
  const token = authService.getToken();

  console.log('🔐 Token from localStorage:', token ? token.substring(0, 30) + '...' : 'NO TOKEN');

  if (token) {
    const clonedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    console.log('✅ Added Authorization header');
    return next(clonedReq);
  }

  console.warn('⚠️ No token - sending without auth');
  return next(req);
};