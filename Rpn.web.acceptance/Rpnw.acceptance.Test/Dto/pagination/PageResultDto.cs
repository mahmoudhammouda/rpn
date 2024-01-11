using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.acceptance.Test
{
    public class PageResultDto<T>
    {
       public PageDto Page { get;set; }
       public IEnumerable<T> Items { get; set; } 

    public PageResultDto(IEnumerable<T> item,int pageIndex,int pageSize, int? totalPage= null)
        {
            Page = new PageDto();
            Page.PageSize = pageSize;
            Page.PageIndex = pageIndex;
            if (totalPage.HasValue)
                Page.TotalPages = totalPage.Value;
            
            Items = item;
            Page.ItemCount = item.Count();
        }


        public static  PageResultDto<T> Create(IEnumerable<T> items, int pageIndex, int pageSize, int? maxPageSize = null)
        {
            if (maxPageSize.HasValue && pageSize> maxPageSize.Value)
                pageSize = maxPageSize.Value;

            var pagedItems = items.Skip(pageSize * (pageIndex - 1)).Take(pageSize);

            return new PageResultDto<T>(pagedItems, pageIndex, pageSize);
        }



    }
}
