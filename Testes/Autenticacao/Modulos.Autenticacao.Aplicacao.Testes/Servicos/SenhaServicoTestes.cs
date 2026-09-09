using Bogus;
using Modulos.Autenticacao.Aplicacao.Servicos;
using System.Globalization;

namespace Modulos.Autenticacao.Aplicacao.Testes.Servicos
{
    public class SenhaServicoTestes
    {
        private readonly Faker _faker;
        private readonly SenhaServico _senhaServico;

        public SenhaServicoTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");

            _senhaServico = new SenhaServico();
        }

        [Fact]
        public void GerarHash_ComMesmaSenha_DeveGerarHashDiferentes()
        {
            var senha = _faker.Internet.Password();

            var primeiroHash = _senhaServico.GerarHash(senha);
            var segundoHash = _senhaServico.GerarHash(senha);

            Assert.NotEqual(primeiroHash, segundoHash);
        }

        [Fact]
        public void VerificarHash_ComMesmaSenha_DeveRetornarVerdadeiro()
        {
            var senha = _faker.Internet.Password();
            var senhaHash = _senhaServico.GerarHash(senha);

            Assert.True(_senhaServico.VerificarHash(senha, senhaHash));
        }

        [Fact]
        public void VerificarHash_ComSenhasDistintas_DeveRetornarFalso()
        {
            var senha = _faker.Internet.Password();
            var senhaHash = _senhaServico.GerarHash(_faker.Internet.Password());

            Assert.False(_senhaServico.VerificarHash(senha, senhaHash));
        }
    }
}