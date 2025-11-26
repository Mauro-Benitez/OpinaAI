# Sistema de Feedbacks NPS com IA (OpinaAI)

Este projeto é  um sistema completo para coleta, processamento e análise de feedbacks via NPS (Net Promoter Score). O diferencial do sistema é o uso de Inteligência Artificial (OpenAI) para analisar automaticamente o sentimento e extrair tópicos relevantes dos comentários dos usuários.

# 🚀Funcionalidades Principais
- Coleta de Feedback: API de alta performance para receber notas (0-10) e comentários.

- Cálculo de NPS: Processamento em segundo plano (Background Worker) para fechar o score mensal.

- Análise com IA: Worker dedicado que consome a API da OpenAI (GPT) para classificar feedbacks como Positivo/Neutro/Negativo e extrair tópicos-chave (ex: "Atendimento", "Lentidão").

- Relatórios Detalhados: Geração automática de relatórios em PDF salvos localmente (ou S3).

- Endpoints para visualizar o caclulos do NPS, distribuição de sentimentos e tópicos.

# 🛠️ Tecnologias Utilizadas
- .NET 9 (C#)

- Arquitetura Limpa (Clean Architecture) & DDD

- Entity Framework Core (PostgreSQL)

- Background Services (IHostedService para Workers)

- OpenAI API (Integração de IA)

# 🏗️ Arquitetura
O sistema segue os princípios da Clean Architecture, dividindo as responsabilidades em camadas:

- Domain: Entidades (Feedback, Report) e Enums.

- Application: Interfaces, Serviços e DTOs.

- Infrastructure: Implementação de Repositórios, Contexto do Banco, Clientes de IA e Workers.

- API: Controllers e Endpoints REST.



<img width="584" height="429" alt="OpinaAI" src="https://github.com/user-attachments/assets/d9f6e9f6-4814-4330-8044-b1c2917a53df" />

