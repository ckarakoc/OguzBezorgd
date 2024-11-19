import { Component, ElementRef, HostListener, inject, OnDestroy, OnInit, ViewChild, viewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { NgOptimizedImage } from '@angular/common';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faTruck } from '@fortawesome/free-solid-svg-icons/faTruck';
import { faShop } from '@fortawesome/free-solid-svg-icons/faShop';
import { faUser as faUserSolid } from '@fortawesome/free-solid-svg-icons/faUser';
import { faUser } from '@fortawesome/free-regular-svg-icons/faUser';
import { faUserPlus } from '@fortawesome/free-solid-svg-icons/faUserPlus';
import { BreakpointService } from '../../services/breakpoint.service';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    NgOptimizedImage,
    FaIconComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnDestroy {
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

  hamburgerClick = (event: Event) => {
    this.toggleBurgerClickDiv();
  }

  toggleLoginModal(event: Event) {
    this.loginModal.nativeElement.classList.toggle('hidden');
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
}
