﻿using Splat;

namespace Diagramer.Infrastructure.Extensions;
public static class ReadonlyDependencyResolverExtension
{
    public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver)
    {
        var service = resolver.GetService<TService>();
        if (service is null)
        {
            throw new InvalidOperationException($"Failed to resolve object of type {typeof(TService)}");
        }
        return service;
    }
}

