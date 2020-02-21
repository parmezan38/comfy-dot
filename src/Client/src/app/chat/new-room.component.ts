import { Component } from '@angular/core';

import { IUser, IRoomRegister } from '../shared/interfaces';
import { ApiService } from '../services/api.service';
import { AuthService } from '../services/auth.service';


@Component({
  selector: 'new-room',
  templateUrl: './new-room.component.html',
  styleUrls: ['./new-room.component.html']
})
export class NewRoomComponent {
  user: IUser;
  public name: string;
  public capacity: number;

  constructor(private auth: AuthService, private api: ApiService) { }

  ngOnInit() {
    this.auth.getUserProfile()
    .then(user => this.user = user);
  }

  submit() {
    if (!this.user) return;
    const room: IRoomRegister = {
      name: this.name,
      capacity: this.capacity,
      userId: this.user.id
    }
    this.api.createRoom(room).subscribe(res => {
      console.log(res); // TODO: snackbar
    });
  }
}
