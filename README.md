# LifeSure Sigorta Yönetim ve Müşteri Portalı

**LifeSure**, ASP.NET MVC 5 ve Entity Framework 6 tabanlı bir sigorta yönetim platformudur. Proje, **müşteri portalı** ve **yönetim panelini** tek bir çatıda birleştirir ve içerik yönetimi, kullanıcı/rol yönetimi, poliçe süreçleri ile AI destekli hizmetler sunar.

---

## 🚀 Öne Çıkan Özellikler

### Kurumsal Web Arayüzü

* Hakkımızda, Hizmetler, Blog, SSS, Ekip, Referanslar, İletişim gibi sayfalar
* Modern, responsive tasarım (Bootstrap + Razor)

### Yönetim Paneli (Admin Area)

* CRUD tabanlı içerik yönetimi
* Kullanıcı ve rol yönetimi
* Gelen iletişim mesajlarının sınıflandırılması ve otomatik yanıt akışı

### Poliçe & Paket Yönetimi

* Sigorta paketleri ve poliçe verisi yönetimi
* Satış verisi üretme ve temizleme araçları

### AI Destekli Servisler

* **ChatGPT**: Müşteri mesajlarına otomatik yanıt
* **Hugging Face**: Mesaj kategorilendirme (şikayet, teşekkür, bilgi talebi vb.)
* **Gemini**: Sentetik poliçe satış verisi üretimi
* **Tavily**: Web araması tabanlı yanıt üretimi

### Tahminleme (Forecast) Modülü

* Şehir bazlı satış tahmini
* Yıllık ve çeyreklik özet raporlar
* CSV dışa aktarma

---

## 🧱 Teknik Mimari

* `Controllers/` ve `Areas/Admin/Controllers/` – MVC action uçları
* `Services/` – İş kuralları ve uygulama servisleri
* `Repositories/` – Veri erişim soyutlamaları
* `Data/Context/` – EF DbContext ve konfigürasyonlar
* `Data/Entities/` – Domain/entity sınıfları
* `Dtos/` – Controller-Service veri taşıma modelleri
* `Validators/` – FluentValidation kuralları
* `App_Start/` – Route, bundle, DI (Ninject), OWIN başlangıç ayarları

### Bağımlılık Enjeksiyonu

* **Ninject** ile merkezi DI
* Repository, service ve validator binding `App_Start/NinjectWebCommon.cs` içerisinde

### Kimlik Doğrulama & Yetkilendirme

* ASP.NET Identity + OWIN Cookie Authentication
* `AppUser`, `AppRole` ve ilişkili Identity tabloları EF üzerinden yönetilir

### Veri Katmanı

* Entity Framework 6 (Db First yaklaşımı + migration)
* `AppDbContext` içerik ve Identity varlıklarını birlikte yönetir
* `Migrations` klasörü şema değişikliklerini izler

---

## 🛠️ Kullanılan Teknolojiler

* **Backend:** .NET Framework 4.8, ASP.NET MVC 5, EF 6.4.4, ASP.NET Identity 2.2.4, OWIN, Ninject, FluentValidation, Mapster, Newtonsoft.Json
* **AI/ML:** OpenAI, Hugging Face, Gemini, Tavily, Microsoft.ML
* **Frontend:** Bootstrap, Razor View Engine
* **Mail:** MailKit / MimeKit

---

## 📂 Örnek Dizin Yapısı

```text
Mvc_LifeSure_DbFirst/
├─ Areas/
│  └─ Admin/
├─ App_Start/
├─ Controllers/
├─ Data/
│  ├─ Context/
│  ├─ Entities/
│  └─ Identity/
├─ Dtos/
├─ Migrations/
├─ Repositories/
├─ Services/
├─ Validators/
└─ Views/
```

## 🤖 AI & ML Modülleri

* **ChatGPTService:** Profesyonel otomatik yanıt
* **HuggingFaceService:** Mesaj kategorilendirme
* **GeminiService:** Sentetik veri üretimi
* **TavilyService:** Web tabanlı yanıt üretimi
* **ForecastService:** Şehir bazlı satış tahmini, dashboard ve CSV export

---

<img width="1907" height="852" alt="Ekran görüntüsü 2026-03-24 165523" src="https://github.com/user-attachments/assets/c0282e48-1af2-407d-80df-b40604980f55" />

<img width="1909" height="851" alt="Ekran görüntüsü 2026-03-24 163057" src="https://github.com/user-attachments/assets/c53baa2a-06df-4065-bdcb-4224b154dbba" />

<img width="1897" height="854" alt="Ekran görüntüsü 2026-03-24 165741" src="https://github.com/user-attachments/assets/50276350-88ca-43da-a54b-eb28b29f4f92" />
