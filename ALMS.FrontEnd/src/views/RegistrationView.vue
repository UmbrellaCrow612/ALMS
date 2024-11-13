<template>
  <div class="min-h-screen bg-gray-100 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
    <div class="sm:mx-auto sm:w-full sm:max-w-md">
      <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
        Register for AML
      </h2>
      <p class="mt-2 text-center text-sm text-gray-600">
        Join the Advanced Media Library network
      </p>
    </div>

    <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
      <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
        <form class="space-y-6" @submit.prevent="handleSubmit">
          <div v-if="successMessage" class="bg-green-50 p-4 rounded-md">
            <p class="text-sm text-green-700">{{ successMessage }}</p>
          </div>
          <div v-if="errorMessage" class="bg-red-50 p-4 rounded-md">
            <p class="text-sm text-red-700">{{ errorMessage }}</p>
          </div>

          <div>
            <label for="name" class="block text-sm font-medium text-gray-700">
              Full Name
            </label>
            <div class="mt-1">
              <input id="name" name="name" type="text" required v-model="form.name"
                class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              />
            </div>
          </div>

          <div>
            <label for="address" class="block text-sm font-medium text-gray-700">
              Address
            </label>
            <div class="mt-1">
              <textarea id="address" name="address" rows="3" required v-model="form.address"
                class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              ></textarea>
            </div>
          </div>

          <div>
            <label for="email" class="block text-sm font-medium text-gray-700">
              Email address
            </label>
            <div class="mt-1">
              <input id="email" name="email" type="email" autocomplete="email" required v-model="form.email"
                class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              />
            </div>
          </div>

          <div>
            <label for="phone" class="block text-sm font-medium text-gray-700">
              Phone number
            </label>
            <div class="mt-1">
              <input id="phone" name="phone" type="tel" autocomplete="tel" required v-model="form.phone"
                class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              />
            </div>
          </div>

          <div>
            <label for="password" class="block text-sm font-medium text-gray-700">
              Password
            </label>
            <div class="mt-1">
              <input id="password" name="password" type="password" autocomplete="new-password" required v-model="form.password"
                class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              />
            </div>
          </div>

          <div>
            <button type="submit"
              class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
            >
              Register
            </button>
          </div>
        </form>

        <div class="mt-6">
          <div class="relative">
            <div class="absolute inset-0 flex items-center">
              <div class="w-full border-t border-gray-300"></div>
            </div>
            <div class="relative flex justify-center text-sm">
              <span class="px-2 bg-white text-gray-500">
                Already have an account?
              </span>
            </div>
          </div>

          <div class="mt-6">
            <a href="/login" class="w-full inline-flex justify-center py-2 px-4 border border-gray-300 rounded-md shadow-sm bg-white text-sm font-medium text-indigo-600 hover:bg-gray-50">
              Sign in
            </a>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import axiosInstance from '@/plugins/axios'; 

const router = useRouter();

const form = ref({
  name: '',
  address: '',
  email: '',
  phone: '',
  password: ''
});

const successMessage = ref('');
const errorMessage = ref('');

const handleSubmit = async () => {
  successMessage.value = '';
  errorMessage.value = '';
  
  try {
    const payload = {
      userName: form.value.email,
      email: form.value.email,
      password: form.value.password,
      firstName: form.value.name.split(' ')[0],
      lastName: form.value.name.split(' ').slice(1).join(' '),
      address: form.value.address,
    };

    const response = await axiosInstance.post('/auth/register', payload);
    
    if (response.status === 201) {
      successMessage.value = 'You successfully created an account. Please wait for a branch librarian to approve it.';
    }
  } catch (error) {
    if (error.response?.status === 401) {
      errorMessage.value = 'Failed to register account. Please check your details and try again.';
    } else {
      errorMessage.value = 'An unexpected error occurred. Please try again later.';
    }
  }
};
</script>
