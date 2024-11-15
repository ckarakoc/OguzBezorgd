import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Aura } from 'primeng/themes/aura';
import { PrimeNGConfig } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { definePreset, palette } from 'primeng/themes';
import { DividerModule } from 'primeng/divider';
import { InputTextModule } from 'primeng/inputtext';



@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ButtonModule, DividerModule, InputTextModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'client';

  constructor(private config: PrimeNGConfig) {
    const MyPreset = definePreset(Aura, {
      semantic: {
        primary: palette('{cyan}')
      }
    });

    this.config.theme.set({
      preset: MyPreset,
      options: {
        darkModeSelector: '.my-app-dark',
      }
    });
  }
}
