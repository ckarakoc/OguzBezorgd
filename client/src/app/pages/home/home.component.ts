import { Component, ElementRef, HostListener, inject, OnDestroy, OnInit, ViewChild, viewChild } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BreakpointService } from '../../services/breakpoint.service';
import { TopbarComponent } from '../../topbar/topbar.component';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TopbarComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
