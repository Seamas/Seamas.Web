namespace Wang.Seamas.Web.Common;

/// <summary>
/// 分页查询结果
/// </summary>
/// <typeparam name="T">数据项类型</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// 当前页的数据列表
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// 总记录数（用于计算总页数）
    /// </summary>
    public long TotalCount { get; set; }

    /// <summary>
    /// 当前页码（从 1 开始）
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// 每页大小
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总页数（只读计算属性）
    /// </summary>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

    /// <summary>
    /// 是否有上一页
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// 是否有下一页
    /// </summary>
    public bool HasNextPage => Page < TotalPages;

    // 构造函数（可选）
    public PagedResult()
    {
    }

    public PagedResult(List<T> items, long totalCount, int page = 1, int pageSize = 10)
    {
        Items = items ?? new List<T>();
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }
}