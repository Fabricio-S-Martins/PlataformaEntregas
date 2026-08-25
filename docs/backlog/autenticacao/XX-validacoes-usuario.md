# XX — Domínio: reforçar validação de invariantes do Usuario

**Módulo:** Autenticação
**Camada:** Domínio
**Status:** backlog (não detalhada ainda — número/ordem definidos quando entrar na fila)

## Contexto

A entidade `Usuario` hoje só valida o e-mail. Nome vazio, senha vazia/fraca e outras invariantes não são verificadas. Isso é intencional por enquanto (task 01 focou em estrutura básica), mas precisa ser endereçado antes de fechar o módulo Autenticação — não como um ajuste cosmético de "adicionar uns ifs", e sim como oportunidade de discutir e escolher uma estratégia de validação mais robusta, já que isso é justamente o tipo de decisão de design que vale a pena explorar num projeto de estudo.

## A discutir quando esta task for detalhada

- Lançar exceção no construtor (abordagem atual) vs. **Notification Pattern** (acumula todos os erros de validação em vez de parar no primeiro) vs. **Result Pattern** (retorna sucesso/falha explícito em vez de exceção).
- Uso de biblioteca como `FluentValidation` (comum em Aplicação, mas há debate se cabe no Domínio) vs. validação manual no construtor/métodos de fábrica.
- Se `Usuario` deve ter um método de fábrica estático (`Usuario.Criar(...)`) em vez de construtor público, para permitir retorno de erro sem exceção.

Esta task será detalhada (com "O que fazer", arquivos exatos e checklist) quando chegar a vez dela no backlog — por ora é só um marcador pra não perder o contexto.
