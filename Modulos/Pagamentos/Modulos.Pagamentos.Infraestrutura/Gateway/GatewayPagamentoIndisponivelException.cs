namespace Modulos.Pagamentos.Infraestrutura.Gateway;

public class GatewayPagamentoIndisponivelException : Exception
{
    public GatewayPagamentoIndisponivelException()
        : base("O gateway de pagamento está indisponível.")
    {
    }
}
