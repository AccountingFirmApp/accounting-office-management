export class AppComponent {
  title = 'accounting-system';
}
import { Component ,signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';  // ← וודא שיש את כל 3
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule, 
    RouterOutlet, 
    RouterLink, 
    RouterLinkActive  // ← חשוב! זה חייב להיות כאן
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})




export class App {
  protected readonly title = signal('angular-app');
}
