import { Injectable } from '@angular/core';
import { State, Action, StateContext, Selector } from '@ngxs/store';
import { IRoom } from '../shared/interfaces';
import { AddRooms, UpdateRoom, AddCurrentRoom, RemoveCurrentRoom } from './lobby.actions';

export class LobbyStateModel {
  rooms: IRoom[];
  currentRoom: IRoom;
}

@State<LobbyStateModel>({ name: 'lobby', defaults: { rooms: [], currentRoom: null } })
@Injectable()
export class LobbyState {
  @Selector()
  static getRooms(state: LobbyStateModel) {
    return state.rooms;
  }

  @Action(AddRooms)
  addRooms({ patchState, getState }: StateContext<LobbyStateModel>, { payload }: AddRooms) {
    const payloadRooms: IRoom[] = payload;
    let stateRooms = getState().rooms;
    stateRooms = stateRooms.filter(a => {
      const exists = payloadRooms.filter((b, i) => {
        if (b.id === a.id) {
          payloadRooms.splice(i, 1);
          return true;
        }
        return false;
      })[0];
      return exists;
    });
    let newRooms: IRoom[] = [...stateRooms, ...payloadRooms];
    newRooms = newRooms.map(it => {
      it.numberOfUsers = it.numberOfUsers || 0;
      return it;
    });
    patchState({ rooms: newRooms });
  }

  @Action(UpdateRoom)
  updateRoom({ patchState, getState }: StateContext<LobbyStateModel>, { payload }: UpdateRoom) {
    const rooms = getState().rooms;
    const index = rooms.findIndex(it => it.id === payload.id);
    rooms[index].numberOfUsers = payload.numberOfUsers;
    patchState({ rooms });
  }

  @Action(AddCurrentRoom)
  addCurrentRoom({ patchState }: StateContext<LobbyStateModel>, { payload }: AddCurrentRoom) {
    payload.numberOfUsers = payload.numberOfUsers < payload.capacity ? payload.numberOfUsers + 1 : payload.numberOfUsers;
    patchState({ currentRoom: payload });
  }

  @Action(RemoveCurrentRoom)
  removeCurrentRoom({ patchState }: StateContext<LobbyStateModel>) {
    patchState({ currentRoom: null });
  }
}