import { Component, ElementRef, HostListener, OnInit, ViewChild, viewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { NgOptimizedImage } from '@angular/common';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faTruck } from '@fortawesome/free-solid-svg-icons/faTruck';
import { faShop } from '@fortawesome/free-solid-svg-icons/faShop';
import { faUser } from '@fortawesome/free-solid-svg-icons/faUser';


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

  @ViewChild('hamburgerBtn') hamburger!: ElementRef<HTMLButtonElement>;

  hamburgerClick = (event: Event) => {
    this.hamburger.nativeElement.classList.toggle('open');
  };
}
