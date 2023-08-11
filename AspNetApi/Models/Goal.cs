using System;
using System.Collections.Generic;

namespace DreamComeTrueApi.Models;

public partial class Goal
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid DreamId { get; set; }

    public DateOnly Deadline { get; set; }

    public virtual Dream Dream { get; set; }
}
