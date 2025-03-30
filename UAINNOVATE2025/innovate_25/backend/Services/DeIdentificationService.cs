using HcaDeidentifier.Models;
using HcaDeidentifier.Services.Interfaces;

namespace HcaDeidentifier.Services;

public class DeIdentificationService : IDeIdentificationService
{
    private readonly Dictionary<string, string> _patientMrnMap = new();

    public List<HL7Message> DeIdentify(List<HL7Message> messages)
    {
        foreach (var msg in messages)
        {
            var mrn = msg.Segments
                .FirstOrDefault(s => s.SegmentId == "PID")?
                .Fields.ElementAtOrDefault(2)?
                .Split('^').FirstOrDefault();

            if (mrn != null)
            {
                if (!_patientMrnMap.ContainsKey(mrn))
                    _patientMrnMap[mrn] = $"FAKE{new Random().Next(10000000, 99999999)}";

                var pid = msg.Segments.First(s => s.SegmentId == "PID");
                pid.Fields[2] = _patientMrnMap[mrn];
                pid.Fields[4] = "Doe^John";
            }
        }
        return messages;
    }

    public List<HL7Message> RedactPHI(List<HL7Message> messages)
    {
        return messages.Select(m =>
        {
            var redacted = m.Clone();
            // Add redaction logic here
            return redacted;
        }).ToList();
    }
}