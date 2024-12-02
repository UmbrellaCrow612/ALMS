<template>
  <div class="min-h-screen bg-gradient-to-br from-indigo-100 to-purple-100 py-12 px-4 sm:px-6 lg:px-8 flex items-center justify-center">
    <div class="max-w-md w-full bg-white rounded-2xl shadow-xl overflow-hidden transform transition-all hover:scale-105 duration-300">
      <div class="p-8">
        <h2 class="text-3xl font-extrabold text-center text-indigo-800 mb-8">Payment Details</h2>
        <form @submit.prevent="processPayment" class="space-y-6">
          <div class="relative">
            <label for="cardNumber" class="absolute -top-2 left-2 inline-block bg-white px-1 text-xs font-medium text-indigo-600">
              Card Number
            </label>
            <input 
              type="text" 
              id="cardNumber" 
              v-model="cardNumber" 
              @input="formatCardNumber"
              placeholder="1234 5678 9012 3456"
              class="mt-1 block w-full px-3 py-3 border-2 border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              required
            >
          </div>
          <div class="grid grid-cols-2 gap-4">
            <div class="relative">
              <label for="expiryDate" class="absolute -top-2 left-2 inline-block bg-white px-1 text-xs font-medium text-indigo-600">
                Expiry Date
              </label>
              <input 
                type="text" 
                id="expiryDate" 
                v-model="expiryDate" 
                @input="formatExpiryDate"
                placeholder="MM/YY"
                class="mt-1 block w-full px-3 py-3 border-2 border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                required
              >
            </div>
            <div class="relative">
              <label for="cvv" class="absolute -top-2 left-2 inline-block bg-white px-1 text-xs font-medium text-indigo-600">
                CVV
              </label>
              <input 
                type="text" 
                id="cvv" 
                v-model="cvv" 
                placeholder="123"
                maxlength="4"
                class="mt-1 block w-full px-3 py-3 border-2 border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                required
              >
            </div>
          </div>
          <div class="relative">
            <label for="cardholderName" class="absolute -top-2 left-2 inline-block bg-white px-1 text-xs font-medium text-indigo-600">
              Cardholder Name
            </label>
            <input 
              type="text" 
              id="cardholderName" 
              v-model="cardholderName" 
              placeholder="John Doe"
              class="mt-1 block w-full px-3 py-3 border-2 border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              required
            >
          </div>
          <div class="relative">
            <label for="amount" class="absolute -top-2 left-2 inline-block bg-white px-1 text-xs font-medium text-indigo-600">
              Amount
            </label>
            <div class="mt-1 relative rounded-md shadow-sm">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <span class="text-gray-500 sm:text-sm">$</span>
              </div>
              <input 
                type="number" 
                id="amount" 
                v-model="amount" 
                disabled
                class="block w-full pl-7 pr-12 px-3 py-3 border-2 border-gray-300 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                required
              >
            </div>
          </div>
          <div>
            <button 
              type="submit" 
              class="w-full flex justify-center py-3 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 transition-colors duration-200"
              :disabled="isProcessing"
            >
              <span class="flex items-center">
                <svg v-if="isProcessing" class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                {{ isProcessing ? 'Processing...' : 'Pay Now' }}
              </span>
            </button>
          </div>
        </form>
      </div>
    </div>
    <!-- Payment confirmation modal -->
    <div v-if="showConfirmation" class="fixed inset-0 bg-gray-500 bg-opacity-75 flex items-center justify-center z-50">
      <div class="bg-white p-8 rounded-lg shadow-xl max-w-md w-full transform transition-all duration-300 scale-100">
        <h3 class="text-2xl font-bold text-indigo-800 mb-4">Payment Successful</h3>
        <p class="text-lg text-gray-600 mb-6">Your payment of ${{ amount }} has been processed successfully.</p>
        <button 
          @click="closeConfirmation" 
          class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-3 bg-indigo-600 text-base font-medium text-white hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:text-sm transition-colors duration-200"
        >
          Close
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import axiosInstance from '@/plugins/axios';

const route = useRoute();
const router = useRouter();
const cardNumber = ref('');
const expiryDate = ref('');
const cvv = ref('');
const cardholderName = ref('');
const amount = ref(route.query.rate || '5.00');
const isProcessing = ref(false);
const showConfirmation = ref(false);

const formatCardNumber = (event) => {
  let value = event.target.value.replace(/\s/g, '').replace(/\D/g, '');
  if (value.length > 16) value = value.slice(0, 16);
  cardNumber.value = value.replace(/(\d{4})/g, '$1 ').trim();
};

const formatExpiryDate = (event) => {
  let value = event.target.value.replace(/\D/g, '');
  if (value.length > 4) value = value.slice(0, 4);
  if (value.length > 2) {
    expiryDate.value = value.slice(0, 2) + '/' + value.slice(2);
  } else {
    expiryDate.value = value;
  }
};

const processPayment = async () => {
  isProcessing.value = true;
  try {
    const payload = {
      amount: parseFloat(amount.value),
      subscriptionType: 'Basic', // Example subscription type, adjust as needed
      endDate: new Date(new Date().setMonth(new Date().getMonth() + 1)).toISOString() // Example end date, 1 month from now
    };

    const response = await axiosInstance.post('/y-subscriptions/subscribe', payload);
    if (response.status === 200) {
      showConfirmation.value = true;
      setTimeout(() => {
        router.push('/profile');
      }, 2000);
    }
  } catch (error) {
    console.error('Error processing payment:', error);
  } finally {
    isProcessing.value = false;
  }
};

const closeConfirmation = () => {
  showConfirmation.value = false;
  // Reset form
  cardNumber.value = '';
  expiryDate.value = '';
  cvv.value = '';
  cardholderName.value = '';
  amount.value = '';
  router.push('/profile');
};
</script>