using System;
using System.Collections.Generic;

namespace GymAppLite.Models;

public partial class Member
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int SubscriptionTypeId { get; set; }

    public int TrainerId { get; set; }

    public virtual ICollection<ProgressLog> ProgressLogs { get; set; } = new List<ProgressLog>();

    public virtual SubscriptionType SubscriptionType { get; set; } = null!;

    public virtual Trainer Trainer { get; set; } = null!;
}
