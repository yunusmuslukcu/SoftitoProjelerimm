using System;
using System.Collections.Generic;

namespace GymAppLite.Models;

public partial class ProgressLog
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public decimal Weight { get; set; }

    public int Height { get; set; }

    public virtual Member Member { get; set; } = null!;
}
