using OpenCvSharp;

namespace XXX.FrameGrabber.Features.Abstract;

public interface IStorageImage
{
    Task SaveImage(Mat image);
}