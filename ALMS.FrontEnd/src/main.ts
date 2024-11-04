import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { BookOpen, Users, Clock, Search, Menu, X } from 'lucide-vue-next'

const app = createApp(App)

app.use(router)
app.component('BookOpenIcon', BookOpen)
app.component('UsersIcon', Users)
app.component('ClockIcon', Clock)
app.component('SearchIcon', Search)
app.component('MenuIcon', Menu)
app.component('XIcon', X)

app.mount('#app')
