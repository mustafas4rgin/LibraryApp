import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5172/api",
});

// ===== Access Token Ekleme =====
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// ===== Yetkisizliği yakalamak için dışarıdan handler =====
let onUnauthorized: (() => void) | null = null;

export const setUnauthorizedHandler = (handler: () => void) => {
  onUnauthorized = handler;
};

// ===== Refresh Token Interceptor Yapısı =====
let isRefreshing = false;
let failedQueue: any[] = [];

const processQueue = (error: any, token: string | null = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });

  failedQueue = [];
};

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      if (isRefreshing) {
        return new Promise(function (resolve, reject) {
          failedQueue.push({ resolve, reject });
        })
          .then((token) => {
            originalRequest.headers.Authorization = `Bearer ${token}`;
            return api(originalRequest);
          })
          .catch((err) => Promise.reject(err));
      }

      isRefreshing = true;

      try {
        const refreshToken = localStorage.getItem("refreshToken");

        const response = await axios.post("http://localhost:5172/api/Auth/refresh-token", {
          refreshToken,
        });

        const newAccessToken = response.data.accessToken;
        localStorage.setItem("accessToken", newAccessToken);

        processQueue(null, newAccessToken);
        originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;

        return api(originalRequest);
      } catch (err) {
        processQueue(err, null);
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");

        if (onUnauthorized) {
          onUnauthorized();
        } else {
          window.location.href = "/login";
        }

        return Promise.reject(err);
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

// === Diğer export'lar ===
export const updateBook = (id: number, data: any) =>
  api.put(`/Book/Update/${id}`, data);

export const updateUser = (id: number, data: any) =>
  api.put(`/User/Update/${id}`, data);

export const updateRole = (userId: number, roleId: number) =>
  api.put('/Admin/UpdateRole', { userId, roleId });

export const deleteBook = (id: number) => api.delete(`/Book/Delete/${id}`);
export const deleteUser = (id: number) => api.delete(`/User/Delete/${id}`);

export default api;
