import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-back-button',
  standalone: true,
  imports: [],
  templateUrl: './back-button.component.html',
  styleUrl: './back-button.component.css'
})
export class BackButtonComponent {
  @Input() route: string = '/home';

  constructor(
    private router: Router
  ) {}

  goBack(): void {
    this.router.navigate([this.route]);
  }
}
