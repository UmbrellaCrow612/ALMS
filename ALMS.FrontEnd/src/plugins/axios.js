import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: 'https://localhost:7066', // Replace with your API base URL
  timeout: 10000, // Request timeout (optional)
  headers: {
    'Content-Type': 'application/json', // Default headers (optional)
  },
});

export default axiosInstance;
