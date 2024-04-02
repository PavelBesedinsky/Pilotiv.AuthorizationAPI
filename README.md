#### Описание

Сервис авторизации пользователей в процессе разработки.

Для хранения данных при локальной разработке применяется `UserSecrets`.

Для `appsettings.json` требуется переопределить значения или задать их в `UserSecrets`.

#### Создание секретного хранилища

Для создания секретного хранилища в терминале вызвать команду:
```
dotnet user-secrets init
```

###### Пример команды для генерации "UserSecrets" и добавления его в проект "Pilotiv.AuthorizationAPI.WebUI"
```
dotnet user-secrets init --project Pilotiv.AuthorizationAPI.WebUI/Pilotiv.AuthorizationAPI.WebUI.csproj
```

###### Пример определения полей для "OAuthVkCredentials" в "UserSecrets"
```
dotnet user-secrets set "OAuthVkCredentials:ClientId" "client_id" --project Pilotiv.AuthorizationAPI.WebUI/Pilotiv.AuthorizationAPI.WebUI.csproj
```
```
dotnet user-secrets set "OAuthVkCredentials:ClientSecret" "client_secret" --project Pilotiv.AuthorizationAPI.WebUI/Pilotiv.AuthorizationAPI.WebUI.csproj
```
```
dotnet user-secrets set "OAuthVkCredentials:RedirectUri" "redirect_uri" --project Pilotiv.AuthorizationAPI.WebUI/Pilotiv.AuthorizationAPI.WebUI.csproj
```