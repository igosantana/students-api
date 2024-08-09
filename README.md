# Student API

Este projeto é uma API RESTful desenvolvida com .NET 6 que gerencia informações de estudantes e fornece autenticação básica usando JWT. Ele utiliza o Entity Framework Core com um banco de dados em memória para armazenar os dados dos estudantes e usuários.

## Funcionalidades

- **Endpoints para Estudantes:**
  - `GET /api/students`: Retorna todos os estudantes (autenticado).
  - `GET /api/students/{id}`: Retorna um estudante específico (autenticado).
  - `POST /api/students`: Cria um novo estudante (autenticado).
  - `PATCH /api/students/{id}`: Atualiza um estudante existente (autenticado).
  - `DELETE /api/students/{id}`: Deleta um estudante (autenticado).

- **Endpoint para Autenticação:**
  - `POST /api/auth/login`: Autentica um usuário e retorna um token JWT.

## Configuração

### Requisitos

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [CsvHelper](https://joshclose.github.io/CsvHelper/)
- [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

### Instalação

1. **Clone o repositório:**

   ```bash
   git clone https://github.com/username/student-api.git
2. **Navegue até o diretório do projeto:**

   ```bash
   cd student-api
3. **Restaure as dependências do projeto:**

   ```bash
   dotnet restore 
4. **Compile o projeto:**

   ```bash
   dotnet build
5. **Execute o projeto:**

   ```bash
   dotnet run

### Configuração do JWT

1. **Configure o segredo do JWT:**
   
  No arquivo appsettings.json, adicione a configuração do JWT:
  ```json
    {
        "JwtConfig": {
          "Secret": "YOUR_SECRET_HERE",
          "Issuer": "yourIssuer",
          "Audience": "yourAudience"
        }
    }
  ```

### Dados iniciais

- CSV para estudantes, coloque o caminho do arquivo que está na sua máquina.
- Usuário padrão: Um usuário padrão com o nome de usuário "admin" e a senha "admin" é adicionado para fins de teste.

### Testes

Para testar a API, você pode usar ferramentas como Postman ou cURL. Aqui estão alguns exemplos de como testar os endpoints:

**Autenticação**

`POST localhost:5055/api/auth/login`


Body:
  ```json
    {
      "username": "admin",
      "password": "admin"
    }
  ```


Resposta:
   ```json
   {
  "token": "YOUR_JWT_TOKEN"
   }
  ```

**Gerenciar Estudantes**

`GET http://localhost:5000/api/students`


Headers:
  ```makefile
   Authorization: Bearer YOUR_JWT_TOKEN
  ```
Resposta:

   ```json
   [
    {
      "id": 1,
      "nome": "Alice",
      "idade": 10,
      "serie": 5,
      "notaMedia": 8.5,
      "endereco": "123 Main St",
      "nomePai": "John Doe",
      "nomeMae": "Jane Doe",
      "dataNascimento": "2013-05-15T00:00:00"
    },
    ...
  ]
  ```

### Swagger

A API está documentada usando Swagger. Para acessar a documentação e testar os endpoints, execute a aplicação e navegue para:

```arduino
http://localhost:5000/
```

Para autenticar as requisições no Swagger:

1. Clique no botão "Authorize" na parte superior direita da interface do Swagger.
2. Insira o token JWT no campo "Value", precedido pela palavra "Bearer " (por exemplo, "Bearer eyJhbGciOiJIUzI1NiIsInR...").
3. Clique em "Authorize".

### Estrutura do projeto
- `Program.cs:` Configuração e inicialização da aplicação.
- `AppDbContext.cs:` Configuração do Entity Framework Core.
- `Models:` Definições de modelos de dados.
- `Controllers:` Definições de controladores da API.
- `Repositories:` Implementações de repositórios para acesso a dados.
- `Dtos:` DTOs de requisição


### Licença

  *MIT*
