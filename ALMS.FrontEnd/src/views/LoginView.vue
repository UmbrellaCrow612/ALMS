<template>
  <div class="min-h-screen bg-gray-100 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
    <div class="sm:mx-auto sm:w-full sm:max-w-md">
      <div class="flex justify-center">
        <BookOpen class="h-12 w-12 text-indigo-600" />
      </div>
      <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
        Login to AML
      </h2>
      <p class="mt-2 text-center text-sm text-gray-600">
        Access the Advanced Media Library network
      </p>
    </div>

    <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
      <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
        <form class="space-y-6" @submit.prevent="handleSubmit">
          <div v-if="error" class="bg-red-50 p-4 rounded-md">
            <p class="text-sm text-red-700">{{ error }}</p>
          </div>

          <div>
            <label for="email" class="block text-sm font-medium text-gray-700">
              Email address
            </label>
            <div class="mt-1">
              <input 
                id="email" 
                name="email" 
                type="email" 
                autocomplete="email" 
                required 
                v-model="form.email"
                class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              />
            </div>
          </div>

          <div>
            <label for="password" class="block text-sm font-medium text-gray-700">
              Password
            </label>
            <div class="mt-1">
              <input 
                id="password" 
                name="password" 
                type="password" 
                autocomplete="current-password" 
                required 
                v-model="form.password"
                class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              />
            </div>
          </div>

          <div class="flex items-center justify-between">
            <div class="flex items-center">
              <input
                id="remember-me"
                name="remember-me"
                type="checkbox"
                v-model="form.rememberMe"
                class="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
              />
              <label for="remember-me" class="ml-2 block text-sm text-gray-900">
                Remember me
              </label>
            </div>

            <div class="text-sm">
              <router-link 
                to="/forgot-password" 
                class="font-medium text-indigo-600 hover:text-indigo-500"
              >
                Forgot your password?
              </router-link>
            </div>
          </div>

          <div>
            <button 
              type="submit"
              :disabled="loading"
              class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50"
            >
              <template v-if="loading">
                <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Signing in...
              </template>
              <template v-else>
                Sign in
              </template>
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
                Don't have an account?
              </span>
            </div>
          </div>

          <div class="mt-6">
            <router-link 
              to="/registration" 
              class="w-full inline-flex justify-center py-2 px-4 border border-gray-300 rounded-md shadow-sm bg-white text-sm font-medium text-indigo-600 hover:bg-gray-50"
            >
              Register
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { BookOpen } from 'lucide-vue-next'
import axiosInstance from '@/plugins/axios';

const router = useRouter()
const loading = ref(false)
const error = ref(null)

const form = ref({
  email: '',
  password: '',
  rememberMe: false
})

const handleSubmit = async () => {
  console.log('ran submit')
  try {
    loading.value = true
    error.value = null
    
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 1000))

    axiosInstance.post('/auth/login', {
      email: form.value.email,
      password: form.value.password,
})
.then(response => console.log(response))
.catch(error => console.error(error));

    const random = ''
 

    
    // Mock successful login
    const user = {
      email: form.value.email,
      name: 'Test User',
      id: '123'
    }
    
    // Store user data
    localStorage.setItem('user', JSON.stringify(user))
    
    // Redirect to dashboard
    router.push('/')
    
  } catch (err) {
  console.error('Login failed:', err); // Logs the entire error object
  error.value = err.response?.data?.message || 'Invalid email or password';

  } finally {
    loading.value = false
  }
}


</script>