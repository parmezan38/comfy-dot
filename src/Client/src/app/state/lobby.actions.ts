import { IRoom } from '../shared/interfaces';

export class AddRooms {
  static readonly type = '[LOBBY] Add';

  constructor(public payload: IRoom[]) { }
}

export class UpdateRoom {
  static readonly type = '[LOBBY] Update';

  constructor(public payload: IRoom) { }
}

export class ResetRooms {
  static readonly type = '[LOBBY] Reset';

  constructor() { }
}
