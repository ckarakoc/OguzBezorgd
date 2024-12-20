import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TopbarComponent } from '../../components/topbar/topbar.component';
import { BoxComponent } from '../../components/box/box.component';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TopbarComponent,
    BoxComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
