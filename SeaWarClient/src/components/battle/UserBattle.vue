<template>
  <table class="battle-table">
    <tr v-for="rowIndex in verticarrArr" :key="rowIndex">
      <td v-for="colIndex in horizontalArr" :key="colIndex" v-bind:class="{
        ship: isShip({ x: colIndex, y: rowIndex }),
        move: isMove({ x: colIndex, y: rowIndex }),
      }" @click="startMoveShip({ x: colIndex, y: rowIndex })" @mousemove="toMoveShip({ x: colIndex, y: rowIndex })">
      </td>
    </tr>
  </table>
</template>

<script lang="ts">
import {
  type IShip,
  CoordinateSimpleEqual,
  EShipStatus,
} from "@/models/IShip.model";
import { defineComponent, type PropType } from "vue";

import type{   ICoordinateSimple } from "@/models/dto.model";

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
  methods: {
    isShip(coordinate: ICoordinateSimple) {
      return !!this.getShipByCoordiante(coordinate);
    },
    isMove(coordinate: ICoordinateSimple) {
      const ship = this.getShipByCoordiante(coordinate);
      return ship?.isMove();
    },
    startMoveShip(coordinate: ICoordinateSimple) {
      this.movedShip = null;
      const ship = this.getShipByCoordiante(coordinate);
      this.ships.filter(s => s !== ship).forEach(s => s.setStatus(EShipStatus.None));
      if (ship) {
        ship.setStatus(ship.isMove() ? EShipStatus.None : EShipStatus.Move)
        this.movedShip = ship.isMove() ? ship : null;
      }
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
    getShipByCoordiante(coordinate: ICoordinateSimple) {
      return this.ships?.find((s) => s.isExist(coordinate));
    },
  },
});
</script>

<style lang="less" scoped>
@import "./battle.less";
</style>
