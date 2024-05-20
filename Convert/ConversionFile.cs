namespace WizardConvert.Convert
{
    internal class ConversionFile
    {
        public string OriginalFileName { get; }
        public string NewFileName { get; }
        public string Command { get; }

        public ConversionFile(string originalFileName, string newFileName, string command)
        {
            OriginalFileName = originalFileName;
            NewFileName = newFileName;
            Command = command;
        }
    }
}
