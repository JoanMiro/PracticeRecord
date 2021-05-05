namespace PracticeRecord.Services
{
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjectionContainer
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IViewModelService, ViewModelService>();
            return services;
        }
    }
}