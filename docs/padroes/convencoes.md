# Convenções da linguagem

Convenções de C# do projeto; todas `(preferível)`.

- Campo privado: `private readonly Tipo _camelCase`; `readonly` quando não muda após o construtor. (preferível) — Grep de `private\s+(readonly\s+)?[\w<>\[\]?]+\s+[A-Za-z]\w*\s*[;=]` (campo sem `_`); `readonly` lendo o arquivo.
- Visibilidade: classe de implementação usada só no próprio assembly (DI do mesmo projeto) é `internal`, salvo motivo real. (preferível) — Grep de `^\s*public\s+(sealed\s+)?class` e ver quem a usa.
- Namespace file-scoped (`namespace X;`), nunca em bloco; exceção: código gerado (Migrations). (preferível) — Grep de `^namespace\s+[\w.]+\s*(\{|$)`.
- Fim de arquivo: termina direto na última `}`, sem linha em branco. (preferível) — ler o fim de alguns arquivos.
- Construção: objeto de dados sem comportamento (request, response, VO simples) é `record`; tipo pequeno, imutável e em volume é `readonly struct`; classe ou método sem estado é `static`. (preferível) — Grep de `\brecord\b`.
- Nomes 100% em português (arquivo, classe, pasta, projeto); exceção: tipo imposto por biblioteca ou framework. (preferível)
