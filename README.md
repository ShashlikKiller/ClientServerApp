# Архитектуры Информационных Систем


Клиент-серверное приложение, реализованное по принципу клиент-сервер на UDP протоколе.

### Готовые лабораторные работы:
#### [Лабораторная работа №1](https://github.com/ShashlikKiller/Arch_IS_Lab1/releases/tag/lab)
#### [Лабораторная работа №2](https://github.com/ShashlikKiller/IS_Arch/tree/d868e8d366d2c942b6675b5b7a5fdf8e81cb5790#%D0%BB%D0%B0%D0%B1%D0%BE%D1%80%D0%B0%D1%82%D0%BE%D1%80%D0%BD%D0%B0%D1%8F-%D1%80%D0%B0%D0%B1%D0%BE%D1%82%D0%B0-2-%D0%B2-%D1%80%D0%B0%D0%B1%D0%BE%D1%82%D0%B5)
#### [Лабораторная работа №3](https://github.com/ShashlikKiller/IS_Arch)
#### [Лабораторная работа №4](https://github.com/ShashlikKiller/ClientServerApp/releases/tag/lab)
#### [Лабораторная работа №5](https://github.com/ShashlikKiller/OAuthVK)
#### [Лабораторная работа №6](https://github.com/ShashlikKiller/TpuRaspParser)
#### [Лабораторная работа №7](https://github.com/ShashlikKiller/ClientServerApp/releases/tag/lab7)

### Возможности, реализуемые в приложении:

 Клиент:

 * Отправляет на сервер команды при выполнении каких-либо событий(запуск приложения, обработка нажатия на кнопки и т.д.)
 * Выводит информацию, полученную с сервера
 * Отправляет сереализованные данные для сохранения на базе данных сервера

 Сервер:

 * Передача запрошенных данных из базы данных
 * Удаление записи в базе данных по индексу
 * Добавление новой записи при получении команды и сереализованной строки от клиента
 * Логирование операций на стороне сервера (NLog)
 * Сервер реализует многопоточность при помощи асинхронных методов
