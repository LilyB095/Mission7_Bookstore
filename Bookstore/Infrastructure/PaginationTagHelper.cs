﻿using Bookstore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-link")]
    public class PaginationTagHelper : TagHelper
    {
        //Dynamically create the page links for us
        private IUrlHelperFactory uhf;

        public PaginationTagHelper (IUrlHelperFactory temp)
        {
            uhf = temp;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext vc { get; set; }
        
        //Different than the View Context
        public PageInfo PageLink { get; set; } //PageInfo = page-info from Index.cshtml
        public string PageAction { get; set; }
        public override void Process (TagHelperContext thc, TagHelperOutput tho)
        {
            IUrlHelper uh = uhf.GetUrlHelper(vc);
            
            TagBuilder final = new TagBuilder("div");
            
            for (int i = 1; i <= PageLink.TotalPages; i++)
            {
                TagBuilder tb = new TagBuilder("a");
                tb.Attributes["href"] = uh.Action(PageAction, new { pageNum = i });
                tb.InnerHtml.Append(i.ToString());

                final.InnerHtml.AppendHtml(tb);
                
                if (i < PageLink.TotalPages)
                {
                    final.InnerHtml.Append(" | ");
                }
            }

            tho.Content.AppendHtml(final.InnerHtml);
        }

    }
}
