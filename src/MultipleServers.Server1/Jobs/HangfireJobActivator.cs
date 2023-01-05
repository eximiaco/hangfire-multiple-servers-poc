using Hangfire;

namespace MultipleServers.Server1.Jobs;

public sealed class HangfireJobActivator : JobActivator
{
    private readonly IServiceProvider _serviceProvider;

    public HangfireJobActivator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override object ActivateJob(Type jobType)
    {
        
        return _serviceProvider.GetService(jobType);
    }
}