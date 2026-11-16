---
tags: [decisao, modulo/compartilhado, decisao/aceita]
---
# Módulos se comunicam por eventos de integração em contratos compartilhados

- **Decisão e porquê:** a comunicação entre módulos usa Integration Events, records imutáveis em `Compartilhado.Contratos`, sem dependência de nenhum módulo. Hoje são publicados in-process via MediatR (`INotification`/`IPublisher`); ao migrar para RabbitMQ os contratos continuam os mesmos.
- **Descartado:** referência direta entre módulos — acopla as unidades e impede extrair um módulo depois.
- **Não fazer:** um módulo referenciar o projeto de outro; o que cruza a fronteira vira contrato em `Compartilhado.Contratos`.
