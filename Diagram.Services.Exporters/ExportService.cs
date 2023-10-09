using System.Net.Http.Headers;
using System.Text;
using Diagram.Services.Exporters.Core;
using Diagramer.Infrastructure.Exporters.Core;
using Diagramer.SharedModels.Core;

namespace Diagram.Services.Exporters;

public class ExportService<TExporter> : IExportService where TExporter : IExporter, new()
{
    private readonly TExporter exporter;
    private readonly string apiUrl;

    private HttpClient client;

    public ExportService(string apiUrl)
    {
        exporter = new TExporter();
        this.apiUrl = apiUrl;

        InitializeHttpClient();
    }
    
    public Task ExportToSvg(string filePath, byte[] diagram)
    {
        File.WriteAllBytes(string.Concat(filePath, ".svg"), diagram);
        return Task.CompletedTask;
    }

    public async Task<Result<byte[]>> ExportToImageBytes(string diagram)
    {
        return await exporter.ExportToBytes(diagram);
    }

    public async Task<Result<string>> ShareDiagram(byte[] diagram)
    {
        string fullPathUri = string.Concat(apiUrl, "api/Diagrams");
        string responseString = "";
        try
        {
            ByteArrayContent content = new ByteArrayContent(diagram);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

            HttpResponseMessage response = await client.PostAsync(fullPathUri, content);

            Stream stream = await response.Content.ReadAsStreamAsync();

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                responseString = await reader.ReadToEndAsync();
            }
        }
        catch (Exception e)
        {
            return new Result<string>(errorMessage: e.Message);
        }

        return new Result<string>(resultObject: responseString);
    }
    
    private void InitializeHttpClient()
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        {
            return true;
        };
        
        client = new HttpClient(clientHandler);
    }
}