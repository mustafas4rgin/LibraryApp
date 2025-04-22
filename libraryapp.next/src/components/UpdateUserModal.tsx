'use client';
import { Label } from "@/components/ui/label";
import { updateUser } from "@/utils/api";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
    DialogTrigger,
    DialogClose,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useState, useEffect } from "react";
import { User } from "@/types/User";

interface Props {
    user: User | null;
    onClose: () => void;
    open: boolean;
    onSave: (updatedUser: User) => void;
}

export default function UpdateUserModal({ open, onClose, user, onSave }: Props) {
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [address, setAddress] = useState("");

    useEffect(() => {
        if (user) {
            console.log("Gelen user:", user);

            setName(user.name);
            setEmail(user.email);
            setUsername(user.username);
            setAddress(user.address);
        }
    }, [user]);



    const handleSubmit = async () => {
        if (user) {
            try {
                const updatedUser = {
                    id: user.id,
                    name,
                    email,
                    userName: user.username,
                    address: user.address,
                };

                const result = await updateUser(user.id, updatedUser);
                onSave(result.data); // veya sadece `result` eğer fonksiyon sadeleştirildiyse
                onClose();
            } catch (error) {
                alert("Kullanıcı güncellenemedi.");
            }
        }
    };


    return (
        <Dialog open={open} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-[500px]">
  <DialogHeader>
    <DialogTitle className="text-xl font-semibold">Kullanıcıyı Güncelle</DialogTitle>
  </DialogHeader>

  <div className="space-y-4 py-4">
    <div className="flex flex-col space-y-1">
      <Label htmlFor="name">İsim</Label>
      <Input
        id="name"
        value={name}
        onChange={(e) => setName(e.target.value)}
        placeholder="İsim girin"
      />
    </div>

    <div className="flex flex-col space-y-1">
      <Label htmlFor="email">E-posta</Label>
      <Input
        id="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        placeholder="E-posta girin"
      />
    </div>

    <div className="flex flex-col space-y-1">
      <Label htmlFor="username">Kullanıcı Adı</Label>
      <Input
        id="username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        placeholder="Kullanıcı adı girin"
      />
    </div>

    <div className="flex flex-col space-y-1">
      <Label htmlFor="address">Adres</Label>
      <Input
        id="address"
        value={address}
        onChange={(e) => setAddress(e.target.value)}
        placeholder="Adres girin"
      />
    </div>
  </div>

  <DialogFooter>
    <Button onClick={handleSubmit}>Kaydet</Button>
    <DialogClose asChild>
      <Button variant="outline">İptal</Button>
    </DialogClose>
  </DialogFooter>
</DialogContent>
        </Dialog>
    );
}
