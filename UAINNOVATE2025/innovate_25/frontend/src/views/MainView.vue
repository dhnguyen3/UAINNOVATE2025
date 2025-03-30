<template>
  <div class="main-view">
    <div class="container">
      <MessageViewer />
    </div>
  </div>
</template>

<script>
import MessageViewer from '@/components/MessageViewer.vue'

export default {
  components: { MessageViewer }
}
</script>

<style scoped>
.main-view {
  padding: 2rem 0;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}
</style>