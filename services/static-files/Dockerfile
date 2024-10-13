FROM mcr.microsoft.com/dotnet/sdk:8.0 AS compile-img

WORKDIR /root/app

COPY . .

RUN dotnet restore --use-current-runtime

RUN dotnet publish -c Release --os linux --arch x64 --use-current-runtime --self-contained false ./WebApi/StaticFiles.WebApi.csproj -o /root/app/bin

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime-img

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

RUN apt-get update && \
    apt-get install libterm-readkey-perl -yq && \
    apt-get install -yq tzdata && \
    ln -fs /usr/share/zoneinfo/Europe/Athens /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata && \
    apt-get upgrade -yq

WORKDIR /root/app/bin

COPY --from=compile-img /root/app/bin .

EXPOSE 5421

ENTRYPOINT ["dotnet", "StaticFiles.WebApi.dll"]
