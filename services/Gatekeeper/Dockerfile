FROM mcr.microsoft.com/dotnet/sdk:8.0 AS compile-img

WORKDIR /root/app

COPY . .

RUN dotnet restore --use-current-runtime ./WebApi/Gatekeeper.WebApi.csproj

RUN dotnet publish -c Release --os linux --arch x64 --use-current-runtime --self-contained false ./WebApi/Gatekeeper.WebApi.csproj -o /root/app/bin

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime-img

WORKDIR /root/app/bin

COPY --from=compile-img /root/app/bin .

EXPOSE 5420

ENTRYPOINT ["dotnet", "Gatekeeper.WebApi.dll"]
