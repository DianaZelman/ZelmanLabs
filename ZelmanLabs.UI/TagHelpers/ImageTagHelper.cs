using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ZelmanLabs.UI.TagHelpers
{
  [HtmlTargetElement("img", Attributes = "img-action, img-controller")]
  public class ImageTagHelper : TagHelper
  {
    private readonly LinkGenerator _linkGenerator;

    public string ImgController { get; set; }
    public string ImgAction { get; set; }

    public ImageTagHelper(LinkGenerator linkGenerator)
    {
      _linkGenerator = linkGenerator;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      var url = _linkGenerator.GetPathByAction(ImgAction, ImgController);
      output.Attributes.SetAttribute("src", url);
    }
  }
}