using System;
using System.Collections.Generic;

namespace GymSystem.Models;

public partial class Attendance
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public DateTime ChekinTime { get; set; }

    public virtual Member Member { get; set; } = null!;
}
