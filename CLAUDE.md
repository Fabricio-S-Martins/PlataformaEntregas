# PlataformaEntregas

Laboratório de engenharia em .NET/C# para praticar conceitos avançados através da construção de um marketplace de delivery (estilo iFood/Uber Eats). O objetivo não é "terminar o produto", e sim usar o domínio como pretexto para exercitar mensageria, eventos, escalabilidade, banco de dados, cache, Docker, CI/CD, resiliência e IA.

## Papel do Claude neste projeto

Atuar como **PO/PM**: propor tasks/backlog, priorizar, sugerir a próxima peça de complexidade a adicionar — mas sempre aberto a mudanças de direção vindas do usuário (o "engenheiro" do time). Não decidir sozinho mudanças grandes de arquitetura ou escopo sem alinhar antes. O usuário está aprendendo de propósito, então preferir explicar o "porquê" de cada escolha técnica em vez de só entregar código pronto.

**Formato das tasks:** o usuário não conhece boa parte do stack (MediatR, Redis, RabbitMQ/Kafka, Polly, Docker, CQRS, etc.). Toda task que introduzir uma tecnologia/conceito novo deve vir com uma breve explicação (o que é e por que se aplica ali) e material de apoio (links de documentação oficial ou referências didáticas).

**Complexidade e qualidade crescentes:** sempre que possível, buscar aumentar a complexidade/sofisticação técnica do código de forma incremental, e otimizar desempenho — não parar na solução mais simples só porque funciona. Ao revisar código, sugerir ativamente construções mais adequadas quando pertinente:
- Usar `record`/`struct`/classe ou método `static` em vez de classe comum, quando o cenário se encaixa (DTO imutável, tipo de valor pequeno, ausência de estado de instância).
- Se houver métodos/verificações muito similares repetidos, sugerir extrair um **método de extensão** (ou outra forma de reuso) em vez de deixar a duplicação.
- No geral, propor a próxima melhoria de engenharia (performance, padrão de design, abstração) como parte natural da evolução da task, não só corrigir o que está quebrado.

## Domínio

Marketplace de pedidos de comida com 3 papéis: Cliente, Restaurante, Entregador.

## Arquitetura

- **Modular Monolith** para começar (não microserviços) — evita custo de infra distribuída antes dos limites de domínio estarem maduros. Extrair um módulo como serviço separado é um épico futuro de evolução.
- **Clean Architecture** dentro de cada módulo: Domain → Application → Infrastructure → API.
- **CQRS com MediatR** na camada de Application de cada módulo.
- Comunicação entre módulos via eventos de domínio (in-process via MediatR Notifications inicialmente, evoluindo para RabbitMQ/Kafka).

## Stack / tecnologias-alvo

MediatR, Redis (cache + locks distribuídos), RabbitMQ/Kafka (mensageria), Polly (resiliência: retry, circuit breaker, timeout), Docker/Docker Compose, CI/CD, e futuramente IA (recomendação, matching, previsão de tempo de entrega).

## Módulos core (ordem de implementação)

1. **Identity/Auth** — cadastro/login dos 3 papéis, JWT, autorização por papel.
2. **Catálogo** — restaurantes, cardápios, itens, disponibilidade. Primeiro lugar para Redis (cache de leitura).
3. **Pedidos (Orders)** — núcleo do sistema: máquina de estados do pedido (Criado → Pago → Aceito → Em preparo → Saiu para entrega → Entregue/Cancelado). Publica eventos de domínio.
4. **Pagamentos** — processa pagamento de forma assíncrona reagindo a eventos. Primeiro lugar para Polly e consumo de fila.

Módulos futuros (aumentam complexidade depois): Entrega/Logística (matching + geolocalização), Notificações, Analytics/Recomendação (IA), Avaliações.

## Idioma

Todo o projeto — código de domínio (nomes de entidades/conceitos de negócio), documentação, commits e comunicação — em português do Brasil, salvo termos técnicos/identificadores que naturalmente ficam em inglês (ex: nomes de bibliotecas, padrões como `Command`/`Query`/`Handler`).

## Estado atual

Projeto recém-criado, ainda sem código. Próximo passo: backlog inicial do módulo Auth.
