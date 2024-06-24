using System;
using System.Collections.Generic;

namespace ETLWorkerService.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Stock { get; set; }

    public decimal Price { get; set; }
}
