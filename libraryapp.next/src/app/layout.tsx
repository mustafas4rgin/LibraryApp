import "./globals.css";
import type { Metadata } from "next";
import Sidebar from "@/components/Sidebar";
import Header from "@/components/Header";
import { AuthProvider } from "@/context/AuthContext";

export const metadata: Metadata = {
  title: "LibraryApp",
  description: "Admin panel for managing library system",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body className="flex">
        <AuthProvider>
        <Sidebar />
        <div className="ml-64 w-full min-h-screen bg-gray-50">
          <Header />
          <main className="pt-20 px-6">{children}</main>
        </div>
        </AuthProvider>
        
      </body>
    </html>
  );
}
