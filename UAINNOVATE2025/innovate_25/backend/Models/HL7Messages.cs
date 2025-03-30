using System;
using System.Collections.Generic;
using System.Linq;

namespace HcaDeidentifier.Models
{
    public class HL7Message
    {
        public string MessageControlId { get; set; }
        public DateTime MessageDateTime { get; set; }
        public List<HL7Segment> Segments { get; set; } = new List<HL7Segment>();

        public HL7Message Clone()
        {
            return new HL7Message
            {
                MessageControlId = this.MessageControlId,
                MessageDateTime = this.MessageDateTime,
                Segments = this.Segments.Select(s => new HL7Segment
                {
                    SegmentId = s.SegmentId,
                    Fields = new List<string>(s.Fields)
                }).ToList()
            };
        }
    }

    public class HL7Segment
    {
        public string SegmentId { get; set; }
        public List<string> Fields { get; set; } = new List<string>();
    }
}