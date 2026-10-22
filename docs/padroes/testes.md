# Testes

Regras dos testes do projeto; sem marca é obrigatória, `(preferível)` é recomendada.

- O que merece teste direto fica em classe pública, nunca privado nem testado por reflection.
- Teste xUnit com nome `Metodo_ComCenario_DeveResultado`; o segmento do meio começa com `Com`. (preferível) — Grep de `\[(Fact|Theory)\]` com 2 linhas depois e olhar os nomes.
