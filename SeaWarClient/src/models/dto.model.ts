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
/** статусы попадания */
export const SHOOT_TARGET_STATUSES = [
  EShootStatus.Hit,
  EShootStatus.Killing,
  EShootStatus.KillingAll,
];

export interface ShootResultDto {
  shootStatus: EShootStatus;
  sourceGamerConnectionId: string;
  targetGamerConnectionId: string;
  nextGamerShooterConnectionId: string;
  coordinate: ICoordinateSimple;
}
