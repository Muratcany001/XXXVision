using OpenCvSharp.CPlusPlus;

namespace XXX.FrameGrabber.Features.Abstract;

public interface IGrabber
{
    event Action<Mat> OnFrameRecieved;
    void StartGrab();
    void StopGrab();
}