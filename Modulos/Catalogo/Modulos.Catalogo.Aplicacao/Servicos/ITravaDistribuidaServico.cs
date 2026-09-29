namespace Modulos.Catalogo.Aplicacao.Servicos
{
    public interface ITravaDistribuidaServico
    {
        Task<bool> AdquirirAsync(string chave, Guid token, TimeSpan tempo);
        Task LiberarAsync(string chave, Guid token);
    }
}