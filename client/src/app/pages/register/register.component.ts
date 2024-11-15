import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  private authService: AuthService = inject(AuthService);
  registerForm!: FormGroup;

  ngOnInit() {
    this.registerForm = new FormGroup({
      userName: new FormControl('', [Validators.required, Validators.minLength(2)]),
      password: new FormControl('', [Validators.required, Validators.minLength(8)]),
      email: new FormControl('', [Validators.required, Validators.email]),
      phoneNumber: new FormControl('', []), //todo phone number validator
      role: new FormControl('', []),
      userAddress: new FormGroup({
        streetAddress: new FormControl('', []),
        city: new FormControl('', []),
        state: new FormControl('', []),
        zipCode: new FormControl('', [Validators.required]), //todo zipCode validator
      }),
    });
  }

  register() {
    let test = {
      password: "Pa$$w0rd",
      role: "Customer"
    }
    Object.keys(this.registerForm.controls).forEach(key => {
      let errors = this.registerForm.get(key)?.errors;
      if (errors) {
        console.log(key, errors);
      }
    });

    console.log();
    // this.registerForm.patchValue(test)
    // console.log(this.registerForm.value)
    console.log(this.authService.register(this.registerForm.value).subscribe());
  }
}
