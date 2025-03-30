namespace HcaDeidentifier.Services.Interfaces;

public interface IFileProcessingService
{
    Task<ProcessResult> ProcessHl7FilesAsync(IFormFile file);
}