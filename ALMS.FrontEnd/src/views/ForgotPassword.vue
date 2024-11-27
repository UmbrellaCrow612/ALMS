<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100">
    <div class="bg-white p-8 rounded-lg shadow-md w-full max-w-md">
      <h2 class="text-2xl font-bold text-center mb-6">Forgot Password</h2>
      <form @submit.prevent="sendResetLink" class="space-y-6">
        <div>
          <label for="email" class="block text-sm font-medium text-gray-700">Email Address</label>
          <div class="mt-1">
            <input 
              type="email" 
              id="email" 
              v-model="email" 
              class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" 
              required 
            />
          </div>
        </div>
        <button type="submit" class="w-full bg-indigo-600 text-white py-2 rounded-md hover:bg-indigo-700">Send Reset Link</button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import axiosInstance from '@/plugins/axios';

const email = ref('');

const sendResetLink = async () => {
  try {
    await axiosInstance.post('/forgot-password', { email: email.value });
    alert('A password reset link has been sent to your email.');
  } catch (error) {
    console.error('Failed to send reset link:', error);
    alert('Failed to send reset link. Please try again.');
  }
};
</script> 