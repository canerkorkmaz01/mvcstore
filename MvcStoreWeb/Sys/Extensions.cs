using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X.PagedList.Web.Common;

namespace MvcStoreWeb
{
    public static class Extensions
    {

        public static string ToSafeUrlString(this string Text) => Regex.Replace(string.Concat(Text.Where(p => char.IsLetterOrDigit(p) || char.IsWhiteSpace(p))), @"\s+", "-").ToLower();

        public static PagedListRenderOptions PagedListPagerOptions => new PagedListRenderOptions
        {
            UlElementClasses = new[] { "pagination", "m-0", "shadow-sm" },
            ActiveLiElementClass = "active",
            LiElementClasses = new[] { "page-item " },
            PageClasses = new[] { "page-link" },
            EllipsesElementClass = "page-link",
            LinkToFirstPageFormat = "<i class=\"fa fa-angle-double-left\"></i>",
            LinkToPreviousPageFormat = "<i class=\"fa fa-angle-left\"></i>",
            LinkToNextPageFormat = "<i class=\"fa fa-angle-right\"></i>",
            LinkToLastPageFormat = "<i class=\"fa fa-angle-double-right\"></i>",
            Display = PagedListDisplayMode.IfNeeded
        };
    }
}
