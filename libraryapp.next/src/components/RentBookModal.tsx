'use client';

import { useEffect, useState } from 'react';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import api from '@/utils/api';
import { User } from '@/types/User';

interface RentBookModalProps {
  bookId: number;
  open: boolean;
  onClose: () => void;
  onRented: () => void;
}

 interface Rental {
  user: {
    id: number;
    name: string;
    email: string;
    // adres veya username lazımsa eklenebilir
  };
  book: {
    id: number;
    title: string;
    author: string;
    isbn: string;
    // diğer kitap özellikleri gerekiyorsa eklenebilir
  };
  rentalDate: string;
}

const RentBookModal = ({ bookId, open, onClose, onRented }: RentBookModalProps) => {
  const [users, setUsers] = useState<User[]>([]);
  const [rentals, setRentals] = useState<Rental[]>([]);
  const [filteredUsers, setFilteredUsers] = useState<User[]>([]);
  const [search, setSearch] = useState('');
  const [selectedUserId, setSelectedUserId] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);

  const fetchUsers = async () => {
    try {
      const res = await api.get('/User/GetAll');
      setUsers(res.data);
      setFilteredUsers(res.data);
    } catch {
      alert('Kullanıcılar yüklenemedi.');
    }
  };

  const fetchRentals = async () => {
    try {
      const res = await api.get('/BookRental/get-book-rentals?include=book-users');
      setRentals(res.data);
    } catch {
      alert('Kiralamalar yüklenemedi.');
    }
  };

  useEffect(() => {
    if (open) {
      fetchUsers();
      fetchRentals();
      setSearch('');
      setSelectedUserId(null);
    }
  }, [open]);

  useEffect(() => {
    const filtered = users.filter(
      (user) =>
        user.name.toLowerCase().includes(search.toLowerCase()) ||
        user.email.toLowerCase().includes(search.toLowerCase())
    );
    setFilteredUsers(filtered);
  }, [search, users]);

  const handleRent = async () => {
    if (!selectedUserId) return;

    const alreadyRented = rentals.some(
      (r) => r.book.id === bookId && r.user.id === selectedUserId
    );

    if (alreadyRented) {
      alert('Bu kullanıcı zaten bu kitabı kiralamış.');
      return;
    }

    setLoading(true);
    try {
      await api.post('/BookRental/rent-book', {
        userId: selectedUserId,
        bookId,
      });
      onRented();
      onClose();
    } catch {
      alert('Kiralama işlemi başarısız.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={onClose}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Kullanıcı Seç ve Kirala</DialogTitle>
        </DialogHeader>

        <Input
          placeholder="Kullanıcı ara..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />

        <div className="max-h-52 overflow-auto mt-2 space-y-2">
          {filteredUsers.slice(0, 10).map((user) => (
            <div
              key={user.id}
              className={`p-2 rounded-md border cursor-pointer ${
                selectedUserId === user.id ? 'bg-indigo-100 border-indigo-400' : ''
              }`}
              onClick={() => setSelectedUserId(user.id)}
            >
              <p className="font-medium">{user.name}</p>
              <p className="text-sm text-muted-foreground">{user.email}</p>
            </div>
          ))}
        </div>

        <Button
          disabled={!selectedUserId || loading}
          onClick={handleRent}
          className="w-full mt-4"
        >
          {loading ? 'Kiralanıyor...' : 'Kirala'}
        </Button>
      </DialogContent>
    </Dialog>
  );
};

export default RentBookModal;
