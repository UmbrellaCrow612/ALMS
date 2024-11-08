import './index.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import LandingView from '@/views/LandingView.vue'
import RegistrationView from './views/RegistrationView.vue'
import LoginView from './views/LoginView.vue'
import SearchView from './views/SearchView.vue'



const app = createApp(App)

app.use(router)
app.component('LandingView', LandingView)
app.component('RegistrationView', RegistrationView)
app.component('LoginView', LoginView)
app.component('SearchView', SearchView)

app.mount('#app')
