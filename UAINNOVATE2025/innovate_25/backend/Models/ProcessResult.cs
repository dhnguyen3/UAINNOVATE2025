namespace HcaDeidentifier.Models;

public class ProcessResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public List<HL7Message> OriginalMessages { get; set; }
    public List<HL7Message> RedactedMessages { get; set; }
    public List<HL7Message> DeidentifiedMessages { get; set; }

    public static ProcessResult Fail(string error) => new()
    {
        Success = false,
        ErrorMessage = error
    };

    public static ProcessResult SuccessResult(
        List<HL7Message> original,
        List<HL7Message> redacted,
        List<HL7Message> deidentified) => new()
    {
        Success = true,
        OriginalMessages = original,
        RedactedMessages = redacted,
        DeidentifiedMessages = deidentified
    };
}