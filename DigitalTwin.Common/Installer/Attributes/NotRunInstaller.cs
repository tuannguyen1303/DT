namespace DigitalTwin.Common.Installer.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class NotRunInstallerAttribute : Attribute
    {
        public NotRunInstallerAttribute()
        {
        }
    }
}
