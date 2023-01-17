export interface ICoordinateSimple {
  x: number;
  y: number;
}

export interface IMessageDto<T> {
  uid: string;
  payload: T;
}
export interface IShipDto {
  coordinates: Array<ICoordinateSimple>;
}

export interface IInitGameDto {
  otherGamerConnectionId: string;
  shootGamerConnectionId: string;
}
export enum EShootStatus {
  Error = -1,
  Unknown = 0,
  Miss = 1,
  Hit = 2,
  Killing = 3,
  KillingAll = 4,
}

export interface ShootResultDto {
  shootStatus: EShootStatus;
  sourceGamerConnectionId: string;
  targetGamerConnectionId: string;
}
