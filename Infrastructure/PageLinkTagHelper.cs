using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SklepAI.Models;
using System.Collections.Generic;

namespace SklepAI.Infrastructure
{
    [HtmlTargetElement("ul", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; }
        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();
        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }
        public string PageCount { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var pagesCount = int.Parse(PageCount);
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new("ul");
            //previous button <li> a w nim <a> 
            TagBuilder previousA = new("a");
            previousA.AddCssClass("page-link");
            if (PageModel.CurrentPage < 2)
            {
                previousA.Attributes["aria-disabled"] = "true";
                previousA.Attributes["tabindex"] = "-1";
            }
            PageUrlValues["productPage"] = PageModel.CurrentPage - 1;
            previousA.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            previousA.InnerHtml.Append("Previous");

            TagBuilder previousLi = new("li");
            previousLi.AddCssClass(PageClass);
            if (PageModel.CurrentPage < 2)
            {
                previousLi.AddCssClass("disabled");
            }
            //zagnieżdżenie a w li 
            previousLi.InnerHtml.AppendHtml(previousA);
            result.InnerHtml.AppendHtml(previousLi);


            var startPage = PageModel.CurrentPage - 1;
            if (startPage < 1)
                startPage = 1;
            if (startPage + pagesCount > PageModel.TotalPages)
                startPage = PageModel.TotalPages - pagesCount + 1;

            //trzy w srodku
            for (int i = startPage; i < startPage + pagesCount; i++)
            {
                PageUrlValues["productPage"] = i;
                TagBuilder aTag = new("a");
                aTag.AddCssClass("page-link");
                aTag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                aTag.InnerHtml.Append(i.ToString());
                TagBuilder liTag = new("li");
                if (PageClassesEnabled)
                {
                    liTag.AddCssClass(PageClass);
                    liTag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                }
                if (PageModel.CurrentPage == i)
                    liTag.Attributes["aria-current"] = "page";

                liTag.InnerHtml.AppendHtml(aTag);
                result.InnerHtml.AppendHtml(liTag);
            }
            //next button
            TagBuilder nextA = new("a");
            nextA.AddCssClass("page-link");
            if (PageModel.CurrentPage == PageModel.TotalPages)
            {
                nextA.Attributes["aria-disabled"] = "true";
            }
            PageUrlValues["productPage"] = PageModel.CurrentPage + 1;
            nextA.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            nextA.InnerHtml.Append("Next");

            TagBuilder nextLi = new("li");

            nextLi.AddCssClass(PageClass);
            if (PageModel.CurrentPage == PageModel.TotalPages)
            {
                nextLi.AddCssClass("disabled");
            }
            //zagnieżdżenie a w li 
            nextLi.InnerHtml.AppendHtml(nextA);
            result.InnerHtml.AppendHtml(nextLi);

            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
