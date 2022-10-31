using netCoreApi.ApiModels;
using Quartz;
using Quartz.Spi;

namespace netCoreApi.Service
{
//    public class Quartz :IHostedService
//    {
//        public IScheduler Scheduler { get; set; }
//        private readonly IJobFactory jobFactory;
//        private readonly List<JobsModel> jobMetadatas;
//        private readonly ISchedulerFactory schedulerFactory;

//        public Quartz(ISchedulerFactory schedulerFactory, List<JobsModel> jobMetadatas, IJobFactory jobFactory)
//        {
//            this.jobFactory = jobFactory;
//            this.schedulerFactory = schedulerFactory;
//            this.jobMetadatas = jobMetadatas;
//        }
//        public async Task StartAsync(CancellationToken cancellationToken)
//        {
//            //Creating Schdeular
//            Scheduler = await schedulerFactory.GetScheduler();
//            Scheduler.JobFactory = jobFactory;

//            //Suporrt for Multiple Jobs
//            jobMetadatas?.ForEach(jobMetadata =>
//            {
//                //Create Job
//                IJobDetail jobDetail = CreateJob(jobMetadata);
//                //Create trigger
//                ITrigger trigger = CreateTrigger(jobMetadata);
//                //Schedule Job
//                Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken).GetAwaiter();
//                //Start The Schedular
//            });
//            await Scheduler.Start(cancellationToken);
//        }

//        private ITrigger CreateTrigger(JobsModel jobMetadata)
//        {
//            return TriggerBuilder.Create()
//                .WithIdentity(jobMetadata.JobId.ToString())
//                .WithCronSchedule(jobMetadata.CronExpression)
//                .WithDescription(jobMetadata.JobName)
//                .Build();
//        }

//        private IJobDetail CreateJob(JobsModel jobMetadata)
//        {
//            return JobBuilder.Create(jobMetadata.JobType)
//                .WithIdentity(jobMetadata.JobId.ToString())
//                .WithDescription(jobMetadata.JobName)
//                .Build();
//        }

//        public async Task StopAsync(CancellationToken cancellationToken)
//        {
//            await Scheduler.Shutdown();
//        }
//    }
}
