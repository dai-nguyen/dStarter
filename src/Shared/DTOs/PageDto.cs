using System.Collections.Generic;

namespace Shared.DTOs
{
    public class PageDto<T>
    {
        public long Total { get; set; }
        public T[] Data { get; set; }

        public PageDto()
        {
            Total = 0;
        }

        public PageDto(long total, T[] data)
        {
            Total = total;
            Data = data;
        }
    }
}
