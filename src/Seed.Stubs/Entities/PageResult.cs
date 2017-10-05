using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Seed.Stubs.Entities
{
    public class PageResult<T>
    {
        public PageResult()
        {
        }

        public PageResult(HttpRequestMessage request, IEnumerable<T> entities, int page, int pageSize)
        {
            var list = entities as IList<T> ?? entities.ToList();
            Results = list;
            Page = page;
            PageSize = pageSize;
            Count = list.Count();

            var skip = (Page - 1) * PageSize;
            Results = list.Skip(skip).Take(PageSize);
        }

        public PageResult(List<T> list)
        {
            Results = list;
            Page = 1;
            PageSize = list.Count();
            Count = list.Count();
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Results { get; set; }
        public Uri NextPageLink { get; set; }
        public Uri PreviousPageLink { get; set; }

        public static PageResult<T> From<TFrom>(HttpRequestMessage request, IEnumerable<TFrom> entities, int page,
            int pageSize, Func<TFrom, T> mapper)
        {
            var enumerable = entities as TFrom[] ?? entities.ToArray();
            var res = new PageResult<T>
            {
                Page = page,
                PageSize = pageSize,
                Count = enumerable.Count()
            };

            var skip = (res.Page - 1) * res.PageSize;
            res.Results = enumerable.Skip(skip).Take(res.PageSize).Select(mapper);

            return res;
        }
    }
}