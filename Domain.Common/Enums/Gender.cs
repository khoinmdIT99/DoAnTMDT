using Infrastructure.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common.Enums
{
    [CustomEnum(true)]
    public enum Gender
    {
        [TextValue("Nam")]
        Male = 0,
        [TextValue("Nữ")]
        Female = 1
    }
}
