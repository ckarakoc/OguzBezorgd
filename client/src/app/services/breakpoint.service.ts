import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BreakpointService {
  private breakpoints = {
    // default tailwind breakpoints in pixels
    sm: 640,
    md: 768,
    lg: 1024,
    xl: 1280,
    '2xl': 1536,
  }

  getBreakpoint(bp: keyof typeof this.breakpoints) {
    return this.breakpoints[bp];
  }

  getAllBreakpoints(): Record<string, number> {
    return this.breakpoints;
  }
}
