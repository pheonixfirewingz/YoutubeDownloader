namespace YoutubeDownloaderCLI.Downloads
{
    public enum Format
    {
        None,
        MP3,
        MP4
    };

    internal class DownloadInfo(in string yt_link_in, in string yt_output_in, in string yt_output_format)
    {
        public readonly string link = yt_link_in;
        public readonly string output = yt_output_in;
        public readonly Format format = StrToEnum(yt_output_format);

        private static Format StrToEnum(in string yt_output_format)
        {
            if (string.IsNullOrEmpty(yt_output_format))
            {
                return Format.None;
            }
            return Enum.TryParse(yt_output_format, true, out Format result) ? result : Format.None;
        }
    }
}
