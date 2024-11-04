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
          <div class="hidden sm:ml-6 sm:flex sm:space-x-8">
            <router-link 
              v-for="link in navigationLinks" 
              :key="link.name"
              :to="link.href"
              class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
            >
              {{ link.name }}
            </router-link>
          </div>
          <div class="hidden sm:ml-6 sm:flex sm:items-center">
            <button 
              @click="$router.push('/registration')"
              class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-indigo-600 hover:bg-indigo-700"
            >
              Sign up
            </button>
          </div>
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
          <router-link
            v-for="link in navigationLinks"
            :key="link.name"
            :to="link.href"
            class="block pl-3 pr-4 py-2 border-l-4 text-base font-medium border-transparent text-gray-600 hover:bg-gray-50 hover:border-gray-300 hover:text-gray-800"
          >
            {{ link.name }}
          </router-link>
          <router-link
            to="/signup"
            class="block pl-3 pr-4 py-2 border-l-4 text-base font-medium border-transparent text-indigo-600 hover:bg-gray-50 hover:border-indigo-300 hover:text-indigo-800"
          >
            Sign up
          </router-link>
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

      <!-- Search form -->
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <form @submit.prevent="handleSearch" class="mt-10 max-w-xl mx-auto">
          <div class="flex rounded-md shadow-sm">
            <input
              v-model="searchQuery"
              type="search"
              placeholder="Search for books, journals, CDs, DVDs..."
              class="flex-grow px-4 py-2 border border-gray-300 rounded-l-md focus:ring-indigo-500 focus:border-indigo-500"
            />
            <button
              type="submit"
              class="ml-3 inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-indigo-600 hover:bg-indigo-700"
            >
              <Search class="h-5 w-5" />
              <span class="ml-2">Search</span>
            </button>
          </div>
        </form>
      </div>

      <!-- Services section -->
      <div class="mt-32 max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="relative">
          <div class="absolute inset-0 flex items-center" aria-hidden="true">
            <div class="w-full border-t border-gray-300"></div>
          </div>
          <div class="relative flex justify-center">
            <span class="px-3 bg-gradient-to-b from-indigo-100 to-white text-lg font-medium text-gray-900">
              Our Services
            </span>
          </div>
        </div>

        <div class="mt-12 grid gap-16 lg:grid-cols-3 lg:gap-x-8 lg:gap-y-12">
          <div
            v-for="service in services"
            :key="service.title"
            :class="[
              'rounded-lg shadow-lg overflow-hidden',
              service.gradientClass
            ]"
          >
            <div class="px-6 py-8">
              <div class="flex items-center">
                <component :is="service.icon" class="h-6 w-6 mr-2" />
                <h3 class="text-lg font-medium text-white">{{ service.title }}</h3>
              </div>
              <p class="mt-4 text-base text-indigo-100">
                {{ service.description }}
              </p>
            </div>
          </div>
        </div>
      </div>

      <!-- CTA Section -->
      <div class="mt-32 bg-gradient-to-r from-indigo-700 to-purple-700">
        <div class="max-w-2xl mx-auto text-center py-16 px-4 sm:py-20 sm:px-6 lg:px-8">
          <h2 class="text-3xl font-extrabold text-white sm:text-4xl">
            <span class="block">Boost your knowledge.</span>
            <span class="block">Start using AML today.</span>
          </h2>
          <p class="mt-4 text-lg leading-6 text-indigo-200">
            Join our growing community of readers, researchers, and lifelong learners.
          </p>
          <button
            @click="$router.push('/registration')"
            class="mt-8 w-full inline-flex items-center justify-center px-5 py-3 border border-transparent text-base font-medium rounded-md text-indigo-600 bg-white hover:bg-indigo-50 sm:w-auto"
          >
            Sign up for free
          </button>
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
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { BookOpen, Users, Clock, Search, Menu, X } from 'lucide-vue-next'

const router = useRouter()
const isMenuOpen = ref(false)
const searchQuery = ref('')

const navigationLinks = [
  { name: 'About', href: '#' },
  { name: 'Services', href: '#' },
  { name: 'Contact', href: '#' }
]

const services = [
  {
    title: 'Vast Collection',
    description: 'Access a wide range of books, journals, periodicals, CDs, DVDs, and multimedia titles from our extensive library network.',
    icon: BookOpen,
    gradientClass: 'bg-gradient-to-br from-blue-500 to-indigo-600'
  },
  {
    title: 'Multiple Branches',
    description: 'Visit our branches across all cities and towns in England for in-person services and local community engagement.',
    icon: Users,
    gradientClass: 'bg-gradient-to-br from-purple-500 to-pink-600'
  },
  {
    title: '24/7 Online Access',
    description: 'Enjoy round-the-clock access to our online services, including search, reservations, and digital content streaming.',
    icon: Clock,
    gradientClass: 'bg-gradient-to-br from-green-500 to-teal-600'
  }
]

const socialLinks = [
  {
    name: 'Facebook',
    href: '#',
    icon: 'FacebookIcon' // You'll need to import or create these icons
  },
  {
    name: 'Twitter',
    href: '#',
    icon: 'TwitterIcon'
  }
]

const handleSearch = () => {
  console.log('Searching for:', searchQuery.value)
  // Implement search functionality here
}
</script>