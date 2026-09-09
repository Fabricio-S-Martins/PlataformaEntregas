using MediatR;
using Modulos.Catalogo.Aplicacao.Repositorios;
using Modulos.Catalogo.Dominio.Entidades;

namespace Modulos.Catalogo.Aplicacao.CasosDeUso.CadastrarRestaurante
{
    public class CadastrarRestauranteHandler : IRequestHandler<CadastrarRestauranteCommand>
    {
        private readonly IRestauranteRepositorio _restauranteRepositorio;

        public CadastrarRestauranteHandler(IRestauranteRepositorio restauranteRepositorio)
        {
            _restauranteRepositorio = restauranteRepositorio;
        }

        public async Task Handle(CadastrarRestauranteCommand request, CancellationToken cancellationToken)
        {
            var resultadoRestaurante = Restaurante.Criar(request.Nome, request.Cnpj);
            if (!resultadoRestaurante.Sucesso)
                throw new ArgumentException(string.Join(Environment.NewLine, resultadoRestaurante.Erros));

            await _restauranteRepositorio.AdicionarAsync(resultadoRestaurante.Valor);
        }
    }
}