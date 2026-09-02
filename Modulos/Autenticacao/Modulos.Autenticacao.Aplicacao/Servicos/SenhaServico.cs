using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Modulos.Autenticacao.Aplicacao.Servicos
{
    public class SenhaServico : ISenhaServico
    {
        private const int SaltSizeBytes = 128 / 8;
        private const int NumBytesRequested = 256 / 8;
        private const int IterationCount = 100000;

        public string GerarHash(string senha)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSizeBytes);
            string hashed = HashearSenha(senha, salt);

            return $"{hashed}-{Convert.ToBase64String(salt)}";
        }

        private string HashearSenha(string senha, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(senha, salt, KeyDerivationPrf.HMACSHA256, IterationCount, NumBytesRequested));
        }

        public bool VerificarHash(string senha, string senhaHash)
        {
            var posicaoHifen = senhaHash.IndexOf('-');
            var salt = Convert.FromBase64String(senhaHash.Substring(posicaoHifen + 1));

            var novaSenhaHasheada = $"{HashearSenha(senha, salt)}-{Convert.ToBase64String(salt)}";
            return senhaHash == novaSenhaHasheada;
        }
    }
}