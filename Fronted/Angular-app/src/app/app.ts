import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router'; // Ensure routing modules are imported
import { CommonModule } from '@angular/common';
import { MatSnackBarModule } from '@angular/material/snack-bar';
@Component({
  selector: 'app-root',
  standalone: true, // Mark as standalone
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive,MatSnackBarModule], // Add necessary imports
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class AppComponent {
  title = 'accounting-system';
}