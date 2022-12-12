import { SAFE_DELTA_AREA } from "./consts";

export interface ICoordinateSimple {
  x: number;
  y: number;
}
export interface ICoordinate extends ICoordinateSimple {
  x: number;
  y: number;
  isEqual(coordinate: ICoordinateSimple): boolean;
}

export class CoordinateSimpleEqual implements ICoordinate {
  constructor(public x: number, public y: number) {}
  isEqual(coordinate: ICoordinateSimple): boolean {
    return coordinate.x == this.x && coordinate.y === this.y;
  }
}

export class Coordinate extends CoordinateSimpleEqual implements ICoordinate {
  private safeArea: Array<ICoordinate> = [];

  constructor(x: number, y: number) {
    super(x, y);
    this.recalcSafeArea();
  }

  isSafeArea(coordinate: ICoordinateSimple) {
    return this.safeArea.some((s) => s.isEqual(coordinate));
  }

  private recalcSafeArea() {
    this.safeArea = SAFE_DELTA_AREA.map(
      (s) => new CoordinateSimpleEqual(this.x + s.x, this.y + s.y)
    );
  }
}

export interface IShip {
  length: number;
  coordinates: Array<ICoordinate>;
  isMove: boolean;
  isExist(coordinate: ICoordinateSimple): boolean;
  canMove(coordinates: ICoordinateSimple, shepps: Array<IShip>): boolean;
  setNewCoordinate(coordinates: ICoordinateSimple): void;
  isSafeArea(coordinates: ICoordinateSimple): boolean;
}

abstract class AShape implements IShip {
  isMove = false;
  constructor(public length: number, public coordinates: Array<Coordinate>) {}

  isSafeArea(coordinates: ICoordinateSimple): boolean {
    return this.coordinates.some((c) => c.isSafeArea(coordinates));
  }

  isExist(coordinates: ICoordinateSimple): boolean {
    return this.coordinates.some((c) => c.isEqual(coordinates));
  }
  abstract canMove(coordinates: ICoordinateSimple, shepps: IShip[]): boolean;
  abstract setNewCoordinate(coordinates: ICoordinateSimple): void;
}

export class ShipOne extends AShape {
  constructor(startCoordinate: Coordinate) {
    super(1, [startCoordinate]);
  }
  canMove(coordinates: ICoordinateSimple, shepps: IShip[]): boolean {
    return !shepps
      .filter((s) => !s.isMove)
      .some((s) => s.isSafeArea(coordinates));
  }
  setNewCoordinate(coordinates: ICoordinateSimple): void {
    this.coordinates = [new Coordinate(coordinates.x, coordinates.y)];
  }
}

export class ShipTwo extends AShape {
  constructor(startCoordinate: Coordinate) {
    super(2, [
      startCoordinate,
      new Coordinate(startCoordinate.x + 1, startCoordinate.y),
    ]);
  }
  canMove(coordinates: ICoordinateSimple, shepps: IShip[]): boolean {
    return !shepps
      .filter((s) => !s.isMove)
      .some((s) =>
        [
          new Coordinate(coordinates.x, coordinates.y),
          new Coordinate(coordinates.x + 1, coordinates.y),
        ].some((c) => s.isSafeArea(c))
      );
  }
  setNewCoordinate(coordinates: ICoordinateSimple): void {
    this.coordinates = [
      new Coordinate(coordinates.x, coordinates.y),
      new Coordinate(coordinates.x + 1, coordinates.y),
    ];
  }
}

export const SHIPS: Array<IShip> = [
  new ShipOne(new Coordinate(0, 0)),
  new ShipOne(new Coordinate(2, 0)),
  new ShipOne(new Coordinate(4, 0)),
  new ShipOne(new Coordinate(6, 0)),
  new ShipOne(new Coordinate(8, 0)),
  new ShipTwo(new Coordinate(0, 2)),
  new ShipTwo(new Coordinate(4, 2)),
];
