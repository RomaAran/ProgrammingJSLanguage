using System;
using System.Collections.Generic;

namespace lab2_11.Models.DB;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public int Pages { get; set; }

    public string Publisher { get; set; } = null!;
}
