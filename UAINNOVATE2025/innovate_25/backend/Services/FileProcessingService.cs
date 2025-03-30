using System.IO;
using System.Threading.Tasks;
using HcaDeidentifier.Services.Interfaces;
using HcaDeidentifier.Models;

namespace HcaDeidentifier.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly IHL7ParserService _parserService;
        private readonly IDeIdentificationService _deIdService;

        public FileProcessingService(
            IHL7ParserService parserService,
            IDeIdentificationService deIdService)
        {
            _parserService = parserService;
            _deIdService = deIdService;
        }

        public async Task<ProcessResult> ProcessHl7FilesAsync(IFormFile file)
        {
            try
            {
                using var reader = new StreamReader(file.OpenReadStream());
                var content = await reader.ReadToEndAsync();
                
                var originalMessages = _parserService.ParseAndSort(content);
                var redactedMessages = _deIdService.RedactPHI(originalMessages.Select(m => m.Clone()).ToList());
                var deidentifiedMessages = _deIdService.DeIdentify(originalMessages.Select(m => m.Clone()).ToList());

                return ProcessResult.SuccessResult(
                    originalMessages,
                    redactedMessages,
                    deidentifiedMessages
                );
            }
            catch (Exception ex)
            {
                return ProcessResult.Fail($"Error processing file: {ex.Message}");
            }
        }
    }
}