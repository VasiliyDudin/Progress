import type {
  IShipDto,
  ICoordinateSimple,
  EShootStatus,
} from "@/models/dto.model";
import { defineStore } from "pinia";
import { filter, switchMap, take, tap, map } from "rxjs";
import { wsWorker } from "../services/wsWorker.service";

export enum EStatusGame {
  Init = "Init",
  Find = "Find",
  Game = "Game",
}
export interface IcoordinateShoot extends ICoordinateSimple {
  status: EShootStatus;
}
export const gameStore = defineStore("game", {
  state() {
    return {
      /** статус игры */
      gameStatus: EStatusGame.Init,

      /** ID противника */
      enemyGamerId: null as string,

      /** ID игрока текущего хода */
      shootGamerId: null as string,

      /** история выстрелов по полю противника (ходы текущего) */
      enemyShootHistory: [] as Array<IcoordinateShoot>,

      /** история выстрелов по полю текущего игрока (ходы соперника) */
      myShootHistory: [] as Array<IcoordinateShoot>,
    };
  },
  actions: {
    connected() {},
    gameStart(sheeps: Array<IShipDto>) {
      wsWorker
        .getAnswer$<boolean>(wsWorker.sendMsg("GameStart", sheeps))
        .pipe(
          filter((result) => result),
          tap(() => (this.gameStatus = EStatusGame.Find)),
          switchMap(() =>
            wsWorker.startGame$.pipe(
              take(1),
              tap((result) => {
                this.enemyGamerId = result.otherGamerConnectionId;
                this.shootGamerId = result.shootGamerConnectionId;
                this.gameStatus = EStatusGame.Game;
              })
            )
          ),
          switchMap(() => wsWorker.resultShoot$),
          map((msg) => msg.payload)
        )
        .subscribe((result) => {
          this.shootGamerId = result.nextGamerShooterConnectionId;
          if (result.targetGamerConnectionId === this.enemyGamerId) {
            this.enemyShootHistory.push({
              ...result.coordinate,
              status: result.shootStatus,
            });
          }
          if (result.targetGamerConnectionId === wsWorker.connectionId$.value) {
            this.myShootHistory.push({
              ...result.coordinate,
              status: result.shootStatus,
            });
          }
        });
    },
    /** отправить выстрел */
    sendShoot(coordinate: ICoordinateSimple) {
      wsWorker.sendMsg("Shoot", coordinate);
    },
  },
  getters: {
    /** текущий ID коннекта  */
    connectionId$() {
      return wsWorker.connectionId$;
    },

    /** проверка что выстрел текущего игрока */
    isMyShoot(state) {
      return wsWorker.connectionId$.value === state.shootGamerId;
    },
    /** получить ход соперника */
    getEnemyShootInHistory(state) {
      return (coordinate: ICoordinateSimple) => {
        return state.myShootHistory.find(
          (c) => c.x === coordinate.x && c.y === coordinate.y
        );
      };
    },
    /** получить ход текущего игрока */
    getMyShootInHistory(state) {
      return (coordinate: ICoordinateSimple) => {
        return state.enemyShootHistory.find(
          (c) => c.x === coordinate.x && c.y === coordinate.y
        );
      };
    },
  },
});
