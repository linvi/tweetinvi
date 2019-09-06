using System.Collections.ObjectModel;

namespace Examplinvi.AccountActivity.ASP.NET.Areas.HelpPage
{
    public class ComplexTypeModelDescription : ModelDescription
    {
        public ComplexTypeModelDescription()
        {
            Properties = new Collection<ParameterDescription>();
        }

        public Collection<ParameterDescription> Properties { get; private set; }
    }
}