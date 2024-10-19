using YoutubeDownloader.Core.Downloading;
using YoutubeDownloader.Core.Tagging;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloaderCLI.Downloads
{
    internal class Downloader(DownloadInfo download_info_in)
    {
        private readonly DownloadInfo download_info = download_info_in;

        private static bool IsDownloadAudio(in DownloadInfo download_info)
        {
            switch (download_info.format)
            {
                case Format.MP3:
                    return true;
                case Format.MP4:
                    return false;
                default:
                    break;
            }
            throw new NotImplementedException();
        }

        private static Container GetContainer(in DownloadInfo download_info) 
        {
            switch (download_info.format)
            {
                case Format.MP3:
                    return Container.Mp3;
                case Format.MP4:
                    return Container.Mp4;
                default:
                    break;
            }
            throw new NotImplementedException();
        }

        public async Task Download()
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(download_info.link);
            var video_streams = await youtube.Videos.Streams.GetManifestAsync(download_info.link);
            var downloader = new VideoDownloader(null);
            var streams = VideoDownloadOption.ResolveAll(video_streams);
            if (IsDownloadAudio(download_info))
            {
                VideoDownloadOption audio_stream_info = streams.Where(s => s.IsAudioOnly && s.Container == GetContainer(download_info)).First();
                foreach (VideoDownloadOption audio in streams.Where(s => s.IsAudioOnly && s.Container == GetContainer(download_info)))
                {
                    AudioOnlyStreamInfo? current_audio = audio_stream_info.StreamInfos.FirstOrDefault() as AudioOnlyStreamInfo;
                    AudioOnlyStreamInfo? check_audio = audio.StreamInfos.FirstOrDefault() as AudioOnlyStreamInfo;
                    if (check_audio is not null && current_audio is not null)
                    {
                        if (current_audio.Bitrate > check_audio.Bitrate)
                        {
                            audio_stream_info = audio;
                        }
                    }
                }
                var stream = audio_stream_info.StreamInfos.FirstOrDefault() as AudioOnlyStreamInfo;
                Console.WriteLine($"downloading: <Name:{video.Title},Biterate:{stream!.Bitrate}>");

                await downloader.DownloadVideoAsync(download_info.output, video, audio_stream_info, false);
                var injector = new MediaTagInjector();
                await injector.InjectTagsAsync(download_info.output, video);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
