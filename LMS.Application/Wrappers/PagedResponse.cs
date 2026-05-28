using X.PagedList;

namespace LMS.Application.Wrappers
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public PagedListMetaData MetaData { get; set; }

        public PagedResponse(IPagedList<T> pagedList)
        {
            MetaData = pagedList.GetMetaData();
            Data = pagedList.ToList();
        }

        public PagedResponse(PagedListMetaData metaData, IEnumerable<T> data)
        {
            MetaData = metaData;
            Data = data;
        }
    }
}