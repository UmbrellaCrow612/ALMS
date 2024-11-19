import { createRouter, createWebHistory } from 'vue-router'
import LandingView from '@/views/LandingView.vue'
import RegistrationView from '@/views/RegistrationView.vue'
import LoginView from '@/views/LoginView.vue'
import SearchView from '@/views/SearchView.vue'
import MediaDetail from '@/views/MediaDetail.vue'
import ApprovalView from '@/views/ApprovalView.vue'
import InventoryView from '@/views/InventoryView.vue'


const isAuthenticated = () => {
  
  return localStorage.getItem('authToken') !== null; 
}

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
      component: RegistrationView,
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
    },
    {
      path: '/search',
      name: 'search',
      component: SearchView,
      meta: { requiresAuth: true }, 
    },
    {
      path: '/media/:id',
      name: 'mediaDetail',
      component: MediaDetail,
      props: true,
      meta: { requiresAuth: true }, 
    },
    {
      path: '/approval',
      name: 'approval',
      component: ApprovalView,
      props: true,
      meta: { requiresAuth: true }, 
    },
    {
      path: '/inventory',
      name: 'inventory',
      component: InventoryView,
      props: true,
      meta: { requiresAuth: true }, 
    },
  ],
})


router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth && !isAuthenticated()) {
    next({ name: 'login' }) 
  } else {
    next() 
  }
})

export default router
