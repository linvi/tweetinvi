using System;
using System.Reflection;

namespace Examplinvi.AccountActivity.ASP.NET.Areas.HelpPage
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}