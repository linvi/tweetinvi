using System.Collections.ObjectModel;

namespace Examplinvi.AccountActivity.ASP.NET.Areas.HelpPage
{
    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values { get; private set; }
    }
}