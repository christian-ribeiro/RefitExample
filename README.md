
# RefitMicroserviceAuth

Projeto de estudo utilizando **Refit** para simular a obtenção de credenciais, autenticação e reautenticação automática ao consumir múltiplos microservices.

---

## 🧠 Objetivo

Demonstrar, de forma prática, como estruturar um sistema baseado em microservices com autenticação via token, reutilização de tokens em cache e reautenticação transparente, tudo utilizando **Refit** como client HTTP fortemente tipado.

---

## 📦 Principais Projetos e Responsabilidades
```
RefitMicroserviceAuth/
├── RefitMicroserviceAuth.Api/              # API principal com endpoints mockados
│   └── Controllers/                        # Controladores por microserviço
├── RefitMicroserviceAuth.ApiClient/        # Configuração e consumo via Refit
│   └── Refit/                              # Interfaces, handlers e extensions Refit
├── RefitMicroserviceAuth.Arguments/        # DTOs, Enums, Cache e Constantes
├── RefitMicroserviceAuth.Domain/           # Serviços de domínio e interfaces
```
---

## 🔌 Endpoints de Demonstração

### 📦 API Gateway

- `GET /api/microservicegateway/{enterpriseId}` – Endpoint que consome os microservices simulados.

### 🔐 Simulação de Autenticação

- `POST /api/authenticate` – Simula um login e retorna um token.

### 📇 Simulação de Credenciais

- `POST /api/credential` – Retorna credenciais baseadas no ID da empresa e microservice.

### 🧪 Microservices Simulados

- `GET /gateway/ACBr/api/acbr`
- `GET /gateway/Mercos/api/mercos`

Ambos exigem header `Authorization` para responder com sucesso.

---

## 🧠 Arquitetura Refit Plugável

Este projeto utiliza uma abordagem genérica e dinâmica para registrar múltiplos clients Refit via atributo e herança.

### 🔌 Registro Genérico de Múltiplos Microservices

Interfaces Refit que consomem os microservices herdam `IMicroserviceRefitInterface` e são decoradas com o atributo `[MicroserviceRefit(MicroserviceEnum)]`.

Registro dinâmico no `Program.cs`:

```csharp
builder.Services.AppendRefitInterfaces<IMicroserviceRefitInterface>(
    httpClientConfigurator: (client, type, _) =>
    {
        client.BaseAddress = MicroserviceEnvironmentVariable.BaseAddress;

        var microserviceRefitAttribute = type.GetCustomAttributeFromHierarchy<MicroserviceRefitAttribute>();
        if (microserviceRefitAttribute != null)
            client.DefaultRequestHeaders.Add(ConfigurationConst.RefitClientHeader, microserviceRefitAttribute.Microservice.ToString());
    },
    refitSettings: refitSettings,
    configureHttpClientBuilder: builder => builder.AddHttpMessageHandler<MicroserviceHandler>()
);
```

### 🧪 Exemplo de Interface

```csharp
[MicroserviceRefit(EnumMicroservice.Mercos)]
public interface IMicroserviceMercosRefit : IMicroserviceRefitInterface
{
    [Get("/api/Mercos")]
    Task<ApiResponse<string>> GetMercos();
}
```

### ✅ Vantagens

- Plug-and-play: basta herdar e anotar para registrar.
- Evita duplicações no setup.
- Permite configuração centralizada para todos os microservices.

---

## 🧰 Tecnologias Utilizadas

- ASP.NET Core
- Refit
- DelegatingHandler para autenticação automática
- Injeção de Dependência
- ConcurrentDictionary para cache thread-safe

---

## 🚦 Empresas Simuladas

| Empresa   | Microservices Ativos |
|-----------|-----------------------|
| 1         | ACBr, Mercos          |
| 2         | ACBr                  |
| 3         | Mercos                |

---

## ▶️ Executando o projeto

```bash
git clone https://github.com/seu-usuario/RefitMicroserviceAuth.git
cd RefitMicroserviceAuth
dotnet restore
dotnet build
dotnet run --project RefitMicroserviceAuth.Api
```

---

## ✅ Aprendizados

- Como estruturar um handler reutilizável com autenticação dinâmica.
- Como evitar múltiplas requisições desnecessárias utilizando cache.
- Como reaproveitar token e lidar com reautenticação automática.
- Como registrar dinamicamente múltiplos clients Refit com base em atributos.

---
