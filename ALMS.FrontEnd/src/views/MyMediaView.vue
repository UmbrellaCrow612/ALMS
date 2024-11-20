<template>
  <div class="min-h-screen bg-slate-50">
    <NavBar />
    <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6">
        <h1 class="text-3xl font-bold text-indigo-800 mb-6">My Borrowed Media</h1>

        <div v-if="loading" class="text-center py-12">
          <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mx-auto"></div>
          <p class="mt-4 text-slate-600">Loading your borrowed media...</p>
        </div>

        <div v-else-if="error" class="text-center py-12">
          <p class="text-red-600">{{ error }}</p>
        </div>

        <div v-else-if="borrowedTransactions.length === 0" class="text-center py-12">
          <p class="text-slate-600">You haven't borrowed any media yet.</p>
        </div>

        <div v-else class="overflow-x-auto">
          <table class="table-auto w-full text-left border-collapse">
            <thead class="bg-slate-100 text-slate-700">
              <tr>
                <th class="px-4 py-2 border-b">Media Title</th>
                <th class="px-4 py-2 border-b">Author</th>
                <th class="px-4 py-2 border-b">Genre</th>
                <th class="px-4 py-2 border-b">Cover</th>
                <th class="px-4 py-2 border-b">Borrowed At</th>
                <th class="px-4 py-2 border-b">Due Date</th>
                <th class="px-4 py-2 border-b">Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="transaction in borrowedTransactions"
                :key="transaction.id"
                class="border-b hover:bg-slate-50"
              >
                <td class="px-4 py-2">{{ transaction.media.title }}</td>
                <td class="px-4 py-2">{{ transaction.media.author }}</td>
                <td class="px-4 py-2">{{ transaction.media.genre }}</td>
                <td class="px-4 py-2">
                  <img 
                    :src="transaction.media.imgUrl" 
                    alt="Cover" 
                    class="w-12 h-16 object-cover rounded"
                  />
                </td>
                <td class="px-4 py-2">{{ formatDate(transaction.borrowedAt) }}</td>
                <td class="px-4 py-2">{{ formatDate(transaction.dueDate) }}</td>
                <td class="px-4 py-2">
                  <button
                    @click="showReturnModal(transaction.id)"
                    class="px-4 py-2 text-sm font-medium rounded-lg bg-indigo-600 text-white hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
                  >
                    Return
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </main>

    <!-- Modal for Return Confirmation -->
    <Modal v-if="isModalVisible" @close="closeModal">
      <template #header>Confirm Return</template>
      <template #body>
        <div v-if="modalError" class="text-red-600 mb-4">{{ modalError }}</div>
        Are you sure you want to return this media?
      </template>
      <template #footer>
        <button 
          @click="confirmReturn" 
          class="px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700"
        >
          Confirm
        </button>
        <button 
          @click="closeModal" 
          class="px-4 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 ml-2"
        >
          Cancel
        </button>
      </template>
    </Modal>
  </div>
</template>



<script setup>
import { ref, onMounted } from 'vue';
import NavBar from '@/components/NavBar.vue';
import Modal from '@/components/Modal.vue'; // Import the Modal component
import axiosInstance from '@/plugins/axios';

const borrowedTransactions = ref([]);
const loading = ref(true);
const error = ref(null);
const isModalVisible = ref(false); // Modal visibility state
const modalError = ref(null); // Error message in the modal
const selectedTransactionId = ref(null); // ID of the transaction being returned

const loadBorrowedTransactions = async () => {
  try {
    loading.value = true;
    error.value = null;
    const response = await axiosInstance.get('/users/borrowed-transactions');
    borrowedTransactions.value = response.data;
  } catch (err) {
    error.value = 'Failed to load borrowed media. Please try again later.';
    console.error('Error loading borrowed transactions:', err);
  } finally {
    loading.value = false;
  }
};

const showReturnModal = (transactionId) => {
  modalError.value = null; // Reset modal error
  selectedTransactionId.value = transactionId;
  isModalVisible.value = true;
};

const closeModal = () => {
  isModalVisible.value = false;
  selectedTransactionId.value = null;
  modalError.value = null;
};

const confirmReturn = async () => {
  try {
    await axiosInstance.post(`/users/borrowed-transactions/${selectedTransactionId.value}/return`);
    closeModal();
    await loadBorrowedTransactions(); // Refresh the list after returning
  } catch (err) {
    modalError.value = 'Failed to return the media. Please try again later.';
    console.error('Error returning media:', err);
  }
};

const formatDate = (date) => {
  return new Date(date).toLocaleDateString();
};

onMounted(loadBorrowedTransactions);
</script>
