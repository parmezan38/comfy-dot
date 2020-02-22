import { Component, Input } from '@angular/core';
import { Store } from '@ngxs/store';
import { ActivatedRoute } from '@angular/router';

import { SignalRService } from '../services/signalR.service';
import { IChatMessage, IUser, IRoom } from '../shared/interfaces';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})

export class ChatComponent {
  @Input() room: IRoom;
  user: IUser;
  message: string = '';
  messages: IChatMessage[] = [];

  constructor(private route: ActivatedRoute, private signalR: SignalRService, private auth: AuthService, private store: Store) {
    this.auth.getUserProfile()
      .then(user => this.user = user)
      .then(() => {
        this.route.params.subscribe(params => {
          const id = parseInt(params['id'], 10);
          this.setRoom(id);
        });
      })
  }

  private setRoom(id: number) {
    const rooms = this.store.select(state => state.lobby.rooms);
    rooms.subscribe(it => {
      const room = it.find(it => it.id === id)
      this.room = room;
      this.setupSignalR();
    });
  }

  private setupSignalR() {
    this.signalR.startConnection(this.user.id, this.room);
    this.signalR.connection.on('Chat', (msg: IChatMessage) => {
      this.messages.push(msg);
    });
  }

  public send() {
    const msg: IChatMessage = {
      roomId: this.room.id,
      user: this.user,
      message: this.message
    };
    this.message = '';
    this.signalR.connection.invoke('Chat', msg);
  }

  public keyPress(event) {
    console.log(event);
    if (event.key === 'Enter' && event.shiftKey === false) {
      this.send();
    }
  }
}
