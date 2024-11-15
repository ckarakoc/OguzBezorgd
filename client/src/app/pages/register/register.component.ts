import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ButtonModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private authService: AuthService = inject(AuthService);

  register() {
    let test = {
      "userName": "superuser",
      "password": "Pa$$w0rd"
    }
    console.log(this.authService.login(test).subscribe());
  }
}
