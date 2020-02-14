import { IRoom } from '../shared/interfaces';

export class AddRooms {
  static readonly type = '[LOBBY] Add';

  constructor(public payload: IRoom[]) {}
}

export class UpdateRoom {
  static readonly type = '[LOBBY] Update';

  constructor(public payload: IRoom) {}
}

export class AddCurrentRoom {
  static readonly type = '[LOBBY] AddCurrentRoom';

  constructor(public payload: IRoom) {}
}

export class RemoveCurrentRoom {
  static readonly type = '[LOBBY] RemoveCurrentRoom';

  constructor() {}
}