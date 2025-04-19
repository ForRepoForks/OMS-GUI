import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:8080',
  timeout: 10000,
});

// Response interceptor for error handling
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // You can customize global error handling here
    if (error.response) {
      // Server responded with a status outside 2xx
      console.error('API Error:', error.response.data);
    } else if (error.request) {
      // No response from server
      console.error('API No Response:', error.request);
    } else {
      // Something else
      console.error('API Error:', error.message);
    }
    return Promise.reject(error);
  },
);

export default api;
