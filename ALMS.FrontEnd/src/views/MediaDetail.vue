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
            <div class="space-y-5">
              <div
                v-for="(history, index) in mediaDetails.borrowHistory"
                :key="index"
                class="flex items-center justify-between text-sm"
              >
                <div class="flex items-center text-slate-600">
                  <Calendar class="mr-2 h-4 w-4" />
                  <span>{{ history.date }}</span>
                </div>
                <span class="font-medium text-emerald-600">{{ history.status }}</span>
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
                <span class="px-3 py-1 text-sm font-medium bg-slate-100 text-slate-700 rounded-full">{{ mediaTypes[mediaDetails.mediaType] }}</span>
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
              :disabled="!mediaDetails.isAvailable"
              class="px-6 py-2.5 text-sm font-medium rounded-lg transition-all duration-200 bg-emerald-50 text-emerald-600 hover:bg-emerald-100 disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:bg-emerald-50 focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:ring-offset-2"
            >
              Reserve
            </button>
            <button 
              @click="borrowItem" 
              :disabled="!mediaDetails.isAvailable" 
              class="px-6 py-2.5 text-sm font-medium rounded-lg transition-all duration-200 bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
            >
              Borrow
            </button>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>


<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import axiosInstance from '@/plugins/axios';
import { Clock, Calendar, User } from 'lucide-vue-next';
import NavBar from '@/components/NavBar.vue';

const route = useRoute();
const mediaDetails = ref({});
const similarItems = ref([]);
const mediaTypes = ['DVD', 'Book', 'AudioBook', 'Games', 'Journal', 'Periodicals', 'CDs', 'MultimediaTitles'];

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

const formatDate = (date) => new Date(date).toLocaleDateString();

const reserveItem = async () => {
  alert('Reserve functionality is under development.');
};

const borrowItem = async () => {
  try {
    await axiosInstance.post(`/media/${route.params.id}/borrow`);
    alert('Item borrowed successfully!');
  } catch (error) {
    console.error('Failed to borrow item:', error);
  }
};

onMounted(loadMediaDetails);
</script>



<style scoped>
/* Removed custom styles in favor of Tailwind utilities */
</style>
