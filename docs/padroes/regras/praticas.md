---
tags: [padrao]
---

# Práticas adotadas

Regras de código do projeto; sem marca é obrigatória, `(preferível)` é recomendada.

- LSP: subtipos substituem o tipo base sem quebrar o comportamento esperado.
- DIP: depender de abstração, na direção que a arquitetura do projeto definir; injeção via construtor.
- Demeter: um objeto só fala com seus vizinhos diretos.
- 1 indentação por método.
- Sem else — Grep de `\belse\b` nos arquivos de código do escopo; nenhuma ocorrência é indício de uso.
- KISS no método: método privado só com reuso real ou (complexidade isolável + conceito de domínio nomeável); senão inline.
- YAGNI: interface só com 2 ou mais implementações ou em fronteira de infraestrutura.
- DRY: extrair na 3ª duplicação.
- Prioridade em conflito: KISS/YAGNI > padronização > DRY > SOLID > calisthenics.
- Método com no máximo 15 linhas. (preferível)
- Classe com no máximo 150 linhas e no máximo 5 atributos. (preferível)
- OCP: aberto para extensão, fechado para modificação. (preferível)
- ISP: interfaces pequenas e específicas. (preferível)
- Value objects para primitivos de domínio. (preferível)
- Coleção de primeira classe. (preferível)
- Sem getters/setters expondo estado. (preferível)
- Sem abreviações nos nomes. (preferível)
