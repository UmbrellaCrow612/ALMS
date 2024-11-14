<template>
  <div class="min-h-screen bg-gradient-to-b from-indigo-100 to-white">
    <!-- Header -->
    <header class="bg-white shadow-sm">
      <nav class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between h-16">
          <div class="flex">
            <div class="flex-shrink-0 flex items-center">
              <BookOpen class="h-8 w-8 text-indigo-600" />
              <span class="ml-2 text-2xl font-bold text-gray-900">AML</span>
            </div>
          </div>

          <!-- Desktop Navigation -->
          <div class="hidden sm:ml-6 sm:flex sm:space-x-8">
            <template v-for="link in visibleLinks" :key="link.name">
              <router-link
                :to="link.href"
                class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
              >
                {{ link.name }}
              </router-link>
            </template>
          </div>

          <!-- User Actions -->
          <div class="hidden sm:ml-6 sm:flex sm:items-center">
            <template v-if="isAuthenticated">
              <div class="ml-3 relative">
                <button
                  @click="isProfileOpen = !isProfileOpen"
                  class="bg-white flex text-sm rounded-full focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                >
                  <User class="h-8 w-8 text-gray-400" />
                </button>

                <!-- Profile dropdown -->
                <div
                  v-if="isProfileOpen"
                  class="origin-top-right absolute right-0 mt-2 w-48 rounded-md shadow-lg py-1 bg-white ring-1 ring-black ring-opacity-5 focus:outline-none"
                >
                  <router-link
                    to="/profile"
                    class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                  >
                    Your Profile
                  </router-link>
                  <router-link
                    to="/subscriptions"
                    class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                  >
                    Manage Subscriptions
                  </router-link>
                  <button
                    @click="handleLogout"
                    class="block w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                  >
                    Sign out
                  </button>
                </div>
              </div>
            </template>
            <template v-else>
              <button
                @click="$router.push('/login')"
                class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-indigo-600 bg-white hover:bg-gray-50 mr-2"
              >
                Login
              </button>
              <button
                @click="$router.push('/registration')"
                class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-indigo-600 hover:bg-indigo-700"
              >
                Sign up
              </button>
            </template>
          </div>

          <!-- Mobile menu button -->
          <div class="-mr-2 flex items-center sm:hidden">
            <button
              @click="isMenuOpen = !isMenuOpen"
              class="inline-flex items-center justify-center p-2 rounded-md text-gray-400 hover:text-gray-500 hover:bg-gray-100"
            >
              <span class="sr-only">Open main menu</span>
              <component :is="isMenuOpen ? X : Menu" class="block h-6 w-6" />
            </button>
          </div>
        </div>
      </nav>

      <!-- Mobile menu -->
      <div v-if="isMenuOpen" class="sm:hidden">
        <div class="pt-2 pb-3 space-y-1">
          <template v-for="link in visibleLinks" :key="link.name">
            <router-link
              :to="link.href"
              class="block pl-3 pr-4 py-2 border-l-4 text-base font-medium border-transparent text-gray-600 hover:bg-gray-50 hover:border-gray-300 hover:text-gray-800"
            >
              {{ link.name }}
            </router-link>
          </template>
          <button
            v-if="isAuthenticated"
            @click="handleLogout"
            class="block w-full text-left pl-3 pr-4 py-2 border-l-4 text-base font-medium border-transparent text-red-600 hover:bg-gray-50 hover:border-red-300 hover:text-red-800"
          >
            Sign out
          </button>
        </div>
      </div>
    </header>

    <main>
      <!-- Hero section -->
      <div class="bg-gradient-to-r from-indigo-600 to-purple-600 text-white">
        <div class="max-w-7xl mx-auto py-24 px-4 sm:py-32 sm:px-6 lg:px-8">
          <h1 class="text-4xl font-extrabold tracking-tight sm:text-5xl lg:text-6xl">
            Welcome to the Advanced Media Library
          </h1>
          <p class="mt-6 max-w-3xl text-xl">
            Access a wide range of media services across all cities and towns in England.
          </p>
          <div class="mt-10 max-w-sm mx-auto sm:max-w-none sm:flex sm:justify-start">
            <div class="space-y-4 sm:space-y-0 sm:mx-auto sm:inline-grid sm:grid-cols-2 sm:gap-5">
              <button
                @click="$router.push('/registration')"
                class="flex items-center justify-center px-4 py-3 border border-transparent text-base font-medium rounded-md shadow-sm text-indigo-700 bg-white hover:bg-indigo-50 sm:px-8"
              >
                Get started
              </button>
              <button
                @click="$router.push('/about')"
                class="flex items-center justify-center px-4 py-3 border border-transparent text-base font-medium rounded-md shadow-sm text-white bg-indigo-500 bg-opacity-60 hover:bg-opacity-70 sm:px-8"
              >
                Learn more
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Search section -->
      <div class="max-w-7xl mx-auto py-16 px-4 sm:py-24 sm:px-6 lg:px-8">
        <div class="text-center">
          <h2 class="text-base font-semibold text-indigo-600 tracking-wide uppercase">Discover</h2>
          <p class="mt-1 text-4xl font-extrabold text-gray-900 sm:text-5xl sm:tracking-tight lg:text-6xl">
            Explore Our Collection
          </p>
          <p class="max-w-xl mt-5 mx-auto text-xl text-gray-500">
            Find books, journals, CDs, DVDs, and more in our extensive library network.
          </p>
        </div>
      </div>
    </main>

    <!-- Footer -->
    <footer class="bg-gray-800 text-white">
      <div class="max-w-7xl mx-auto py-12 px-4 sm:px-6 md:flex md:items-center md:justify-between lg:px-8">
        <div class="flex justify-center space-x-6 md:order-2">
          <a
            v-for="social in socialLinks"
            :key="social.name"
            :href="social.href"
            class="text-gray-400 hover:text-gray-300"
          >
            <span class="sr-only">{{ social.name }}</span>
            <component :is="social.icon" class="h-6 w-6" />
          </a>
        </div>
        <div class="mt-8 md:mt-0 md:order-1">
          <p class="text-center text-base text-gray-400">
            &copy; {{ new Date().getFullYear() }} Advanced Media Library. All rights reserved.
          </p>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useUserStore } from '@/stores/userStore';
import { BookOpen, User, Menu, X, Search } from 'lucide-vue-next'

const router = useRouter()
const userStore = useUserStore()

const isMenuOpen = ref(false)
const isProfileOpen = ref(false)
const searchQuery = ref('')
const isAuthenticated = computed(() => !!userStore.user)

const roleBasedLinks = [
  { name: 'Home', href: '/', roles: ['LibaryMember', 'BranchLibarian'] },
  { name: 'About', href: '/about', roles: ['LibaryMember', 'BranchLibarian'] },
  { name: 'Services', href: '/services', roles: ['LibaryMember', 'BranchLibarian'] },
  { name: 'Contact', href: '/contact', roles: ['LibaryMember', 'BranchLibarian'] },
  { name: 'Search', href: '/search', roles: ['LibaryMember', 'BranchLibarian'] },
  { name: 'Borrow', href: '/borrow', roles: ['LibaryMember', 'BranchLibarian'] },
  { name: 'Inventory', href: '/inventory', roles: ['BranchLibarian'] },
  { name: 'Subscriptions', href: '/subscriptions', roles: ['LibaryMember', 'BranchLibarian'] },
  { name: 'Approval', href: '/approval', roles: ['BranchLibarian'] }
]

const visibleLinks = computed(() => {
  const roles = userStore.user?.roles || []
  return roleBasedLinks.filter(link => link.roles.some(role => roles.includes(role)))
})

const handleLogout = () => {
  userStore.clearUser()
  router.push('/login')
}

const handleSearch = () => {
  if (searchQuery.value.trim()) {
    router.push({
      path: '/search',
      query: { q: searchQuery.value }
    })
  }
}

onMounted(() => {
  document.addEventListener('click', (event) => {
    if (!event.target.closest('.relative')) isProfileOpen.value = false
  })
})
</script>