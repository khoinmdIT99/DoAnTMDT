using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Database.DynamicLinq
{
    public enum FilterOperator
    {
        IsLessThan = 0,
        IsLessThanOrEqualTo = 1,
        IsEqualTo = 2,
        IsNotEqualTo = 3,
        IsGreaterThanOrEqualTo = 4,
        IsGreaterThan = 5,
        StartsWith = 6,
        EndsWith = 7,
        Contains = 8,
        IsContainedIn = 9,
        DoesNotContain = 10,
        IsNull = 11,
        IsNotNull = 12,
        IsEmpty = 13,
        IsNotEmpty = 14
    }
}
