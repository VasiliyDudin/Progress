import {
  HORIZONTAL_AREA_LENGTH,
  SAFE_DELTA_AREA,
  VERTICAL_AREA_LENGTH,
} from "./consts";
import type { ICoordinateSimple, IShipDto } from "./dto.model";

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

export enum EShipStatus {
  None = "None",
  Game = "Game",
  Move = "Move",
}

export interface IShip {
  /**
   * точки от "базовой", задают форму корабля
   */
  deltaCoordinates: Array<ICoordinateSimple>;

  /**
   * все точки корабля
   */
  coordinates: Array<ICoordinate>;
  /**
   * перемещаем на текущий момент
   */
  status: EShipStatus;

  /**
   * Проверка вхождения координаты в корабль
   * @param coordinate
   */
  isExist(coordinate: ICoordinateSimple): boolean;

  /**
   * корабль можно переместить в указанные координаты
   * @param coordinates
   * @param shepps
   */
  canMove(coordinates: ICoordinateSimple, shepps: Array<IShip>): boolean;

  /**
   * задать новые координаты кораблю
   * @param coordinates
   */
  setNewCoordinate(coordinates: ICoordinateSimple): void;

  /**
   * зона безопасности (нельзя размещать другой корабль)
   * @param coordinates
   */
  isSafeArea(coordinates: ICoordinateSimple): boolean;

  /**
   * проверка что корабль передвигаем
   */
  isMove(): boolean;

  setStatus(newStatus: EShipStatus): void;

  toDto(): IShipDto;
}

abstract class AShape implements IShip {
  status = EShipStatus.None;
  public coordinates: Array<Coordinate>;
  /**
   *
   * @param deltaCoordinates  точки от "базовой", задают форму корабля
   * @param baseCoordinate базовая координата
   */
  constructor(
    public deltaCoordinates: Array<ICoordinateSimple>,
    baseCoordinate: ICoordinateSimple
  ) {
    this.setNewCoordinate(baseCoordinate);
  }

  isSafeArea(coordinates: ICoordinateSimple): boolean {
    return this.coordinates.some((c) => c.isSafeArea(coordinates));
  }

  isExist(coordinates: ICoordinateSimple): boolean {
    return this.coordinates.some((c) => c.isEqual(coordinates));
  }

  canMove(coordinates: ICoordinateSimple, shepps: IShip[]) {
    const futureCoordinate = this.getNewCoordinate(coordinates);
    return (
      !shepps
        .filter((s) => !s.isMove())
        .some((s) => futureCoordinate.some((c) => s.isSafeArea(c))) &&
      futureCoordinate.reduce(
        (result, c) => result && this.isValidCoordinate(c),
        true
      )
    );
  }

  setNewCoordinate(coordinates: ICoordinateSimple) {
    this.coordinates = this.getNewCoordinate(coordinates);
  }

  isMove() {
    return this.status === EShipStatus.Move;
  }

  setStatus(newStatus: EShipStatus): void {
    this.status = newStatus;
  }

  toDto(): IShipDto {
    return {
      coordinates: this.coordinates.map((c) => ({
        x: c.x,
        y: c.y,
      })),
    };
  }

  private getNewCoordinate(coordinates: ICoordinateSimple) {
    return this.deltaCoordinates.map(
      (c) => new Coordinate(coordinates.x + c.x, coordinates.y + c.y)
    );
  }

  private isValidCoordinate(coordinates: ICoordinateSimple) {
    return (
      coordinates.x >= 0 &&
      coordinates.x < HORIZONTAL_AREA_LENGTH &&
      coordinates.y >= 0 &&
      coordinates.y < VERTICAL_AREA_LENGTH
    );
  }
}

export class ShipOne extends AShape {
  constructor(startCoordinate: Coordinate) {
    super([{ x: 0, y: 0 }], startCoordinate);
  }
}

export class ShipTwo extends AShape {
  constructor(startCoordinate: Coordinate) {
    super(
      [
        { x: 0, y: 0 },
        { x: 1, y: 0 },
      ],
      startCoordinate
    );
  }
}

export class ShipThreeTypeOne extends AShape {
  constructor(startCoordinate: Coordinate) {
    super(
      [
        { x: 0, y: 0 },
        { x: 1, y: 0 },
        { x: 1, y: 1 },
      ],
      startCoordinate
    );
  }
}
export class ShipThreeTypeTwo extends AShape {
  constructor(startCoordinate: Coordinate) {
    super(
      [
        { x: 0, y: 0 },
        { x: -1, y: 0 },
        { x: -1, y: -1 },
      ],
      startCoordinate
    );
  }
}

export class ShipFor extends AShape {
  constructor(startCoordinate: Coordinate) {
    super(
      [
        { x: 0, y: 0 },
        { x: 0, y: -1 },
        { x: 0, y: -2 },
        { x: 1, y: -2 },
      ],
      startCoordinate
    );
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
  new ShipTwo(new Coordinate(4, 2)),
  new ShipThreeTypeOne(new Coordinate(0, 4)),
  new ShipThreeTypeTwo(new Coordinate(0, 4)),
  new ShipFor(new Coordinate(0, 4)),
];
