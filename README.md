# 🚀 Clean Architecture

Bu proje, **Clean Architecture** prensipleri doğrultusunda geliştirilmiş olup, modüler, esnek ve sürdürülebilir bir yazılım mimarisi sunmaktadır. Uygulamanın her bileşeni belirli bir sorumluluğa sahip olacak şekilde ayrılmıştır, böylece bağımsız olarak geliştirilebilir, değiştirilebilir ve test edilebilir.

## 🏗️ Mimari Yapı

Proje, aşağıdaki ana katmanlardan oluşmaktadır:

- **📌 Domain Katmanı**: İş mantığı ve iş kurallarını içerir. Uygulamanın temelini oluşturur ve diğer katmanlardan bağımsızdır.
- **⚙️ Application Katmanı**: Uygulama ile ilgili servisler ve iş akışlarını barındırır. Use-case'ler burada tanımlanır.
- **💾 Infrastructure Katmanı**: Veri erişimi, dış servis entegrasyonları ve diğer altyapı ile ilgili bileşenleri içerir.
- **🌐 Presentation Katmanı**: Kullanıcı arayüzü veya API gibi dış dünyaya açılan kısımları kapsar.

## 🎯 Kullanılan Design Patterns

- ✅ **Result Pattern**: İşlemlerin sonucunu standart bir şekilde temsil eder.
- 📂 **Repository Pattern**: Veri erişim işlemlerini soyutlayarak, veri kaynağından bağımsız bir yapı sağlar.
- 🔀 **CQRS (Command Query Responsibility Segregation) Pattern**: Komutlar ve sorguların ayrılmasıyla, veri okuma ve yazma işlemlerinin ayrıştırılmasını sağlar.
- 🔄 **UnitOfWork Pattern**: Birim işlemlerinin tek bir işlem (transaction) altında yönetilmesini sağlar.

## 🛠️ Kullanılan Kütüphaneler ve Teknolojiler

- 🏆 **TS.Result** → Standart sonuç modellemeleri için.
- 🔄 **Mapster** → Nesne eşlemeleri için.
- 🔍 **FluentValidation** → Veri doğrulama işlemleri için.
- 🗂️ **TS.EntityFrameworkCore.GenericRepository** → Genel amaçlı repository işlemleri için.
- 🏛️ **EntityFrameworkCore** → ORM (Object-Relational Mapping) için.
- 🔗 **OData** → RESTful API'lerde gelişmiş sorgulama yetenekleri için.
- 📌 **Scrutor** → Dependency Injection yönetimi ve dinamik servis kaydı için.
- 🔐 **Microsoft.AspNetCore.Authentication.JwtBearer** → JWT tabanlı kimlik doğrulama için.
- 🔑 **Keycloak.AuthServices.Authentication** → Keycloak ile merkezi kimlik yönetimi için.
- ⚡ **Minimal API** → .NET 6+ ile gelen, hızlı ve performanslı API geliştirmek için kullanılan bir API mimarisi.

## 📦 Kurulum ve Kullanım

1. **Depoyu Klonlayın**:
   ```bash
   git clone https://github.com/osmn-yldiz/Clean-Architecture.git
   cd Clean-Architecture

2. **Keycloak Docker Kodu**:
   ```bash
    docker run -d --name keycloak -p 8080:8080 -e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:25.0.2 start-dev
   ```
