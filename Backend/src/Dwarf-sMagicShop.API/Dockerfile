# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Dwarf-sMagicShop.API/Dwarf-sMagicShop.API.csproj", "src/Dwarf-sMagicShop.API/"]
COPY ["src/Acсounts/Dwarf-sMagicShop.Accounts.Domain/Dwarf-sMagicShop.Accounts.Domain.csproj", "src/Acсounts/Dwarf-sMagicShop.Accounts.Domain/"]
COPY ["src/Dwarf-sMagicShop.Shared/Dwarf-sMagicShop.Core.csproj", "src/Dwarf-sMagicShop.Shared/"]
COPY ["src/Acсounts/Dwarf-sMagicShop.Accounts.Infrastructure/Dwarf-sMagicShop.Accounts.Infrastructure.csproj", "src/Acсounts/Dwarf-sMagicShop.Accounts.Infrastructure/"]
COPY ["src/Acсounts/Dwarf-sMagicShop.Accounts.Application/Dwarf-sMagicShop.Accounts.Application.csproj", "src/Acсounts/Dwarf-sMagicShop.Accounts.Application/"]
COPY ["src/Dwarf-sMagicShop.Crafters.Application/Dwarf-sMagicShop.Crafters.Application.csproj", "src/Dwarf-sMagicShop.Crafters.Application/"]
COPY ["src/Dwarf-sMagicShop.Crafters.Domain/Dwarf-sMagicShop.Crafters.Domain.csproj", "src/Dwarf-sMagicShop.Crafters.Domain/"]
COPY ["src/Dwarf-sMagicShop.Species.Application/Dwarf-sMagicShop.Species.Application.csproj", "src/Dwarf-sMagicShop.Species.Application/"]
COPY ["src/Dwarf-sMagicShop.Species.Domain/Dwarf-sMagicShop.Species.Domain.csproj", "src/Dwarf-sMagicShop.Species.Domain/"]
COPY ["src/Dwarf-sMagicShop.Crafters.Infrastructure/Dwarf-sMagicShop.Crafters.Infrastructure.csproj", "src/Dwarf-sMagicShop.Crafters.Infrastructure/"]
COPY ["src/Dwarf-sMagicShop.Species.Infrastructure/Dwarf-sMagicShop.Species.Infrastructure.csproj", "src/Dwarf-sMagicShop.Species.Infrastructure/"]
RUN dotnet restore "./src/Dwarf-sMagicShop.API/Dwarf-sMagicShop.API.csproj"
COPY . .
WORKDIR "/src/src/Dwarf-sMagicShop.API"
RUN dotnet build "./Dwarf-sMagicShop.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Dwarf-sMagicShop.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dwarf-sMagicShop.API.dll"]