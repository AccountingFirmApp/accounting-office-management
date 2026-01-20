// import { ApplicationConfig,/* provideBrowserGlobalErrorListeners */} from '@angular/core';
// import { provideRouter } from '@angular/router';

// import { routes } from './app.routes';
// import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

// export const appConfig: ApplicationConfig = {
//   providers: [
//     // provideBrowserGlobalErrorListeners(),
//     provideRouter(routes), provideAnimationsAsync()
//   ]
// };


// app.config.ts
// import { ApplicationConfig } from '@angular/core';
// import { provideRouter } from '@angular/router';
// import { provideHttpClient, withInterceptors } from '@angular/common/http'; // 🆕
// import { routes } from './app.routes';
// import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
// import { authInterceptor } from '../Interceptor/auth.interceptor'; // 🆕

// export const appConfig: ApplicationConfig = {
//   providers: [
//     provideRouter(routes),
//     provideAnimationsAsync(),
//     provideHttpClient(                    // 🆕
//       withInterceptors([authInterceptor]) // 🆕
//     )
//   ]
// };

// import { ApplicationConfig } from '@angular/core';
// import { provideRouter } from '@angular/router';
// import { provideHttpClient, withInterceptors, withInterceptorsFromDi } from '@angular/common/http'; // 🔥 הוסף withInterceptorsFromDi
// import { routes } from './app.routes';
// import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
// import { authInterceptor } from '../Interceptor/auth.interceptor';

// export const appConfig: ApplicationConfig = {
//   providers: [
//     provideRouter(routes),
//     provideAnimationsAsync(),
//     provideHttpClient(
//       withInterceptorsFromDi(), // 🔥 חובה!
//       withInterceptors([authInterceptor])
//     )
//   ]
// };

import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { authInterceptor } from '../Interceptor/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(
      withInterceptors([authInterceptor]) // 🔥 רק את זה!
    )
  ]
};