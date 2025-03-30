using Microsoft.AspNetCore.Mvc;
using HcaDeidentifier.Services.Interfaces;
using HcaDeidentifier.Models;

namespace HcaDeidentifier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HL7Controller : ControllerBase
    {
        private readonly IFileProcessingService _fileService;

        public HL7Controller(IFileProcessingService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessHL7File(IFormFile file)
        {
            var result = await _fileService.ProcessHl7FilesAsync(file);
            return result.Success 
                ? Ok(new { result.OriginalMessages, result.RedactedMessages, result.DeidentifiedMessages })
                : BadRequest(result.ErrorMessage);
        }
    }
}