<template>
  <table class="battle-table">
    <tr v-for="rowIndex in verticarrArr" :key="rowIndex">
      <td
        v-for="colIndex in horizontalArr"
        :key="colIndex"
        @click="click(rowIndex, colIndex)"
      ></td>
    </tr>
  </table>
</template>

<script lang="ts">
import type { ICoordinateSimple } from "@/models/dto.model";
import { defineComponent } from "vue";

export default defineComponent({
  name: "RivalBatle",
  props: {
    verticalColumn: Number,
    horizontalColumn: Number,
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
  },
  methods: {
    click(row: number, col: number) {
      this.$emit("click", { x: col, y: row });
    },
  },
});
</script>

<style lang="less" scoped>
@import "./battle.less";
</style>
