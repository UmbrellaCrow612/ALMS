<template>
  <div class="min-h-screen bg-gray-100">
    <NavBar />
    <main class="container mx-auto px-4 py-8">
      <h1 class="text-3xl font-bold text-indigo-800 mb-6">Media Inventory</h1>

      <div class="mb-6 flex justify-between items-center">
        <input
          type="search"
          placeholder="Search inventory..."
          v-model="searchQuery"
          class="p-2 border border-gray-300 rounded-md w-64 focus:outline-none focus:ring-2 focus:ring-indigo-500"
        />
        <button
          @click="openAddModal"
          class="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700"
        >
          Add Media
        </button>
      </div>

      <div class="bg-white shadow-md rounded-lg overflow-hidden">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th v-for="column in columns" :key="column" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                {{ column }}
              </th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Actions
              </th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-for="item in filteredInventory" :key="item.id" class="hover:bg-gray-50">
              <td class="px-6 py-4">{{ item.title }}</td>
              <td class="px-6 py-4">{{ item.description }}</td>
              <td class="px-6 py-4">{{ formatDate(item.publishedAt) }}</td>
              <td class="px-6 py-4">{{ item.isbn }}</td>
              <td class="px-6 py-4">{{ item.author }}</td>
              <td class="px-6 py-4">{{ item.genre }}</td>
              <td class="px-6 py-4">{{ mediaTypes[item.mediaType] }}</td>
              <td class="px-6 py-4">
                <img :src="item.imgUrl" alt="Cover" class="w-16 h-16 object-cover rounded" />
              </td>
              <td class="px-6 py-4 flex space-x-2">
                <button @click="openEditModal(item)" class="text-blue-500 hover:text-blue-700">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5M18.5 2.5L21.5 5.5M21.5 2.5L18.5 5.5M12 11l9-9"></path>
                  </svg>
                </button>
                <button @click="confirmDelete(item)" class="text-red-500 hover:text-red-700">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
                  </svg>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Add/Edit Modal -->
      <Modal v-if="isModalOpen" @close="closeModal">
        <template #header>
          <h2 class="text-xl font-semibold">{{ editingMedia ? 'Edit Media' : 'Add Media' }}</h2>
        </template>
        <template #body>
          <form @submit.prevent="handleSubmit" class="space-y-4">
            <!-- Form Fields Here -->
          </form>
        </template>
      </Modal>

      <!-- Delete Confirmation Modal -->
      <Modal v-if="isDeleteModalOpen" @close="closeDeleteModal">
        <template #header>
          <h3 class="text-xl font-semibold">Confirm Delete</h3>
        </template>
        <template #body>
          <p>Are you sure you want to delete <strong>{{ itemToDelete?.title }}</strong>?</p>
        </template>
        <template #footer>
          <button @click="closeDeleteModal" class="bg-gray-300 text-gray-800 px-4 py-2 rounded hover:bg-gray-400">
            Cancel
          </button>
          <button @click="deleteMedia(itemToDelete.id)" class="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700 ml-2">
            Confirm
          </button>
        </template>
      </Modal>
    </main>
  </div>
</template>

<script setup>
import NavBar from '@/components/NavBar.vue';
import Modal from '@/components/Modal.vue';
import { ref, computed, onMounted } from 'vue';
import axiosInstance from '@/plugins/axios';

const columns = ['Title', 'Description', 'Published At', 'ISBN', 'Author', 'Genre', 'Media Type', 'Cover'];
const inventory = ref([]);
const searchQuery = ref('');
const mediaTypes = ['DVD', 'Book', 'AudioBook', 'Games', 'Journal', 'Periodicals', 'CDs', 'MultimediaTitles'];

const isModalOpen = ref(false);
const isDeleteModalOpen = ref(false);
const itemToDelete = ref(null);
const formData = ref({
  title: '',
  description: '',
  publishedAt: '',
  isbn: '',
  imgUrl: '',
  author: '',
  genre: '',
  mediaType: 0,
});
const editingMedia = ref(null);

const filteredInventory = computed(() => {
  const query = searchQuery.value.toLowerCase();
  return inventory.value.filter(
    (item) =>
      item.title?.toLowerCase().includes(query) ||
      item.author?.toLowerCase().includes(query) ||
      item.genre?.toLowerCase().includes(query)
  );
});

const formatDate = (date) => new Date(date).toLocaleDateString();

const loadInventory = async () => {
  try {
    const response = await axiosInstance.get('/media');
    inventory.value = response.data;
  } catch (error) {
    console.error('Failed to load inventory:', error);
  }
};

const confirmDelete = (item) => {
  itemToDelete.value = item;
  isDeleteModalOpen.value = true;
};

const deleteMedia = async (id) => {
  try {
    await axiosInstance.delete(`/media/${id}`);
    inventory.value = inventory.value.filter((item) => item.id !== id);
    closeDeleteModal();
  } catch (error) {
    console.error('Failed to delete media:', error);
  }
};

const closeDeleteModal = () => {
  isDeleteModalOpen.value = false;
};

onMounted(loadInventory);
</script>

<style scoped>
.container {
  max-width: 1200px;
}
</style>
