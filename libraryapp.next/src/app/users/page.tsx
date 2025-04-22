'use client';

import api, { deleteUser, updateRole, setUnauthorizedHandler } from "@/utils/api";
import UpdateUserModal from "@/components/UpdateUserModal";
import { useEffect, useState } from "react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Pencil, Trash2 } from "lucide-react";
import { User } from "@/types/User";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";

interface Role {
  id: number;
  name: string;
}

export default function UserManagementPage() {
  const [roles, setRoles] = useState<Role[]>([]);
  const [selectedUser, setSelectedUser] = useState<User | null>(null);
  const [isUpdateModalOpen, setIsUpdateModalOpen] = useState(false);
  const [users, setUsers] = useState<User[]>([]);
  const [filteredUsers, setFilteredUsers] = useState<User[]>([]);
  const [search, setSearch] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const usersPerPage = 10;
  const [loading, setLoading] = useState(false);
  const [authError, setAuthError] = useState(false);

  useEffect(() => {
    setUnauthorizedHandler(() => setAuthError(true));
  }, []);

  const fetchRoles = async () => {
    try {
      const res = await api.get("/Role/GetAll");
      setRoles(res.data);
    } catch (err: any) {
      if (err.response?.status === 401) {
        setAuthError(true);
        return;
      }
    }
  };

  const fetchUsers = async () => {
    setLoading(true);
    try {
      const res = await api.get("/User/GetAll?include=role");
      setUsers(res.data);
      setFilteredUsers(res.data);
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
    fetchUsers();
    fetchRoles();
  }, []);

  const handleUpdateClick = (user: User) => {
    setSelectedUser(user);
    setIsUpdateModalOpen(true);
  };

  const handleSaveUser = async () => {
    await fetchUsers();
    setIsUpdateModalOpen(false);
  };

  const handleRoleChange = async (userId: number, newRoleId: string) => {
    const roleId = parseInt(newRoleId);
    const selectedRole = roles.find((r) => r.id === roleId);
    if (!selectedRole) return;

    try {
      await updateRole(userId, roleId);
      setUsers((prevUsers) =>
        prevUsers.map((user) =>
          user.id === userId ? { ...user, role: selectedRole } : user
        )
      );
    } catch (err) {
      console.error("Rol güncellenemedi:", err);
    }
  };

  useEffect(() => {
    const filtered = users.filter((user) =>
      user.name.toLowerCase().includes(search.toLowerCase()) ||
      user.email.toLowerCase().includes(search.toLowerCase())
    );
    setFilteredUsers(filtered);
    setCurrentPage(1);
  }, [search, users]);

  const indexOfLastUser = currentPage * usersPerPage;
  const indexOfFirstUser = indexOfLastUser - usersPerPage;
  const currentUsers = filteredUsers.slice(indexOfFirstUser, indexOfLastUser);
  const totalPages = Math.ceil(filteredUsers.length / usersPerPage);

  const handleDelete = async (id: number) => {
    try {
      await deleteUser(id);
      setUsers((prev) => prev.filter((user) => user.id !== id));
    } catch (err: any) {
      if (err.response?.status === 401) {
        setAuthError(true);
        return;
      }
      alert("Kullanıcı silinirken bir hata oluştu.");
    }
  };

  if (authError) {
    return (
      <Card className="p-6">
        <h1 className="text-2xl font-bold mb-4">Kullanıcı Yönetimi</h1>
        <div className="text-center py-10 text-red-500">
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
      <h1 className="text-2xl font-bold mb-4">Kullanıcı Yönetimi</h1>
      <Input
        placeholder="Kullanıcı ara..."
        className="mb-4"
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />

      {loading ? (
        <div className="text-center py-10 text-muted-foreground">Kullanıcılar yükleniyor...</div>
      ) : (
        <>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>#</TableHead>
                <TableHead>İsim</TableHead>
                <TableHead>E-posta</TableHead>
                <TableHead>Rol</TableHead>
                <TableHead>İşlemler</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {currentUsers.map((user, index) => (
                <TableRow key={user.id}>
                  <TableCell>{indexOfFirstUser + index + 1}</TableCell>
                  <TableCell>{user.name}</TableCell>
                  <TableCell>{user.email}</TableCell>
                  <TableCell>
                    <Select
                      value={user.role?.id.toString()}
                      onValueChange={(value: string) => handleRoleChange(user.id, value)}
                    >
                      <SelectTrigger className="w-[140px]">
                        <SelectValue placeholder="Rol seç" />
                      </SelectTrigger>
                      <SelectContent>
                        {roles.map((role) => (
                          <SelectItem key={role.id} value={role.id.toString()}>
                            {role.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </TableCell>
                  <TableCell className="space-x-2">
                    <Button variant="outline" size="sm" onClick={() => handleUpdateClick(user)}>
                      <Pencil className="w-4 h-4" />
                    </Button>
                    <Button
                      variant="destructive"
                      size="sm"
                      onClick={() => handleDelete(user.id)}
                    >
                      <Trash2 className="w-4 h-4" />
                    </Button>
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

      <UpdateUserModal
        open={isUpdateModalOpen}
        onClose={() => setIsUpdateModalOpen(false)}
        user={selectedUser}
        onSave={handleSaveUser}
      />
    </Card>
  );
}
