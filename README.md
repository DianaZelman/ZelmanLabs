#  CarShop - Веб-приложение для каталога автомобилей

##  Описание проекта

CarShop - учебный веб-проект. Представляет собой интернет-магазин автомобилей с админ-панелью, корзиной заказов и системой логирования.

---

##  Технологический стек

| Компонент | Технология |
|-----------|------------|
| **Backend API** | ASP.NET Core Web API (.NET 10) |
| **Frontend (UI)** | ASP.NET Core MVC + Razor Pages |
| **Blazor** | Blazor Server (SSR) |
| **База данных** | SQLite (Identity) |
| **ORM** | Entity Framework Core |
| **Логирование** | Serilog |
| **Тестирование** | xUnit + NSubstitute |
| **Стили** | Bootstrap 5 + Font Awesome |

---

##  Запуск проекта

```bash
# 1. API
cd CarShop.API && dotnet watch run

# 2. UI приложение
cd ZelmanLabs.UI && dotnet watch run --launch-profile https

# 3. Blazor приложение
cd CarShop.Blazor && dotnet watch run

# 4. Тесты
cd CarShop.Tests && dotnet test

```
---

**Главная страница (каталог)**
<img width="1706" height="897" alt="Screenshot 2026-06-18 at 20 35 34" src="https://github.com/user-attachments/assets/831e5049-0e17-4214-8fc5-40c38d6e326d" />

**Корзина**
<img width="1706" height="859" alt="Screenshot 2026-06-18 at 20 38 06" src="https://github.com/user-attachments/assets/09133be1-b95a-49bb-9a96-a92d18634d31" />

**Админ-панель**
<img width="1706" height="662" alt="Screenshot 2026-06-18 at 20 39 14" src="https://github.com/user-attachments/assets/f4bac9f0-2856-421b-b3bb-20a5d4107123" />
<img width="1706" height="791" alt="Screenshot 2026-06-18 at 20 39 39" src="https://github.com/user-attachments/assets/9a8b4fb0-a1cf-4e32-8945-dbb45e05825e" />

**Blazor приложение**
<img width="1706" height="945" alt="Screenshot 2026-06-18 at 20 27 20" src="https://github.com/user-attachments/assets/d5573614-623f-4b6e-aa54-eeec5cfcf5ea" />




