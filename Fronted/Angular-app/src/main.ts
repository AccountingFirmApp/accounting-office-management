import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app';
import 'zone.js'; // Ensure Zone.js is imported here as well
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(), // Add this to provide HttpClient in standalone mode
    provideRouter(routes) // Provide routing configuration
  ],
}).catch((err) => console.error(err));
