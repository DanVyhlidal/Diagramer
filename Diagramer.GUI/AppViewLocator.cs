using System;
using ReactiveUI;

namespace Diagramer.GUI;

public class AppViewLocator : IViewLocator
{
    public IViewFor? ResolveView<T>(T viewModelType, string contract = "")
    {
        ;
        var viewModelName = viewModelType?.GetType().FullName;
        var viewName = viewModelName?.Replace("ViewModel", "View");

        if (viewName == null) return null;

        try
        {
            var viewType = Type.GetType(viewName);
            if (viewType == null)
            {
                return null;
            }
            return Activator.CreateInstance(viewType) as IViewFor;
        }
        catch (Exception)
        {
            throw;
        }
    }
}