"use client";

import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import { Book } from "@/types/Book";
import { updateBook } from "@/utils/api";
import { motion } from "framer-motion";

// Form tipi
type UpdateBookDTO = {
  title: string;
  author: string;
  page: string;
  isbn: string;
  stock: number;
};

type Props = {
    book: Book;
    onClose: () => void;
    onUpdated: () => Promise<void>;
    isOpen?: boolean; 
  };
  

// Validasyon şeması
const schema = yup.object({
  title: yup.string().required("Başlık zorunlu."),
  author: yup.string().required("Yazar zorunlu."),
  page: yup.string().required("Sayfa sayısı zorunlu."),
  isbn: yup
    .string()
    .length(13, "ISBN 13 haneli olmalı")
    .required("ISBN zorunlu."),
  stock: yup
    .number()
    .typeError("Stok sayı olmalı")
    .required("Stok zorunlu")
    .min(1, "En az 1 kitap olmalı"),
});

const UpdateBookModal = ({ book, isOpen, onClose, onUpdated }: Props) => {
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting },
  } = useForm<UpdateBookDTO>({
    resolver: yupResolver(schema),
  });

  useEffect(() => {
    if (book) reset(book);
  }, [book]);

  const onSubmit = async (data: UpdateBookDTO) => {
    try {
      await updateBook(book.id, data);
      onUpdated();
      onClose();
    } catch (err) {
      console.error(err);
      alert("Güncelleme sırasında hata oluştu.");
    }
  };

  if (!isOpen) return null;

  return (
    <motion.div
      className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50"
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
    >
      <div className="bg-white p-8 rounded-2xl shadow-2xl w-full max-w-md">
        <h2 className="text-2xl font-bold text-gray-800 mb-6">✏️ Kitap Güncelle</h2>
  
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          {/* Başlık */}
          <div>
            <label className="block font-medium text-gray-700">Başlık</label>
            <input
              {...register("title")}
              placeholder="Kitap başlığı"
              className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 text-gray-800 placeholder:text-gray-500"
            />
            {errors.title && <p className="text-red-500 text-sm mt-1">{errors.title.message}</p>}
          </div>
  
          {/* Yazar */}
          <div>
            <label className="block font-medium text-gray-700">Yazar</label>
            <input
              {...register("author")}
              placeholder="Yazar adı"
              className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 text-gray-800 placeholder:text-gray-500"
            />
            {errors.author && <p className="text-red-500 text-sm mt-1">{errors.author.message}</p>}
          </div>
  
          {/* Sayfa */}
          <div>
            <label className="block font-medium text-gray-700">Sayfa</label>
            <input
              {...register("page")}
              placeholder="Sayfa sayısı"
              className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 text-gray-800 placeholder:text-gray-500"
            />
            {errors.page && <p className="text-red-500 text-sm mt-1">{errors.page.message}</p>}
          </div>
  
          {/* ISBN */}
          <div>
            <label className="block font-medium text-gray-700">ISBN</label>
            <input
              {...register("isbn")}
              placeholder="ISBN numarası"
              className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 text-gray-800 placeholder:text-gray-500"
            />
            {errors.isbn && <p className="text-red-500 text-sm mt-1">{errors.isbn.message}</p>}
          </div>
  
          {/* Stok */}
          <div>
            <label className="block font-medium text-gray-700">Stok</label>
            <input
              type="number"
              {...register("stock")}
              placeholder="Stok miktarı"
              className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 text-gray-800 placeholder:text-gray-500"
            />
            {errors.stock && <p className="text-red-500 text-sm mt-1">{errors.stock.message}</p>}
          </div>
  
          {/* Butonlar */}
          <div className="flex justify-end gap-4 pt-4">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 bg-gray-100 hover:bg-gray-200 text-gray-700 rounded-md"
            >
              Vazgeç
            </button>
            <button
              type="submit"
              disabled={isSubmitting}
              className="px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white font-medium rounded-md disabled:opacity-60"
            >
              {isSubmitting ? "Kaydediliyor..." : "Kaydet"}
            </button>
          </div>
        </form>
      </div>
    </motion.div>
  );
  
};

export default UpdateBookModal;
