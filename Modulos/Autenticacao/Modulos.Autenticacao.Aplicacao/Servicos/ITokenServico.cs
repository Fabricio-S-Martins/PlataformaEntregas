using Modulos.Autenticacao.Dominio.Entidades;

namespace Modulos.Autenticacao.Aplicacao.Servicos
{
    public interface ITokenServico
    {
        string GerarToken(Usuario usuario);
    }
}