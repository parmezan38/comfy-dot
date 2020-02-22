export interface IUser {
  id: string;
  name: string;
  color1: string;
  color2: string;
}

export interface IRoomRegister {
  name: string;
  capacity: number;
  userId: string;
}

export interface IRoom {
  id: number;
  name: string;
  capacity: number;
  numberOfUsers: number;
  owned: boolean;
}

export interface IChatMessage {
  roomId: number;
  user: IUser;
  message: string;
}

export interface IResponseMessage {
  type: string,
  message: string
}
