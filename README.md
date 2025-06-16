# TopMed Login API

Uma API Web ASP.NET Core segura que fornece funcionalidade de autenticação e autorização baseada em JWT.

## 🚀 Funcionalidades

- **Autenticação JWT**: Autenticação segura baseada em tokens
- **Gerenciamento de Usuários**: login de usuários
- **Segurança de Senhas**: Hash de senhas com BCrypt
- **Endpoints Protegidos**: Controle de acesso
- **Entity Framework Core**: Operações de banco de dados com abordagem code-first
- **Logging**: Logging estruturado
- **Validação de Entrada**: Validação de modelos e tratamento de erros

## 🛠️ Stack Tecnológica

- **Framework**: .NET 8.0
- **Banco de Dados**: Entity Framework Core com SQL Server
- **Autenticação**: JWT (JSON Web Tokens)
- **Hash de Senhas**: BCrypt.Net
- **Logging**: Microsoft.Extensions.Logging

## 📋 Pré-requisitos

- .NET 8.0 SDK ou posterior
- SQL Server (LocalDB, Express ou versão completa)
- Visual Studio 2022 ou VS Code (opcional)

## ⚙️ Configuração e Instalação

### 1. Clonar o Repositório
```bash
git clone <url-do-repositorio>
cd LoginTopMed
```

### 2. Configurar User Secrets
Configure sua configuração JWT usando os user secrets do .NET:

```bash
dotnet user-secrets set "JwtSettings:SecretKey" "sua-chave-super-secreta-com-pelo-menos-32-caracteres"
dotnet user-secrets set "JwtSettings:Issuer" "https://localhost:7199"
dotnet user-secrets set "JwtSettings:Audience" "https://localhost:7199"
dotnet user-secrets set "JwtSettings:AccessTokenExpireMinutes" "60"
dotnet user-secrets set "connectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=TopMedLoginDB;Trusted_Connection=true;MultipleActiveResultSets=true"
```

### 3. Executar a Aplicação
```bash
dotnet run
```

A API estará disponível em `https://localhost:7199` (ou a porta especificada nas suas configurações de inicialização).

## 📚 Endpoints da API

### Endpoints de Autenticação

#### POST `/api/auth/login`
Autentica um usuário e recebe um token JWT.

**Corpo da Requisição:**
```json
{
  "username": "user_1",
  "password": "usertopmed123*"
}
```

**Resposta (200 OK):**
```json
{
  "success": true,
  "message": "Login bem sucedido.",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "user_1",
    "name": "Maria",
    "email": "user1@topmed.com"
  }
}
```

**Resposta (401 Unauthorized):**
```json
{
  "success": false,
  "message": "Nome de usuário inválido."
}
```

#### POST `/api/auth/logout`
Faz logout do usuário autenticado (requer token JWT válido).

**Cabeçalhos:**
```
Authorization: Bearer <seu-jwt-token>
```

**Resposta (200 OK):**
```json
{
  "success": true,
  "message": "Logout realizado com sucesso.",
  "timestamp": "2025-06-16T10:30:00.000Z"
}
```

### Endpoints Protegidos

#### GET `/api/protectedendpoint`
Acessa dados protegidos (requer token JWT válido).

**Cabeçalhos:**
```
Authorization: Bearer <seu-jwt-token>
```

**Resposta (200 OK):**
```json
{
  "message": "Você está autenticado! Este é um endpoint protegido.",
  "userId": "1",
  "username": "user_1",
  "accessTime": "2025-06-16T10:30:00.000Z",
  "serverTime": "2025-06-16 10:30:00"
}
```

## 👥 Usuários Padrão

A aplicação vem com dois usuários pré-cadastrados para testes:

| Username | Email | Nome | Senha |
|----------|-------|------|-------|
| user_1 | user1@topmed.com | Maria | usertopmed123* |
| user_2 | user2@topmed.com | João | usertopmed123* |

## 🔧 Configuração

### Configurações JWT
Configure as configurações JWT nos user secrets ou appsettings.json:

```json
{
  "JwtSettings": {
    "SecretKey": "sua-chave-secreta",
    "Issuer": "https://localhost:7199",
    "Audience": "https://localhost:7199",
    "AccessTokenExpireMinutes": 60,
    "ConnectionStrings:DefaultConnection": ""
  }
}
```

### Configuração do Banco de Dados
A aplicação usa Entity Framework Core com SQL Server. Atualize a string de conexão no `appsettings.json` para corresponder à sua configuração de banco de dados.

## 🧪 Testando a API

### Usando cURL

**Login:**
```bash
curl -X POST https://localhost:7199/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"user_1","password":"usertopmed123*"}'
```

**Acessar Endpoint Protegido:**
```bash
curl -X GET https://localhost:7199/api/protectedendpoint \
  -H "Authorization: Bearer <seu-jwt-token>"
```

### Usando Postman ou Ferramentas Similares

1. Importe os endpoints da API
2. Configure variáveis de ambiente para a URL base e token
3. Teste o fluxo de autenticação
4. Verifique o acesso ao endpoint protegido

## 🏗️ Estrutura do Projeto

```
LoginTopMed/
├── Controllers/
│   ├── AuthController.cs          # Endpoints de autenticação
│   └── ProtectedEndpointController.cs  # Endpoints de recursos protegidos
├── Data/
│   └── LoginDBContext.cs          # Contexto do Entity Framework
├── DTOs/                          # Objetos de Transferência de Dados
├── Models/                        # Modelos de entidade
├── Services/
│   ├── Auth/
│   │   ├── AuthService.cs         # Lógica de negócio de autenticação
│   │   └── IAuthService.cs        # Interface de autenticação
│   └── Token/
│       ├── TokenService.cs        # Geração de tokens JWT
│       └── ITokenService.cs       # Interface do serviço de token
└── Program.cs                     # Inicialização da aplicação
```

## 🔐 Recursos de Segurança

- **Hash de Senhas**: Senhas são hasheadas usando BCrypt com salt
- **Segurança JWT**: Tokens são assinados com HMAC SHA256
- **Validação de Entrada**: Validação de estado de modelo em todos os endpoints
- **Cabeçalhos Seguros**: Códigos de status HTTP e formatos de resposta adequados
- **User Secrets**: Configuração sensível armazenada com segurança

## 📝 Logging

A aplicação inclui logging abrangente:

- **Informação**: Operações bem-sucedidas e atividades do usuário
- **Aviso**: Entrada inválida ou tentativas de autenticação falhadas
- **Erro**: Exceções e erros do sistema

Os logs incluem contexto do usuário e detalhes da operação para depuração e monitoramento.
