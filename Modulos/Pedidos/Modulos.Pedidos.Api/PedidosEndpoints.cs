using Microsoft.AspNetCore.Routing;
using Modulos.Pedidos.Api.Endpoints.Aceitar;
using Modulos.Pedidos.Api.Endpoints.AdicionarItem;
using Modulos.Pedidos.Api.Endpoints.Cancelar;
using Modulos.Pedidos.Api.Endpoints.ConfirmarPagamento;
using Modulos.Pedidos.Api.Endpoints.CriarPedido;
using Modulos.Pedidos.Api.Endpoints.Entregar;
using Modulos.Pedidos.Api.Endpoints.IniciarPreparo;
using Modulos.Pedidos.Api.Endpoints.SairParaEntrega;

namespace Modulos.Pedidos.Api
{
    public static class PedidosEndpoints
    {
        public static void MapPedidosEndpoints(this IEndpointRouteBuilder rotas)
        {
            rotas.MapCriarPedidoEndpoint();
            rotas.MapAdicionarItemEndpoint();
            rotas.MapConfirmarPagamentoEndpoint();
            rotas.MapAceitarEndpoint();
            rotas.MapIniciarPreparoEndpoint();
            rotas.MapSairParaEntregaEndpoint();
            rotas.MapEntregarEndpoint();
            rotas.MapCancelarEndpoint();
        }
    }
}