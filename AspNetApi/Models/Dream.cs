using System;
using System.Collections.Generic;

namespace DreamComeTrueApi.Models;

public partial class Dream
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
}
