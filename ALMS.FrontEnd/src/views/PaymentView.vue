<template>
  <div class="min-h-screen bg-gradient-to-br from-indigo-100 to-purple-100 py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md mx-auto bg-white rounded-xl shadow-md overflow-hidden">
      <div class="p-8">
        <h2 class="text-2xl font-bold text-center text-gray-800 mb-8">Payment Details</h2>
        <form @submit.prevent="processPayment" class="space-y-6">
          <div>
            <label for="cardNumber" class="block text-sm font-medium text-gray-700">Card Number</label>
            <input 
              type="text" 
              id="cardNumber" 
              v-model="cardNumber" 
              @input="formatCardNumber"
              placeholder="1234 5678 9012 3456"
              class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              required
            >
          </div>
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label for="expiryDate" class="block text-sm font-medium text-gray-700">Expiry Date</label>
              <input 
                type="text" 
                id="expiryDate" 
                v-model="expiryDate" 
                @input="formatExpiryDate"
                placeholder="MM/YY"
                class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                required
              >
            </div>
            <div>
              <label for="cvv" class="block text-sm font-medium text-gray-700">CVV</label>
              <input 
                type="text" 
                id="cvv" 
                v-model="cvv" 
                placeholder="123"
                maxlength="4"
                class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                required
              >
            </div>
          </div>
          <div>
            <label for="cardholderName" class="block text-sm font-medium text-gray-700">Cardholder Name</label>
            <input 
              type="text" 
              id="cardholderName" 
              v-model="cardholderName" 
              placeholder="John Doe"
              class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              required
            >
          </div>
          <div>
            <label for="amount" class="block text-sm font-medium text-gray-700">Amount</label>
            <div class="mt-1 relative rounded-md shadow-sm">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <span class="text-gray-500 sm:text-sm">$</span>
              </div>
              <input 
                type="number" 
                id="amount" 
                v-model="amount" 
                disabled
                class="block w-full pl-7 pr-12 border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                required
              >
            </div>
          </div>
          <div>
            <button 
              type="submit" 
              class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
              :disabled="isProcessing"
            >
              {{ isProcessing ? 'Processing...' : 'Pay Now' }}
            </button>
          </div>
        </form>
      </div>
    </div>
    <!-- Payment confirmation modal -->
    <div v-if="showConfirmation" class="fixed inset-0 bg-gray-500 bg-opacity-75 flex items-center justify-center">
      <div class="bg-white p-8 rounded-lg shadow-xl">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Payment Successful</h3>
        <p class="text-sm text-gray-500 mb-4">Your payment of ${{ amount }} has been processed successfully.</p>
        <button 
          @click="closeConfirmation" 
          class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-indigo-600 text-base font-medium text-white hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:text-sm"
        >
          Close
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();
const cardNumber = ref('');
const expiryDate = ref('');
const cvv = ref('');
const cardholderName = ref('');
const amount = ref(route.query.rate || '0.00');
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
  // Simulate API call
  await new Promise(resolve => setTimeout(resolve, 2000));
  isProcessing.value = false;
  showConfirmation.value = true;
};

const closeConfirmation = () => {
  showConfirmation.value = false;
  // Reset form
  cardNumber.value = '';
  expiryDate.value = '';
  cvv.value = '';
  cardholderName.value = '';
  amount.value = '';
};
</script>