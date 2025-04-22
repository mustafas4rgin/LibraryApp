'use client';

import { useEffect, useState } from "react";
import api, { setUnauthorizedHandler } from "@/utils/api";
import { format } from "date-fns";
import { tr } from "date-fns/locale";
import { Card } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Check } from "lucide-react";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer } from 'recharts';

interface Rental {
  id: number;
  rentalDate: string;
  user: {
    id: number;
    name: string;
    email: string;
  };
  book: {
    id: number;
    title: string;
    author: string;
    isbn: string;
  };
}

export default function RentalManagementPage() {
  const [rentals, setRentals] = useState<Rental[]>([]);
  const [loading, setLoading] = useState(false);
  const [search, setSearch] = useState("");
  const [authError, setAuthError] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 10;

  useEffect(() => {
    setUnauthorizedHandler(() => setAuthError(true));
  }, []);

  const fetchRentals = async () => {
    setLoading(true);
    try {
      const res = await api.get("/BookRental/get-book-rentals?include=book-users");
      setRentals(res.data);
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
    fetchRentals();
  }, []);

  const handleReturn = async (rental: Rental) => {
    try {
      await api.delete(`/BookRental/Delete?userId=${rental.user.id}&bookId=${rental.book.id}`);
      setRentals((prev) =>
        prev.filter((r) => !(r.user.id === rental.user.id && r.book.id === rental.book.id))
      );
    } catch (err: any) {
      if (err.response?.status === 401) {
        setAuthError(true);
        return;
      }
      alert("Teslim işlemi başarısız.");
    }
  };

  const filteredRentals = rentals.filter((rental) =>
    rental.user.name.toLowerCase().includes(search.toLowerCase()) ||
    rental.book.title.toLowerCase().includes(search.toLowerCase())
  );

  const sortedRentals = [...filteredRentals].sort(
    (a, b) => new Date(b.rentalDate).getTime() - new Date(a.rentalDate).getTime()
  );

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = sortedRentals.slice(indexOfFirstItem, indexOfLastItem);
  const totalPages = Math.ceil(sortedRentals.length / itemsPerPage);

  const rentalsPerBook = rentals.reduce((acc: Record<string, number>, rental) => {
    const title = rental.book.title;
    acc[title] = (acc[title] || 0) + 1;
    return acc;
  }, {});

  const chartData = Object.entries(rentalsPerBook).map(([title, count]) => ({
    title,
    count,
  }));

  if (authError) {
    return (
      <Card className="p-6">
        <h1 className="text-2xl font-bold mb-4">Kitap Kiralama Yönetimi</h1>
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
    <Card className="p-6">
      <h1 className="text-2xl font-bold mb-4">Kitap Kiralama Yönetimi</h1>

      <Input
        placeholder="Kullanıcı veya kitap ara..."
        className="mb-4"
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />

      <div className="h-72 w-full mb-8">
        <ResponsiveContainer width="100%" height="100%">
          <BarChart data={chartData}>
            <XAxis dataKey="title" hide />
            <YAxis allowDecimals={false} />
            <Tooltip />
            <Bar dataKey="count" />
          </BarChart>
        </ResponsiveContainer>
      </div>

      {loading ? (
        <div className="text-muted-foreground text-center py-10">Yükleniyor...</div>
      ) : (
        <>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Kullanıcı</TableHead>
                <TableHead>Kitap</TableHead>
                <TableHead>Yazar</TableHead>
                <TableHead>Kiralama Tarihi</TableHead>
                <TableHead>İşlem</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {currentItems.map((rental, index) => (
                <TableRow key={index}>
                  <TableCell>{rental.user.name}</TableCell>
                  <TableCell>{rental.book.title}</TableCell>
                  <TableCell>{rental.book.author}</TableCell>
                  <TableCell>
                    {rental.rentalDate === "0001-01-01T00:00:00"
                      ? "—"
                      : format(new Date(rental.rentalDate), "PPPpp", { locale: tr })}
                  </TableCell>
                  <TableCell>
                    <AlertDialog>
                      <AlertDialogTrigger asChild>
                        <Button variant="destructive" size="sm" className="flex items-center gap-1">
                          <Check className="w-4 h-4" />
                          Teslim Al
                        </Button>
                      </AlertDialogTrigger>
                      <AlertDialogContent>
                        <AlertDialogHeader>
                          <AlertDialogTitle>Bu kitabı teslim al?</AlertDialogTitle>
                          <AlertDialogDescription>
                            {rental.book.title} kitabı {rental.user.name} tarafından teslim edildi mi?
                          </AlertDialogDescription>
                        </AlertDialogHeader>
                        <AlertDialogFooter>
                          <AlertDialogCancel>İptal</AlertDialogCancel>
                          <AlertDialogAction onClick={() => handleReturn(rental)}>
                            Evet, teslim alındı
                          </AlertDialogAction>
                        </AlertDialogFooter>
                      </AlertDialogContent>
                    </AlertDialog>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>

          <div className="flex justify-center mt-4 space-x-2">
            {Array.from({ length: totalPages }, (_, i) => i + 1).map((page) => (
              <Button
                key={page}
                variant={page === currentPage ? "default" : "outline"}
                size="sm"
                onClick={() => setCurrentPage(page)}
              >
                {page}
              </Button>
            ))}
          </div>
        </>
      )}
    </Card>
  );
}
