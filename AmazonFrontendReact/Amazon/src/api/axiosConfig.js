import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7130/api/v1',
  // 🚀 Removing the fixed 'application/json' header lets Axios determine boundaries automatically
});

export default api;