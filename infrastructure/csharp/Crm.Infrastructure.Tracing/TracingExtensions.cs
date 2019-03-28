﻿using Jaeger;
using Jaeger.Samplers;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing;
using OpenTracing.Util;

namespace Crm.Infrastructure.Tracing
{
    public static class TracingExtensions
    {
        public static IServiceCollection ConfigureTracing(this IServiceCollection services,
            string applicationName)
        {
            services.AddOpenTracing();
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var tracer = new Tracer.Builder(applicationName)
                    .WithSampler(new ConstSampler(true))
                    .Build();

                GlobalTracer.Register(tracer);

                return tracer;
            });

            return services;
        }
    }
}