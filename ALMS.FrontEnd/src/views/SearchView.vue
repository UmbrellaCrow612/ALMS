<template>
  <div class="min-h-screen bg-gradient-to-b from-indigo-100 to-white">
    <!-- NavBar Component -->
    <NavBar />

    <div class="container mx-auto px-4 py-8">
      <h1 class="text-3xl font-extrabold mb-8 text-center text-indigo-700">Search Media</h1>

      <!-- Search Input and Button -->
      <div class="flex flex-col md:flex-row gap-4 mb-8">
        <input
          type="search"
          v-model="searchQuery"
          placeholder="Search by title"
          class="w-full p-3 border rounded focus:outline-none focus:ring focus:ring-indigo-300"
        />
        <button @click="handleSearch" class="flex items-center px-6 py-3 bg-indigo-600 text-white font-semibold rounded hover:bg-indigo-700">
          <span v-html="searchIcon" class="mr-2"></span> Search
        </button>
      </div>

      <!-- Filters and Results -->
      <div class="grid md:grid-cols-4 gap-6">
        <!-- Filters Sidebar -->
        <div class="md:col-span-1 p-4 border rounded shadow bg-white">
          <h2 class="text-lg font-semibold mb-4 text-indigo-600">Filters</h2>
          
          <!-- Author Filter -->
          <div class="mb-4">
            <label for="author" class="block mb-1 font-semibold">Author</label>
            <input
              type="text"
              v-model="author"
              id="author"
              class="w-full p-2 border rounded"
              placeholder="Search by author"
            />
          </div>

          <!-- Genre Filter -->
          <div class="mb-4">
            <label for="genre" class="block mb-1 font-semibold">Genre</label>
            <input
              type="text"
              v-model="genre"
              id="genre"
              class="w-full p-2 border rounded"
              placeholder="e.g., Fiction, Drama"
            />
          </div>

          <!-- Media Type Filter -->
          <div class="mb-4">
            <label for="mediaType" class="block mb-1 font-semibold">Media Type</label>
            <select v-model="mediaType" id="mediaType" class="w-full p-2 border rounded">
              <option value="">All</option>
              <option v-for="(type, index) in mediaTypes" :key="index" :value="index">
                {{ type }}
              </option>
            </select>
          </div>

          <!-- Availability Filter -->
          <div>
            <label class="flex items-center space-x-2">
              <input type="checkbox" v-model="isAvailable" class="h-4 w-4" />
              <span class="text-sm">Available Only</span>
            </label>
          </div>
        </div>

        <!-- Results Section -->
        <div class="md:col-span-3">
          <div v-if="loading" class="text-center text-gray-500">
            Loading...
          </div>
          <div v-if="searchResults.length > 0" class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            <div v-for="item in searchResults" :key="item.id" class="p-4 border rounded shadow bg-white">
              <div class="flex items-center mb-2">
                <span v-html="getMediaIcon(item.mediaType)" class="h-6 w-6 mr-2 text-indigo-600"></span>
                <h3 class="text-lg font-bold truncate">{{ item.title }}</h3>
              </div>
              <p class="text-gray-600">{{ item.author }}</p>
              <p class="mt-2"><strong>Type:</strong> {{ mediaTypes[item.mediaType] }}</p>
              <p><strong>Genre:</strong> {{ item.genre }}</p>
              <p><strong>Status:</strong> {{ item.isAvailable ? 'Available' : 'Unavailable' }}</p>
              <p><strong>Date Added:</strong> {{ formatDate(item.dateAdded) }}</p>
            </div>
          </div>

          <div v-else-if="!loading" class="p-4 border rounded shadow text-center">
            <h3 class="text-lg font-bold">No Results Found</h3>
            <p>Try a different search term or adjust your filters.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import NavBar from '@/components/NavBar.vue';
import { ref } from 'vue';
import axiosInstance from '@/plugins/axios'; // Your axios instance

const searchQuery = ref('');
const author = ref('');
const genre = ref('');
const mediaType = ref('');
const isAvailable = ref(false);
const searchResults = ref([]);
const loading = ref(false);

const mediaTypes = [
  'DVD',
  'Book',
  'AudioBook',
  'Games',
  'Journal',
  'Periodicals',
  'CDs',
  'MultimediaTitles'
];

const searchIcon = `<svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 3a8 8 0 018 8m-8 8a8 8 0 100-16 8 8 0 000 16zm0 0l6 6" /></svg>`;

const handleSearch = async () => {
  loading.value = true;
  try {
    const response = await axiosInstance.get('/media', {
      params: {
        Title: searchQuery.value || undefined,
        Author: author.value || undefined,
        Genre: genre.value || undefined,
        MediaType: mediaType.value !== '' ? Number(mediaType.value) : undefined,
        IsAvailable: isAvailable.value || undefined
      }
    });
    searchResults.value = response.data;
  } catch (error) {
    console.error('Error fetching media:', error);
  } finally {
    loading.value = false;
  }
};

const getMediaIcon = (type) => {
  const icons = {
    0: `<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12a5 5 0 100 10 5 5 0 000-10zm0-5a5 5 0 110-10 5 5 0 010 10z" /></svg>`, // DVD Icon
    1: `<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.25l-1.5 1.25L9 4.25m-3 0a2 2 0 00-2 2V20a2 2 0 002 2h12a2 2 0 002-2V6.25a2 2 0 00-2-2H6z" /></svg>`, // Book Icon
  };
  return icons[type] || '';
};

const formatDate = (dateString) => {
  const options = { year: 'numeric', month: 'short', day: 'numeric' };
  return new Date(dateString).toLocaleDateString(undefined, options);
};
</script>

<style scoped>
.container {
  max-width: 1200px;
}
</style>