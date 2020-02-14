import { Component } from '@angular/core';
import { AuthService } from '../shared/auth.service';
import { ApiService } from '../shared/api.service';
import { SignalRService } from '../shared/signalR.service';


@Component({
  selector: 'login',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  public message: string;

  constructor(public signalRService: SignalRService, private apiService: ApiService, private authService: AuthService) { }

  log(args: any[]) {
    args.forEach(it => {
      if (it instanceof Error) {
        it = `Error: ${it.message}`;
      }
      else if (typeof it !== 'string') {
        it = JSON.stringify(it, null, 2);
      }
      this.message = `${it}\r\n`;
    });
  }

  login() {
    this.authService.login();
  }

  logout() {
    this.authService.logout();
  }
}
