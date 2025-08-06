using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.Models;

public partial class Subscription
{
    public int Id { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int MemberId { get; set; }

    public int PlanId { get; set; }

    public bool? Status { get; set; }

    [Required]
    [MaxLength(100)]
    public string? PlanName {  get; set; }   

    public int PlanPrice {  get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual Plan Plan { get; set; } = null!;
}
