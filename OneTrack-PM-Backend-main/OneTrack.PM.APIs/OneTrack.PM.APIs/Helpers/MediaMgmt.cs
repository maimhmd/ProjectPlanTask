using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xabe.FFmpeg;

public static class MediaMgmt
{
    public async static Task<double?> GetMediaFileDuration(string file)
    {
        IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(Path.Combine(Directory.GetCurrentDirectory(), file.Replace("/", @"\")));
        double? duration = (double)mediaInfo.VideoStreams.First().Duration.TotalMinutes;
        return duration;
    }
}