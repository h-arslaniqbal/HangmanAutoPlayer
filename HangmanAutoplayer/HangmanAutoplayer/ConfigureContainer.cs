using Autofac;
using HangmanAutoplayer.Interfaces;
using HangmanAutoplayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HangmanAutoplayer
{
    public static class ConfigureContainer
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>().As<IApplication>();
            builder.RegisterType<HangmanService>().As<IHangmanService>();

            return builder.Build();
        }
    }
}
