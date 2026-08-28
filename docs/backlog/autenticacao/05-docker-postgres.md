# 05 — Infraestrutura: subir PostgreSQL via Docker Compose

**Módulo:** Autenticação
**Camada:** Infraestrutura (ambiente)
**Status:** feito

## Contexto

Para implementar `IUsuarioRepositorio` de verdade (task futura), precisamos de um banco de dados rodando. Em vez de instalar PostgreSQL direto na máquina, vamos rodá-lo em container — ambiente isolado, reproduzível, fácil de recriar do zero.

## Conceito novo: Docker e Docker Compose

**Docker** empacota uma aplicação (aqui, o PostgreSQL) com tudo que ela precisa pra rodar, num container isolado do seu sistema operacional. **Docker Compose** é uma ferramenta pra descrever, num único arquivo YAML, quais containers seu projeto precisa (banco, cache, etc.) e como eles se conectam — em vez de digitar comandos `docker run` longos toda vez, você roda `docker compose up` e ele sobe tudo.

Referência: https://docs.docker.com/compose/gettingstarted/

## O que fazer

1. Criar o arquivo `docker-compose.yml` na raiz do repositório (sem subpasta), definindo um serviço de PostgreSQL.
2. Definir usuário, senha e nome do banco via variáveis de ambiente do serviço (não deixar senha fixa/óbvia no arquivo — usar um valor de desenvolvimento simples está ok, já que é ambiente local).
3. Mapear a porta padrão do PostgreSQL (5432) para a máquina host, e um volume nomeado para persistir os dados entre reinicializações do container.
4. Subir o container e confirmar que o banco aceita conexão (ex: via `docker exec` rodando `psql`, ou uma ferramenta cliente de sua preferência).

## Arquivos a criar/alterar

- `docker-compose.yml` (raiz do repositório, sem subpasta)

## Checklist

- [X] `docker-compose.yml` criado na raiz do repositório
- [X] Serviço de PostgreSQL definido, com usuário/senha/nome do banco via variáveis de ambiente
- [X] Porta 5432 mapeada para o host
- [X] Volume nomeado configurado para persistência de dados
- [X] Container sobe com `docker compose up` sem erro
- [X] Conexão ao banco confirmada (consegue conectar e listar databases)

## Notas / decisões tomadas

- Banco escolhido: PostgreSQL.
- Resolvido o crash loop: imagem fixada em `postgres:16` (em vez de `postgres` sem tag), e o volume antigo/incompatível foi apagado com `docker compose down -v` antes de subir de novo. Conexão validada via `psql`, listando os databases (`plataformaentregas`, `postgres`, `template0`, `template1`).

## Histórico de dúvidas
