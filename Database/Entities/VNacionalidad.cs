using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class VNacionalidad
{
    public int Id { get; set; }

    public string Nacionalidad { get; set; } = null!;

    public string Iso { get; set; } = null!;
}
