import './index.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import LandingView from '@/views/LandingView.vue'



const app = createApp(App)

app.use(router)
app.component('LandingView', LandingView)

app.mount('#app')
