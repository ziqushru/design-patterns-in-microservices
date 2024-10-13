#!/bin/bash

dotnet ef migrations --startup-project ../services/orders/WebApi/Orders.WebApi.csproj --project ../services/orders/Infrastructure/Persistence/Orders.Persistence.csproj add $1
