#pragma checksum "D:\UniverCity\iTransition\Comics\Comics.Web\Views\Error\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "bf0b227fa13e70e7b98ba0b470e004cfff4d0a56"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Error_Index), @"mvc.1.0.view", @"/Views/Error/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\UniverCity\iTransition\Comics\Comics.Web\Views\_ViewImports.cshtml"
using System;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\UniverCity\iTransition\Comics\Comics.Web\Views\_ViewImports.cshtml"
using Comics.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\UniverCity\iTransition\Comics\Comics.Web\Views\_ViewImports.cshtml"
using Comics.Domain;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\UniverCity\iTransition\Comics\Comics.Web\Views\_ViewImports.cshtml"
using Comics.Web.ViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\UniverCity\iTransition\Comics\Comics.Web\Views\_ViewImports.cshtml"
using Westwind.AspNetCore.Markdown;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\UniverCity\iTransition\Comics\Comics.Web\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\UniverCity\iTransition\Comics\Comics.Web\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\UniverCity\iTransition\Comics\Comics.Web\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bf0b227fa13e70e7b98ba0b470e004cfff4d0a56", @"/Views/Error/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"035048ab6ed1451b309340a0aa9283d8f22745f8", @"/Views/_ViewImports.cshtml")]
    public class Views_Error_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\UniverCity\iTransition\Comics\Comics.Web\Views\Error\Index.cshtml"
   ViewData["Title"] = "Error page"; 

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"container\">\r\n    <div class=\"row\">\r\n        <div class=\"col\" style=\"margin-top: 50px;\">\r\n            <h4 class=\"text-center border rounded\" style=\"background-color:white\">Ошибка ");
#nullable restore
#line 6 "D:\UniverCity\iTransition\Comics\Comics.Web\Views\Error\Index.cshtml"
                                                                                    Write(ViewData["Error"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n        </div>\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591