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
