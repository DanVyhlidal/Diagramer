using Diagramer.SharedModels.Core;

namespace Diagram.Services.Exporters.Core;

public interface IExportService
{
    Task ExportToSvg(string filePath, byte[] diagram);
    Task<Result<byte[]>> ExportToImageBytes(string diagram);
    Task<Result<string>> ShareDiagram(byte[] diagram);
}