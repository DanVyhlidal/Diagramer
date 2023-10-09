using Diagramer.Infrastructure.Exporters.Core;
using Diagramer.SharedModels.Core;
using PlantUml.Net;

namespace Diagramer.Infrastructure.Exporters;

public class PlantUmlExporter : IExporter
{
    private IPlantUmlRenderer renderer;
    public PlantUmlExporter()
    {
        RendererFactory factory = new RendererFactory();

        renderer = factory.CreateRenderer(new PlantUmlSettings()
        {
            RenderingMode = RenderingMode.Remote
        });
    }
    public async Task<Result<byte[]>> ExportToBytes(string diagram)
    {
        byte[] diagramBytes;
        
        try
        {
            diagramBytes = await renderer.RenderAsync(diagram, OutputFormat.Svg);
        }
        catch (Exception e)
        {
            return new Result<byte[]>(e.Message);
        }

        return new Result<byte[]>(diagramBytes);
    }
}