﻿using Abp.BackgroundJobs;
using Abp.NHibernate.EntityMappings;
using System;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public class BackgroundJobInfoMap : EntityMap<BackgroundJobInfo, Guid>
    {
        public BackgroundJobInfoMap()
            : base("AbpBackgroundJobs")
        {
            Map(x => x.JobType);
            Map(x => x.JobArgs);
            Map(x => x.TryCount);
            Map(x => x.NextTryTime);
            Map(x => x.LastTryTime);
            Map(x => x.IsAbandoned);
            Map(x => x.Priority).CustomType<BackgroundJobPriority>();

            this.MapCreationAudited();
        }
    }
}