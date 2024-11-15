<template>
  <div class="min-h-screen bg-gradient-to-b from-indigo-100 to-white">
    <!-- Importing the NavBar -->
    <NavBar />

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
import NavBar from '@/components/NavBar.vue'; 
</script>
