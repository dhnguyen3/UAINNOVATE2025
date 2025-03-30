<template>
  <div id="app">
    <header class="app-header">
      <h1>HCA Healthcare Data De-identifier</h1>
      <p>UR INNOVATE Hackathon Submission</p>
    </header>
    <main>
      <router-view />
    </main>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style>
#app {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}

.app-header {
  background: #005b94;
  color: white;
  padding: 1.5rem;
  text-align: center;
  margin-bottom: 2rem;
  border-radius: 0 0 8px 8px;
}

.app-header h1 {
  margin: 0;
  font-size: 1.8rem;
}

.app-header p {
  margin: 0.5rem 0 0;
  opacity: 0.9;
  font-size: 1rem;
}
</style>