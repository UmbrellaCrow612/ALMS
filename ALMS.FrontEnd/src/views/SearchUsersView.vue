<template>
    <div class="min-h-screen bg-gray-100">
      <NavBar />
      <main class="container mx-auto px-4 py-8">
        <h1 class="text-3xl font-bold text-indigo-800 mb-6 text-center">User Management</h1>
  
        <!-- Search Form -->
        <form @submit.prevent="fetchUsers" class="mb-8 grid md:grid-cols-4 gap-6">
          <input
            v-model="searchFilters.firstName"
            placeholder="First Name"
            class="p-3 border rounded w-full focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          <input
            v-model="searchFilters.lastName"
            placeholder="Last Name"
            class="p-3 border rounded w-full focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          <input
            v-model="searchFilters.email"
            placeholder="Email"
            class="p-3 border rounded w-full focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          <input
            v-model="searchFilters.phoneNumber"
            placeholder="Phone"
            class="p-3 border rounded w-full focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          <input
            v-model="searchFilters.address"
            placeholder="Address"
            class="p-3 border rounded w-full focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          <input
            v-model="searchFilters.username"
            placeholder="Username"
            class="p-3 border rounded w-full focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          <button
            type="submit"
            class="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700"
          >
            Search
          </button>
        </form>
  
        <!-- User Table -->
        <div class="bg-white shadow-md rounded-lg overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">First Name</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Last Name</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Username</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Email</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Phone</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Address</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr v-for="user in users" :key="user.id" class="hover:bg-gray-50">
                <td class="px-6 py-4 whitespace-normal max-w-[150px] break-words">{{ user.firstName }}</td>
                <td class="px-6 py-4 whitespace-normal max-w-[150px] break-words">{{ user.lastName }}</td>
                <td class="px-6 py-4 whitespace-normal max-w-[150px] break-words">{{ user.userName }}</td>
                <td class="px-6 py-4 whitespace-normal max-w-[200px] break-words">{{ user.email }}</td>
                <td class="px-6 py-4 whitespace-normal max-w-[150px] break-words">{{ user.phoneNumber }}</td>
                <td class="px-6 py-4 whitespace-normal max-w-[200px] break-words">{{ user.address }}</td>
                <td class="px-6 py-4 flex space-x-2">
                  <button
                    v-if="!isAccountant"
                    @click="editUser(user)"
                    class="text-blue-500 hover:text-blue-700"
                  >
                    Edit
                  </button>
                  <button 
                    v-else
                    @click="router.push(`/users/${user.id}`)"
                    class="text-red-500 hover:text--700"
                  >
                    Manage 
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </main>
  
      <!-- Edit User Modal -->
      <Modal v-if="isModalOpen" @close="closeModal">
        <template #header>
          <h2 class="text-xl font-semibold">Edit User Details</h2>
        </template>
        <template #body>
          <form @submit.prevent="saveUser" class="space-y-4">
            <div>
              <label class="block text-sm font-medium text-gray-700">First Name</label>
              <input
                v-model="currentUser.firstName"
                type="text"
                class="p-2 border rounded w-full"
              />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">Last Name</label>
              <input
                v-model="currentUser.lastName"
                type="text"
                class="p-2 border rounded w-full"
              />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">Email</label>
              <input
                v-model="currentUser.email"
                type="email"
                class="p-2 border rounded w-full"
                disabled
              />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">Phone</label>
              <input
                v-model="currentUser.phoneNumber"
                type="tel"
                class="p-2 border rounded w-full"
              />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">Username</label>
              <input
                v-model="currentUser.userName"
                type="text"
                class="p-2 border rounded w-full"
                disabled
              />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700">Address</label>
              <textarea
                v-model="currentUser.address"
                class="p-2 border rounded w-full"
              ></textarea>
            </div>
            <button type="submit" class="mt-4 bg-indigo-600 text-white px-4 py-2 rounded">Save</button>
          </form>
        </template>
      </Modal>
    </div>
  </template>
  
  <script setup>
  import NavBar from '@/components/NavBar.vue';
  import Modal from '@/components/Modal.vue';
  import { ref, onMounted, computed } from 'vue';
  import axiosInstance from '@/plugins/axios';
  import { useUserStore } from '@/stores/userStore';
  import { useRouter } from 'vue-router';
  
  const router = useRouter();
  const userStore = useUserStore();
  const isAccountant = computed(() => userStore.user?.roles.includes('Accountant'));
  
  const searchFilters = ref({
    firstName: '',
    lastName: '',
    address: '',
    userName: '',
    email: '',
    phoneNumber: ''
  });
  
  const users = ref([]);
  const isModalOpen = ref(false);
  const currentUser = ref({});
  
  const fetchUsers = async () => {
    try {
      const response = await axiosInstance.get('/profile/search', {
        params: { ...searchFilters.value }
      });
      users.value = response.data;
    } catch (error) {
      console.error('Error fetching users:', error);
    }
  };
  
  const editUser = (user) => {
    currentUser.value = { ...user };
    isModalOpen.value = true;
  };
  
  const closeModal = () => {
    isModalOpen.value = false;
  };
  
  const saveUser = async () => {
    try {
      const payload = {
        firstName: currentUser.value.firstName,
        lastName: currentUser.value.lastName,
        phoneNumber: currentUser.value.phoneNumber,
        address: currentUser.value.address,
      };
  
      await axiosInstance.patch(`/profile/update/${currentUser.value.id}`, payload);
  
      const index = users.value.findIndex(u => u.id === currentUser.value.id);
      if (index !== -1) {
        users.value[index] = { ...users.value[index], ...payload };
      }
  
      closeModal();
    } catch (error) {
      console.error('Error updating user:', error);
    }
  };
  
  onMounted(fetchUsers);
  </script>
  
  <style scoped>
  .container {
    max-width: 1200px;
  }
  </style>