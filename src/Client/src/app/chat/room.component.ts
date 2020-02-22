import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

import { IRoom, IResponseMessage } from '../shared/interfaces';
import { ApiService } from '../services/api.service';
import { SnackbarService } from '../services/snackbar.service';

@Component({
  selector: 'room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent {
  @Input() room: IRoom;

  constructor(private router: Router, private api: ApiService, private snackbar: SnackbarService) { }

  public join() {
    this.router.navigate(['/room', this.room.id], { state: { name: this.room.name } });
  }

  public delete() {
    this.api.deleteRoom(this.room.id)
      .subscribe((res: IResponseMessage) => this.snackbar.show(res));
  }
}
