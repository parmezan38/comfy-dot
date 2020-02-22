import { Component } from '@angular/core';
import { Store } from '@ngxs/store';

import { AuthService } from '../services/auth.service';
import { ResetRooms } from '../state/lobby.actions';


@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  public message: string;

  constructor(private auth: AuthService, private store: Store) {
    this.store.dispatch(new ResetRooms());
  }

  public login() {
    this.auth.login();
  }
}
