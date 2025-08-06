using System;
using System.Collections.Generic;

namespace GymSystem.Models;

public partial class Plan
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int DurationInMonths { get; set; }

    public int Price { get; set; }

    public string? AspNetUser { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
