namespace Modulos.Autenticacao.Api.Endpoints.CriarUsuario;

public record CriarUsuarioRequest(string Nome, string Email, string Senha, string Papel);