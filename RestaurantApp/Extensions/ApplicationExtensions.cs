using Microsoft.Extensions.DependencyInjection;
using System;

namespace RestaurantApp.Extensions
{
    public static class ApplicationExtensions
    {
        private static IServiceProvider _serviceProvider;

        public static void ConfigureServices(this System.Windows.Application app, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static IServiceProvider GetServiceProvider(this System.Windows.Application app)
        {
            return _serviceProvider;
        }

        public static object GetService(this System.Windows.Application app, Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }
    }
} 