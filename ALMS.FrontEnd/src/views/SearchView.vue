<template>
    <div class="container mx-auto px-4 py-8">
      <h1 class="text-3xl font-bold mb-8">Search Media</h1>
      
      <!-- Search Input and Button -->
      <div class="flex flex-col md:flex-row gap-4 mb-8">
        <input
          type="search"
          v-model="searchQuery"
          placeholder="Search for books, DVDs, CDs..."
          class="w-full p-2 border rounded"
        />
        <button @click="handleSearch" class="flex items-center px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600">
          <span v-html="searchIcon" class="mr-2 h-4 w-4"></span> Search
        </button>
      </div>
      
      <!-- Filters and Results -->
      <div class="grid md:grid-cols-4 gap-6">
        
        <!-- Filters Sidebar -->
        <div class="md:col-span-1 p-4 border rounded shadow">
          <h2 class="text-lg font-semibold mb-4">Filters</h2>
          <div class="mb-4">
            <label for="mediaType" class="block mb-1 font-semibold">Media Type</label>
            <select v-model="mediaType" id="mediaType" class="w-full p-2 border rounded">
              <option value="All">All</option>
              <option value="Book">Books</option>
              <option value="DVD">DVDs</option>
              <option value="CD">CDs</option>
            </select>
          </div>
          <div>
            <label for="genre" class="block mb-1 font-semibold">Genre</label>
            <select v-model="genre" id="genre" class="w-full p-2 border rounded">
              <option value="All">All</option>
              <option value="Fiction">Fiction</option>
              <option value="Science Fiction">Science Fiction</option>
              <option value="Drama">Drama</option>
              <option value="Romance">Romance</option>
              <option value="Crime">Crime</option>
              <option value="Rock">Rock</option>
            </select>
          </div>
        </div>
        
        <!-- Results Section -->
        <div class="md:col-span-3">
          <div v-if="searchResults.length > 0" class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            <div v-for="item in searchResults" :key="item.id" class="p-4 border rounded shadow">
              <div class="flex items-center mb-2">
                <span v-html="getMediaIcon(item.type)" class="h-6 w-6 mr-2"></span>
                <h3 class="text-lg font-bold truncate">{{ item.title }}</h3>
              </div>
              <p class="text-gray-600">{{ item.author || item.director || item.artist }}</p>
              <p class="mt-2"><strong>Type:</strong> {{ item.type }}</p>
              <p><strong>Genre:</strong> {{ item.genre }}</p>
              <button @click="goToDetail(item.id)" class="w-full mt-4 px-4 py-2 border rounded text-blue-600 border-blue-600 hover:bg-blue-50">
          View Details
        </button>
            </div>
          </div>
          
          <div v-else class="p-4 border rounded shadow">
            <h3 class="text-lg font-bold">No Results Found</h3>
            <p>Sorry, we couldn't find any media matching your search criteria. Please try a different search term or adjust your filters.</p>
          </div>
        </div>
        
      </div>
    </div>
  </template>
  
  <script>
  import { ref, watch } from 'vue'
  import { useRouter, useRoute } from 'vue-router'
  
  // Dummy data for demonstration
  const dummyMedia = [
    { id: 1, title: "To Kill a Mockingbird", author: "Harper Lee", type: "Book", genre: "Fiction" },
    { id: 2, title: "1984", author: "George Orwell", type: "Book", genre: "Science Fiction" },
    { id: 3, title: "The Shawshank Redemption", director: "Frank Darabont", type: "DVD", genre: "Drama" },
    { id: 4, title: "Abbey Road", artist: "The Beatles", type: "CD", genre: "Rock" },
    { id: 5, title: "Inception", director: "Christopher Nolan", type: "DVD", genre: "Science Fiction" },
    { id: 6, title: "Pride and Prejudice", author: "Jane Austen", type: "Book", genre: "Romance" },
    { id: 7, title: "The Dark Side of the Moon", artist: "Pink Floyd", type: "CD", genre: "Rock" },
    { id: 8, title: "The Godfather", director: "Francis Ford Coppola", type: "DVD", genre: "Crime" },
  ]
  
  export default {
    name: 'SearchPage',
    setup() {
      const router = useRouter()
      const route = useRoute()
      const searchQuery = ref(route.query.q || '')
      const mediaType = ref('All')
      const genre = ref('All')
      const searchResults = ref(dummyMedia)
  
      const handleSearch = () => {
        let filteredResults = dummyMedia.filter(item =>
          item.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
          (item.author && item.author.toLowerCase().includes(searchQuery.value.toLowerCase())) ||
          (item.director && item.director.toLowerCase().includes(searchQuery.value.toLowerCase())) ||
          (item.artist && item.artist.toLowerCase().includes(searchQuery.value.toLowerCase()))
        )
  
        if (mediaType.value !== 'All') {
          filteredResults = filteredResults.filter(item => item.type === mediaType.value)
        }
  
        if (genre.value !== 'All') {
          filteredResults = filteredResults.filter(item => item.genre === genre.value)
        }
  
        searchResults.value = filteredResults
        router.push({ name: 'search', query: { q: searchQuery.value } })
      }
  
      const getMediaIcon = (type) => {
        switch (type) {
          case 'Book':
            return `<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.25l-1.5 1.25L9 4.25m-3 0a2 2 0 00-2 2V20a2 2 0 002 2h12a2 2 0 002-2V6.25a2 2 0 00-2-2H6z" /></svg>`
          case 'CD':
            return `<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8a4 4 0 110 8 4 4 0 010-8zm0 8v1m0-14V4m8 8h1m-14 0H4m9-9a9 9 0 100 18 9 9 0 000-18z" /></svg>`
          case 'DVD':
            return `<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12a5 5 0 100 10 5 5 0 000-10zM12 12a5 5 0 100-10 5 5 0 000 10zm0 10v2m0-14V2m8 8h2M4 10H2" /></svg>`
          default:
            return ''
        }
      }
  
      const searchIcon = `<svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 3a8 8 0 018 8m-8 8a8 8 0 100-16 8 8 0 000 16zm0 0l6 6" /></svg>`
      
      const goToDetail = (id) => {
      router.push({ name: 'mediaDetail', params: { id } })
    }

      watch([searchQuery, mediaType, genre], handleSearch)
  
      return {
        searchQuery,
        mediaType,
        genre,
        searchResults,
        handleSearch,
        getMediaIcon,
        goToDetail,
        searchIcon
      }
    }
  }

  </script>
  
  <style scoped>
  .container {
    max-width: 1200px;
  }
  </style>
  