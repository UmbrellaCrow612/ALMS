import Registration from '../pages/Registration.vue'
import Landing from '@/views/Landing.vue'
import { createRouter, createWebHistory } from 'vue-router'



const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: Landing
    },
    {
      path: '/registration',
      name: 'registration',
      component: Registration
    },
  ]
})

export default router
