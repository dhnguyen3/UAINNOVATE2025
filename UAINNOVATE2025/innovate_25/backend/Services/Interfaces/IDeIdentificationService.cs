namespace HcaDeidentifier.Services.Interfaces;


public interface IDeIdentificationService
{
    List<HL7Message> DeIdentify(List<HL7Message> messages);
    List<HL7Message> RedactPHI(List<HL7Message> messages);
}