using Diagramer.SharedModels.Core;

namespace Diagramer.Infrastructure.Exporters.Core;

public interface IExporter
{
    Task<Result<byte[]>> ExportToBytes(string diagram);
}