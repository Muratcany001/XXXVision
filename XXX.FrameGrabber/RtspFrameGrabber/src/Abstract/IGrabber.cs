using OpenCvSharp.CPlusPlus;

namespace XXX.FrameGrabber.Features.Abstract;

public interface IGrabber
{
    void GrabvVideo(string rtspUrl, string user, string pass);
    void Stop();
    void Dispose();
}