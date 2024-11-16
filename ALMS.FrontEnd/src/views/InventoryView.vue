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
                <button @click="deleteMedia(item.id)" class="text-red-500 hover:text-red-700">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
                  </svg>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <Modal v-if="isModalOpen" @close="closeModal">
        <template #header>
          <h2 class="text-xl font-semibold">{{ editingMedia ? 'Edit Media' : 'Add Media' }}</h2>
        </template>
        <template #body>
          <form @submit.prevent="handleSubmit" class="space-y-4">
            <div class="grid grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700">Title</label>
                <input v-model="formData.title" type="text" class="p-2 border rounded w-full" required />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700">Author</label>
                <input v-model="formData.author" type="text" class="p-2 border rounded w-full" required />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700">Genre</label>
                <input v-model="formData.genre" type="text" class="p-2 border rounded w-full" required />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700">ISBN</label>
                <input v-model="formData.isbn" type="text" class="p-2 border rounded w-full" required />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700">Image URL</label>
                <input v-model="formData.imgUrl" type="text" class="p-2 border rounded w-full" required />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700">Published At</label>
                <input v-model="formData.publishedAt" type="date" class="p-2 border rounded w-full" required />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700">Media Type</label>
                <select v-model="formData.mediaType" class="p-2 border rounded w-full">
                  <option v-for="(type, index) in mediaTypes" :key="index" :value="index">
                    {{ type }}
                  </option>
                </select>
              </div>
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">Description</label>
              <textarea v-model="formData.description" class="p-2 border rounded w-full" rows="3" required></textarea>
            </div>
            <button type="submit" class="mt-4 bg-indigo-600 text-white px-4 py-2 rounded">Save</button>
          </form>
        </template>
      </Modal>
    </main>
  </div>
</template>

<script setup>
import NavBar from '@/components/NavBar.vue'
import Modal from '@/components/Modal.vue'
import { ref, computed, onMounted } from 'vue'
import axiosInstance from '@/plugins/axios'

const columns = ['Title', 'Description', 'Published At', 'ISBN', 'Author', 'Genre', 'Media Type', 'Cover']
const inventory = ref([])
const searchQuery = ref('')
const mediaTypes = ['DVD', 'Book', 'AudioBook', 'Games', 'Journal', 'Periodicals', 'CDs', 'MultimediaTitles']

const isModalOpen = ref(false)
const formData = ref({
  title: '',
  description: '',
  publishedAt: '',
  isbn: '',
  imgUrl: '',
  author: '',
  genre: '',
  mediaType: 0,
})
const editingMedia = ref(null)

const filteredInventory = computed(() => {
  const query = searchQuery.value.toLowerCase()
  return inventory.value.filter(
    (item) =>
      item.title?.toLowerCase().includes(query) ||
      item.author?.toLowerCase().includes(query) ||
      item.genre?.toLowerCase().includes(query)
  )
})

const formatDate = (date) => new Date(date).toLocaleDateString()

const loadInventory = async () => {
  try {
    const response = await axiosInstance.get('/media')
    inventory.value = response.data
  } catch (error) {
    console.error('Failed to load inventory:', error)
  }
}

const handleSubmit = async () => {
  try {
    if (editingMedia.value) {
  
      await axiosInstance.patch(`/media/${editingMedia.value.id}`, formData.value);
      const updatedItem = inventory.value.find((item) => item.id === editingMedia.value.id);
      Object.assign(updatedItem, formData.value);
    } else {
  
      await axiosInstance.post('/media', formData.value);
    }
    closeModal();
    await loadInventory(); 
  } catch (error) {
    console.error('Failed to save media:', error);
  }
};

const deleteMedia = async (id) => {
  try {
    await axiosInstance.delete(`/media/${id}`)
    inventory.value = inventory.value.filter((item) => item.id !== id)
  } catch (error) {
    console.error('Failed to delete media:', error)
  }
}

const openAddModal = () => {
  editingMedia.value = null
  formData.value = {
    title: '',
    description: '',
    publishedAt: '',
    isbn: '',
    imgUrl: '',
    author: '',
    genre: '',
    mediaType: 0,
  }
  isModalOpen.value = true
}

const openEditModal = (media) => {
  editingMedia.value = media
  formData.value = { ...media }
  isModalOpen.value = true
}

const closeModal = () => {
  isModalOpen.value = false
}

onMounted(loadInventory)
</script>

<style scoped>
.container {
  max-width: 1200px;
}
</style>
