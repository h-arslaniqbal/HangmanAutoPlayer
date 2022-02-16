using Autofac;
using HangmanAutoplayer.Interfaces;
using System;
using System.Net.Http;

namespace HangmanAutoplayer
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ConfigureContainer.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IApplication>();
                app.Run();
            }
        }


    }
}
