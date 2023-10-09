using ReactiveUI;

namespace Diagramer.GUI.Models;

public class DiagramReactiveImage : ReactiveObject
{
    private byte[] imageData;
    public byte[] ImageData
    {
        get => imageData;
        set => this.RaiseAndSetIfChanged(ref imageData, value);
    }

    private string imageSource;

    public string ImageSource
    {
        get => imageSource;
        set => this.RaiseAndSetIfChanged(ref imageSource, value);
    }

    private bool isImageEnabled;
    public bool IsImageEnabled
    {
        get => isImageEnabled;
        set => this.RaiseAndSetIfChanged(ref isImageEnabled, value);
    }

    public bool IsReadyForExport => ImageData != null && ImageData.Length > 0;
}