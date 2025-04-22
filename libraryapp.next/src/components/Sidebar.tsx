"use client";
import { useAuth } from "@/context/AuthContext";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { logout } from "@/utils/logout";
import {
    LayoutDashboard,
    BookOpenText,
    Users,
    ClipboardList,
    LogOut,
    LogIn,
} from "lucide-react";

const Sidebar = () => {
    const pathname = usePathname();
    const { isAuthenticated, logout } = useAuth();
    const links = [
        { name: "Dashboard", href: "/", icon: <LayoutDashboard className="w-5 h-5" /> },
        { name: "Books", href: "/books", icon: <BookOpenText className="w-5 h-5" /> },
        { name: "Users", href: "/users", icon: <Users className="w-5 h-5" /> },
        { name: "Rentals", href: "/rentals", icon: <ClipboardList className="w-5 h-5" /> },
    ];

    return (
        <aside className="w-64 h-screen bg-gray-900 text-white fixed top-0 left-0 flex flex-col justify-between py-6 px-4 shadow-lg">
            {/* App title */}
            <div>
                <h1 className="text-2xl font-bold mb-8 text-center text-white">
                    📚 LibraryApp
                </h1>

                {/* Navigation links */}
                <nav className="flex flex-col gap-2">
                    {links.map((link) => (
                        <Link
                            key={link.href}
                            href={link.href}
                            className={`flex items-center gap-3 py-2 px-3 rounded-md text-sm font-medium transition-all ${pathname === link.href
                                    ? "bg-indigo-600 text-white shadow"
                                    : "hover:bg-gray-800 text-gray-300"
                                }`}
                        >
                            {link.icon}
                            {link.name}
                        </Link>
                    ))}
                </nav>
            </div>

            {/* Auth actions */}
            <div className="flex flex-col gap-2 border-t border-gray-800 pt-4">
                {!isAuthenticated && (
                    <Link
                        href="/login"
                        className="flex items-center gap-2 py-2 px-3 rounded-md text-sm font-medium transition-all hover:bg-gray-800 text-gray-300"
                    >
                        <LogIn className="w-5 h-5" />
                        Login
                    </Link>
                )}
                {isAuthenticated && (
                    <button
                        onClick={logout}
                        className="flex items-center gap-2 py-2 px-3 rounded-md text-sm font-medium transition-all hover:bg-red-600 bg-red-500 text-white"
                    >
                        <LogOut className="w-5 h-5" />
                        Logout
                    </button>
                )}
            </div>
        </aside>
    );
};

export default Sidebar;
