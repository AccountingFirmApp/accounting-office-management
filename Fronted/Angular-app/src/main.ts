import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app';
import 'zone.js'; // Ensure Zone.js is imported here as well

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
