# 11 — Cobrir geração e validação do token JWT com testes automatizados

**Módulo:** Autenticação
**Camada:** Infraestrutura
**Status:** feito

## Contexto

A task 09 destravou a validação de JWT na API (`AddJwtBearer`), mas `TokenServico` — quem efetivamente gera o token no login — segue sem nenhum teste. É a mesma lógica sensível já discutida nas tasks 09/10: um bug aqui é silencioso (a aplicação roda normalmente, só a segurança do token sai errada, ex: claim errada, token que nunca expira, assinatura fraca). Diferente da task 10 (camada Aplicação), `TokenServico` vive na Infraestrutura — não existe ainda projeto de testes pra essa camada no módulo Autenticação, esta task cria o primeiro.

## O que fazer

1. Criar o projeto de testes `Modulos.Autenticacao.Infraestrutura.Testes` (xUnit) em `Testes/Autenticacao/Modulos.Autenticacao.Infraestrutura.Testes/`.
2. Adicionar o pacote NuGet `Microsoft.Extensions.Configuration` ao projeto de testes, necessário para montar um `IConfiguration` real em memória via `ConfigurationBuilder().AddInMemoryCollection(...)`, sem precisar de `appsettings.json`.
3. Criar a pasta `Servicos/` dentro do projeto de testes.
4. Criar `TokenServicoTestes.cs` nessa pasta, usando a implementação real de `TokenServico` e um `IConfiguration` montado com valores de teste (chave, issuer, audience, expiração).

## Cenários a cobrir

- `GerarToken` retorna uma string no formato de JWT válido (três segmentos separados por `.`).
- O token gerado contém a claim de Id (`sub`) com o mesmo valor de `usuario.Id`.
- O token gerado contém a claim de Papel com o mesmo valor de `usuario.Papel`.
- O token gerado é validado com sucesso (`JwtSecurityTokenHandler.ValidateToken`) usando a mesma chave/issuer/audience configurados — confirma que a assinatura está correta, não só que o token "parece" um JWT.
- O token gerado tem uma data de expiração futura, consistente com o valor de `ExpiresJWT` configurado.

## Arquivos a criar/alterar

- `Testes/Autenticacao/Modulos.Autenticacao.Infraestrutura.Testes/Modulos.Autenticacao.Infraestrutura.Testes.csproj` (novo projeto)
- `Testes/Autenticacao/Modulos.Autenticacao.Infraestrutura.Testes/Servicos/TokenServicoTestes.cs`

## Notas / decisões tomadas

- Sem Moq nesta task: `IConfiguration` é montado real (em memória), e `TokenServico` também é testado na implementação real — não há nenhuma dependência externa (banco, rede) que justifique mock aqui.
- Validar o token gerado contra `TokenValidationParameters` reais é o que garante que o teste pega um bug de assinatura/issuer/audience — só checar o formato da string (3 segmentos) não seria suficiente.

## Histórico de dúvidas
