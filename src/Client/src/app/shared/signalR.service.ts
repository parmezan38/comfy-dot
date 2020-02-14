import { Store } from '@ngxs/store';
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

import { UpdateRoom } from '../state/lobby.actions';
import { IRoom } from './interfaces';

@Injectable()
export class SignalRService {
  public connection: signalR.HubConnection

  constructor(private store: Store) {}

  public startConnection = async (userId: string, room?: IRoom) => {
    this.connection = new signalR.HubConnectionBuilder()
      // TODO: Change to https when changed on server
      .withUrl('http://localhost:5000/chathub')
      .build();

    this.connection.on('UpdateRoom', (room: IRoom) => {
      this.store.dispatch(new UpdateRoom(room));
    });
    
    await this.connection
      .start()
      .then(() => {console.log('Connection started')})
      .catch(err => console.log('Error while starting connection: ' + err))
    
    if (room) {
      this.connection.invoke('Join', { userId, room });
    } else {
      this.connection.invoke('Leave', userId);
    }
  }
}
