using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Comics.Web.Helper
{
    public class ItemData
    {
        public static List<string> publisher;
        public static List<string> bikeproducer;
        public static List<string> brand;

        public ItemData(IStringLocalizer<ItemData> localizer)
        {
            publisher = new List<string>() { localizer["Marvel"], localizer["DC"], localizer["Image"], localizer["Titan"], localizer["Boom"], localizer["Dynamite"], localizer["Valiant"] };
            bikeproducer = new List<string>() { localizer["LTD"], localizer["Stels"], localizer["Trek"], localizer["AIST"], localizer["WTP"], localizer["Radio"], localizer["Forward"], localizer["Greenway"] };
            brand = new List<string>() { localizer["Ardbeg"], localizer["Laphroaig"], localizer["The Balvenie"], localizer["Talisker"], localizer["Oban"], localizer["Highland Park"] , localizer["Bowmore"] };
        }
    }
}
