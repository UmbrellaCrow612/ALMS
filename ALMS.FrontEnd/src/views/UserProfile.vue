<template>
  <div class="min-h-screen bg-gray-100">
    <NavBar />
    <main class="container mx-auto px-4 py-12">
      <div class="bg-white p-8 rounded-2xl shadow-lg max-w-2xl mx-auto">
        <h2 class="text-3xl font-bold text-center mb-8 text-indigo-800">User Profile</h2>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div v-for="(value, key) in userDisplayData" :key="key" class="space-y-2">
            <label class="block text-sm font-medium text-gray-600 uppercase tracking-wide">{{ formatLabel(key) }}</label>
            <p class="text-lg text-gray-900 border-b border-gray-200 pb-2">{{ value }}</p>
          </div>
        </div>

        <div class="mt-10 text-center">
          <button 
            @click="goToPayment"
            class="px-6 py-3 bg-indigo-600 text-white font-semibold rounded-full hover:bg-indigo-700 transform transition duration-200 hover:scale-105 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-opacity-50"
          >
            Subscribe Now
          </button>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup>
import { computed } from 'vue';
import { useRouter } from 'vue-router';
import { useUserStore } from '@/stores/userStore';
import NavBar from '@/components/NavBar.vue';

const router = useRouter();
const userStore = useUserStore();
const user = computed(() => userStore.user);

const userDisplayData = computed(() => ({
  firstName: user.value.firstName,
  lastName: user.value.lastName,
  email: user.value.email,
  phoneNumber: user.value.phoneNumber,
  address: user.value.address
}));

const formatLabel = (key) => {
  return key.replace(/([A-Z])/g, ' $1').replace(/^./, (str) => str.toUpperCase());
};

const goToPayment = () => {
  router.push('/payment');
};
</script>