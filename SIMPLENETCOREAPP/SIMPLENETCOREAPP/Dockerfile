FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app
RUN apt-get update && apt-get install -y build-essential openssh-server gdb gdbserver && mkdir /var/run/sshd
RUN echo 'root:toor' | chpasswd


RUN sed -i 's/#PermitRootLogin prohibit-password/PermitRootLogin yes/' /etc/ssh/sshd_config && \
 	sed -i 's@session\s*required\s*pam_loginuid.so@session optional pam_loginuid.so@g' /etc/pam.d/sshd && \
 	apt-get clean

EXPOSE 22

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY SIMPLENETCOREAPP/SIMPLENETCOREAPP.csproj SIMPLENETCOREAPP/
RUN dotnet restore SIMPLENETCOREAPP/SIMPLENETCOREAPP.csproj
COPY . .
WORKDIR /src/SIMPLENETCOREAPP
RUN dotnet build SIMPLENETCOREAPP.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SIMPLENETCOREAPP.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SIMPLENETCOREAPP.dll"]
