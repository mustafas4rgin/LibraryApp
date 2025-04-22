import { FaTrash } from "react-icons/fa";
import { FaBookOpen } from "react-icons/fa6";
import { Book } from "@/types/Book";
import { deleteBook } from "@/utils/api";

type Props = {
  book: Book;
  onDeleted: () => void;
  onEdit: (book: Book) => void;
};

const BookCard = ({ book, onDeleted, onEdit }: Props) => {
  const handleDelete = async () => {
    if (!confirm(`${book.title} silinsin mi?`)) return;
    try {
      await deleteBook(book.id);
      onDeleted();
    } catch (err) {
      alert("Silinemedi.");
    }
  };

  return (
    <div className="bg-white border rounded-2xl shadow-md p-5 w-full max-w-sm hover:shadow-lg transition-all relative">
      {/* Butonlar container */}
      <div className="absolute top-3 right-3 flex items-center gap-2">
        <button
          onClick={() => onEdit(book)}
          className="flex items-center gap-1 text-xs bg-blue-100 hover:bg-blue-200 text-blue-700 font-medium px-2 py-1 rounded-md transition-all"
          title="Kitabı Güncelle"
        >
          ✏️ Güncelle
        </button>
        <button
          onClick={handleDelete}
          className="text-red-500 hover:text-red-600 transition"
          title="Sil"
        >
          <FaTrash size={16} />
        </button>
      </div>

      {/* İçerik */}
      <h3 className="text-2xl font-bold text-gray-900 mb-1 mt-10">{book.title}</h3>

      <p className="text-sm text-gray-600 mb-4 flex items-center gap-1">
        <FaBookOpen className="text-indigo-500" /> {book.author}
      </p>

      <div className="text-sm text-gray-700 space-y-2">
        <p>
          <span className="font-medium text-gray-500">📘 ISBN:</span> {book.isbn}
        </p>
        <p>
          <span className="font-medium text-gray-500">📄 Sayfa:</span> {book.page}
        </p>
        <p>
          <span className="font-medium text-gray-500">📦 Stok:</span> {book.stock}
        </p>
        <p>
          <span className="font-medium text-gray-500">🕒 Eklenme:</span>{" "}
          {book.createdAt}
        </p>
        <p>
          <span className="font-medium text-gray-500">✏️ Güncelleme:</span>{" "}
          {book.updatedAt === "01.01.0001 00:00" ? "-" : book.updatedAt}
        </p>
      </div>
    </div>
  );
};

export default BookCard;
