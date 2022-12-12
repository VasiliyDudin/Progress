<template>
  <table class="battle-table">
    <tr v-for="rowIndex in verticarrArr" :key="rowIndex">
      <td
        v-for="colIndex in horizontalArr"
        :key="colIndex"
        v-bind:class="{
          ship: isSheep({ x: colIndex, y: rowIndex }),
          move: isMove({ x: colIndex, y: rowIndex }),
        }"
        @click="startMoveSheep({ x: colIndex, y: rowIndex })"
        @mousemove="toMoveShip({ x: colIndex, y: rowIndex })"
      ></td>
    </tr>
  </table>
</template>

<script lang="ts">
import {
  type ICoordinateSimple,
  type IShip,
  CoordinateSimpleEqual,
} from "@/models/IShip.model";
import { defineComponent, type PropType } from "vue";

export default defineComponent({
  name: "UserBattle",
  props: {
    verticalColumn: Number,
    horizontalColumn: Number,
    ships: Object as PropType<Array<IShip>>,
  },
  setup() {
    return {
      lastMoveCoordinate: null as CoordinateSimpleEqual,
    };
  },
  data() {
    return {
      movedShip: null as IShip,
    };
  },
  computed: {
    verticarrArr() {
      return Array.from(Array(this.verticalColumn).keys());
    },
    horizontalArr() {
      return Array.from(Array(this.horizontalColumn).keys());
    },
  },
  mounted() {
    // window.addEventListener("mouseup", () => {
    //   this.ships.forEach((s) => (s.isMove = false));
    //   this.movedShip = null;
    //   this.lastMoveCoordinate = new CoordinateSimpleEqual(-1, -1);
    //   this.$forceUpdate();
    // });
  },
  methods: {
    isSheep(coordinate: ICoordinateSimple) {
      return !!this.getSheepByCoordiante(coordinate);
    },
    isMove(coordinate: ICoordinateSimple) {
      const sheep = this.getSheepByCoordiante(coordinate);
      return sheep?.isMove;
    },
    startMoveSheep(coordinate: ICoordinateSimple) {
      const ship = this.getSheepByCoordiante(coordinate);
      this.ships.filter((s) => s !== ship).forEach((s) => (s.isMove = false));
      //   const ship = this.getSheepByCoordiante(coordinate);
      if (ship) {
        ship.isMove = !ship.isMove;
      }
      this.movedShip = ship?.isMove ? ship : null;
      this.$forceUpdate();
    },
    toMoveShip(coordinate: ICoordinateSimple) {
      if (!this.movedShip || this.lastMoveCoordinate?.isEqual(coordinate)) {
        return;
      }
      this.lastMoveCoordinate = new CoordinateSimpleEqual(
        coordinate.x,
        coordinate.y
      );
      if (this.movedShip.canMove(coordinate, this.ships)) {
        this.movedShip.setNewCoordinate(coordinate);
        this.$forceUpdate();
      }
    },
    getSheepByCoordiante(coordinate: ICoordinateSimple) {
      return this.ships?.find((s) => s.isExist(coordinate));
    },
  },
});
</script>

<style lang="less" scoped>
@import "./battle.less";
</style>
