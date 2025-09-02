# TaskTracker
ASP.NET Core Web API для управления проектами и задачами.

# Инструкция по развёртыванию:
- Клонирование командой: git clone -b dev https://github.com/IlyaMatveyev/TaskTracker
- Переходим в TaskTracker командой: cd TaskTracker
- Затем поднимаем контейнеры Docker командой: docker-compose up --build
- После того как появилось сообщение об успешном применении миграций: "The database migrations were applied successfully."
Можно переходить в Swagger UI по url: http://localhost:5000/swagger/index.html
- Чтобы удалить контейнеры и очистить память используйте команду: docker-compose down -v



