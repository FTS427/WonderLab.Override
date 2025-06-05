﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using WonderLab.Extensions.Hosting.UI;

namespace WonderLab.Extensions.Hosting;

public sealed class AvaloniaHostBuilder : IHostApplicationBuilder {
    private readonly HostApplicationBuilder _hostBuilder;

    public ILoggingBuilder Logging => _hostBuilder.Logging;
    public IMetricsBuilder Metrics => _hostBuilder.Metrics;
    public IServiceCollection Services => _hostBuilder.Services;
    public IHostEnvironment Environment => _hostBuilder.Environment;
    public IConfigurationManager Configuration => _hostBuilder.Configuration;
    public IDictionary<object, object> Properties => (_hostBuilder as IHostApplicationBuilder).Properties;

    public AvaloniaPageProviderBuilder PageProvider { get; private set; } = new();
    public AvaloniaDialogProviderBuilder DialogProvider { get; private set; } = new();

    public AvaloniaHostBuilder() {
        _hostBuilder = new HostApplicationBuilder();
    }

    public IHost Build() {
        RegisterDescriptors(PageProvider.RegisteredPages,
            descriptor => descriptor.PageType,
            descriptor => descriptor.ViewModelType);

        RegisterDescriptors(DialogProvider.RegisteredDialogs,
            descriptor => descriptor.DialogType,
            descriptor => descriptor.ViewModelType);

        Services.AddSingleton(PageProvider.Build);
        Services.AddSingleton(DialogProvider.Build);

        return _hostBuilder.Build();
    }

    private void RegisterDescriptors<TDescriptor>(
        IReadOnlyDictionary<string, TDescriptor> descriptors,
        Func<TDescriptor, Type> typeSelector,
        Func<TDescriptor, Type> viewModelSelector) where TDescriptor : class {
        foreach (var (_, descriptor) in descriptors) {
            var type = typeSelector(descriptor);
            if (type != null)
                Services.AddTransient(type);

            var viewModelType = viewModelSelector(descriptor);
            if (viewModelType != null)
                Services.AddTransient(viewModelType);
        }
    }

    void IHostApplicationBuilder.ConfigureContainer<TBuilder>
        (IServiceProviderFactory<TBuilder> factory,
        Action<TBuilder> configure) =>
        _hostBuilder.ConfigureContainer(factory, configure);
}