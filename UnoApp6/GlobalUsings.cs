global using System.Collections.Immutable;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using UnoApp6.Models;
global using UnoApp6.Presentation;
global using UnoApp6.DataContracts;
global using UnoApp6.DataContracts.Serialization;
global using UnoApp6.Services.Caching;
global using UnoApp6.Services.Endpoints;
global using ApplicationExecutionState = Windows.ApplicationModel.Activation.ApplicationExecutionState;

[assembly: Uno.Extensions.Reactive.Config.BindableGenerationTool(3)]