<template>
  <div class="min-h-screen bg-slate-50">
    <NavBar />
    <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6">
        <h1 class="text-2xl font-bold text-slate-900 mb-6">My Media</h1>

        <div v-if="loading" class="text-center py-12">
          <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mx-auto"></div>
          <p class="mt-4 text-slate-600">Loading your media...</p>
        </div>

        <div v-else-if="error" class="text-center py-12">
          <p class="text-red-600">{{ error }}</p>
        </div>

        <div v-else-if="media.length === 0" class="text-center py-12">
          <p class="text-slate-600">You haven't borrowed any media yet.</p>
        </div>

        <div v-else class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          <div v-for="media in medias" :key="media.id" class="bg-white rounded-lg border border-slate-200 overflow-hidden">
            <div class="aspect-[3/4] relative">
              <img 
                :src="media.imgUrl" 
                :alt="media.title"
                class="absolute inset-0 w-full h-full object-cover"
              />
            </div>
            <div class="p-4">
              <h3 class="font-semibold text-slate-900">{{ media.title }}</h3>
              <p class="text-sm text-slate-600 mt-1">Due: {{ formatDate(media.dueDate) }}</p>
              <div class="mt-4 flex justify-end">
                <button 
                  @click="returnBook(media.id)"
                  class="px-4 py-2 text-sm font-medium rounded-lg bg-indigo-600 text-white hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
                >
                  Return
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import NavBar from '@/components/NavBar.vue';
import axiosInstance from '@/plugins/axios';

const medias = ref([]);
const loading = ref(true);
const error = ref(null);

const loadMedia = async () => {
  try {
    loading.value = true;
    error.value = null;
    // Endpoint to be implemented
    const response = await axiosInstance.get('/media/borrowed');
    books.value = response.data;
  } catch (err) {
    error.value = 'Failed to load your media. Please try again later.';
    console.error('Error loading media:', err);
  } finally {
    loading.value = false;
  }
};

const returnBook = async (mediaId) => {
  try {
    await axiosInstance.post(`/media/${mediaId}/return`);
    await loadMedia(); 
  } catch (err) {
    console.error('Error returning media:', err);

  }
};

const formatDate = (date) => {
  return new Date(date).toLocaleDateString();
};

onMounted(loadMedia);
</script>
