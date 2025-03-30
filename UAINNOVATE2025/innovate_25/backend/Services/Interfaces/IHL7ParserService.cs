using System.Collections.Generic;
using HcaDeidentifier.Models;

namespace HcaDeidentifier.Services.Interfaces
{
    public interface IHL7ParserService
    {
        List<HL7Message> ParseAndSort(string hl7Content);
    }
}