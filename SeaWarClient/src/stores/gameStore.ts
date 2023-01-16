import { defineStore } from "pinia";
import {
  HubConnectionBuilder,
  HttpTransportType,
  HubConnection,
} from "@microsoft/signalr";
import type { IMessageDto, IShipDto } from "@/models/dto.model";
import { v4 as uuidv4 } from "uuid";
import { filter, map, Subject, take } from "rxjs";

class WsWorker {
  reciveMsg$ = new Subject<IMessageDto<any>>();
  connection: HubConnection;

  constructor(url: string) {
    this.connection = new HubConnectionBuilder()
      .withUrl(url, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      })
      .build();

    this.connection.start().then(() => {
      console.log("соединение по WS установленно");
      this.subscribeMsg();
    });
    this.reciveMsg$.subscribe((msg) => {
      console.log("от сервера: ", msg);
    });
  }

  sendMsg<T>(commandName: string, payload: T) {
    const msgUid = uuidv4();
    this.connection.invoke(commandName, <IMessageDto<T>>{
      uid: msgUid,
      payload,
    });
    return msgUid;
  }

  getAnswer$<T>(msgUid: string) {
    return this.reciveMsg$.pipe(
      filter((payload) => payload.uid === msgUid),
      take(1),
      map((payload) => payload.payload as T)
    );
  }

  private subscribeMsg() {
    this.connection.on("Responce", (data: IMessageDto<any>) => {
      this.reciveMsg$.next(data);
    });
  }
}

const wsWorker = new WsWorker("http://localhost:5127/gameHub");

export enum EStatusGame {
  Init = "Init",
  Find = "Find",
  Game = "Game",
}

export const gameStore = defineStore("game", {
  state() {
    return {
      gameStatus: EStatusGame.Init,
    };
  },
  actions: {
    connected() {},
    gameStart(sheeps: Array<IShipDto>) {
      wsWorker
        .getAnswer$<boolean>(wsWorker.sendMsg("GameStart", sheeps))
        .subscribe((result) => {
          if (result) {
            this.gameStatus = EStatusGame.Find;
          }
        });
    },
  },
});
