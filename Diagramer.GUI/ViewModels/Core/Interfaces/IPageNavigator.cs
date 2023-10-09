namespace Diagramer.GUI.ViewModels.Core.Interfaces;
public interface IPageNavigator
{
    void Navigate<TViewModel>() where TViewModel : BaseViewModel;
    void Navigate<TViewModel, TParameter>(TParameter parameter) where TViewModel : BaseViewModelWithParameter<TParameter>;
}