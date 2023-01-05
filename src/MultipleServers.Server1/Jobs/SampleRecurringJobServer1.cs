using CSharpFunctionalExtensions;
using Hangfire;
using MultipleServers.Jobs.Abstract;

namespace MultipleServers.Server1.Jobs;

public class SampleRecurringJobServer1 : IRecurringJob<SampleRecurringJobServer1>
{
    public async Task<Result> Execute(object @params, CancellationToken cancellationToken)
    {
        Console.WriteLine("Server 1 - executing");
        Thread.Sleep(5000);
        Console.WriteLine("Server 1 - executed");
        return Result.Success();
    }
}