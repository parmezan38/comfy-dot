import { Component } from '@angular/core';
import { Store } from '@ngxs/store';

import { AuthService } from '../services/auth.service';
import { ApiService } from '../services/api.service';
import { ResetRooms } from '../state/lobby.actions';


@Component({
  selector: 'login',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  public message: string;

  constructor(private auth: AuthService, private store: Store) {
    this.store.dispatch(new ResetRooms());
  }

  login() {
    this.auth.login();
  }

  logout() {
    this.auth.logout();
  }
}
