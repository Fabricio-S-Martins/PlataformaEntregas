---
tags: [decisao, modulo/catalogo, decisao/aceita]
---
# Cache de leitura do Restaurante em Redis (cache-aside + write-through)

- **Decisão e porquê:** o Catálogo é leitura-pesada (cadastrado uma vez, lido milhares de vezes). A leitura usa cache-aside no Redis com TTL e chave `catalogo:restaurante:{id}`, e o cadastro aquece o cache (write-through).
- **Descartado:** ler sempre do Postgres sem cache — o Catálogo é leitura-pesada e cada leitura bateria no banco.
- **Não fazer:** gravar no cache fora do padrão da chave `catalogo:restaurante:{id}`; tratar o cache como fonte da verdade.
