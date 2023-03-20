# Используем образ для сборки приложения ASP.NET 6.0
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Копируем файлы проекта и восстанавливаем зависимости
COPY *.csproj .
RUN dotnet restore

# Копируем исходный код и выполняем сборку
COPY . .
RUN dotnet publish -c Release -o out

# Создаем конечный образ и копируем скомпилированные файлы приложения
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Устанавливаем переменную среды ASPNETCORE_URLS

# Открываем порт, который будет слушать приложение
EXPOSE 80

# Запускаем приложение
ENTRYPOINT ["dotnet", "AssistantApi.dll"]
