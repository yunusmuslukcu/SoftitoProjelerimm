using System;
using System.Collections.Generic;

namespace GymAppLite.Models;

public partial class SubscriptionType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
