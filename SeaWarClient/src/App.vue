<template>
  <div class="main h-100 w-100">
    <div class="header">
      <AppHeader></AppHeader>
    </div>
    <div class="content">
      <div class="left-panel h-100 border">
        <div class="left-panel-info">
          <Info> </Info>
        </div>
        <div class="chat">
          <div class="accordion" id="accordionExample">
            <div class="accordion-item">
              <h2 class="accordion-header">
                <button
                  class="accordion-button"
                  type="button"
                  data-bs-toggle="collapse"
                  data-bs-target="#collapseOne"
                  aria-expanded="true"
                  aria-controls="collapseOne"
                >
                  Чат
                </button>
              </h2>
            </div>
            <div
              id="collapseOne"
              class="accordion-collapse collapse show"
              aria-labelledby="headingOne"
              data-bs-parent="#accordionExample"
            >
              <div class="accordion-body">
                <Chat></Chat>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="game-area h-100 border">
        <UserBattle
          :horizontal-column="HORIZONTAL_AREA_LENGTH"
          :vertical-column="VERTICAL_AREA_LENGTH"
          :ships="ships"
        >
        </UserBattle>
        <div class="rival d-flex justify-content-center align-items-center">
          <RivalBatle
            v-if="gameStore.gameStatus === EStatusGame.Game"
            :horizontal-column="HORIZONTAL_AREA_LENGTH"
            :vertical-column="VERTICAL_AREA_LENGTH"
          >
          </RivalBatle>
          <button
            v-if="gameStore.gameStatus === EStatusGame.Init"
            type="button"
            class="btn btn-primary"
            @click="startGame()"
          >
            Начать игру
          </button>
          <div
            v-if="gameStore.gameStatus === EStatusGame.Find"
            class="spinner-border"
            role="status"
          >
            <span class="visually-hidden">Loading...</span>
          </div>
        </div>
      </div>
      <div class="right-panel h-100 border">
        <div class="timer">
          <Timer :secunds-out="seconds"></Timer>
        </div>
        <div class="statistics">
          <Statistics></Statistics>
        </div>
        <div class="timer">
          <Timer :secunds-out="seconds"></Timer>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import AppHeader from "./components/AppHeader.vue";
import Info from "./components/left-panel/Info.vue";
import Chat from "./components/Chat.vue";
import Timer from "./components/right-panel/Timer.vue";
import Statistics from "./components/right-panel/Statistics.vue";
import UserBattle from "./components/battle/UserBattle.vue";
import RivalBatle from "./components/battle/RivalBatle.vue";
import { SHIPS, type IShip } from "./models/IShip.model";
import { HORIZONTAL_AREA_LENGTH, VERTICAL_AREA_LENGTH } from "./models/consts";
import { ShipHelper } from "./services/ship.helper";
import { gameStore, EStatusGame } from "./stores/gameStore";
import { mapStores } from "pinia";

const timeMock = 30;
export default defineComponent({
  components: {
    AppHeader,
    Info,
    Chat,
    Timer,
    Statistics,
    UserBattle,
    RivalBatle,
  },
  setup() {
    return {
      HORIZONTAL_AREA_LENGTH,
      VERTICAL_AREA_LENGTH,
      shipHelper: new ShipHelper(
        SHIPS,
        HORIZONTAL_AREA_LENGTH,
        VERTICAL_AREA_LENGTH
      ),
      EStatusGame,
    };
  },
  data() {
    return {
      ships: [] as Array<IShip>,
      seconds: timeMock,
    };
  },
  computed: {
    ...mapStores(gameStore),
  },
  mounted() {
    this.setRandomShips();
    setInterval(() => {
      --this.seconds;
      if (this.seconds <= 0) {
        this.seconds = timeMock;
      }
    }, 1000);
  },
  methods: {
    setRandomShips() {
      this.ships = this.shipHelper.setRandomCoordinate();
    },
    startGame() {
      this.gameStore.gameStart(this.ships.map((s) => s.toDto()));
    },
  },
});
</script>

<style lang="less" scoped>
.main {
  display: flex;
  flex-direction: column;

  .header {
    height: 80px;
    border: 1px solid black;
  }

  .content {
    display: flex;
    flex-grow: 1;

    .left-panel {
      width: 20%;
      display: flex;
      flex-direction: column;
      justify-content: space-between;

      .left-panel-info {
        height: 20%;
      }

      .chat {
        height: 60%;
      }
    }

    .right-panel {
      width: 20%;
      display: flex;
      flex-direction: column;
      justify-content: center;

      .statistics {
        height: 70%;
      }

      .timer {
        height: 10%;
      }
    }

    .game-area {
      flex-grow: 1;
      display: flex;
      align-items: center;
      justify-content: space-around;
      overflow: auto;
    }

    .rival {
      width: 400px;
      height: 400px;
    }
  }
}
</style>
