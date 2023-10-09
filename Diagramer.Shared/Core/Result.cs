namespace Diagramer.SharedModels.Core;

public class Result<TObject>
{
    public string ErrorMessage { get; set; }
    public bool HasError => ErrorMessage != null && ErrorMessage != String.Empty;
    public TObject ResultObject { get; set; }

    public Result(TObject resultObject)
    {
        ResultObject = resultObject;
    }

    public Result(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
