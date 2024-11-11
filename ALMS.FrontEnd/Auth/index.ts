import axios from 'axios';

const API_URL = 'https://localhost:7066/auth';

export const login = async (email: string, password: string) => {
  try {
    const response = await axios.post(`${API_URL}/login`, {
      email,
      password,
    });
    return response.data; // This contains the accessToken
  } catch (error: any) {
    if (error.response) {
      throw new Error(error.response.data || 'Login failed');
    } else {
      throw new Error('Unable to connect to the server');
    }
  }
};

export const setToken = (token: string) => {
  localStorage.setItem('accessToken', token);
};

export const getToken = () => {
  return localStorage.getItem('accessToken');
};

export const clearToken = () => {
  localStorage.removeItem('accessToken');
};
