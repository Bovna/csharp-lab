namespace KinoKlik.Web.ViewModels;

using Microsoft.AspNetCore.Mvc.Rendering;

public class AutocompleteViewModel
{
    public string InputName { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string SearchPlaceholder { get; set; } = string.Empty;
    public string EmptyText { get; set; } = "- odaberite -";
    public string RequiredMessage { get; set; } = string.Empty;
    public bool EnableRemoteSearch { get; set; }
    public List<SelectListItem> Items { get; set; } = new();
}