namespace ExpenseTracker.Application.Common.Patterns;

public class PaginationParams
{
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Clamp(value, 1, 50);
    }
}