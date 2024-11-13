<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useUserStore } from '@/stores/userStore';

const userStore = useUserStore(); 


onMounted(() => {

  const token = localStorage.getItem('accessToken');
  
  if (token) {
    userStore.setUser(token);
  } else {
    userStore.clearUser(); 
  }

  console.log('User Store:', userStore.user);
});
</script>

<template>

  <RouterView />
</template>

<style scoped>

</style>
