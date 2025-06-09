
# 💹 Corretora de Investimentos - Renda Variável

Este projeto simula uma plataforma de controle de investimentos em renda variável, desenvolvida como parte de um teste técnico. A aplicação permite cadastro de usuários, registro de operações de compra e venda de ativos, cálculo de posições, P&L (lucro/prejuízo), consumo de cotações em tempo real via Kafka, e exposição de APIs RESTful com dados consolidados.

---

## 📘 Contexto

Investimentos em renda variável não têm retorno previsível e dependem da oscilação do mercado. Este sistema foi modelado para lidar com:

- Ações, FIIs, ETFs
- Operações de compra e venda com corretagem
- Atualizações de cotação em tempo real
- Cálculo de posição por cliente e ativo
- APIs REST para consultas de indicadores financeiros

---

## 🧱 Estrutura da Solução

| Projeto                 | Função                                                       |
|-------------------------|--------------------------------------------------------------|
| `Investimentos.Api`     | Backend REST em ASP.NET Core                                 |
| `Investimentos.Web`     | Frontend MVC (tela de login, operações, posição etc.)        |
| `Investimentos.Producer`| Worker Service Kafka (consumo de cotações)
| `Investimentos.Tests`   | Testes unitários com xUnit                                   |

---

## 📦 Executar com Docker

### Pré-requisitos
- Docker + Docker Compose

### Comando único para subir tudo:

```bash
git clone https://github.com/carlosrodr1/corretora-investimentos-app.git

cd corretora-investimentos-app

docker-compose up --build
```

### Serviços disponíveis:

| Serviço         | URL                           |
|-----------------|--------------------------------|
| API             | http://localhost:5030/swagger |
| Web (MVC)       | http://localhost:5040         |
| MySQL           | localhost:3307 (root/1234)     |

---

## 🗃️ Modelagem de Dados (MySQL)

```sql
CREATE TABLE usuario (
  id INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(100) NOT NULL,
  email VARCHAR(150) NOT NULL,
  senha_hash VARCHAR(255) NOT NULL,
  corretagem_percentual DECIMAL(5,2) NOT NULL
);

CREATE TABLE ativo (
  id INT AUTO_INCREMENT PRIMARY KEY,
  codigo VARCHAR(10) NOT NULL,
  nome VARCHAR(100) NOT NULL
);

CREATE TABLE operacao (
  id INT AUTO_INCREMENT PRIMARY KEY,
  usuario_id INT NOT NULL,
  ativo_id INT NOT NULL,
  quantidade INT NOT NULL,
  preco_unitario DECIMAL(15,4) NOT NULL,
  tipo_operacao VARCHAR(10) NOT NULL,
  corretagem DECIMAL(10,2) NOT NULL,
  data_hora DATETIME NOT NULL
);

CREATE TABLE cotacao (
  id INT AUTO_INCREMENT PRIMARY KEY,
  ativo_id INT NOT NULL,
  preco_unitario DECIMAL(15,4) NOT NULL,
  data_hora DATETIME NOT NULL
);

CREATE TABLE posicao (
  id INT AUTO_INCREMENT PRIMARY KEY,
  usuario_id INT NOT NULL,
  ativo_id INT NOT NULL,
  quantidade INT NOT NULL,
  preco_medio DECIMAL(15,4) NOT NULL,
  pl DECIMAL(15,2) NOT NULL
);

```

---

## 📊 Consultas Otimizadas

```sql
-- Índice sugerido
CREATE INDEX idx_usuario_ativo_data ON operacao(usuario_id, ativo_id, data_hora);

-- Consulta rápida de operações por usuário e ativo nos últimos 30 dias
SELECT * FROM operacao
WHERE usuario_id = @usuarioId
  AND ativo_id = @ativoId
  AND data_hora >= NOW() - INTERVAL 30 DAY;

```

---

## 📈 Lógica de Negócio

- Cálculo de **preço médio ponderado**
- Atualização automática da **posição e P&L** com base em novas cotações Kafka
- Circuit Breaker aplicado caso o serviço de cotações esteja fora

---

### OpenAPI disponível em:
📄 [http://localhost:5030/swagger](http://localhost:5030/swagger)

---

## 🧪 Testes

- ✅ Testes de unidade com xUnit para lógica de cálculo de preço médio
- ✅ Cobertura para cenários inválidos (quantidade zero, lista vazia)

---

## 🧠 Testes Mutantes (explicação)

> Testes mutantes validam a eficácia dos testes, modificando ligeiramente o código para ver se os testes ainda detectam erros.

Exemplo:  
Se trocarmos um `+` por `-` no cálculo do preço médio, os testes devem falhar.

---

## 🔁 Kafka + Worker

- Novo microserviço publica cotações em `cotacoes`
- Worker `.NET` consome e atualiza as posições com `retry` e `idempotência`

---

## ⚙️ Resiliência e Observabilidade

- Circuit breaker com fallback (ex: cotação indisponível → usa última conhecida)
- `try/catch` com logs e tratamento adequado em todas as camadas

---

## 📈 Escalabilidade

- Suporte a **auto-scaling horizontal**
- Estratégia de **round-robin** ou por **latência** para balanceamento de carga

---

## 👨‍💻 Desenvolvido por

Carlos Rodrigues — Full Stack Developer  
[github.com/carlosrodr1](https://github.com/carlosrodr1)
