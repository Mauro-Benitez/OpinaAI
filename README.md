# Sistema de Feedbacks NPS com IA (OpinaAI)

Este projeto é um sistema de pedidos baseado em uma arquitetura moderna de microserviços, utilizando AWS e comunicação assíncrona.

# 🚀Funcionalidades Principais
- Coleta de Feedback: API de alta performance para receber notas (0-10) e comentários.

- Cálculo de NPS: Processamento em segundo plano (Background Worker) para fechar o score mensal.

- Análise com IA: Worker dedicado que consome a API da OpenAI (GPT) para classificar feedbacks como Positivo/Neutro/Negativo e extrair tópicos-chave (ex: "Atendimento", "Lentidão").

- Relatórios Detalhados: Geração automática de relatórios em CSV salvos em um Bucket S3.

- Endpoints para visualizar o caclulos do NPS, distribuição de sentimentos e tópicos.

# 🛠️ Tecnologias Utilizadas
- .NET 9 (C#)

- Arquitetura Limpa (Clean Architecture) & DDD

- Entity Framework Core (PostgreSQL)
  
- Bucket S3
  
- Background Services (IHostedService para Workers)

- OpenAI API (Integração de IA)

# 🏗️ Arquitetura
O sistema segue os princípios da Clean Architecture, dividindo as responsabilidades em camadas:

- Domain: Entidades (Feedback, Report) e Enums.

- Application: Interfaces, Serviços e DTOs.

- Infrastructure: Implementação de Repositórios, Contexto do Banco, Clientes de IA e Workers.

- API: Controllers e Endpoints REST.



![OpinaAI](https://github.com/user-attachments/assets/affec56c-cf51-4c21-b591-bcef8c925e43)


