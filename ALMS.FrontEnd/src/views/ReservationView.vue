<template>
  <div class="min-h-screen bg-slate-50">
    <NavBar></NavBar>
    <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <h1 class="text-2xl font-bold text-slate-900 mb-8">Pending Reservations</h1>

      <!-- Grid of Reservations -->
      <div class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        <div v-for="reservation in reservations" :key="reservation.id" 
             class="bg-white rounded-xl shadow-sm border border-slate-200 p-6">
          <div class="flex space-x-4">
            <!-- Media Image -->
            <div class="flex-shrink-0">
              <img :src="reservation.media.imgUrl" 
                   :alt="reservation.media.title"
                   class="w-24 h-24 object-cover rounded-lg" />
            </div>

            <!-- Reservation Details -->
            <div class="flex-1">
              <h3 class="font-semibold text-slate-900">{{ reservation.media.title }}</h3>
              <p class="text-sm text-slate-600 mt-1">
                Requested by: {{ reservation.user.username }}
              </p>
              <p class="text-sm text-slate-500 mt-1">
                Reserved on: {{ formatDate(reservation.reservedOn) }}
              </p>
              <p class="text-sm text-slate-500 mt-1">
                Reserved to: {{ formatDate(reservation.reservedTo) }}
              </p>

              <!-- Action Buttons -->
              <div class="mt-4 flex space-x-2">
                <button @click="handleAccept(reservation.id)"
                        class="px-3 py-1.5 text-sm font-medium rounded-lg bg-emerald-50 text-emerald-600 hover:bg-emerald-100">
                  Accept
                </button>
                <button @click="handleDeny(reservation.id)"
                        class="px-3 py-1.5 text-sm font-medium rounded-lg bg-red-50 text-red-600 hover:bg-red-100">
                  Deny
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Empty State -->
      <div v-if="reservations.length === 0" 
           class="text-center py-12 bg-white rounded-xl shadow-sm border border-slate-200">
        <p class="text-slate-600">No pending reservations</p>
      </div>
    </main>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import NavBar from '@/components/NavBar.vue';

// Dummy data for now
const reservations = ref([
  {
    id: 1,
    media: {
      id: 1,
      title: "The Great Gatsby",
      imgUrl: "https://example.com/gatsby.jpg"
    },
    user: {
      id: 1,
      username: "john_doe"
    },
    reservedOn: new Date(),
    reservedTo: new Date(new Date().setDate(new Date().getDate() + 14))
  },
  {
    id: 2,
    media: {
      id: 2,
      title: "1984",
      imgUrl: "https://example.com/1984.jpg"
    },
    user: {
      id: 2,
      username: "jane_smith"
    },
    reservedOn: new Date(),
    reservedTo: new Date(new Date().setDate(new Date().getDate() + 14))
  }
]);

const formatDate = (date) => {
  return new Date(date).toLocaleDateString();
};

const handleAccept = async (reservationId) => {
  try {
    // Will be replaced with actual API call
    console.log('Accepting reservation:', reservationId);
    reservations.value = reservations.value.filter(r => r.id !== reservationId);
  } catch (error) {
    console.error('Error accepting reservation:', error);
  }
};

const handleDeny = async (reservationId) => {
  try {
    // Will be replaced with actual API call
    console.log('Denying reservation:', reservationId);
    reservations.value = reservations.value.filter(r => r.id !== reservationId);
  } catch (error) {
    console.error('Error denying reservation:', error);
  }
};

onMounted(() => {
  // Will fetch actual reservations when API is ready
});
</script> 