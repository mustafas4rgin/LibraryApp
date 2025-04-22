'use client';

import { useEffect, useState } from 'react';
import api, { setUnauthorizedHandler } from '@/utils/api';
import { Card } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import Link from 'next/link';
import { Users, BookOpenText, ClipboardList } from 'lucide-react';

export default function AdminDashboard() {
  const [bookCount, setBookCount] = useState(0);
  const [userCount, setUserCount] = useState(0);
  const [rentalCount, setRentalCount] = useState(0);
  const [authError, setAuthError] = useState(false);

  useEffect(() => {
    setUnauthorizedHandler(() => setAuthError(true));
  }, []);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const [booksRes, usersRes, rentalsRes] = await Promise.all([
          api.get('/Book/GetAll'),
          api.get('/User/GetAll'),
          api.get('/BookRental/get-book-rentals'),
        ]);

        setBookCount(booksRes.data.length);
        setUserCount(usersRes.data.length);
        setRentalCount(rentalsRes.data.length);
      } catch (err: any) {
        if (err.response?.status === 401) {
          setAuthError(true);
          return;
        }
      }
    };

    fetchStats();
  }, []);

  if (authError) {
    return (
      <Card className="p-6">
        <h1 className="text-2xl font-bold mb-4">Admin Paneli</h1>
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
    <div className="p-8 space-y-6">
      <h1 className="text-3xl font-bold">📊 Admin Paneli</h1>

      <div className="grid gap-4 grid-cols-1 sm:grid-cols-2 lg:grid-cols-3">
        <Card className="p-6 flex items-center justify-between">
          <div>
            <p className="text-muted-foreground">Toplam Kitap</p>
            <h2 className="text-2xl font-semibold">{bookCount}</h2>
          </div>
          <BookOpenText className="w-8 h-8 text-indigo-500" />
        </Card>

        <Card className="p-6 flex items-center justify-between">
          <div>
            <p className="text-muted-foreground">Toplam Kullanıcı</p>
            <h2 className="text-2xl font-semibold">{userCount}</h2>
          </div>
          <Users className="w-8 h-8 text-green-500" />
        </Card>

        <Card className="p-6 flex items-center justify-between">
          <div>
            <p className="text-muted-foreground">Aktif Kiralama</p>
            <h2 className="text-2xl font-semibold">{rentalCount}</h2>
          </div>
          <ClipboardList className="w-8 h-8 text-yellow-500" />
        </Card>
      </div>

      <div className="grid gap-4 grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 mt-6">
        <Link href="/books">
          <Button className="w-full justify-start gap-2">
            <BookOpenText /> Kitapları Yönet
          </Button>
        </Link>
        <Link href="/users">
          <Button className="w-full justify-start gap-2">
            <Users /> Kullanıcıları Yönet
          </Button>
        </Link>
        <Link href="/rentals">
          <Button className="w-full justify-start gap-2">
            <ClipboardList /> Kiralamaları Yönet
          </Button>
        </Link>
      </div>
    </div>
  );
}
