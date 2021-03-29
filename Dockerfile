FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /ToDoApp
EXPOSE 5432

COPY *.sln ./
COPY ToDoApp/*.csproj ./ToDoApp/
COPY ToDoAppTests/*.csproj ./ToDoAppTests/
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /ToDoApp
COPY --from=build-env /ToDoApp/out .
ENTRYPOINT ["dotnet", "ToDoApp.dll"]