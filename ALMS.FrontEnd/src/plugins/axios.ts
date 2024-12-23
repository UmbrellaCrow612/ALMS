import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();

const axiosInstance = axios.create({
  baseURL: 'https://localhost:7066',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor
axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('accessToken');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

axiosInstance.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response && 
        error.response.status === 401 && 
        !error.config.url.includes('/login')) {
      router.push('/login');
    }
    return Promise.reject(error);
  }
);

export default axiosInstance;