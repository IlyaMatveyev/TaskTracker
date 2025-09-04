# TaskTracker
ASP.NET Core Web API для управления проектами и задачами. Реализован с использованием **чистой архитектуры** с разделением на слои (API, Application, Domain, Infrastructure).

## Основные возможности
- Управление проектами (**Project**) и задачами (**Task**).
- Аутентификация с использованием **JWT** и хранения токена в **Cookie**.
- Работа с **PostgreSQL** в контейнере через **Docker**.
- Логирование с помощью **Serilog** в файл.
- Маппинг моделей и DTO реализован с помощью **Mapster**.

## Технологии
- **ASP.NET Core Web API**
- **.NET 8**
- **Entity Framework Core** + **PostgreSQL**
- **JWT + Cookie аутентификация**
- **Docker / Testcontainers**
- **Serilog**
- **FluentValidation**
- **MemoryCache**
- **Mapster**
- **xUnit** для Unit и интеграционных тестов.

# Инструкция по развёртыванию:
- Клонирование командой: git clone -b dev https://github.com/IlyaMatveyev/TaskTracker
- Переходим в TaskTracker командой: cd TaskTracker
- Затем поднимаем контейнеры Docker командой: docker-compose up --build
- После того как появилось сообщение об успешном применении миграций: "The database migrations were applied successfully."
Можно переходить в Swagger UI по url: http://localhost:5000/swagger/index.html
- Чтобы удалить контейнеры и очистить память используйте команду: docker-compose down -v

# Инструкция по тестированию:
- В проекте реализованы тесты двух видов: **Unit тесты** и **Интеграционные тесты**.
- Для запуска интеграционных тестов на компьютере должен быть установлен Docker (используется Testcontainers для запуска PostgreSQL).

- Для запуска тестов можно использовать Visual Studio (Инструмент: Обозреватель тестов).
- Также для запуска тестов можно использовать командную строку:
	Находясь в командной строке в основной директории - TaskTracker, для запуска всех тестов, введите команду: dotnet test 
Тесты Unit и Интеграционные тесты можно запускать отдельно:
	1. Для запуска unit тестов введите команду: dotnet test ./xUnitTests/TaskTracker.API.Tests
	2. Для запуска интеграционных тестов введите команду: dotnet test ./xUnitTests/TaskTracker.API.IntegrationTests

