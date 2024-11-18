<template>
  <div class="min-h-screen bg-gray-100">
    <NavBar />
    <main class="container mx-auto px-4 py-8">
      <h1 class="text-3xl font-bold text-indigo-800 mb-6 text-center">Search Media</h1>
      <form @submit.prevent="handleSearch" class="mb-8 grid md:grid-cols-4 gap-6">
        <input
          v-model="searchQuery"
          placeholder="Search by title"
          class="p-3 border rounded w-full focus:outline-none focus:ring-2 focus:ring-indigo-500"
        />
        <input
          v-model="author"
          placeholder="Author"
          class="p-3 border rounded w-full focus:outline-none focus:ring-2 focus:ring-indigo-500"
        />
        <input
          v-model="genre"
          placeholder="Genre"
          class="p-3 border rounded w-full focus:outline-none focus:ring-2 focus:ring-indigo-500"
        />
        <select v-model="mediaType" class="p-3 border rounded w-full focus:outline-none focus:ring-2 focus:ring-indigo-500">
          <option value="">All Media Types</option>
          <option v-for="(type, index) in mediaTypes" :key="index" :value="index">{{ type }}</option>
        </select>
        <div class="flex items-center space-x-2">
          <input type="checkbox" v-model="isAvailable" class="h-4 w-4" />
          <label>Available Only</label>
        </div>
        <button
          type="submit"
          class="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700"
        >
          Search
        </button>
      </form>

      <div v-if="loading" class="text-center text-gray-500">Loading...</div>

      <div v-if="searchResults.length > 0" class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
  <div v-for="item in searchResults" :key="item.id" class="p-4 border rounded shadow bg-white flex flex-col md:flex-row">
    <div class="flex-1">
      <h3 class="text-lg font-bold truncate">{{ item.title }}</h3>
      <p class="text-gray-600">{{ item.author }}</p>
      <p><strong>Type:</strong> {{ mediaTypes[item.mediaType] }}</p>
      <p><strong>Genre:</strong> {{ item.genre }}</p>
      <p><strong>Status:</strong> {{ item.isAvailable ? 'Available' : 'Unavailable' }}</p>
      <button
        @click="goToMediaDetails(item.id)"
        class="mt-2 bg-indigo-500 text-white px-3 py-1 rounded hover:bg-indigo-600"
      >
        View Details
      </button>
    </div>
    <div class="md:ml-4 mt-4 md:mt-0 flex-shrink-0">
      <img :src="item.imgUrl" alt="Media Cover" class="w-32 h-32 object-cover rounded" />
    </div>
  </div>
</div>


      <div v-else-if="!loading" class="text-center text-gray-500">No results found.</div>
    </main>
  </div>
</template>

<script setup>
import NavBar from '@/components/NavBar.vue';
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import axiosInstance from '@/plugins/axios';

const searchQuery = ref('');
const author = ref('');
const genre = ref('');
const mediaType = ref('');
const isAvailable = ref(false);
const searchResults = ref([]);
const loading = ref(false);
const router = useRouter();

const mediaTypes = [
  'DVD',
  'Book',
  'AudioBook',
  'Games',
  'Journal',
  'Periodicals',
  'CDs',
  'MultimediaTitles',
];

const fetchAllMedia = async () => {
  loading.value = true;
  try {
    const response = await axiosInstance.get('/media');
    searchResults.value = response.data;
  } catch (error) {
    console.error('Error fetching media:', error);
  } finally {
    loading.value = false;
  }
};

const handleSearch = async () => {
  loading.value = true;
  try {
    const response = await axiosInstance.get('/media', {
      params: {
        Title: searchQuery.value || undefined,
        Author: author.value || undefined,
        Genre: genre.value || undefined,
        MediaType: mediaType.value !== '' ? Number(mediaType.value) : undefined,
        IsAvailable: isAvailable.value || undefined,
      },
    });
    searchResults.value = response.data.filter(item => {
      return (
        (!genre.value || item.genre?.toLowerCase() === genre.value.toLowerCase()) &&
        (!mediaType.value || item.mediaType === Number(mediaType.value))
      );
    });
  } catch (error) {
    console.error('Error fetching media:', error);
  } finally {
    loading.value = false;
  }
};

const goToMediaDetails = (id) => {
  router.push({ name: 'mediaDetail', params: { id } });
};

onMounted(() => {
  fetchAllMedia();
});
</script>

