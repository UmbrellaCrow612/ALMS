<template>
  <div class="min-h-screen bg-gradient-to-br from-gray-100 to-gray-200">
    <NavBar />
    <main class="container mx-auto px-4 py-8">
      <div class="flex flex-col lg:flex-row gap-8">
        <!-- User Details -->
        <div class="w-full lg:w-1/3">
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h2 class="text-2xl font-bold mb-6 text-indigo-800 border-b pb-2">User Details</h2>
            <div class="space-y-3">
              <p><span class="font-semibold text-gray-600">First Name:</span> {{ user.firstName }}</p>
              <p><span class="font-semibold text-gray-600">Last Name:</span> {{ user.lastName }}</p>
              <p><span class="font-semibold text-gray-600">Username:</span> {{ user.userName }}</p>
              <p><span class="font-semibold text-gray-600">Email:</span> {{ user.email }}</p>
              <p><span class="font-semibold text-gray-600">Phone:</span> {{ user.phoneNumber }}</p>
              <p><span class="font-semibold text-gray-600">Address:</span> {{ user.address }}</p>
            </div>
          </div>
        </div>

        <!-- User Subscriptions -->
        <div class="w-full lg:w-2/3">
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h2 class="text-2xl font-bold mb-6 text-indigo-800 border-b pb-2">Subscriptions</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div v-for="subscription in subscriptions" :key="subscription.id" 
                   class="bg-gray-50 p-4 rounded-lg shadow-sm hover:shadow-md transition-shadow duration-200">
                <h3 class="font-semibold text-lg text-gray-800 mb-2">{{ subscription.subscriptionType }}</h3>
                <p class="text-gray-600 mb-4">Amount: ${{ subscription.amount }}</p>
                <p class="text-gray-600 mb-4">Start: {{ formatDate(subscription.startDate) }}</p>
                <p class="text-gray-600 mb-4">End: {{ formatDate(subscription.endDate) }}</p>
                <div class="flex justify-end space-x-2">
                  <button @click="editSubscription(subscription)" 
                          class="px-3 py-1 bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors duration-200">
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                      <path d="M17.414 2.586a2 2 0 00-2.828 0L7 10.172V13h2.828l7.586-7.586a2 2 0 000-2.828zM4 12v4h4l10-10-4-4L4 12z" />
                    </svg>
                  </button>
                  <button @click="confirmDelete(subscription.id)" 
                          class="px-3 py-1 bg-red-500 text-white rounded hover:bg-red-600 transition-colors duration-200">
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                      <path d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z" />
                    </svg>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </main>

    <!-- Edit Subscription Modal -->
    <div v-if="isEditModalOpen" class="fixed inset-0 bg-gray-500 bg-opacity-75 flex items-center justify-center">
      <div class="bg-white p-6 rounded-lg shadow-xl w-full max-w-md">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Edit Subscription</h3>
        <div class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700">Amount</label>
            <input type="number" v-model="currentSubscription.amount" 
                   class="mt-1 block w-full p-2 border rounded-md"/>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700">Subscription Type</label>
            <select v-model="currentSubscription.subscriptionType" 
                    class="mt-1 block w-full p-2 border rounded-md">
              <option value="Basic">Basic</option>
              <option value="Silver">Silver</option>
            </select>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700">Start Date</label>
            <input type="date" :value="formatDateForInput(currentSubscription.startDate)" disabled
                   class="mt-1 block w-full p-2 border rounded-md bg-gray-100"/>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700">End Date</label>
            <input  type="date" v-model="currentSubscription.endDate"
                   class="mt-1 block w-full p-2 border rounded-md"/>
          </div>
        </div>
        <div class="mt-6 flex justify-end space-x-4">
          <button @click="saveSubscriptionChanges" 
                  class="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700">
            Save Changes
          </button>
          <button @click="closeEditModal" 
                  class="px-4 py-2 bg-gray-300 text-gray-700 rounded hover:bg-gray-400">
            Cancel
          </button>
        </div>
      </div>
    </div>

    <!-- Delete Confirmation Modal -->
    <div v-if="isDeleteModalOpen" class="fixed inset-0 bg-gray-500 bg-opacity-75 flex items-center justify-center">
      <div class="bg-white p-6 rounded-lg shadow-xl w-full max-w-md">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Confirm Delete</h3>
        <p class="text-gray-600 mb-6">Are you sure you want to delete this subscription? This action cannot be undone.</p>
        <div class="flex justify-end space-x-4">
          <button @click="deleteSubscription()" 
                  class="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700">
            Delete
          </button>
          <button @click="closeDeleteModal" 
                  class="px-4 py-2 bg-gray-300 text-gray-700 rounded hover:bg-gray-400">
            Cancel
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import axiosInstance from '@/plugins/axios';
import NavBar from '@/components/NavBar.vue';

const route = useRoute();
const user = ref({});
const subscriptions = ref([]);
const isDeleteModalOpen = ref(false);
const isEditModalOpen = ref(false);
const subscriptionToDelete = ref(null);
const currentSubscription = ref({});

const fetchUserDetails = async () => {
  try {
    const response = await axiosInstance.get(`/users/${route.params.id}`);
    user.value = response.data;
    if (!user.value) {
      throw new Error('User not found');
    }
  } catch (error) {
    console.error('Error fetching user details:', error);
  }
};

const fetchSubscriptions = async () => {
  try {
    const response = await axiosInstance.get(`/y-subscriptions/users/${route.params.id}/subscriptions`);
    subscriptions.value = response.data;
    
    subscriptions.value.forEach(subscription => {
      subscription.endDate = formatDate(subscription.endDate);
    });

  } catch (error) {
    console.error('Error fetching subscriptions:', error);
  }
};

const confirmDelete = (subscriptionId) => {
  subscriptionToDelete.value = subscriptionId;
  isDeleteModalOpen.value = true;
};

const deleteSubscription = async () => {
  try {
    await axiosInstance.delete(`/y-subscriptions/subscription/${subscriptionToDelete.value}`);
    await fetchSubscriptions(); 
    closeDeleteModal();
  } catch (error) {
    console.error('Error deleting subscription:', error);
  }
};

const closeDeleteModal = () => {
  isDeleteModalOpen.value = false;
  subscriptionToDelete.value = null;
};

const editSubscription = (subscription) => {
  currentSubscription.value = { ...subscription };
  isEditModalOpen.value = true;
};

const closeEditModal = () => {
  isEditModalOpen.value = false;
  currentSubscription.value = {};
};

const saveSubscriptionChanges = async () => {
  try {
    const payload = {
      amount: currentSubscription.value.amount,
      subscriptionType: currentSubscription.value.subscriptionType,
      endDate: new Date(currentSubscription.value.endDate).toISOString().split('T')[0]
    };
    await axiosInstance.patch(`/y-subscriptions/subscription/${currentSubscription.value.id}`, payload);
    await fetchSubscriptions(); 
    closeEditModal();
  } catch (error) {
    console.error('Error updating subscription:', error);
  }
};

const formatDate = (date) => new Date(date).toISOString().slice(0, 10);

const formatDateForInput = (date) => {
  return new Date(date).toISOString().split('T')[0];
};

onMounted(() => {
  fetchUserDetails();
  fetchSubscriptions();
});
</script>