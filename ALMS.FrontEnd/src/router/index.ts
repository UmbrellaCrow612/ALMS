import { createRouter, createWebHistory } from 'vue-router'
import LandingView from '@/views/LandingView.vue'
import RegistrationView from '@/views/RegistrationView.vue'
import LoginView from '@/views/LoginView.vue'
import SearchView from '@/views/SearchView.vue'
import MediaDetail from '@/views/MediaDetail.vue'
import ApprovalView from '@/views/ApprovalView.vue'
import InventoryView from '@/views/InventoryView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: LandingView,
    },
    {
      path: '/registration',
      name: 'registration',
      component: RegistrationView
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/search',
      name: 'search',
      component: SearchView
    },
    {
      path: '/media/:id',
      name: 'mediaDetail',
      component: MediaDetail,
      props: true,  
    },
    {
      path: '/approval',
      name: 'approval',
      component: ApprovalView,
      props: true,  
    },
    {
      path: '/inventory',
      name: 'inventroy',
      component: InventoryView,
      props: true,  
    },
  ]
})

export default router
