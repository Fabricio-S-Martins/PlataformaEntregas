using Modulos.Autenticacao.Dominio.Entidades;

namespace Modulos.Autenticacao.Aplicacao.Repositorios;

public interface IUsuarioRepositorio
{
    Task AdicionarAsync(Usuario usuario);
}