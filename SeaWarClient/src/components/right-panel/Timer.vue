<template>
    <div class="border v-100 h-100 timer">
        {{ secundsOut }}
    </div>
</template>
  
<script  lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    name: 'Chat',
    props: {
        secundsOut: Number
    }
});</script>
  
<style lang="less" scoped>
.timer {
    font-size: 30px;
    display: flex;
    justify-content: center;
    align-items: center;
}
</style>
  