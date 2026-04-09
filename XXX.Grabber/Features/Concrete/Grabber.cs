using OpenCvSharp.CPlusPlus;
using XXX.FrameGrabber.Features.Abstract;
using RtspFrameGrabber;
using RtspFrameGrabber.Video;

namespace XXX.FrameGrabber.Features.Concrete;

public class Grabber : IGrabber
{
    public event Action<Mat>? OnFrameRecieved;
    
    public void StartGrab()
    {
        using var grabber = new RtspVideoFrameGrabber(logCallback : Console.WriteLine);
        grabber.Open("rtsp://localhost:9000",
            options: new Dictionary<string, string>
            {
                ["rtsp_transport"] = "rstp"
            }
            );
    }
    
    public void StopGrab()
    {
        throw new NotImplementedException();
    }
}