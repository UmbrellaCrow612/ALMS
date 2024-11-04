<template>
    <div class="flex items-center justify-center min-h-screen bg-gray-100">
      <v-card class="w-full max-w-md">
        <v-card-title>Register for AML</v-card-title>
        <v-card-subtitle>Create your Advanced Media Library account</v-card-subtitle>
  
        <v-card-text>
          <form @submit.prevent="handleRegistration">
            <div class="space-y-4">
              <div class="space-y-2">
                <v-label for="name">Full Name</v-label>
                <v-text-field
                  id="name"
                  placeholder="Enter your full name"
                  v-model="name"
                  required
                />
              </div>
  
              <div class="space-y-2">
                <v-label for="address">Address</v-label>
                <v-textarea
                  id="address"
                  placeholder="Enter your address"
                  v-model="address"
                  required
                />
              </div>
  
              <div class="space-y-2">
                <v-label for="email">Email</v-label>
                <v-text-field
                  id="email"
                  type="email"
                  placeholder="Enter your email"
                  v-model="email"
                  required
                />
              </div>
  
              <div class="space-y-2">
                <v-label for="phone">Phone Number</v-label>
                <v-text-field
                  id="phone"
                  type="tel"
                  placeholder="Enter your phone number"
                  v-model="phone"
                  required
                />
              </div>
            </div>
  
            <v-alert
              v-if="error"
              type="error"
              dismissible
              class="mt-4"
            >
              <strong>Error:</strong> {{ error }}
            </v-alert>
  
            <v-btn type="submit" class="w-full mt-6" color="primary">
              Register
            </v-btn>
          </form>
        </v-card-text>
  
        <v-card-actions>
          <v-btn
            variant="outlined"
            class="w-full"
            @click="goBack"
          >
            <v-icon left>mdi-arrow-left</v-icon> Back to Home
          </v-btn>
        </v-card-actions>
      </v-card>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue'
  import { useRouter } from 'vue-router'
  
  // Define state variables
  const name = ref('')
  const address = ref('')
  const email = ref('')
  const phone = ref('')
  const error = ref('')
  
  // Initialize router
  const router = useRouter()
  
  // Define registration handler
  const handleRegistration = () => {
    if (name.value && address.value && email.value && phone.value) {
      // Redirect to dashboard or confirmation page
      router.push('/dashboard')
    } else {
      error.value = 'Please fill in all fields'
    }
  }
  
  // Define function to navigate back to home
  const goBack = () => {
    router.push('/')
  }
  </script>
  
  <style scoped>
  /* Custom styling if needed */
  </style>
  