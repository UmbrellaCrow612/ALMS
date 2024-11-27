<template>
  <div class="min-h-screen bg-slate-50">
    <NavBar></NavBar>
    <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <div class="grid gap-8 lg:grid-cols-3">
        <!-- Left Column - Media Photo and Borrow History -->
        <div class="space-y-8">
          <!-- Media Image Card -->
          <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6">
            <div class="aspect-square relative rounded-lg overflow-hidden bg-slate-100">
              <img 
                :src="mediaDetails.imgUrl" 
                alt="Media Cover" 
                class="object-cover w-full h-full"
              />
            </div>
          </div>

          <!-- Borrow History Card -->
          <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6">
            <h2 class="text-lg font-semibold mb-6 flex items-center text-slate-800">
              <Clock class="mr-3 h-5 w-5 text-slate-600" />
              Borrow History
            </h2>
            <div v-if="loadingHistory" class="text-center py-6">
              <div class="animate-spin rounded-full h-6 w-6 border-b-2 border-indigo-600 mx-auto"></div>
              <p class="mt-2 text-slate-600">Loading history...</p>
            </div>
            <div v-else-if="historyError" class="text-red-600 text-center py-6">
              {{ historyError }}
            </div>
            <div v-else-if="borrowHistory.length === 0" class="text-slate-600 text-center py-6">
              No borrow history available.
            </div>
            <div v-else class="space-y-5">
              <div
                v-for="(history, index) in borrowHistory"
                :key="index"
                class="flex items-center justify-between text-sm"
              >
                <span
                  :class="history.status === 'Returned' ? 'text-emerald-600' : 'text-blue-600'"
                  class="font-medium"
                >
                  {{ new Date(history) }}
                </span>
              </div>
            </div>
          </div>
        </div>

        <!-- Right Column - Description and Similar Items -->
        <div class="lg:col-span-2 space-y-8">
          <!-- Media Details Card -->
          <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6">
            <div class="space-y-6">
              <div class="flex items-center justify-between">
                <h1 class="text-2xl font-bold text-slate-900">{{ mediaDetails.title }}</h1>
                <span class="px-3 py-1 text-sm font-medium bg-slate-100 text-slate-700 rounded-full">
                  {{ mediaTypes[mediaDetails.mediaType] }}
                </span>
              </div>
              <div class="flex items-center text-slate-600">
                <User class="mr-2 h-4 w-4" />
                <span>{{ mediaDetails.author }}</span>
              </div>
              <div class="border-t border-slate-200 my-6"></div>
              <div class="space-y-3">
                <h2 class="font-semibold text-slate-900">Description</h2>
                <p class="text-slate-600 leading-relaxed">{{ mediaDetails.description }}</p>
              </div>
              <div class="grid grid-cols-2 gap-6 text-sm">
                <div class="space-y-1">
                  <span class="block font-medium text-slate-700">Genre</span>
                  <span class="text-slate-600">{{ mediaDetails.genre }}</span>
                </div>
                <div class="space-y-1">
                  <span class="block font-medium text-slate-700">Published</span>
                  <span class="text-slate-600">{{ formatDate(mediaDetails.publishedAt) }}</span>
                </div>
                <div class="space-y-1">
                  <span class="block font-medium text-slate-700">ISBN</span>
                  <span class="text-slate-600">{{ mediaDetails.isbn }}</span>
                </div>
                <div class="space-y-1">
                  <span class="block font-medium text-slate-700">Availability</span>
                  <span class="text-slate-600">{{ mediaDetails.isAvailable }}</span>
                </div>
              </div>
            </div>
          </div>

          <!-- Lost Item Notification -->
          <div v-if="mediaDetails.isLost" class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative" role="alert">
            <strong class="font-bold">This item is lost! - </strong>
            <span class="block sm:inline">Wait for the branch librarian to put it back in stock.</span>
          </div>

          <!-- Similar Items Card -->
          <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6">
            <h2 class="text-lg font-semibold mb-6 text-slate-800">Similar Items</h2>
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div
                v-for="item in similarItems"
                :key="item.id"
                class="p-4 rounded-lg border border-slate-200 hover:border-slate-300 hover:bg-slate-50 transition-all duration-200"
              >
                <div class="flex items-center">
                  <img :src="item.imgUrl" alt="Cover" class="w-16 h-16 object-cover rounded-lg mr-4" />
                  <div>
                    <h3 class="font-medium text-slate-900">{{ item.title }}</h3>
                    <p class="text-sm text-slate-600 mt-1">{{ item.author }}</p>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Reserve and Borrow Buttons -->
          <div class="flex justify-end gap-4">
            <button 
              @click="reserveItem"
              :disabled="!mediaDetails.isAvailable || mediaDetails.isLost"
              class="px-6 py-2.5 text-sm font-medium rounded-lg transition-all duration-200 bg-emerald-50 text-emerald-600 hover:bg-emerald-100 disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:bg-emerald-50 focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:ring-offset-2"
            >
              Reserve
            </button>
            <button 
              @click="showBorrowConfirmation" 
              :disabled="!mediaDetails.isAvailable || mediaDetails.isLost" 
              class="px-6 py-2.5 text-sm font-medium rounded-lg transition-all duration-200 bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
            >
              Borrow
            </button>
          </div>
        </div>
      </div>
    </main>

    <!-- Borrow Confirmation Modal -->
    <Modal v-if="isBorrowModalVisible" @close="closeBorrowModal">
      <template #header>Confirm Borrow</template>
      <template #body>
        <div v-if="borrowError" class="text-red-600 mb-4">{{ borrowError }}</div>
        Are you sure you want to borrow this item?
      </template>
      <template #footer>
        <button 
          @click="confirmBorrow" 
          class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700">
          Confirm
        </button>
        <button 
          @click="closeBorrowModal" 
          class="px-4 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 ml-2">
          Cancel
        </button>
      </template>
    </Modal>

    <!-- Reservation Confirmation Modal -->
    <Modal v-if="isReserveModalVisible" @close="closeReserveModal">
      <template #header>Reserve Item</template>
      <template #body>
        <div v-if="reserveError" class="text-red-600 mb-4">{{ reserveError }}</div>
        <div class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Reserve From</label>
            <input 
              type="date" 
              v-model="reserveFromDate"
              class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-emerald-500 focus:ring-emerald-500"
              :min="today"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Reserve Until</label>
            <input 
              type="date" 
              v-model="reserveToDate"
              class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-emerald-500 focus:ring-emerald-500"
              :min="reserveFromDate"
            />
          </div>
        </div>
      </template>
      <template #footer>
        <button 
          @click="confirmReserve" 
          :disabled="!isValidReservation"
          class="px-4 py-2 bg-emerald-600 text-white rounded-lg hover:bg-emerald-700 disabled:opacity-50 disabled:cursor-not-allowed">
          Confirm
        </button>
        <button 
          @click="closeReserveModal" 
          class="px-4 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 ml-2">
          Cancel
        </button>
      </template>
    </Modal>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRoute } from 'vue-router';
import axiosInstance from '@/plugins/axios';
import { Clock, Calendar, User } from 'lucide-vue-next';
import NavBar from '@/components/NavBar.vue';
import Modal from '@/components/Modal.vue';

const route = useRoute();
const mediaDetails = ref({});
const borrowHistory = ref([]);
const similarItems = ref([]);
const isBorrowModalVisible = ref(false);
const borrowError = ref(null);
const loadingHistory = ref(true);
const historyError = ref(null);
const isReserveModalVisible = ref(false);
const reserveError = ref(null);
const reserveFromDate = ref('');
const reserveToDate = ref('');

const mediaTypes = ['DVD', 'Book', 'AudioBook', 'Games', 'Journal', 'Periodicals', 'CDs', 'MultimediaTitles'];

const today = computed(() => {
  const date = new Date();
  return date.toISOString().split('T')[0];
});

const isValidReservation = computed(() => {
  return reserveFromDate.value && reserveToDate.value && reserveFromDate.value <= reserveToDate.value;
});

const loadMediaDetails = async () => {
  try {
    const response = await axiosInstance.get(`/media/${route.params.id}`);
    mediaDetails.value = response.data;

    const similarResponse = await axiosInstance.get('/media', {
      params: {
        MediaType: mediaDetails.value.mediaType,
        Genre: mediaDetails.value.genre,
      },
    });

    similarItems.value = similarResponse.data.filter((item) => item.id !== mediaDetails.value.id);
  } catch (error) {
    console.error('Failed to load media details:', error);
  }
};

const loadBorrowHistory = async () => {
  try {
    loadingHistory.value = true;
    historyError.value = null;
    const response = await axiosInstance.get(`/media/${route.params.id}/history`);
    borrowHistory.value = response.data;
  } catch (error) {
    historyError.value = 'Failed to load borrow history. Please try again later.';
    console.error('Error loading borrow history:', error);
  } finally {
    loadingHistory.value = false;
  }
};

const reserveItem = () => {
  reserveError.value = null;
  reserveFromDate.value = today.value;
  reserveToDate.value = '';
  isReserveModalVisible.value = true;
};

const showBorrowConfirmation = () => {
  borrowError.value = null;
  isBorrowModalVisible.value = true;
};

const closeBorrowModal = () => {
  isBorrowModalVisible.value = false;
  borrowError.value = null;
};

const confirmBorrow = async () => {
  try {
    const response = await axiosInstance.post(`/media/${route.params.id}/borrow`);
    if (response.status === 200) {
      window.location.reload();
    }
  } catch (error) {
    borrowError.value = 'Failed to borrow item. Please try again.';
    console.error('Failed to borrow item:', error);
  }
};

const formatDate = (date) => new Date(date).toLocaleDateString();

const closeReserveModal = () => {
  isReserveModalVisible.value = false;
  reserveError.value = null;
  reserveFromDate.value = '';
  reserveToDate.value = '';
};

const confirmReserve = async () => {
  try {
    const reservationData = {
      reserveFrom: new Date(reserveFromDate.value).toISOString(),
      reserveTo: new Date(reserveToDate.value).toISOString()
    };
    
    const response = await axiosInstance.post(`/reservations/medias/${route.params.id}/reserve`, reservationData);
    
    if (response.status === 204) {
      closeReserveModal();
      window.location.reload();
    }
  } catch (error) {
    reserveError.value = 'Failed to reserve item. Please try again.';
    console.error('Failed to reserve item:', error);
  }
};

onMounted(() => {
  loadMediaDetails();
  loadBorrowHistory();
});
</script>
