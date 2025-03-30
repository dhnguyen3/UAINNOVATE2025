using System;
using System.Collections.Generic;
using System.Linq;
using HcaDeidentifier.Models;
using HcaDeidentifier.Services.Interfaces;

namespace HcaDeidentifier.Services
{
    public class HL7ParserService : IHL7ParserService
    {
        public List<HL7Message> ParseAndSort(string hl7Content)
        {
            return hl7Content.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(block =>
                {
                    var message = new HL7Message();
                    var segments = block.Split('\n');

                    foreach (var segment in segments)
                    {
                        if (string.IsNullOrWhiteSpace(segment)) continue;
                        
                        var parts = segment.Split('|');
                        var hl7Segment = new HL7Segment
                        {
                            SegmentId = parts[0],
                            Fields = parts.Skip(1).ToList()
                        };

                        message.Segments.Add(hl7Segment);

                        if (hl7Segment.SegmentId == "MSH")
                        {
                            message.MessageControlId = parts[9];
                            if (DateTime.TryParseExact(parts[6], "yyyyMMddHHmmss", null, 
                                System.Globalization.DateTimeStyles.None, out var date))
                            {
                                message.MessageDateTime = date;
                            }
                        }
                    }
                    return message;
                })
                .OrderBy(m => m.MessageDateTime)
                .ToList();
        }
    }
}