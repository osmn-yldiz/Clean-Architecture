# 🚀 Clean Architecture

Bu proje, **Clean Architecture** prensipleri doğrultusunda geliştirilmiş olup, modüler, esnek ve sürdürülebilir bir yazılım mimarisi sunmaktadır. Uygulamanın her bileşeni belirli bir sorumluluğa sahip olacak şekilde ayrılmıştır, böylece bağımsız olarak geliştirilebilir, değiştirilebilir ve test edilebilir.

## 🏗️ Mimari Yapı
Proje, aşağıdaki ana katmanlardan oluşmaktadır:

- 📌 **Domain Katmanı**: Uygulamanın iş mantığı ve iş kurallarını içerir. Diğer katmanlardan bağımsızdır ve uygulamanın temelini oluşturur.
- ⚙️ **Application Katmanı**: Uygulamanın iş akışları ve servislerini barındırır. Use-case'ler bu katmanda tanımlanır.
- 💾 **Infrastructure Katmanı**: Veri erişimi, dış servis entegrasyonları ve altyapı ile ilgili bileşenleri içerir. Veritabanı bağlantıları ve API entegrasyonları burada bulunur.
- 🌐 **Presentation Katmanı**: Kullanıcı arayüzü veya dış API’lerle etkileşime giren katmandır. Burada API kontrolcüleri veya UI bileşenleri yer alır.

## 🎯 Kullanılan Design Patterns
- ✅ **Result Pattern**: İşlemlerin başarılı ya da başarısız olma durumlarını standart bir yapıda temsil eder.
- 📂 **Repository Pattern**: Veri erişim işlemlerini soyutlayarak, veri kaynağından bağımsız bir yapı sunar. Böylece farklı veri kaynakları arasında değişim kolaylaşır.
- 🔀 **CQRS (Command Query Responsibility Segregation) Pattern**: Komutlar (yazma işlemleri) ve sorgular (okuma işlemleri) arasında ayrım yaparak, veri işlemlerinin daha verimli bir şekilde yapılmasını sağlar.
- 🔄 **UnitOfWork Pattern**: Birim işlemleri, tek bir işlem altında (transaction) yönetilmesini sağlayarak, veri tutarlılığı ve performans artışı sunar.

## 🛠️ Kullanılan Kütüphaneler ve Teknolojiler
- 🏆 **TS.Result**: İşlemlerin sonucunu temsil eden bir modelleme sunar. Başarı, hata ve hata mesajları gibi durumlar için kullanılır.
- 🔄 **Mapster**: Nesne eşlemeleri için kullanılan bir kütüphanedir. Model dönüşümleri ve veri transfer objeleri arasında veri aktarımını kolaylaştırır.
- 🔍 **FluentValidation**: Verilerin doğrulanması için kullanılan bir kütüphanedir. Özellikle API'den gelen verilerin doğruluğunu test eder.
- 🗂️ **TS.EntityFrameworkCore.GenericRepository**: Genel amaçlı repository yapısını sunarak, veri erişim işlemlerini soyutlar ve kod tekrarını azaltır.
- 🏛️ **EntityFrameworkCore**: ORM (Object-Relational Mapping) aracı olarak kullanılır. Veritabanı işlemleri ile nesne yönelimli programlamayı birleştirir.
- 🔗 **OData**: RESTful API'lerde gelişmiş sorgulama, filtreleme ve sıralama işlemleri sağlar.
- 📌 **Scrutor**: Dependency Injection yönetimini kolaylaştırır ve dinamik servis kaydını sağlar.
- 🔐 **Microsoft.AspNetCore.Authentication.JwtBearer**: JWT tabanlı kimlik doğrulama sağlar. API'lere güvenli erişim için kullanılır.
- 🔑 **Keycloak.AuthServices.Authentication**: Merkezi kimlik yönetimi için kullanılır. Keycloak ile entegrasyon sağlar.
- ⚡ **Minimal API**: Hızlı ve performanslı API geliştirmek için kullanılan yeni nesil API mimarisi. .NET 6+ ile gelen bu yapı, daha az kodla hızlı uygulama geliştirilmesine imkan tanır.
- 🧠 **Memory Cache**: Verileri geçici olarak hafızada tutarak uygulama performansını artırır ve sık erişilen verilere hızlı erişim sağlar.
- 📜 **Serilog**: Uygulama loglama ve hata takibi için gelişmiş bir logging kütüphanesidir. Yapılandırılabilir loglama ile hata yönetimini iyileştirir.

## 📦 Kurulum ve Kullanım

1. **Depoyu Klonlayın**:
   ```bash
   git clone https://github.com/osmn-yldiz/Clean-Architecture.git
   cd Clean-Architecture

2. **Keycloak Docker Kodu**:
   ```bash
    docker run -d --name keycloak -p 8080:8080 -e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:25.0.2 start-dev
   ```
