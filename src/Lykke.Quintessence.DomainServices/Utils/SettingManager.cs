using System;
using System.Threading;
using System.Threading.Tasks;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Services.Utils
{
    public abstract class SettingManager
    {
        private readonly TimeSpan? _cacheDuration;


        protected SettingManager(
            TimeSpan? cacheDuration)
        {
            _cacheDuration = cacheDuration;
            Semaphore = new SemaphoreSlim(1);
            
            SetCurrentValueExpirationMoment();
        }


        protected DateTime CurrentValueExpirationMoment { get; private set; }

        protected SemaphoreSlim Semaphore { get; }
        
        
        protected void SetCurrentValueExpirationMoment()
        {
            CurrentValueExpirationMoment = _cacheDuration.HasValue 
                ? DateTime.UtcNow.Add(_cacheDuration.Value)
                : DateTime.MaxValue;
        }
    }
    
    public class SettingManager<T> : SettingManager
    {
        private readonly IReloadingManager<T> _reloadingManager;
        
        private T _currentValue;
        
        
        public SettingManager(
            IReloadingManager<T> reloadingManager,
            TimeSpan? cacheDuration = null)
        
            : base(cacheDuration)
        {
            _currentValue = reloadingManager.CurrentValue; 
            _reloadingManager = reloadingManager;
        }
        
        public async Task<T> GetValueAsync()
        {
            if (CurrentValueExpirationMoment <= DateTime.UtcNow)
            {
                await Semaphore.WaitAsync();

                try
                {
                    if (CurrentValueExpirationMoment <= DateTime.UtcNow)
                    {
                        _currentValue = await _reloadingManager.Reload();

                        SetCurrentValueExpirationMoment();
                    }
                }
                finally
                {
                    Semaphore.Release();
                }
            }

            return _currentValue;
        }
    }
    
    public class SettingManager<TSource, TValue> : SettingManager
    {
        private readonly Func<TSource, TValue> _converter;
        private readonly IReloadingManager<TSource> _reloadingManager;

        private TValue _currentValue;
        
        
        public SettingManager(
            IReloadingManager<TSource> reloadingManager,
            Func<TSource, TValue> converter,
            TimeSpan? cacheDuration = null)
        
            : base(cacheDuration)
        {
            _converter = converter;
            _currentValue = converter.Invoke(reloadingManager.CurrentValue); 
            _reloadingManager = reloadingManager;
        }

        public async Task<TValue> GetValueAsync()
        {
            if (CurrentValueExpirationMoment <= DateTime.UtcNow)
            {
                await Semaphore.WaitAsync();

                try
                {
                    if (CurrentValueExpirationMoment <= DateTime.UtcNow)
                    {
                        _currentValue = _converter.Invoke(await _reloadingManager.Reload());

                        SetCurrentValueExpirationMoment();
                    }
                }
                finally
                {
                    Semaphore.Release();
                }
            }

            return _currentValue;
        }
    }
}