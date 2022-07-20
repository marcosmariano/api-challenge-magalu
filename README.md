## Introdução

Desafio do LuizaLabs

Projeto utiliza de :

- .NET Core 6.0
- MySql 8.0
- Docker e Docker Compose
- GitLab CI
- GitPod

# Links de Referência

- [.NET Hello World tutorial](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/)
- [.NET 6.0 Download](https://dotnet.microsoft.com/download/dotnet/6.0)
- [MySql 8.0](https://dev.mysql.com/doc/relnotes/mysql/8.0/en/)
- [Docker Download](https://docs.docker.com/get-docker/)
- [Docker Compose Download](https://docs.docker.com/compose/install/)
- [GitLab CI Documentation](https://docs.gitlab.com/ee/ci/)
- [GitPod Documentation](https://docs.gitlab.com/ee/integration/gitpod.html)

# GitPod

Ambiente totalmente automatizado e configurado [Gitpod](https://docs.gitlab.com/ee/integration/gitpod.html) permite que você não precise instalar e configurar um determinado ambiente em sua máquina local para fazer o buid e rodar o projeto ou até mesmo visualizar o código fonte do projeto.

O `.gitpod.yml` garante que ao abrir o GitPod através de seu repositório, voce tenha um ambiente de trabalho .NET Core pré-instalado, e seu projeto automaticamente faça o build e comece a rodar.

# Executando projeto através do Docker

Para uma maior facilidade ao executar o programa, você pode utilizar o `docker-compose` para subir a aplicação junto com suas dependencias em formato de containers.
Porém você deve ter instalado em sua máquina local o `Docker` e `docker-compose`. Para instala-los você pode ir até os [Links de Referência](#links-de-referência) e acessar os respectivos links

Primeiramente deverá ser criado sua rede interna, para que a app do container acesse o banco do container

```
docker network create -d bridge my-network
```

Para subir os containers basta na raiz deste projeto executar o comando:

```
docker-compose up -d
```

O app será exposto em :

```
https://localhost:5555/swagger
```

# Executando o projeto pelo terminal

Primeiramente você deve preparar seu ambiente instalando do client do .NET Core 6.0 . Para instala-lo você pode ir até os [Links de Referência](#links-de-referência) e acessar os respectivos links referente ao .NET

Após instalado, na pasta raiz do projeto você devera executar o seguinte comando:

```
dotnet restore
dotnet build
dotnet run --project src/LuizaLabs.Challenge
```

O app será exposto em :

```
https://localhost:5001/swagger
```

# Autenticação e Autotização

Para autenticar basta ir no endpoint de autenticação em:

```
https://localhost:5001/api/v1/users/autenticate
```

Ou caso esteja pelo Docker:

```
https://localhost:5555/api/v1/users/autenticate
```

A autorização está dividida em dois níveis, sendo uma `Admin` e outra `User`
Para logar como Admin:

```
{
  "username": "admin",
  "password": "admin"
}
```

Para logar como User:

```
{
  "username": "user",
  "password": "user"
}
```

Ao obter o token, adiciona-lo ao `Authorize` do `swagger` no formato:

```
Bearer <seu-token-aqui>
```

</br>

### OBS: Para a documentação dos endpoints acessar Swagger da aplicação
