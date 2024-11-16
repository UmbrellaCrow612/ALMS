<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
    <div class="bg-white rounded-lg shadow-lg w-full max-w-md p-6 relative">
      <!-- Header Section -->
      <div class="flex justify-between items-center border-b pb-3 mb-4">
        <slot name="header">
          <h3 class="text-xl font-semibold">Modal Title</h3>
        </slot>
        <button @click="close" class="text-gray-400 hover:text-gray-600">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>

      <!-- Body Section -->
      <div class="mb-4">
        <slot name="body">
          <p>This is the modal content.</p>
        </slot>
      </div>

      <!-- Footer Section -->
      <div class="flex justify-end border-t pt-3">
        <slot name="footer">
          <button @click="close" class="bg-gray-300 text-gray-800 px-4 py-2 rounded hover:bg-gray-400">
            Close
          </button>
        </slot>
      </div>
    </div>
  </div>
</template>

<script setup>
defineProps({
  isOpen: {
    type: Boolean,
    required: true,
  },
});

const emit = defineEmits(['close']);

const close = () => {
  emit('close');
};
</script>

<style scoped>
.fixed {
  backdrop-filter: blur(5px);
}
</style>
