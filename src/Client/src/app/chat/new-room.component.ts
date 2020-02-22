import { Component, OnInit } from '@angular/core';

import { IUser, IRoomRegister, IResponseMessage } from '../shared/interfaces';
import { ApiService } from '../services/api.service';
import { AuthService } from '../services/auth.service';
import { SnackbarService } from '../services/snackbar.service';

@Component({
  selector: 'new-room',
  templateUrl: './new-room.component.html',
  styleUrls: ['./new-room.component.html']
})
export class NewRoomComponent implements OnInit {
  user: IUser;
  public name: string;
  public capacity: number;

  constructor(private auth: AuthService, private api: ApiService, private snackbar: SnackbarService) { }

  ngOnInit() {
    this.auth.getUserProfile().then(user => this.user = user);
  }

  public submit() {
    if (!this.name || !this.user) return;
    const room: IRoomRegister = {
      name: this.name,
      capacity: this.capacity,
      userId: this.user.id
    }
    this.api.createRoom(room)
      .subscribe((res: IResponseMessage) => this.snackbar.show(res));
  }

  public capacityChanged(val) {
    if (val > 99) {
      val = 99;
    }
    if (val < 1) {
      val = 1;
    }
    this.capacity = val;
  }

  public preventInput(event) {
    const value = this.capacity;
    if (value > 99) {
      event.preventDefault();
      this.capacity = parseInt(value.toString().substring(0, 2));
    }
    if (value < 1) {
      event.preventDefault();
      this.capacity = 0;
    }
  }
}
