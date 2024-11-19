import { Component, ElementRef, HostListener, OnInit, ViewChild, viewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { NgOptimizedImage } from '@angular/common';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faTruck } from '@fortawesome/free-solid-svg-icons/faTruck';
import { faShop } from '@fortawesome/free-solid-svg-icons/faShop';
import { faUser as faUserSolid } from '@fortawesome/free-solid-svg-icons/faUser';
import { faUser } from '@fortawesome/free-regular-svg-icons/faUser';
import { faUserPlus } from '@fortawesome/free-solid-svg-icons/faUserPlus';


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
export class HomeComponent {
  protected readonly faTruck = faTruck;
  protected readonly faShop = faShop;
  protected readonly faUser = faUser;
  protected readonly faUserSolid = faUserSolid;

  @ViewChild('hamburgerBtn') hamburger!: ElementRef<HTMLButtonElement>;
  @ViewChild('loginModal') loginModal!: ElementRef<HTMLDivElement>;

  hamburgerClick = (event: Event) => {
    this.hamburger.nativeElement.classList.toggle('open');
  };
  protected readonly faUserPlus = faUserPlus;

  toggleLoginModal() {
    this.loginModal.nativeElement.classList.toggle('hidden');
  }
}
