using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Builders.Activities
{
    public interface IActivityCommentBuilder
    {
        ActivityCommentBuilder WithActivityId(Guid activityId);

        Task BuildAsync();
    }
}