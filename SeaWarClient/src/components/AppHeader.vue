<template>
    <div class="container-fluid h-100">
        <div class="row justify-content-between h-100">
            <div class="col-md-3 col-5 text-center field border">SeaWar</div>
            <div class="col-md-3 col-5 text-center field border">UserNme</div>
        </div>
    </div>
</template>
  
<script  lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    name: 'AppHeader'
});</script>
  
<style lang="less" scoped>
.field {
    display: flex;
    align-items: center;
    justify-content: center;
}
</style>
  