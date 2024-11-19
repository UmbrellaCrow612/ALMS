<template>
    <div class="min-h-screen bg-gray-100">
      <NavBar />
      <main class="container mx-auto px-4 py-8">
        <h1 class="text-3xl font-bold text-indigo-800 mb-6 text-center">About Us</h1>
        
        <section class="bg-white shadow-md rounded-lg p-6 mb-8">
          <h2 class="text-2xl font-semibold text-indigo-700 mb-4">Who We Are</h2>
          <p class="text-gray-600 leading-relaxed">
            We are a team dedicated to providing users with a seamless experience for managing media inventories.
            Our mission is to empower users by offering tools to manage, search, and access a variety of media resources efficiently.
          </p>
        </section>
  
        <section class="bg-white shadow-md rounded-lg p-6 mb-8">
          <h2 class="text-2xl font-semibold text-indigo-700 mb-4">Our Mission</h2>
          <p class="text-gray-600 leading-relaxed">
            Our goal is to simplify the process of media management by providing intuitive and user-friendly interfaces,
            backed by powerful tools and robust data management solutions.
          </p>
        </section>
  
        <section class="bg-white shadow-md rounded-lg p-6">
          <h2 class="text-2xl font-semibold text-indigo-700 mb-4">Contact Us</h2>
          <p class="text-gray-600 leading-relaxed">
            Have questions or want to get in touch? Feel free to reach out to our support team at:
          </p>
          <ul class="list-disc list-inside mt-4 text-gray-600">
            <li>Email: <a href="mailto:support@example.com" class="text-indigo-600 hover:underline">support@example.com</a></li>
            <li>Phone: <a href="tel:+1234567890" class="text-indigo-600 hover:underline">+44 1234 567890</a></li>
            <li>Address: 123 Media Lane, Library City, ML 45678</li>
          </ul>
        </section>
      </main>
    </div>
  </template>
  
  <script setup>
  import NavBar from '@/components/NavBar.vue';
  </script>
  
  <style scoped>
  .container {
    max-width: 800px;
  }
  </style>
  