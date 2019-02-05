using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services.DependencyInjection;
using Lykke.Quintessence.Settings;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Modules
{
    [UsedImplicitly]
    public class SignServiceModule<T> : Module
        where T : SignServiceSettings
    {
        // ReSharper disable once NotAccessedField.Local : Intended for future use
        private readonly IReloadingManager<AppSettings<T>> _appSettings;
        
        public SignServiceModule(
            IReloadingManager<AppSettings<T>> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(
            ContainerBuilder builder)
        {
            builder
                .RegisterServices()
                .AddDefaultSignService()
                .AddDefaultWalletService();
        }
    }
}