using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ZelmanLabs.UI.TagHelpers
{
  public class PagerTagHelper : TagHelper
  {
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string? Category { get; set; }
    public bool Admin { get; set; } = false;

    int Prev => CurrentPage == 1 ? 1 : CurrentPage - 1;
    int Next => CurrentPage == TotalPages ? TotalPages : CurrentPage + 1;

    public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
      _linkGenerator = linkGenerator;
      _httpContextAccessor = httpContextAccessor;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      output.TagName = "div";
      output.Attributes.SetAttribute("class", "row");

      var nav = new TagBuilder("nav");
      nav.Attributes.Add("aria-label", "pagination");

      var ul = new TagBuilder("ul");
      ul.AddCssClass("pagination");

      // кнопка предыдущей страницы
      ul.InnerHtml.AppendHtml(CreateListItem(Category, Prev, "<span aria-hidden=\"true\">&laquo;</span>"));

      // разметка кнопок переключения страниц
      for (var index = 1; index <= TotalPages; index++)
      {
        ul.InnerHtml.AppendHtml(CreateListItem(Category, index, string.Empty));
      }

      // кнопка следующей страницы
      ul.InnerHtml.AppendHtml(CreateListItem(Category, Next, "<span aria-hidden=\"true\">&raquo;</span>"));

      nav.InnerHtml.AppendHtml(ul);
      output.Content.AppendHtml(nav);
    }

    /// <summary>
    /// Разметка одной кнопки пейджера
    /// </summary>
    /// <param name="category">имя категории</param>
    /// <param name="pageNo">номер страницы</param>
    /// <param name="innerText">текст кнопки</param>
    /// <returns></returns>
    TagBuilder CreateListItem(string? category, int pageNo, string? innerText)
    {
      var li = new TagBuilder("li");
      li.AddCssClass("page-item");
      if (pageNo == CurrentPage && string.IsNullOrEmpty(innerText))
        li.AddCssClass("active");

      var a = new TagBuilder("a");
      a.AddCssClass("page-link");

      var routeData = new { pageNo = pageNo, category = category };

      string url;

      if (Admin)
        url = _linkGenerator.GetPathByPage(_httpContextAccessor.HttpContext, page: "./Index", values: routeData);
      else
        url = _linkGenerator.GetPathByAction("index", "product", routeData);

      a.Attributes.Add("href", url);

      var text = string.IsNullOrEmpty(innerText) ? pageNo.ToString() : innerText;
      a.InnerHtml.AppendHtml(text);

      li.InnerHtml.AppendHtml(a);
      return li;
    }
  }
}