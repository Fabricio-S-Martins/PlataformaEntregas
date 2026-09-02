namespace Modulos.Autenticacao.Aplicacao.Servicos
{
    public interface ISenhaServico
    {
        string GerarHash(string senha);
        bool VerificarHash(string senha, string senhaHash);
    }
}