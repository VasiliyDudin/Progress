<template>
  <table class="battle-table">
    <tr v-for="rowIndex in verticarrArr" :key="rowIndex">
      <td
        v-for="colIndex in horizontalArr"
        :key="colIndex"
        @click="click(rowIndex, colIndex)"
        :class="{
          shoot: isShoot(statusCoodinante(colIndex, rowIndex)),
          miss: statusCoodinante(colIndex, rowIndex) === EShootStatus.Miss,
        }"
      ></td>
    </tr>
  </table>
</template>

<script lang="ts">
import { EShootStatus, type ICoordinateSimple } from "@/models/dto.model";
import { gameStore } from "@/stores/gameStore";
import { mapStores } from "pinia";
import { defineComponent } from "vue";

export default defineComponent({
  name: "RivalBatle",
  props: {
    verticalColumn: Number,
    horizontalColumn: Number,
  },
  setup() {
    return {
      EShootStatus,
    };
  },
  emits: {
    click: (coordinate: ICoordinateSimple) => true,
  },
  computed: {
    verticarrArr() {
      return Array.from(Array(this.verticalColumn).keys());
    },
    horizontalArr() {
      return Array.from(Array(this.horizontalColumn).keys());
    },
    ...mapStores(gameStore),
  },
  methods: {
    click(row: number, col: number) {
      this.$emit("click", { x: col, y: row });
    },
    statusCoodinante(x: number, y: number) {
      return this.gameStore.otherCombatField.find((c) => c.x === x && c.y === y)
        ?.status;
    },
    isShoot(status: EShootStatus) {
      return [
        EShootStatus.Hit,
        EShootStatus.Killing,
        EShootStatus.KillingAll,
      ].includes(status);
    },
  },
});
</script>

<style lang="less" scoped>
@import "./battle.less";
td {
  &.shoot {
    background-color: red;
  }
  &.miss {
    background-color: blue;
  }
}
</style>
