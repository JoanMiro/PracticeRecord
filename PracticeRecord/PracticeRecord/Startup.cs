namespace PracticeRecord
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static IServiceProvider Init()
        {
            var serviceProvider = new ServiceCollection().ConfigureServices().BuildServiceProvider();
            ServiceProvider = serviceProvider;

            return serviceProvider;
        }
    }
}