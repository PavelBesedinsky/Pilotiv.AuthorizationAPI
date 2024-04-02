#### Описание

Сервис авторизации пользователей в процессе разработки.

Для хранения данных при локальной разработке применяется `UserSecrets`.

#### Создание секретного хранилища

Для создания секретного хранилища в терминале вызвать команду:

```
dotnet user-secrets init
```

###### Пример команды для генерации `UserSecrets` и добавления его в проект `Pilotiv.AuthorizationAPI.WebUI`

```
dotnet user-secrets init --project Pilotiv.AuthorizationAPI.WebUI/Pilotiv.AuthorizationAPI.WebUI.csproj
```[Pilotiv.AuthorizationAPI.WebUI.csproj](Pilotiv.AuthorizationAPI.WebUI%2FPilotiv.AuthorizationAPI.WebUI.csproj)