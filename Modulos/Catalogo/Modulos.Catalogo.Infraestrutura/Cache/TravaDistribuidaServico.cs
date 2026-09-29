using Modulos.Catalogo.Aplicacao.Servicos;
using StackExchange.Redis;

namespace Modulos.Catalogo.Infraestrutura.Cache
{
    internal class TravaDistribuidaServico : ITravaDistribuidaServico
    {
        private readonly IDatabase _redisDatabase;

        public TravaDistribuidaServico(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        public async Task<bool> AdquirirAsync(string chave, Guid token, TimeSpan tempo)
        {
            RedisKey lockKey = chave;
            RedisValue lockvalue = token.ToString();

            return await _redisDatabase.LockTakeAsync(lockKey, lockvalue, tempo);
        }

        public async Task LiberarAsync(string chave, Guid token)
        {
            RedisKey lockKey = chave;
            RedisValue lockvalue = token.ToString();

            await _redisDatabase.LockReleaseAsync(lockKey, lockvalue);
        }
    }
}