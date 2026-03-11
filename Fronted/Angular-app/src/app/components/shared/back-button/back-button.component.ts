// import { Component, Input } from '@angular/core';
// import { Router } from '@angular/router';

// @Component({
//   selector: 'app-back-button',
//   standalone: true,
//   imports: [],
//   templateUrl: './back-button.component.html',
//   styleUrl: './back-button.component.css'
// })
// export class BackButtonComponent {
//   @Input() route: string = '/home';
//   @Input() text: string = '';

//   constructor(
//     private router: Router
//   ) {}

//   goBack(): void {
//     this.router.navigate([this.route]);
//   }
// }
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-back-button',
  standalone: true,
  imports: [],
  templateUrl: './back-button.component.html',
  styleUrl: './back-button.component.css'
})
export class BackButtonComponent {
  @Input() route: string = '';
  @Input() text: string = '';

  constructor(
    private router: Router,
    private location: Location
  ) {}

  goBack(): void {
    if (this.route) {
      this.router.navigate([this.route]);
    } else {
      this.location.back();
    }
  }
}