using System;
using Diagramer.GUI.Models.Core;
using Diagramer.GUI.ViewModels.Core.Interfaces;
using ReactiveUI;

namespace Diagramer.GUI.ViewModels.Core;

public abstract class BaseViewModel : AReactiveViewModel
{
    protected readonly IPageNavigator pageNavigator;

    public BaseViewModel(IScreen screen,IPageNavigator pageNavigator)
    {
        HostScreen = screen;
        UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);
        this.pageNavigator = pageNavigator;
    }
}
    
public abstract class BaseViewModelWithParameter<TParameter> : BaseViewModel
{
    public TParameter Parameter { get; set; }

    public BaseViewModelWithParameter(IScreen screen,IPageNavigator pageNavigator) : base(screen, pageNavigator)
    {
            
    }
        
    public virtual void InitializeParameter(TParameter paremeter)
    {
        Parameter = paremeter;
    }
}

public abstract class BaseViewModel<TViewModelMapper> : BaseViewModel where TViewModelMapper : IMapper, new()
{
    protected TViewModelMapper modelMapper;

    public BaseViewModel(IScreen screen,IPageNavigator pageNavigator) : base(screen, pageNavigator)
    {
        this.modelMapper = new TViewModelMapper();
    }
}
    
public abstract class BaseViewModel<TViewModelMapper, TParameter> : BaseViewModelWithParameter<TParameter> where TViewModelMapper : IMapper, new()
{
    protected TViewModelMapper modelMapper;

    public BaseViewModel(IScreen screen,IPageNavigator pageNavigator) : base(screen, pageNavigator)
    {
        this.modelMapper = new TViewModelMapper();
    }
}