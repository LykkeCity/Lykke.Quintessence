using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.QueueConsumers;
using Lykke.Quintessence.Domain.Services;

namespace Lykke.Quintessence.QueueConsumers
{
    [UsedImplicitly]
    public class DefaultBlockchainIndexationQueueConsumer : QueueConsumerBase<BigInteger[]>, IBlockchainIndexationQueueConsumer
    {
        private readonly IBlockchainIndexingService _blockchainIndexingService;
        private readonly ILog _log;
        private readonly int _maxDegreeOfParallelism;
        
        
        public DefaultBlockchainIndexationQueueConsumer(
            IBlockchainIndexingService blockchainIndexingService,
            ILogFactory logFactory,
            int maxDegreeOfParallelism)
        
            : base(TimeSpan.FromSeconds(1), 1)
        {
            _blockchainIndexingService = blockchainIndexingService;
            _log = logFactory.CreateLog(this);
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        
        protected override Task<BigInteger[]> TryGetNextTaskAsync()
        {
            return _blockchainIndexingService.GetNonIndexedBlocksAsync(take: _maxDegreeOfParallelism);
        }

        protected override async Task ProcessTaskAsync(
            BigInteger[] nonIndexedBlockBatch)
        {
            var indexedBlocks = await _blockchainIndexingService.IndexBlocksAsync(nonIndexedBlockBatch);

            if (indexedBlocks.Any())
            {
                await _blockchainIndexingService.MarkBlocksAsIndexed(indexedBlocks);
            }
        }
        
        public override void Start()
        {
            _log.Info("Starting blockchain observation...");
            
            base.Start();
            
            _log.Info("Blockchain observation started.");
        }

        public override void Stop()
        {
            _log.Info("Stopping blockchain observation...");
            
            base.Stop();
            
            _log.Info("Blockchain observation stopped.");
        }
    }
}
