using System;
using Autofac;
using Common;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.PeriodicalHandlers;
using Lykke.Quintessence.Domain.Services;

namespace Lykke.Quintessence.PeriodicalHandlers
{
    [UsedImplicitly]
    public class DefaultBalanceCheckScheduler : IBalanceCheckScheduler, IStartable, IStopable
    {
        private readonly TimerTrigger _timerTrigger;
        
        public DefaultBalanceCheckScheduler(
            IBalanceCheckSchedulingService balanceCheckSchedulingService,
            ILogFactory logFactory)
        {
            _timerTrigger = new TimerTrigger
            (
                nameof(DefaultBalanceCheckScheduler),
                TimeSpan.FromMinutes(15),
                logFactory
            );


            _timerTrigger.Triggered += (timer, args, token) 
                => balanceCheckSchedulingService.ScheduleBalanceChecksAsync();
        }

        public void Start()
        {
            _timerTrigger.Start();
        }
        
        public void Stop()
        {
            _timerTrigger.Stop();
        }

        public void Dispose()
        {
            _timerTrigger.Stop();
            _timerTrigger.Dispose();
        }
    }
}