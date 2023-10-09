using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Diagramer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagramsController : ControllerBase
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IHostingEnvironment hostingEnv;
    private const string diagramsFolder = "Diagrams";

    public DiagramsController(IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnv)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.hostingEnv = hostingEnv;
    }
    
    [HttpPost]
    public async Task<string> SaveImageAsync(byte[] imageBytes)
    {
        try
        {
            string fileName = string.Concat(Guid.NewGuid().ToString(), ".svg");
            string rootPath = Path.Combine(hostingEnv.WebRootPath, diagramsFolder);
            string filePath = Path.Combine(rootPath, fileName);

            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
                
            HttpRequest request = httpContextAccessor.HttpContext.Request;
            string url = string.Concat(request.Scheme, "://", request.Host, request.PathBase, "/", diagramsFolder,
                "/", fileName);
            return url;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}