using System.Reflection;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace MultipleServers.Jobs.Abstract;

public interface IRecurringJob
{
    Task<Result> Execute(object @params, CancellationToken cancellationToken);
}

public interface IRecurringJob<T> : IRecurringJob { }

public interface IRecurringJob<in U,in T>
{
    Task<Result> Execute(T param, CancellationToken cancellationToken);
}

public sealed class RecurringJobExecutor
{
    private readonly IServiceProvider _serviceProvider;

    public RecurringJobExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<Result> ExecuteJob(string jobType, object @params, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var type = Assembly.GetEntryAssembly().GetTypes().FirstOrDefault(c => c.AssemblyQualifiedName == jobType);
        //var type = Type.GetType(jobType);
        if(type == null)
            throw new ArgumentException("Job type not found", nameof(jobType)); 
        var service = scope.ServiceProvider.GetService(type!);
        if (service is IRecurringJob recurringJob)
        {
            return await recurringJob.Execute(@params, cancellationToken);
        }

        throw new ArgumentException("Recurring Job doesnt implement IRecurringJob");
    }
}