import { Component, computed, HostListener, inject, signal } from '@angular/core';
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { FormBuilder, FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgOptimizedImage } from "@angular/common";
import { Router, RouterLink } from "@angular/router";
import { faShop, faTruck, faUser as faUserSolid, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-regular-svg-icons/faUser';
import { BreakpointService } from '../../services/breakpoint.service';
import { AuthService } from '../../services/auth.service';
import { map } from 'rxjs';


@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [
    FormsModule,
    NgOptimizedImage,
    RouterLink,
    FaIconComponent,
    ReactiveFormsModule,
  ],
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.css'
})
export class TopbarComponent {
  protected readonly faTruck = faTruck;
  protected readonly faShop = faShop;
  protected readonly faUser = faUser;
  protected readonly faUserSolid = faUserSolid;
  protected readonly faUserPlus = faUserPlus;

  private router = inject(Router);
  private authService: AuthService = inject(AuthService);
  private breakpointService = inject(BreakpointService);
  private formBuilder = inject(FormBuilder);

  private openBurger = signal<boolean>(false);
  openClassBurger = computed(() => this.openBurger() ? 'open' : '')
  hiddenClassBurgerMenu = computed(() => this.openBurger() ? '' : 'hidden')

  private openLogin = signal<boolean>(false);
  hiddenClassLoginModal = computed(() => this.openLogin() ? '' : 'hidden')
  // todo: sticky top bar? needed?
  // fixedClassTopbar = computed(() => this.openBurger() ? 'fixed' : '')

  loginForm = this.formBuilder.group({
    username: ['superuser'],
    password: ['Pa$$w0rd']
  })

  login() {
    let username = 'superuser';
    let password = 'Pa$$w0rd';
    // console.log(this.loginForm.value);
    // todo: safe the accessToken in localStorage and refreshToken in HttpOnlyCookie
    let response = this.authService.login(this.loginForm.value).subscribe({
      next: _ => {
        console.log('login successful');
        return this.router.navigateByUrl('/profile');
      },
      error: err => {
        console.log('login failed ', err);
      }
    });
    console.log(response);
  }

  @HostListener('window:resize', ['$event.target.innerWidth'])
  onResize(width: number) {
    if (width > this.breakpointService.getBreakpoint('md')) {
      if (this.openBurger()) {
        this.openBurger.set(false);
      }
      // if (this.openLogin()) { this.openLogin.set(false); }
    }
  }


  hamburgerClick = (event: Event) => {
    this.openBurger.update(value => !value);
  }

  loginClick = (event: Event) => {
    this.openLogin.update(value => !value);
  }
}
