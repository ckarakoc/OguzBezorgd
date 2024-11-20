import { Component, ElementRef, HostListener, inject, OnDestroy, ViewChild } from '@angular/core';
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { FormsModule } from "@angular/forms";
import { NgOptimizedImage } from "@angular/common";
import { RouterLink } from "@angular/router";
import { faTruck, faShop, faUser as faUserSolid, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-regular-svg-icons/faUser';
import { BreakpointService } from '../services/breakpoint.service';


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
export class TopbarComponent implements OnDestroy {
  protected readonly faTruck = faTruck;
  protected readonly faShop = faShop;
  protected readonly faUser = faUser;
  protected readonly faUserSolid = faUserSolid;
  protected readonly faUserPlus = faUserPlus;

  @ViewChild('hamburgerBtn') hamburger!: ElementRef<HTMLButtonElement>;
  @ViewChild('hamburgerMenu') hamburgerMenu!: ElementRef<HTMLDivElement>;
  @ViewChild('loginModal') loginModal!: ElementRef<HTMLDivElement>;
  @ViewChild('loginButton') loginBtn!: ElementRef<HTMLButtonElement>;

  private breakpointService = inject(BreakpointService);

  @HostListener('window:resize', ['$event.target.innerWidth'])
  onResize(width: number) {
    if (width > this.breakpointService.getBreakpoint('md')) {
      if (this.hamburger.nativeElement.classList.contains('open')) {
        this.toggleBurgerClickDiv();
      }
    }
  }

  toggleBurgerClickDiv(): void {
    this.hamburger.nativeElement.classList.toggle('open');
    this.hamburgerMenu.nativeElement.classList.toggle('hidden');
    this.loginBtn.nativeElement.classList.toggle('hidden');
  }

  ngOnDestroy(): void {
    if (this.hamburger.nativeElement.classList.contains('open')) {
      this.toggleBurgerClickDiv();
    }
  }

  hamburgerClick = (event: Event) => {
    this.toggleBurgerClickDiv();
  }

  toggleLoginModal(event: Event) {
    this.loginModal.nativeElement.classList.toggle('hidden');
  }

}
