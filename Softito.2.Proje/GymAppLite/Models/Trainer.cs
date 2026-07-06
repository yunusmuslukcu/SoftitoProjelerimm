using System;
using System.Collections.Generic;

namespace GymAppLite.Models;

public partial class Trainer
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? Expertise { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
