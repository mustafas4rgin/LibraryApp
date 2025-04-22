"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";
import { logout } from "@/utils/logout";

const Header = () => {
  const router = useRouter();

  const handleLogout = async () => {
    await logout();
    router.push("/login");
  };

  return (
    <header className="w-full h-16 bg-white shadow-md flex items-center px-6 justify-between fixed top-0 left-64 z-50">
      <h2 className="text-xl font-semibold text-gray-800">Library Admin Panel</h2>

      <button
        onClick={handleLogout}
        className="text-sm font-medium bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700 transition"
      >
        Çıkış Yap
      </button>
    </header>
  );
};

export default Header;
