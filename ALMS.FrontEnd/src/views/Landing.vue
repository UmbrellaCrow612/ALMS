<template>
  <div class="min-h-screen bg-gray-50">
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
            <RouterLink v-for="link in links" :key="link" :to="`/${link.toLowerCase()}`" class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
              {{ link }}
            </RouterLink>
          </div>
          <div class="hidden sm:ml-6 sm:flex sm:items-center">
            <Button @click="navigate('/signup')">Sign up</Button>
          </div>
          <div class="-mr-2 flex items-center sm:hidden">
            <Button variant="ghost" @click="toggleMenu">
              <span class="sr-only">Open main menu</span>
              <component :is="isMenuOpen ? 'X' : 'Menu'" class="block h-6 w-6" aria-hidden="true" />
            </Button>
          </div>
        </div>
      </nav>

      <div v-if="isMenuOpen" class="sm:hidden pt-2 pb-3 space-y-1">
        <RouterLink v-for="link in links" :key="link" :to="`/${link.toLowerCase()}`" class="block pl-3 pr-4 py-2 border-l-4 text-base font-medium border-transparent text-gray-600 hover:bg-gray-50 hover:border-gray-300 hover:text-gray-800">
          {{ link }}
        </RouterLink>
        <RouterLink to="/signup" class="block pl-3 pr-4 py-2 border-l-4 text-base font-medium border-transparent text-indigo-600 hover:bg-gray-50 hover:border-indigo-300 hover:text-indigo-800">
          Sign up
        </RouterLink>
      </div>
    </header>

    <!-- Main -->
    <main>
      <div class="relative">
        <div class="absolute inset-0">
          <div class="w-full h-full object-cover"></div>
          <div class="absolute inset-0 bg-gray-600 mix-blend-multiply" aria-hidden="true"></div>
        </div>
        <div class="relative max-w-7xl mx-auto py-24 px-4 sm:py-32 sm:px-6 lg:px-8">
          <h1 class="text-4xl font-extrabold tracking-tight text-white sm:text-5xl lg:text-6xl">Welcome to the Advanced Media Library</h1>
          <p class="mt-6 max-w-3xl text-xl text-gray-300">Access a wide range of media services across all cities and towns in England.</p>
          <div class="mt-10 max-w-sm mx-auto sm:max-w-none sm:flex sm:justify-center">
            <div class="space-y-4 sm:space-y-0 sm:mx-auto sm:inline-grid sm:grid-cols-2 sm:gap-5">
              <Button @click="navigate('/signup')" class="flex items-center justify-center px-4 py-3 border border-transparent text-base font-medium rounded-md shadow-sm text-white bg-indigo-500 hover:bg-indigo-600 sm:px-8">Get started</Button>
              <Button variant="outline" @click="navigate('/about')" class="flex items-center justify-center px-4 py-3 border border-transparent text-base font-medium rounded-md shadow-sm text-white bg-white bg-opacity-20 hover:bg-opacity-30 sm:px-8">Learn more</Button>
            </div>
          </div>
        </div>
      </div>
    </main>

      <!-- Services -->
      <section class="max-w-7xl mx-auto py-16 px-4 sm:py-24 sm:px-6 lg:px-8 text-center">
        <h2 class="text-base font-semibold text-indigo-600 tracking-wide uppercase">Discover</h2>
        <p class="mt-1 text-4xl font-extrabold text-gray-900 sm:text-5xl sm:tracking-tight lg:text-6xl">Explore Our Collection</p>
        <p class="max-w-xl mt-5 mx-auto text-xl text-gray-500">Find books, journals, CDs, DVDs, and more in our extensive library network.</p>
      </section>

      <section class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-10">
        <form @submit.prevent="handleSearch" class="max-w-xl mx-auto">
          <div class="flex rounded-md shadow-sm">
            <input
        v-model="searchQuery"
        type="search"
        placeholder="Search for books, journals, CDs, DVDs..."
        class="flex-grow p-2 border border-gray-300 rounded-l-md focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
      />
      <button type="submit" class="ml-3 px-4 py-2 bg-indigo-600 text-white rounded-r-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2">
        <Search class="h-5 w-5 inline-block" />
        <span class="ml-2">Search</span>
      </button>
          </div>
        </form>
      </section>

      <!-- Footer -->
      <footer class="bg-white">
        <div class="max-w-7xl mx-auto py-12 px-4 sm:px-6 md:flex md:items-center md:justify-between lg:px-8">
          <div class="flex justify-center space-x-6 md:order-2">
            <Link href="#" class="text-gray-400 hover:text-gray-500"><span class="sr-only">Facebook</span> <FacebookIcon /></Link>
            <Link href="#" class="text-gray-400 hover:text-gray-500"><span class="sr-only">Twitter</span> <TwitterIcon /></Link>
          </div>
          <div class="mt-8 md:mt-0 md:order-1">
            <p class="text-center text-base text-gray-400">&copy; 2024 Advanced Media Library. All rights reserved.</p>
          </div>
        </div>
      </footer>
    </div>
  </template>

<script setup>
import { ref } from 'vue'
import { useRouter, RouterLink } from 'vue-router'
import { BookOpen, Users, Clock, Search, Menu, X } from 'lucide-vue-next'


const isMenuOpen = ref(false)
const searchQuery = ref('')
const links = ["About", "Services", "Contact"]
const router = useRouter()

const handleSearch = () => {
  console.log('Searching for:', searchQuery.value)
}

const toggleMenu = () => {
  isMenuOpen.value = !isMenuOpen.value
}

const navigate = (path) => {
  router.push(path)
}
</script>

<style scoped>
/* Add any additional styling here */
</style>
