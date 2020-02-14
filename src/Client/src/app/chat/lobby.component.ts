import { Component } from '@angular/core';
import { Store } from '@ngxs/store';

import { IRoom, IUser } from '../shared/interfaces';
import { AuthService } from '../shared/auth.service';
import { SignalRService } from '../shared/signalR.service';
import { ApiService } from '../shared/api.service';
import { Observable } from 'rxjs';

import { AddRooms } from '../state/lobby.actions';

@Component({
  selector: 'lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.css']
})

export class LobbyComponent {
  user: IUser;
  rooms: Observable<IRoom[]>;

  constructor(private auth: AuthService, private signalR: SignalRService, private api: ApiService, private store: Store) {
    this.auth.getUserProfile()
      .then(user => this.user = user)
      .then(() => {
        this.signalR.startConnection(this.user.id);
        this.signalR.connection.on('RefreshRooms', () => this.refreshRooms());
        this.refreshRooms()
      })

    this.rooms = this.store.select(state => state.lobby.rooms);
  }

  private refreshRooms() {
    this.api.listRooms().subscribe((res: any) => {
      if (res instanceof Array) {
        this.store.dispatch(new AddRooms(res));
      }
    });
  }
}
