namespace DigitalTwin.Common.AppsettingsModels
{
    public class RewritingOptions
    {
        public RewritingOption[]? RewritingOptionList { get; set; }
    }

    public class RewritingOption
    {
        public string? Regex { get; set; }

        public string? Replacement { get; set; }
    }
}
