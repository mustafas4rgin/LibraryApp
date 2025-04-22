"use client";

import { useEffect, useState } from "react";
import api, { setUnauthorizedHandler } from "@/utils/api";
import { Book } from "@/types/Book";
import BookCard from "@/components/Bookcard";
import AddBookModal from "@/components/AddBookModal";
import UpdateBookModal from "@/components/UpdateBookModal";
import RentBookModal from "@/components/RentBookModal";
import { motion } from "framer-motion";
import { FaSearch } from "react-icons/fa";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";

const BooksPage = () => {
  const [books, setBooks] = useState<Book[]>([]);
  const [filteredBooks, setFilteredBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [showAddModal, setShowAddModal] = useState(false);
  const [selectedBook, setSelectedBook] = useState<Book | null>(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [rentalBookId, setRentalBookId] = useState<number | null>(null);
  const [authError, setAuthError] = useState(false);

  useEffect(() => {
    setUnauthorizedHandler(() => setAuthError(true));
  }, []);

  const fetchBooks = async () => {
    setLoading(true);
    try {
      const res = await api.get("/Book/GetAll");
      setBooks(res.data);
      setFilteredBooks(res.data);
    } catch (err: any) {
      if (err.response?.status === 401) {
        setAuthError(true);
        return;
      }
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchBooks();
  }, []);

  useEffect(() => {
    const filtered = books.filter((book) =>
      book.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
      book.author.toLowerCase().includes(searchTerm.toLowerCase())
    );
    setFilteredBooks(filtered);
  }, [searchTerm, books]);

  const handleEdit = (book: Book) => {
    setSelectedBook(book);
  };

  const handleCloseUpdate = () => {
    setSelectedBook(null);
  };

  const handleOpenRental = (bookId: number) => {
    setRentalBookId(bookId);
  };

  const handleCloseRental = () => {
    setRentalBookId(null);
  };

  if (authError) {
    return (
      <Card className="p-6">
        <h1 className="text-2xl font-bold mb-4">Kitaplar</h1>
        <div className="text-red-500 text-center py-10">
          Yetkisiz erişim. Lütfen giriş yapınız.
          <div className="mt-4">
            <Button onClick={() => window.location.href = "/login"}>
              Giriş Yap
            </Button>
          </div>
        </div>
      </Card>
    );
  }

  return (
    <div className="px-4 py-8">
      <motion.h1
        className="text-4xl font-extrabold text-gray-900 mb-8 flex items-center gap-2"
        initial={{ opacity: 0, y: -20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.6 }}
      >
        📚 Kitaplar
      </motion.h1>

      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mb-6">
        <div className="relative w-full sm:w-64">
          <span className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400">
            <FaSearch size={14} />
          </span>
          <input
            type="text"
            placeholder="Kitap veya yazar ara..."
            className="pl-9 pr-4 py-2 border rounded-lg shadow-sm w-full focus:outline-none focus:ring-2 focus:ring-indigo-500 text-gray-800 placeholder:text-gray-400"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>

        <button
          onClick={() => setShowAddModal(true)}
          className="bg-indigo-600 hover:bg-indigo-700 text-white font-semibold px-5 py-2 rounded-md shadow"
        >
          ➕ Kitap Ekle
        </button>
      </div>

      {loading ? (
        <p className="text-gray-600">Yükleniyor...</p>
      ) : filteredBooks.length > 0 ? (
        <div className="grid gap-6 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4">
          {filteredBooks.map((book) => (
            <div key={book.id} className="flex flex-col gap-2">
              <BookCard
                book={book}
                onDeleted={fetchBooks}
                onEdit={handleEdit}
              />
              <button
                onClick={() => handleOpenRental(book.id)}
                className="bg-green-600 hover:bg-green-700 text-white text-sm py-2 rounded-md shadow"
              >
                📦 Kirala
              </button>
            </div>
          ))}
        </div>
      ) : (
        <p className="text-gray-500 italic">Aradığınız kritere uygun kitap bulunamadı.</p>
      )}

      {showAddModal && (
        <AddBookModal
          onClose={() => setShowAddModal(false)}
          onBookAdded={fetchBooks}
        />
      )}

      {selectedBook && (
        <UpdateBookModal
          book={selectedBook}
          onClose={handleCloseUpdate}
          onUpdated={fetchBooks}
        />
      )}

      {rentalBookId && (
        <RentBookModal
          bookId={rentalBookId}
          open={true}
          onClose={handleCloseRental}
          onRented={fetchBooks}
        />
      )}
    </div>
  );
};

export default BooksPage;
