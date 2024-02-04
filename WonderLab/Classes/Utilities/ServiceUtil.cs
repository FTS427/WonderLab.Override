﻿using Microsoft.Extensions.DependencyInjection;
using System;
using WonderLab.Classes.Interfaces;

namespace WonderLab.Classes.Utilities;

public static class ServiceUtil
{
    public static void AddWindowFactory<T>(this IServiceCollection services) where T : class
    {
        services.AddTransient<T>();
        services.AddSingleton((Func<IServiceProvider, Func<T>>)delegate (IServiceProvider s)
        {
            IServiceProvider s2 = s;
            return () => s2.GetService<T>()!;
        });

        services.AddSingleton<IFactory<T>, AbstractFactory<T>>();
    }
}

public class AbstractFactory<T>(Func<T> factory) : IFactory<T> where T : class
{
    private readonly Func<T> _factory = factory;

    public T Create()
    {
        return _factory();
    }
}