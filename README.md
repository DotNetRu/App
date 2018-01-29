# DotNetRu App
Official cross-platform mobile app for DotNetRu


Содеражние
=================
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
* Выбрать **Мобильная разработка с .Net (Mobile development with .NET)** из рабочий сценариев при установке
![installation](https://developer.xamarin.com/guides/cross-platform/getting_started/installation/windows/Images/01-mobile-dev-workload.png)
* Скачать и распаковать репозиторий проекта ![download](https://i.imgur.com/trNraz7.png)
* Открыть `DotNetRuApp.sln`
* Установить `Android SDK level 26` (Android 8.0 - Oreo) с помощью sdk manager
* Собрать проект — `Build` -> `Build solution` (`Ctrl+Shift+B`)
* Запустить (либо на эмуляторе, либо на реальном устройстве). Для запуска на iOS необходим Macintosh, Android можно запускать на любой системе. Инструкции по запуске на [Android](https://developer.xamarin.com/guides/android/getting_started/installation/windows/#Android_Emulator), [iOS](https://developer.xamarin.com/guides/ios/getting_started/installation/)

## Как сделать pull request
[Инструкция](https://help.github.com/articles/creating-a-pull-request-from-a-fork/) от гитхаба (английский), [статья](https://rustycrate.ru/руководства/2016/03/07/contributing.html) на русском. В описании обязательно укажите сделанные Вами изменения

## Как оформить баг (вопрос, фичу)
Для этого воспользуйтесь разделом `Issues`, нажмите `New issue` и заполните необходимые поля

## Чем помочь
Вы можете выбрать любую из задач по своему желанию и возможностям из раздела `Issues`. В первую очередь стоит смотреть на <kbd>[help wanted](https://github.com/DotNetRu/App/issues?q=is%3Aissue+is%3Aopen+label%3A%22help+wanted%22)</kbd> и на <kbd>[good first issue](https://github.com/DotNetRu/App/issues?q=is%3Aissue+is%3Aopen+label%3A%22good+first+issue%22)</kbd>
