using System;
using System.Collections.Generic;

namespace GymSystem.Models;

public partial class Member
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly Birthday { get; set; }

    public int Phone { get; set; }

    public string IdNumber { get; set; } = null!;

    public string Addresse { get; set; } = null!;

    public DateOnly JoinDate { get; set; }

    public string? AspNetUserId { get; set; }

    public ApiUser? ApiUser { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
