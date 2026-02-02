import { Component, Input } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-back-button',
  standalone: true,
  imports: [],
  templateUrl: './back-button.component.html',
  styleUrl: './back-button.component.css'
})
export class BackButtonComponent {
  @Input() text: string = 'חזרה';
  @Input() route?: string;

  constructor(
    private location: Location,
    private router: Router
  ) {}

  goBack(): void {
    if (this.route) {
      this.router.navigate([this.route]);
    } else {
      this.location.back();
    }
  }
}
