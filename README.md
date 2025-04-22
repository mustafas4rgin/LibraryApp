# LibraryApp 📚

Katmanlı mimariye sahip, .NET tabanlı kitap kiralama yönetim sistemi. Proje hem RESTful API (backend) hem de Next.js tabanlı admin paneli (frontend) içermektedir.

---

## 📁 Klasör Yapısı

```
LibraryApp/
├── LibraryApp.API            # ASP.NET Core Web API (entry point)
├── LibraryApp.Application    # İş mantığı (CQRS, servisler vs.)
├── LibraryApp.Data           # EF Core DbContext, Migrations
├── LibraryApp.Domain         # Entity & Interface tanımları
├── LibraryApp.Tests          # Unit testler
├── libraryapp.next           # Next.js tabanlı admin paneli
└── LibraryApp.sln
```

---

## 🔧 Teknolojiler

### Backend (.NET)
- ASP.NET Core Web API
- Entity Framework Core
- POSTGRESQL
- JWT Authentication + Refresh Token
- Katmanlı mimari (Domain, Application, Data, API)

### Frontend (Next.js)
- Next.js 13+ App Router
- React, Tailwind CSS
- ShadCN UI
- Axios ile API entegrasyonu
- JWT auth + refresh token handling

---

## 🚀 Kurulum

### API (Backend)
```bash
cd LibraryApp.Data
dotnet ef database update
cd ../LibraryApp.API

dotnet run
```

### Admin Paneli (Frontend)
```bash
cd libraryapp.next

# .env.local dosyasında API adresini tanımlayın
NEXT_PUBLIC_API_URL=http://localhost:5172/api

npm install
npm run dev
```

---

## 🔐 Giriş Bilgisi (Demo)

```
email: admin@library.com
şifre: Admin123!
```

---


