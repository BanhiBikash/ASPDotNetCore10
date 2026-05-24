import axios from 'axios';

// 📡 Create a central axios instance pointed at your .NET Core API port
const api = axios.create({
  baseURL: 'https://localhost:7130/api', // 👈 Update to match your exact backend port
});

/* ==========================================================================
   🚀 1. REQUEST INTERCEPTOR: Token Injection & Smart Content-Type Fallback
   ========================================================================== */
api.interceptors.request.use(
  (config) => {
    // A. Safely attach your JWT access token if it exists in local storage
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    // B. Smart Header Detection:
    // If we are sending a payload that is NOT a FormData binary stream, fallback to JSON.
    // If it IS a FormData object, we leave it blank so Axios adds multipart/form-data with correct boundaries.
    if (config.data && !(config.data instanceof FormData)) {
      config.headers['Content-Type'] = 'application/json';
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

/* ==========================================================================
   🔄 2. RESPONSE INTERCEPTOR: Dynamic 401 Interception & Silent Refresh Loop
   ========================================================================== */
api.interceptors.response.use(
  (response) => response, // Direct pass-through for successful responses
  async (error) => {
    const originalRequest = error.config;

    // Check if the backend rejected the request with a 401 Unauthorized (Expired Access Token)
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true; // Mark to protect against infinite recursive crash loops

      try {
        const expiredToken = localStorage.getItem('token');
        const refreshToken = localStorage.getItem('refreshToken');

        // If local storage is already empty, bounce them straight out
        if (!expiredToken || !refreshToken) {
          handleLogout();
          return Promise.reject(error);
        }

        console.log('Access token expired. Triggering background token refresh handshake...');

        // 🛰️ Hit your new .NET Core /Auth/refresh endpoint 
        // NOTE: We use standard 'axios' here, NOT 'api', to bypass the request interceptor loops
        const response = await axios.post('https://localhost:7130/api/Auth/refresh', {
          token: expiredToken,
          refreshToken: refreshToken
        });

        // Your backend returns: { jwtToken: "...", refreshToken: "..." }
        const { jwtToken, refreshToken: newRefreshToken } = response.data;

        // 💾 Commit the fresh token rotation set straight to local storage
        localStorage.setItem('token', jwtToken);
        localStorage.setItem('refreshToken', newRefreshToken);

        // 🔁 Re-assign the new working token to the original failed request headers
        originalRequest.headers.Authorization = `Bearer ${jwtToken}`;

        // Re-execute the original user action request seamlessly!
        return axios(originalRequest);

      } catch (refreshError) {
        console.error('Refresh token is dead, expired, or tampered with. Evicting session.', refreshError);
        handleLogout();
        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  }
);

/**
 * Helper utility to wipe the client state clean and redirect to the landing portal
 */
const handleLogout = () => {
  localStorage.removeItem('token');
  localStorage.removeItem('refreshToken');
  
  // Wipe context or enforce state resets if needed, then hard redirect to login card view
  window.location.href = '/login';
};

export default api;