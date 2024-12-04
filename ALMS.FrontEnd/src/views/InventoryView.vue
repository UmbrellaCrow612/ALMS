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
          class="p-2 border border-black rounded-md w-64 focus:outline-none focus:ring-2 focus:ring-indigo-500"
        />
        <button
          @click="openAddModal"
          class="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700"
        >
          Add Media
        </button>
      </div>

      <div class="bg-white shadow-md rounded-lg overflow-y-scroll h-[650px]">
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
              <td class="px-6 py-4">{{ item.isAvailable ? 'Available' : 'Unavailable' }}</td>
              <td class="px-6 py-4">
                <img :src="item.imgUrl" alt="Cover" class="w-16 h-16 object-cover rounded" />
              </td>
              <td class="px-6 py-4 flex space-x-2">
                <button @click="openEditModal(item)" class="text-blue-500 hover:text-blue-700">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 20h9M16.5 3.5l4 4L7 21H3v-4L16.5 3.5z" />
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
            <div>
              <label class="block text-sm font-medium text-gray-700">Title</label>
              <input type="text" v-model="formData.title" class="mt-1 block w-full rounded-md border-black shadow-sm focus:border-indigo-500 focus:ring-indigo-500" required />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">Description</label>
              <textarea v-model="formData.description" rows="3" class="mt-1 block w-full rounded-md border-black shadow-sm focus:border-indigo-500 focus:ring-indigo-500"></textarea>
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">Published At</label>
              <input type="date" v-model="formData.publishedAt" class="mt-1 block w-full rounded-md border-black shadow-sm focus:border-indigo-500 focus:ring-indigo-500" />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">ISBN</label>
              <input type="text" v-model="formData.isbn" class="mt-1 block w-full rounded-md border-black shadow-sm focus:border-indigo-500 focus:ring-indigo-500" />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">Image URL</label>
              <input type="url" v-model="formData.imgUrl" class="mt-1 block w-full rounded-md border-black shadow-sm focus:border-indigo-500 focus:ring-indigo-500" />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">Author</label>
              <input type="text" v-model="formData.author" class="mt-1 block w-full rounded-md border-black shadow-sm focus:border-indigo-500 focus:ring-indigo-500" />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">Genre</label>
              <input type="text" v-model="formData.genre" class="mt-1 block w-full rounded-md border-black shadow-sm focus:border-indigo-500 focus:ring-indigo-500" />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">Media Type</label>
              <select v-model="formData.mediaType" class="mt-1 block w-full rounded-md border-black shadow-sm focus:border-indigo-500 focus:ring-indigo-500">
                <option v-for="(type, index) in mediaTypes" :key="index" :value="index">{{ type }}</option>
              </select>
            </div>
            <div v-if="editingMedia">
              <label class="block text-sm font-medium text-gray-700">Is Lost</label>
              <select v-model="formData.isLost" class="mt-1 block w-full rounded-md border-black shadow-sm focus:border-indigo-500 focus:ring-indigo-500">
                <option :value="true">True</option>
                <option :value="false">False</option>
              </select>
            </div>
            <div class="flex justify-end space-x-3 mt-6">
              <button type="button" @click="closeModal" class="px-4 py-2 border border-gray-300 rounded-md text-sm font-medium text-gray-700 hover:bg-gray-50">
                Cancel
              </button>
              <button type="submit" class="px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
                {{ editingMedia ? 'Update' : 'Create' }}
              </button>
            </div>
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

const columns = ['Title', 'Description', 'Published At', 'ISBN', 'Author', 'Genre', 'Media Type', 'Availability', 'Cover'];
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
  isLost: false,
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

const openAddModal = () => {
  editingMedia.value = null;
  formData.value = {
    title: '',
    description: '',
    publishedAt: '',
    isbn: '',
    imgUrl: '',
    author: '',
    genre: '',
    mediaType: 0,
    isLost: false,
  };
  isModalOpen.value = true;
};

const openEditModal = (item) => {
  editingMedia.value = item;
  formData.value = { ...item };
  isModalOpen.value = true;
};

const closeModal = () => {
  isModalOpen.value = false;
  editingMedia.value = null;
};

const handleSubmit = async () => {
  try {
    if (editingMedia.value) {
      await axiosInstance.patch(`/media/${editingMedia.value.id}`, formData.value);
      const index = inventory.value.findIndex(item => item.id === editingMedia.value.id);
      if (index !== -1) {
        inventory.value[index] = { ...editingMedia.value, ...formData.value };
      }
    } else {
      const response = await axiosInstance.post('/media', formData.value);
      await loadInventory(); // Reload inventory after adding new media
    }
    closeModal();
  } catch (error) {
    console.error('Failed to save media:', error);
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
