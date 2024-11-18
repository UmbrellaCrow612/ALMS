<template>
  <div class="min-h-screen bg-gray-100">
    <NavBar></NavBar>
    <main class="container mx-auto px-4 py-8">
      <div class="grid gap-6 lg:grid-cols-3">
        <!-- Left Column - Media Photo and Borrow History -->
        <div class="space-y-6">
          <!-- Media Image Card -->
          <div class="p-4 border rounded shadow bg-white">
            <div class="aspect-square relative bg-muted rounded-lg overflow-hidden">
              <img 
                :src="mediaDetails.imgUrl" 
                alt="Media Cover" 
                class="object-cover w-full h-full"
              />
            </div>
          </div>

          <!-- Borrow History Card -->
          <div class="p-4 border rounded shadow bg-white">
            <h2 class="text-lg font-semibold mb-4 flex items-center text-theme-primary">
              <Clock class="mr-2 h-5 w-5" />
              Borrow History
            </h2>
            <div class="space-y-4">
              <div
                v-for="(history, index) in mediaDetails.borrowHistory"
                :key="index"
                class="flex items-center justify-between text-sm"
              >
                <div class="flex items-center text-muted-foreground">
                  <Calendar class="mr-2 h-4 w-4 text-theme-muted" />
                  <span>{{ history.date }}</span>
                </div>
                <span class="text-green-600">{{ history.status }}</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Right Column - Description and Similar Items -->
        <div class="lg:col-span-2 space-y-6">
          <!-- Media Details Card -->
          <div class="p-4 border rounded shadow bg-white">
            <div class="space-y-4">
              <div class="flex items-center justify-between">
                <h1 class="text-2xl font-bold text-theme-primary">{{ mediaDetails.title }}</h1>
                <span class="text-sm text-theme-muted">{{ mediaTypes[mediaDetails.mediaType] }}</span>
              </div>
              <div class="flex items-center text-theme-muted">
                <User class="mr-2 h-4 w-4" />
                <span>{{ mediaDetails.author }}</span>
              </div>
              <hr class="my-4" />
              <div class="space-y-2">
                <h2 class="font-semibold text-theme-primary">Description</h2>
                <p class="text-theme-muted">{{ mediaDetails.description }}</p>
              </div>
              <div class="grid grid-cols-2 gap-4 text-sm">
                <div>
                  <span class="font-medium">Genre:</span> {{ mediaDetails.genre }}
                </div>
                <div>
                  <span class="font-medium">Published:</span> {{ formatDate(mediaDetails.publishedAt) }}
                </div>
                <div>
                  <span class="font-medium">ISBN:</span> {{ mediaDetails.isbn }}
                </div>
                <div>
                  <span class="font-medium">Availability:</span> {{ mediaDetails.isAvailable }} 
                </div>
              </div>
            </div>
          </div>

          <!-- Similar Items Card -->
          <div class="p-4 border rounded shadow bg-white">
            <h2 class="text-lg font-semibold mb-4 text-theme-primary">Similar Items</h2>
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div
                v-for="item in similarItems"
                :key="item.id"
                class="p-4 border rounded hover:bg-muted transition"
              >
                <div class="flex items-center mb-2">
                  <img :src="item.imgUrl" alt="Cover" class="w-12 h-12 object-cover rounded mr-4" />
                  <div>
                    <h3 class="text-md font-semibold text-theme-primary">{{ item.title }}</h3>
                    <p class="text-sm text-theme-muted">{{ item.author }}</p>
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
              class="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400 disabled:bg-gray-200 disabled:cursor-not-allowed"
            >
              Reserve
            </button>
            <button 
              @click="borrowItem" 
              :disabled="!mediaDetails.isAvailable" 
              class="bg-indigo-600 text-white px-4 py-2 rounded hover:bg-indigo-700 disabled:bg-indigo-400 disabled:cursor-not-allowed"
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
.container {
  max-width: 1200px;
}

.text-theme-primary {
  color: #2c3e50;
}

.text-theme-muted {
  color: #7f8c8d;
}

.bg-muted {
  background-color: #f5f5f5;
}

.bg-indigo-600:hover {
  background-color: #4c51bf;
}
</style>
