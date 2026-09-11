using Modulos.Catalogo.Dominio.Entidades;

namespace Modulos.Catalogo.Aplicacao.Repositorios
{
    public interface IRestauranteRepositorio
    {
        Task AdicionarAsync(Restaurante restaurante);
        Task<Restaurante> ObterPorIdAsync(Guid id);
    }
}