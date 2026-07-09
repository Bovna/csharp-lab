namespace Vjezba.Web.ViewModels;

public sealed class GlobalSearchPageViewModel
{
    public string Query { get; init; } = string.Empty;
    public int MinQueryLength { get; init; }
    public IReadOnlyList<GlobalSearchResultViewModel> Results { get; init; } =
        Array.Empty<GlobalSearchResultViewModel>();
}
