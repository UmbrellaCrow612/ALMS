<template>
  <div class="min-h-screen bg-gray-100">
    <NavBar />
    <main class="container mx-auto px-4 py-8">
      <h1 class="text-3xl font-bold text-indigo-800 mb-6 text-center">User Approval Page</h1>

      <div v-if="loading" class="text-center text-gray-500">Loading users...</div>
      <div v-else-if="users.length === 0" class="text-center text-gray-500">No users pending approval.</div>
      <div v-else class="overflow-hidden rounded-lg shadow-lg">
        <table class="min-w-full bg-white border border-gray-200">
          <thead class="bg-indigo-600 text-white">
            <tr>
              <th class="px-6 py-3 text-left text-sm font-medium">First Name</th>
              <th class="px-6 py-3 text-left text-sm font-medium">Last Name</th>
              <th class="px-6 py-3 text-left text-sm font-medium">Email</th>
              <th class="px-6 py-3 text-left text-sm font-medium">Status</th>
              <th class="px-6 py-3 text-left text-sm font-medium">Action</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-200">
            <tr
              v-for="user in users"
              :key="user.id"
              class="hover:bg-gray-50"
            >
              <td class="px-6 py-4 text-sm text-gray-700">{{ user.firstName }}</td>
              <td class="px-6 py-4 text-sm text-gray-700">{{ user.lastName }}</td>
              <td class="px-6 py-4 text-sm text-gray-700">{{ user.email }}</td>
              <td class="px-6 py-4 text-sm">
  <span
    v-if="user.approved"
    class="inline-flex items-center px-2 py-1 text-xs font-medium text-green-800 bg-green-100 rounded"
  >
    Approved
  </span>
  <span
    v-else-if="user.denied"
    class="inline-flex items-center px-2 py-1 text-xs font-medium text-red-800 bg-red-100 rounded"
  >
    Denied
  </span>
  <span
    v-else
    class="inline-flex items-center px-2 py-1 text-xs font-medium text-yellow-800 bg-yellow-100 rounded"
  >
    Pending
  </span>
</td>
              <td class="px-6 py-4 text-sm flex space-x-2">
                <button
                  @click="handleApprove(user.id)"
                  :disabled="user.approved"
                  class="px-3 py-2 text-sm font-medium text-white bg-indigo-600 rounded-md hover:bg-indigo-700 focus:outline-none disabled:bg-gray-400"
                >
                  {{ user.approved ? "Approved" : "Approve" }}
                </button>
                <button
                  @click="handleDeny(user.id)"
                  :disabled="user.denied"
                  class="px-3 py-2 text-sm font-medium text-white bg-red-600 rounded-md hover:bg-red-700 focus:outline-none disabled:bg-gray-400"
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
    </main>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import axiosInstance from '@/plugins/axios'; 
import NavBar from '@/components/NavBar.vue';

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
  max-width: 1200px;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  padding: 8px 16px;
  text-align: left;
}
</style>
