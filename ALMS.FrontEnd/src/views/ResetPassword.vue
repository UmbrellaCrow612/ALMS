<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100">
    <div class="bg-white p-8 rounded-lg shadow-md w-full max-w-md">
      <h2 class="text-2xl font-bold text-center mb-6">Reset Password</h2>
      <form @submit.prevent="resetPassword" class="space-y-6">
        <div>
          <label for="new-password" class="block text-sm font-medium text-gray-700">New Password</label>
          <div class="mt-1">
            <input 
              type="password" 
              id="new-password" 
              v-model="newPassword" 
              class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" 
              required 
            />
          </div>
        </div>
        <div>
          <label for="confirm-password" class="block text-sm font-medium text-gray-700">Confirm Password</label>
          <div class="mt-1">
            <input 
              type="password" 
              id="confirm-password" 
              v-model="confirmPassword" 
              class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" 
              required 
            />
          </div>
        </div>
        <button type="submit" class="w-full bg-indigo-600 text-white py-2 rounded-md hover:bg-indigo-700">Reset Password</button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import axiosInstance from '@/plugins/axios';
import { useRoute } from 'vue-router';

const route = useRoute();
const newPassword = ref('');
const confirmPassword = ref('');

const resetPassword = async () => {
  if (newPassword.value !== confirmPassword.value) {
    alert('Passwords do not match.');
    return;
  }

  try {
    const code = route.params.code;
    await axiosInstance.post('/reset-password', { code, newPassword: newPassword.value });
    alert('Your password has been reset successfully.');
  } catch (error) {
    console.error('Failed to reset password:', error);
    alert('Failed to reset password. Please try again.');
  }
};
</script> 