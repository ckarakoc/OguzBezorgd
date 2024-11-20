import { Component, input } from '@angular/core';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-box',
  standalone: true,
  imports: [
    NgClass
  ],
  templateUrl: './box.component.html',
  styleUrl: './box.component.css'
})
export class BoxComponent {
  count = input.required<number>();
  content = input.required<string>();
  svgPath = input.required<string>();
  title = input.required<string>();
}
