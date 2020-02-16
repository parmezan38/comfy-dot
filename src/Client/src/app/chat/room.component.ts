import { Store } from '@ngxs/store';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

import { IRoom } from '../shared/interfaces';
import { ApiService } from '../shared/api.service';

@Component({
  selector: 'room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent {
  @Input() room: IRoom;
  
  constructor(private router: Router, private api: ApiService, private store: Store) { }

  public join() {
    this.router.navigate(['/room', this.room.id], { state: { name: this.room.name } });
  }

  public delete() {
    this.api.deleteRoom(this.room.id).subscribe((res: any) => {
      console.log(res); //TODO: with snackbar
    });
  }
}
