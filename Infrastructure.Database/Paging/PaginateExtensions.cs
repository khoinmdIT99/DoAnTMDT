using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Database.Paging
{
    public static class PaginateExtensions
    {
        public static IPaginate<T> ToPaginate<T>(this IEnumerable<T> source, int index, int size, int from = 0)
        {
            return new Paginate<T>(source, index, size, from);
        }

        public static IPaginate<TTResult> ToPaginate<TSource, TTResult>(this IEnumerable<TSource> source,
            Func<IEnumerable<TSource>, IEnumerable<TTResult>> converter, int index, int size, int from = 0)
        {
            return new Paginate<TSource, TTResult>(source, converter, index, size, from);
        }
    }
}
