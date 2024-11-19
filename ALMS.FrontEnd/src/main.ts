import './index.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import LandingView from '@/views/LandingView.vue'
import RegistrationView from './views/RegistrationView.vue'
import LoginView from './views/LoginView.vue'
import SearchView from './views/SearchView.vue'
import ApprovalView from './views/ApprovalView.vue'
import { createPinia } from 'pinia';
import axios from 'axios';
import InventoryView from './views/InventoryView.vue'
import AboutView from './views/AboutView.vue'



const app = createApp(App)
const pinia = createPinia();

app.use(router)
app.use(pinia);
app.component('LandingView', LandingView)
app.component('RegistrationView', RegistrationView)
app.component('LoginView', LoginView)
app.component('SearchView', SearchView)
app.component('ApprovalView', ApprovalView)
app.component('InventoryView', InventoryView)
app.component('AboutView', AboutView)

app.config.globalProperties.$axios = axios;

app.mount('#app')
