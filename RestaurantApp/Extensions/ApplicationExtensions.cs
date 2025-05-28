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
            if (app is App application)
            {
                return application.Services.GetService(serviceType);
            }
            return null;
        }

        public static T GetService<T>(this System.Windows.Application app) where T : class
        {
            if (app is App application)
            {
                return application.Services.GetService<T>();
            }
            return null;
        }
    }
} 