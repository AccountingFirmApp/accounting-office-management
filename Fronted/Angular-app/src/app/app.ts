import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';  // ← וודא שיש את כל 3
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
    standalone: true,

  templateUrl: './app.html',
  styleUrl: './app.css',

  imports: [
    CommonModule, 
    RouterOutlet, 
    RouterLink, 
    RouterLinkActive  // ← חשוב! זה חייב להיות כאן
  ],
})
export class AppComponent {
  title = 'accounting-system';
}