FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AppInvesting/AppInvesting.csproj", "AppInvesting/"]
RUN dotnet restore "AppInvesting/AppInvesting.csproj"
COPY . .
WORKDIR "/src/AppInvesting"
RUN dotnet build "AppInvesting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppInvesting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppInvesting.dll"]
