import {
  EShipStatus,
  type ICoordinateSimple,
  type IShip,
} from "@/models/IShip.model";

export class ShipHelper {
  constructor(
    private ships: Array<IShip>,
    private horizontalLength: number,
    private verticalLength: number,
    private maxRandomInterations = 50
  ) {}

  setRandomCoordinate() {
    const sortedShips = [...this.ships].sort(
      (a, b) => a.deltaCoordinates.length - b.deltaCoordinates.length
    );

    while (sortedShips.length) {
      const maxedShip = sortedShips.pop();
      maxedShip.setStatus(EShipStatus.Move);
      let countShipSetPoth = 0;
      let randomCoordinate = this.getRandomCoordinate();
      while (!maxedShip.canMove(randomCoordinate, this.ships)) {
        randomCoordinate = this.getRandomCoordinate();
        countShipSetPoth++;
        if (countShipSetPoth > this.maxRandomInterations) {
          throw "Ошибка размещения корабля, превышенно кол-во попыток";
        }
      }
      maxedShip.setNewCoordinate(randomCoordinate);
      maxedShip.setStatus(EShipStatus.None);
    }
    return this.ships;
  }
  private getRandomCoordinate() {
    return <ICoordinateSimple>{
      x: this.getRandomInt(this.horizontalLength),
      y: this.getRandomInt(this.verticalLength),
    };
  }
  private getRandomInt(max: number) {
    return Math.floor(Math.random() * max);
  }
}
