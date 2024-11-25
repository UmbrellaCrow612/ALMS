<template>
  <div class="min-h-screen bg-gray-100">
    <NavBar />
    <main class="container mx-auto px-4 py-8">
      <div class="flex justify-between">
        <!-- User Details -->
        <div class="w-1/3 bg-white p-6 rounded-lg shadow-md">
          <h2 class="text-2xl font-bold mb-4">User Details</h2>
          <p><strong>First Name:</strong> {{ user.firstName }}</p>
          <p><strong>Last Name:</strong> {{ user.lastName }}</p>
          <p><strong>Username:</strong> {{ user.userName }}</p>
          <p><strong>Email:</strong> {{ user.email }}</p>
          <p><strong>Phone:</strong> {{ user.phoneNumber }}</p>
          <p><strong>Address:</strong> {{ user.address }}</p>
        </div>

        <!-- User Subscriptions -->
        <div class="w-2/3 bg-white p-6 rounded-lg shadow-md">
          <h2 class="text-2xl font-bold mb-4">Subscriptions</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            <div v-for="subscription in subscriptions" :key="subscription.id" class="bg-gray-50 p-4 rounded-lg shadow">
              <h3 class="font-semibold">{{ subscription.title }}</h3>
              <p>{{ subscription.description }}</p>
              <div class="mt-2 flex space-x-2">
                <button @click="editSubscription(subscription)" class="text-blue-500 hover:text-blue-700">Edit</button>
                <button @click="confirmDelete(subscription.id)" class="text-red-500 hover:text-red-700">Delete</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </main>

    <!-- Edit Subscription Modal -->
    <Modal v-if="isEditModalOpen" @close="closeEditModal">
      <!-- Modal content for editing subscription -->
    </Modal>

    <!-- Delete Confirmation Modal -->
    <Modal v-if="isDeleteModalOpen" @close="closeDeleteModal">
      <template #header>
        <h3 class="text-xl font-semibold">Confirm Delete</h3>
      </template>
      <template #body>
        <p>Are you sure you want to delete this subscription?</p>
      </template>
      <template #footer>
        <button @click="closeDeleteModal" class="bg-gray-300 text-gray-800 px-4 py-2 rounded hover:bg-gray-400">Cancel</button>
        <button @click="deleteSubscription" class="bg-red-600 text-white px-4 py-2 rounded-md hover:bg-red-700 ml-2">Confirm</button>
      </template>
    </Modal>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import NavBar from '@/components/NavBar.vue';
import Modal from '@/components/Modal.vue';
import axiosInstance from '@/plugins/axios';

const route = useRoute();
const user = ref({});
const subscriptions = ref([]);
const isEditModalOpen = ref(false);
const isDeleteModalOpen = ref(false);
const subscriptionToDelete = ref(null);

const fetchUserDetails = async () => {
  try {
    const response = await axiosInstance.get(`/users/${route.params.id}`);
    user.value = response.data;
  } catch (error) {
    console.error('Error fetching user details:', error);
  }
};

const fetchSubscriptions = async () => {
  try {
    const response = await axiosInstance.get(`/users/${route.params.id}/subscriptions`);
    subscriptions.value = response.data;
  } catch (error) {
    console.error('Error fetching subscriptions:', error);
  }
};

const editSubscription = (subscription) => {
  // Open edit modal with subscription details
  isEditModalOpen.value = true;
};

const confirmDelete = (subscriptionId) => {
  subscriptionToDelete.value = subscriptionId;
  isDeleteModalOpen.value = true;
};

const deleteSubscription = async () => {
  try {
    await axiosInstance.delete(`/subscriptions/${subscriptionToDelete.value}`);
    subscriptions.value = subscriptions.value.filter(s => s.id !== subscriptionToDelete.value);
    closeDeleteModal();
  } catch (error) {
    console.error('Error deleting subscription:', error);
  }
};

const closeEditModal = () => {
  isEditModalOpen.value = false;
};

const closeDeleteModal = () => {
  isDeleteModalOpen.value = false;
};

onMounted(() => {
  fetchUserDetails();
  fetchSubscriptions();
});
</script> 