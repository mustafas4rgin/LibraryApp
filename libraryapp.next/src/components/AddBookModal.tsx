import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { motion } from "framer-motion";
import api from "@/utils/api";

const schema = z.object({
  title: z.string().min(2, "Başlık en az 2 karakter olmalı"),
  author: z.string().min(2, "Yazar adı en az 2 karakter olmalı"),
  page: z.string().min(1, "Sayfa sayısı gerekli"),
  stock: z.coerce.number().positive("Stok en az 1 olmalı")
});

type FormData = z.infer<typeof schema>;

type Props = {
  onClose: () => void;
  onBookAdded: () => void;
};

const AddBookModal = ({ onClose, onBookAdded }: Props) => {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset
  } = useForm<FormData>({ resolver: zodResolver(schema) });

  const onSubmit = async (data: FormData) => {
    try {
      await api.post("/Book/Add", data);
      onBookAdded();
      reset();
      onClose();
    } catch {
      alert("Kitap eklenemedi.");
    }
  };

  return (
    <motion.div
      className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50"
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      exit={{ opacity: 0 }}
      transition={{ duration: 0.3 }}
    >
      <div className="bg-white p-8 rounded-2xl shadow-2xl w-full max-w-md relative">
        <h2 className="text-2xl font-bold text-gray-800 mb-6">📘 Yeni Kitap Ekle</h2>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
          <div>
            <label className="block font-medium text-gray-700 mb-1">Başlık</label>
            <input
              {...register("title")}
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500 text-gray-800 placeholder:text-gray-400"
              placeholder="Kitap başlığı"
            />
            {errors.title && (
              <p className="text-red-500 text-sm mt-1">{errors.title.message}</p>
            )}
          </div>

          <div>
            <label className="block font-medium text-gray-700 mb-1">Yazar</label>
            <input
              {...register("author")}
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500 text-gray-800 placeholder:text-gray-400"
              placeholder="Yazar adı"
            />
            {errors.author && (
              <p className="text-red-500 text-sm mt-1">{errors.author.message}</p>
            )}
          </div>

          <div>
            <label className="block font-medium text-gray-700 mb-1">Sayfa Sayısı</label>
            <input
              {...register("page")}
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500 text-gray-800 placeholder:text-gray-400"
              placeholder="Örn: 250"
            />
            {errors.page && (
              <p className="text-red-500 text-sm mt-1">{errors.page.message}</p>
            )}
          </div>

          <div>
            <label className="block font-medium text-gray-700 mb-1">Stok</label>
            <input
              type="number"
              {...register("stock")}
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500 text-gray-800 placeholder:text-gray-400"
              placeholder="Örn: 5"
            />
            {errors.stock && (
              <p className="text-red-500 text-sm mt-1">{errors.stock.message}</p>
            )}
          </div>

          <div className="flex justify-end gap-4 pt-2">
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
              {isSubmitting ? "Ekleniyor..." : "Kaydet"}
            </button>
          </div>
        </form>
      </div>
    </motion.div>
  );
};

export default AddBookModal;
