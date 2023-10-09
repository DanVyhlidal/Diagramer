using Diagramer.GUI.ViewModels.Core.Interfaces;
using ReactiveUI;
using Splat;

namespace Diagramer.GUI.ViewModels.Core;

public class MainWindowViewModel : ReactiveObject, IScreen, IPageNavigator
{
    public void OnInitilizationComplete()
    {
        Navigate<HomeViewModel>();
    }
        
    public RoutingState Router { get; } = new RoutingState();

    public void Navigate<TViewModel>() where TViewModel : BaseViewModel
    {
        var viewModel = Locator.Current.GetService<TViewModel>();
        if (viewModel == null)
        {
            this.Log().Error($"ViewModel {nameof(TViewModel)} is null");
            return;
        }
        viewModel.OnSwitch();
        Router.Navigate.Execute(viewModel);
    }

    public void Navigate<TViewModel, TParameter>(TParameter parameter) where TViewModel : BaseViewModelWithParameter<TParameter>
    {
        var viewModel = Locator.Current.GetService<TViewModel>();
        if (viewModel == null)
        {
            this.Log().Error($"ViewModel {nameof(TViewModel)} is null");
            return;
        }
        viewModel.InitializeParameter(parameter);
        viewModel.OnSwitch();
        Router.Navigate.Execute(viewModel);
    }
}

