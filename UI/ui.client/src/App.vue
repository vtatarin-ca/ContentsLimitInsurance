<script setup lang="ts">
import ItemList from './components/ItemList.vue'
</script>

<template>
  <div>
    <div class="header">
      <img alt="Nude Solutions logo" class="logo" src="./assets/logo.svg" />
    </div>

    <div class="main">
      <ItemList />
    </div>
  </div>
</template>

<style scoped>
  .header {
    margin: auto;
    display: flex;
    align-items: center;
    justify-content: center;
  }

</style>
