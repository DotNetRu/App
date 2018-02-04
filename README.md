# DotNetRu App

Официальное приложение русскоязычных DotNetRu юзер-групп.

С его помощью вы сможете
- Быть в курсе последних событий
- Просматривать полный архив всех событий DotNetRu
- Смотреть видеозаписи докладов

Сделано командой [DotNetRu](http://dotnet.ru)

<a href="https://play.google.com/store/apps/details?id=com.dotnetru.droid" target="_blank"><img alt="Get it on Google Play" src="https://imgur.com/YQzmZi9.png" width="153" height="46"></a> <a href="https://itunes.apple.com/us/app/dotnetru/id1293895734?ls=1&mt=8" target="_blank"><img src="https://imgur.com/GdGqPMY.png" width="135" height="40"></a>

## Скриншоты
![ios_news](https://is3-ssl.mzstatic.com/image/thumb/Purple118/v4/12/10/1f/12101f51-09cd-a6ac-9e9c-b8f237604634/source/230x0w.png) ![android_news](https://lh3.googleusercontent.com/r99Q2BiavXCV01A5SBiExR-JB8rNiL6q4-yIRDhoslvGoB2ISg8O3X1mT2PmpWmP=h409)

![ios_speakers](https://is5-ssl.mzstatic.com/image/thumb/Purple118/v4/a7/64/5f/a7645fb3-77a5-6105-31b0-5e8c3275ac8a/source/230x0w.png) ![android_spealers](https://lh3.googleusercontent.com/KBKf_B589k1Hffbb9lxY4yOYWopneQ0K-ykzVPs3VtNOPHZP-IqMzfWx6Rb87ZP37w=h409)

![ios_meetups](https://is3-ssl.mzstatic.com/image/thumb/Purple128/v4/56/d6/0a/56d60aa2-8175-3f14-4e8b-12469b968090/source/230x0w.png) ![android_meetups](https://lh3.googleusercontent.com/YbW7QEbM-cSIjS6XYs9IsffkvtSJr665PVL8N_GdoRDUQpG1CllRuEsqY3LhsebWxlA=h409)

Содержание
=================
* [Описание](#dotnetru-app)
* [Скриншоты](#Скриншоты)
* [Состояние сброки](#Состояние-сборки)
* [Список поддерживаемых ОС](#cписок-поддерживаемых-ОС-на-текущий-момент)
* [Как собрать приложение](#Как-собрать-приложение)
* [Как сделать pull request](#Как-сделать-pull-request)
* [Как оформить баг (вопрос, фичу)](#Как-оформить-баг-вопрос-фичу)
* [Чем помочь](#Чем-помочь)

## Состояние сборки
| Branches | Android | iOS  |
|:-------------:|:-------------:|:-----:|
| develop     | [![Build status](https://build.mobile.azure.com/v0.1/apps/d88bbc78-9a83-4700-a3f1-cd76cbd4a249/branches/develop/badge)](https://mobile.azure.com) | [![Build status](https://build.mobile.azure.com/v0.1/apps/c354b957-1495-4cf3-851b-da22c7f36e6e/branches/develop/badge)](https://mobile.azure.com) |
| master      | [![Build status](https://build.mobile.azure.com/v0.1/apps/d88bbc78-9a83-4700-a3f1-cd76cbd4a249/branches/master/badge)](https://mobile.azure.com)     |   [![Build status](https://build.mobile.azure.com/v0.1/apps/c354b957-1495-4cf3-851b-da22c7f36e6e/branches/master/badge)](https://mobile.azure.com) |

## Cписок поддерживаемых ОС (на текущий момент)
* iOS (>= 10)
* Android (>= 5)

## Как собрать приложение
*подробная инструкция по установке visual studio доступна на сайте [xamarin](https://developer.xamarin.com/guides/cross-platform/getting_started/installation/windows/)*
* Установить [visual studio](https://www.visualstudio.com/vs/whatsnew/) или [visual studio for mac](https://www.visualstudio.com/vs/visual-studio-mac/) в любой из редакций
* Выбрать **Мобильная разработка с .Net (Mobile development with .NET)** из рабочих сценариев при установке
![installation](https://developer.xamarin.com/guides/cross-platform/getting_started/installation/windows/Images/01-mobile-dev-workload.png)
* Скачать и распаковать репозиторий проекта ![download](https://i.imgur.com/trNraz7.png)
* Открыть `DotNetRuApp.sln`
* Установить `Android SDK level 26` (Android 8.0 - Oreo) с помощью sdk manager
* Собрать проект — `Build` -> `Build solution` (`Ctrl+Shift+B`)
* Запустить (либо на эмуляторе, либо на реальном устройстве). Для запуска на iOS необходим Macintosh, Android можно запускать на любой системе. Инструкции по запуске на [Android](https://developer.xamarin.com/guides/android/getting_started/installation/windows/#Android_Emulator), [iOS](https://developer.xamarin.com/guides/ios/getting_started/installation/)

## Как сделать pull request
[Инструкция](https://help.github.com/articles/creating-a-pull-request-from-a-fork/) от гитхаба (английский), [статья](https://rustycrate.ru/руководства/2016/03/07/contributing.html) на русском. Pull request направьте в ветку develop, в описании обязательно укажите сделанные Вами изменения

## Как оформить баг (вопрос, фичу)
Для этого воспользуйтесь разделом [`Issues`](https://github.com/DotNetRu/App/issues), нажмите [`New issue`](https://github.com/DotNetRu/App/issues/new) и заполните необходимые поля

## Чем помочь
Вы можете выбрать любую из задач по своему желанию и возможностям из раздела [`Issues`](https://github.com/DotNetRu/App/issues). В первую очередь стоит смотреть на <kbd>[help wanted](https://github.com/DotNetRu/App/issues?q=is%3Aissue+is%3Aopen+label%3A%22help+wanted%22)</kbd> и на <kbd>[good first issue](https://github.com/DotNetRu/App/issues?q=is%3Aissue+is%3Aopen+label%3A%22good+first+issue%22)</kbd>
