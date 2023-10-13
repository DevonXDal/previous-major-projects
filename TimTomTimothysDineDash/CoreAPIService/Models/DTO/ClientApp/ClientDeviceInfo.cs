namespace CoreAPIService.Models.DTO.ClientApp
{
    /// <summary>
    /// This ClientDeviceInfo class holds the information related to a unique, client's machine.
    /// This aids in troubleshooting issues that certain people may be having without actually going
    /// through the bother of asking them for possibly incorrect information.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class ClientDeviceInfo
    {
        /// <summary>
        /// Example: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Example: Windows
        /// </summary>
        public string? OS { get; set; }

        /// <summary>
        /// Example: Chrome
        /// </summary>
        public string? Browser { get; set; }

        /// <summary>
        /// Example: windows-10
        /// </summary>
        public string? OSVersion { get; set; }

        /// <summary>
        /// Example: 112.0.0.0
        /// </summary>
        public string? BrowserVersion { get; set; }

        /// <summary>
        /// May be: desktop, tablet, mobile, or unknown
        /// </summary>
        public string? DeviceType { get; set; }

        /// <summary>
        /// Example: landscape
        /// </summary>
        public string? Orientation { get; set; }
    }
}
