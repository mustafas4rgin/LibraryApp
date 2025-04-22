import api from "@/utils/api"

export const logout = async () => {
  const refreshToken = localStorage.getItem("refreshToken");
  if (refreshToken) {
    try {
      await api.post("/Auth/Logout", {
        refreshToken
      });
    } catch (err) {
      console.warn("Backend logout başarısız:", err);
    }
  }

  localStorage.removeItem("accessToken");
  localStorage.removeItem("refreshToken");
};
