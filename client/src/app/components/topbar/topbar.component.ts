import { Component, computed, HostListener, inject, signal } from '@angular/core';
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { FormsModule } from "@angular/forms";
import { NgOptimizedImage } from "@angular/common";
import { RouterLink } from "@angular/router";
import { faShop, faTruck, faUser as faUserSolid, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-regular-svg-icons/faUser';
import { BreakpointService } from '../../services/breakpoint.service';


@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [
    FormsModule,
    NgOptimizedImage,
    RouterLink,
    FaIconComponent,
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

  private openBurger = signal<boolean>(false);
  openClassBurger = computed(() => this.openBurger() ? 'open' : '')
  hiddenClassBurgerMenu = computed(() => this.openBurger() ? '' : 'hidden')

  private openLogin = signal<boolean>(false);
  hiddenClassLoginModal = computed(() => this.openLogin() ? '' : 'hidden')
  // todo: sticky top bar? needed?
  // fixedClassTopbar = computed(() => this.openBurger() ? 'fixed' : '')

  private breakpointService = inject(BreakpointService);

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
