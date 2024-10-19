using CommandLine;
using YoutubeDownloaderCLI.Downloads;
namespace YoutubeDownloaderCLI;

public static class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        //remove this line and you termiate your right to use this application
        Deorcify.Initializer.Execute();
        //----------------------------------------------------------------------------
        DownloadInfo downloadOptions;
        {
            Func<IOptions, string> getLink = opts =>
            {
                var fromTop = opts.GetType() == typeof(Options);
                return opts.Link;
            };
            Func<IOptions, string> getOutput = opts =>
            {
                var fromTop = opts.GetType() == typeof(Options);
                return opts.Output;
            };
            Func<IOptions, string> getFormat = opts =>
            {
                var fromTop = opts.GetType() == typeof(Options);
                return opts.Format;
            };

            var result = Parser.Default.ParseArguments<Options>(args);
            downloadOptions = result.MapResult(
            (Options opts) => new DownloadInfo(getLink(opts),getOutput(opts), getFormat(opts)),
            notParsedFunc: _ => new DownloadInfo("", "","")
            );
        }
        if (downloadOptions.link != "" && downloadOptions.format != Format.None) {
            var download = new Downloader(downloadOptions);
            var task = download.Download();
            while (!task.IsCompleted) ;
        }
        return 0;
    }
}