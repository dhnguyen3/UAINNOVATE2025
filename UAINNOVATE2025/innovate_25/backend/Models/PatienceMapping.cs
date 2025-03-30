public class PatientMapping
{
    public string OriginalMRN { get; set; }
    public string FakeMRN { get; set; }
    public string OriginalName { get; set; }
    public string FakeName { get; set; }
    public DateTime OriginalDob { get; set; }
    public DateTime FakeDob { get; set; }
    public string OriginalAddress { get; set; }
    public string FakeAddress { get; set; }
    public string OriginalPhone { get; set; }
    public string FakePhone { get; set; }
    public string OriginalSsn { get; set; }
    public string FakeSsn { get; set; }
    public List<string> OriginalAccountNumbers { get; set; } = new();
    public List<string> FakeAccountNumbers { get; set; } = new();
}