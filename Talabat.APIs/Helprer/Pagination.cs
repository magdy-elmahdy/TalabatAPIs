using Talabat.APIs.Dtos;

namespace Talabat.APIs.Helprer
{
    public class Pagination<T>
    {

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }


        public Pagination(int pageSize, int pageIndex,int count, IReadOnlyList<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Data = data;
            count = Count;
        }
    }
}
