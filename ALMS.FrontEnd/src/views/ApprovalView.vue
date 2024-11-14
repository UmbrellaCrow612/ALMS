<template>
    <div class="container mx-auto py-10 px-4">
      <h1 class="text-2xl font-bold mb-6 text-center">User Approval Page</h1>
      <div v-if="loading" class="text-center">Loading users...</div>
      <div v-else-if="users.length === 0" class="text-center">No users pending approval.</div>
      <div v-else class="overflow-x-auto shadow border border-gray-200 rounded-lg">
        <table class="min-w-full bg-white">
          <thead>
            <tr class="bg-indigo-600 text-white">
              <th class="px-6 py-3 text-left text-sm font-medium">First Name</th>
              <th class="px-6 py-3 text-left text-sm font-medium">Last Name</th>
              <th class="px-6 py-3 text-left text-sm font-medium">Email</th>
              <th class="px-6 py-3 text-left text-sm font-medium">Status</th>
              <th class="px-6 py-3 text-left text-sm font-medium">Action</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="user in users"
              :key="user.id"
              class="even:bg-gray-50 hover:bg-gray-100"
            >
              <td class="px-6 py-4 text-sm text-gray-700">{{ user.firstName }}</td>
              <td class="px-6 py-4 text-sm text-gray-700">{{ user.lastName }}</td>
              <td class="px-6 py-4 text-sm text-gray-700">{{ user.email }}</td>
              <td class="px-6 py-4 text-sm">
                <span
                  v-if="user.approved"
                  class="inline-flex items-center text-green-600"
                >
                  Approved
                </span>
                <span
                  v-if="user.denied"
                  class="inline-flex items-center text-red-600"
                >
                  Denied
                </span>
                <span
                  v-else
                  class="inline-flex items-center text-yellow-600"
                >
                  Pending
                </span>
              </td>
              <td class="px-6 py-4 flex space-x-2">
                <button
                  @click="handleApprove(user.id)"
                  :disabled="user.approved"
                  class="px-4 py-2 text-sm font-medium text-white rounded-md shadow focus:outline-none disabled:bg-gray-400 bg-indigo-600 hover:bg-indigo-700"
                >
                  {{ user.approved ? "Approved" : "Approve" }}
                </button>
                <button
                  @click="handleDeny(user.id)"
                  :disabled="user.denied"
                  class="px-4 py-2 text-sm font-medium text-white rounded-md shadow focus:outline-none disabled:bg-gray-400 bg-indigo-600 hover:bg-indigo-700"
                >
                  {{ user.denied ? "Denied" : "Deny" }}
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div
        v-if="notification.message"
        class="mt-4 p-4 rounded-lg border bg-gray-100 text-gray-800"
      >
        {{ notification.message }}
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import axiosInstance from '@/plugins/axios'; 
  
  const users = ref([]);
  const loading = ref(true);
  const notification = ref({ message: '' });
  
  const fetchUsers = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get('/auth/users/un-approved'); 
      users.value = response.data;
    } catch (error) {
      notification.value.message = 'Failed to load users.';
      console.error(error);
    } finally {
      loading.value = false;
    }
  };
  
  const handleApprove = async (id) => {
    try {
      await axiosInstance.post(`/auth/users/${id}/approve`);
      const user = users.value.find((user) => user.id === id);
      if (user) {
        user.approved = true;
        notification.value.message = `User ${user.firstName} ${user.lastName} has been approved.`;
      }
    } catch (error) {
      notification.value.message = 'Failed to approve user.';
      console.error(error);
    } finally {
      setTimeout(() => {
        notification.value.message = '';
      }, 3000);
    }
  };

  const handleDeny = async (id) => {
    try {
      await axiosInstance.post(`/auth/users/${id}/deny`);
      const user = users.value.find((user) => user.id === id);
      if (user) {
        user.denied = true;
        notification.value.message = `User ${user.firstName} ${user.lastName} has been denied.`;
      }
    } catch (error) {
      notification.value.message = 'Failed to deny user.';
      console.error(error);
    } finally {
      setTimeout(() => {
        notification.value.message = '';
      }, 3000);
    }
  };
  
  onMounted(() => {
    fetchUsers();
  });
  </script>
  
  <style scoped>
  .container {
    max-width: 800px;
  }
  table {
    width: 100%;
    border-collapse: collapse;
  }
  th,
  td {
    border: 1px solid #ddd;
    text-align: left;
  }
  </style>
  