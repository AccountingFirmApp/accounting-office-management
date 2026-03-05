

import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app';
import 'zone.js';

bootstrapApplication(AppComponent, appConfig) // 🔥 רק את זה!
  .catch((err) => console.error(err));