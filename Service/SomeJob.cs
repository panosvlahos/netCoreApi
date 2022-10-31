using Quartz.Impl;
using Quartz;
using System.Collections.Specialized;

namespace netCoreApi.Service
{
    public class SomeJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            // do something here
            return Task.FromResult(0);
        }
    }

}
