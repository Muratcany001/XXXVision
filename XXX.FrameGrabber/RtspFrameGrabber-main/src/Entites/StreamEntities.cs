using System.Drawing;
using FFmpeg.AutoGen;

namespace XXX.FrameGrabber.RtspFrameGrabber_main.Entites;

public class StreamEntities
{
    public string? CodecName { get; set; }
    public Size? FrameSize { get; set; }
    public AVPixelFormat? PixelFormat { get; set; }
}