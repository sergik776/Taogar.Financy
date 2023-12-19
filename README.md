# Taogar.Financy

Финансовое приложение с отдельным авторизационным сервером

Авторизационный сервер KeyCloak.
База данных приложения SQLite.

Структура проекта 5 уровней:
1) Домен
   1.1) Taogar.Authentication.Domain
   1.2) Taogar.Notes.Domain
3) Инфраструктара
   2.1) Taogar.Notes.Infrastructure
4) Источник данных
   3.1) Taogar.Notes.Database
5) Приложение
   4.1) Logger
   4.2) Taogar.Authentication.Application
   4.3) Taogar.Notes.Application
6) Презентация
   5.1) Taogar.Notes.Presentation

## Установка

Настройте переменные среды в файле appsettings.json

## Использование

Существующие запросы можно посмотреть в документации Swagger.
