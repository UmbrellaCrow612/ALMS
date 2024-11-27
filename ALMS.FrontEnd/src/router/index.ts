import { createRouter, createWebHistory } from 'vue-router'
import LandingView from '@/views/LandingView.vue'
import RegistrationView from '@/views/RegistrationView.vue'
import LoginView from '@/views/LoginView.vue'
import SearchView from '@/views/SearchView.vue'
import MediaDetail from '@/views/MediaDetail.vue'
import ApprovalView from '@/views/ApprovalView.vue'
import InventoryView from '@/views/InventoryView.vue'
import AboutView from '@/views/AboutView.vue'
import SearchUsersView from '@/views/SearchUsersView.vue'
import MyMediaView from '@/views/MyMediaView.vue'
import ReservationView from '@/views/ReservationView.vue'
import UserDetailsView from '@/views/UserDetailsView.vue'
import ForgotPassword from '@/views/ForgotPassword.vue'
import ResetPassword from '@/views/ResetPassword.vue'

const isAuthenticated = () => {
  
  return localStorage.getItem('accessToken') !== null; 
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
    {
      path: '/about',
      name: 'about',
      component: AboutView,
      meta: { requiresAuth: true }, 
    },
    {
      path: '/search-users',
      name: 'searchUsers',
      component: SearchUsersView,
      meta: { requiresAuth: true }, 
    },
    {
      path: '/my-media',
      name: 'myMedia',
      component: MyMediaView,
      meta: { requiresAuth: true }, 
    },
    {
      path: '/reservations',
      name: 'reservations',
      component: ReservationView,
      meta: { 
        requiresAuth: true,
      },
    },
    {
      path: '/users/:id',
      name: 'userDetails',
      component: UserDetailsView,
      props: true,
      meta: { requiresAuth: true },
    },
    {
      path: '/forgot-password',
      name: 'forgot-password',
      component: ForgotPassword,
    },
    {
      path: '/forgot-password',
      name: 'forgot-password',
      component: ForgotPassword,
    },
    {
      path: '/forgot-password/:code',
      name: 'reset-password',
      component: ResetPassword,
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
