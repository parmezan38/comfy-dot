import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { map } from "rxjs/operators";

@Injectable()
export class AuthGuard {
  constructor(private router: Router, private authService: AuthService) { }

  canActivate(): any {
    return this.authService.isLoggedIn().pipe(
      map(user => {
        if(user) return true;
        this.router.navigate(['/login']);
        return false;
      }));
  }
}
