import {
  type IShipDto,
  type ICoordinateSimple,
  EShootStatus,
} from "@/models/dto.model";
import { defineStore } from "pinia";
import { filter, switchMap, take, tap, map } from "rxjs";
import { wsWorker } from "./wsWorker.service";

export enum EStatusGame {
  Init = "Init",
  Find = "Find",
  Game = "Game",
}

export const gameStore = defineStore("game", {
  state() {
    return {
      gameStatus: EStatusGame.Init,
      otherGamerId: null as string,
      shootGamerId: null as string,
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
                this.otherGamerId = result.otherGamerConnectionId;
                this.shootGamerId = result.shootGamerConnectionId;
                this.gameStatus = EStatusGame.Game;
              })
            )
          ),
          switchMap(() => wsWorker.resultShoot$),
          map((msg) => msg.payload)
        )
        .subscribe((result) => {
          if (
            [
              EShootStatus.Hit,
              EShootStatus.Killing,
              EShootStatus.KillingAll,
            ].includes(result.shootStatus)
          ) {
            this.shootGamerId = result.sourceGamerConnectionId;
            return;
          }
          this.shootGamerId = result.targetGamerConnectionId;
        });
    },
    sendShoot(coordinate: ICoordinateSimple) {
      wsWorker.sendMsg("Shoot", coordinate);
    },
  },
  getters: {
    connectionId() {
      return wsWorker.connectionId$.value;
    },
    isShoot(state) {
      return wsWorker.connectionId$.value === state.shootGamerId;
    },
  },
});
