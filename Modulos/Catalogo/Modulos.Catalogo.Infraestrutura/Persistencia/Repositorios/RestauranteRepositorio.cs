using Modulos.Catalogo.Aplicacao.Repositorios;
using Modulos.Catalogo.Dominio.Entidades;

namespace Modulos.Catalogo.Infraestrutura.Persistencia.Repositorios
{
    public class RestauranteRepositorio : IRestauranteRepositorio
    {
        private readonly CatalogoDbContext _catalogoDbContext;

        public RestauranteRepositorio(CatalogoDbContext catalogoDbContext)
        {
            _catalogoDbContext = catalogoDbContext;
        }

        public async Task AdicionarAsync(Restaurante restaurante)
        {
            await _catalogoDbContext.AddAsync(restaurante);
            await _catalogoDbContext.SaveChangesAsync();
        }

        public async Task<Restaurante> ObterPorIdAsync(Guid id)
        {
            return await _catalogoDbContext.FindAsync<Restaurante>(id);
        }
    }
}