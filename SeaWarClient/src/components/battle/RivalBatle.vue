<template>
    <table class="battle-table">
        <tr v-for="rowIndex in verticarrArr" :key="rowIndex">
            <td v-for="colIndex in horizontalArr" :key="colIndex"></td>
        </tr>
    </table>
</template>
  
<script  lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    name: 'RivalBatle',
    props: {
        verticalColumn: Number,
        horizontalColumn: Number
    },
    computed: {
        verticarrArr() {
            return Array.from(Array(this.verticalColumn).keys());
        },
        horizontalArr() {
            return Array.from(Array(this.horizontalColumn).keys());
        },
    }
});</script>
  
<style lang="less" scoped>
@import './battle.less';
</style>
  