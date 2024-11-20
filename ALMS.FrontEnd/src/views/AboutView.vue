<template>
  <div class="min-h-screen bg-slate-50">
    <NavBar />
    <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <div class="max-w-3xl mx-auto">
        <h1 class="text-4xl font-bold text-slate-900 mb-8 text-center">Welcome to ALMS</h1>
        
        <div class="space-y-8">
          <section class="bg-white rounded-xl shadow-sm border border-slate-200 p-8">
            <h2 class="text-2xl font-semibold text-slate-900 mb-4">Empowering Libraries</h2>
            <p class="text-slate-600 leading-relaxed">
              ALMS (Advanced Library Management System) is a modern solution designed to transform how libraries manage their collections. 
              We combine cutting-edge technology with user-friendly interfaces to create an efficient and enjoyable experience for both librarians and patrons.
            </p>
          </section>

          <section class="bg-white rounded-xl shadow-sm border border-slate-200 p-8">
            <h2 class="text-2xl font-semibold text-slate-900 mb-4">Our Features</h2>
            <div class="grid md:grid-cols-2 gap-6">
              <div class="space-y-2">
                <h3 class="font-medium text-slate-900">Digital Cataloging</h3>
                <p class="text-slate-600">Comprehensive digital management of all media types</p>
              </div>
              <div class="space-y-2">
                <h3 class="font-medium text-slate-900">Smart Search</h3>
                <p class="text-slate-600">Advanced search capabilities across your entire collection</p>
              </div>
              <div class="space-y-2">
                <h3 class="font-medium text-slate-900">User Management</h3>
                <p class="text-slate-600">Efficient handling of borrower accounts and histories</p>
              </div>
              <div class="space-y-2">
                <h3 class="font-medium text-slate-900">Analytics</h3>
                <p class="text-slate-600">Detailed insights into your library's performance</p>
              </div>
            </div>
          </section>

          <section class="bg-white rounded-xl shadow-sm border border-slate-200 p-8">
            <h2 class="text-2xl font-semibold text-slate-900 mb-6">Get in Touch</h2>
            <div class="space-y-4">
              <div class="flex items-center space-x-4">
                <div class="w-10 h-10 bg-blue-50 rounded-full flex items-center justify-center">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-blue-600" viewBox="0 0 20 20" fill="currentColor">
                    <path d="M2.003 5.884L10 9.882l7.997-3.998A2 2 0 0016 4H4a2 2 0 00-1.997 1.884z" />
                    <path d="M18 8.118l-8 4-8-4V14a2 2 0 002 2h12a2 2 0 002-2V8.118z" />
                  </svg>
                </div>
                <div>
                  <p class="text-sm font-medium text-slate-900">Email</p>
                  <a href="mailto:contact@alms.com" class="text-sm text-blue-600 hover:text-blue-700">contact@alms.com</a>
                </div>
              </div>
              
              <div class="flex items-center space-x-4">
                <div class="w-10 h-10 bg-blue-50 rounded-full flex items-center justify-center">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-blue-600" viewBox="0 0 20 20" fill="currentColor">
                    <path d="M2 3a1 1 0 011-1h2.153a1 1 0 01.986.836l.74 4.435a1 1 0 01-.54 1.06l-1.548.773a11.037 11.037 0 006.105 6.105l.774-1.548a1 1 0 011.059-.54l4.435.74a1 1 0 01.836.986V17a1 1 0 01-1 1h-2C7.82 18 2 12.18 2 5V3z" />
                  </svg>
                </div>
                <div>
                  <p class="text-sm font-medium text-slate-900">Phone</p>
                  <a href="tel:+441234567890" class="text-sm text-blue-600 hover:text-blue-700">+44 (123) 456-7890</a>
                </div>
              </div>
            </div>
          </section>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup>
import NavBar from '@/components/NavBar.vue';
</script>

<style scoped>
/* All styling handled by Tailwind classes */
</style>