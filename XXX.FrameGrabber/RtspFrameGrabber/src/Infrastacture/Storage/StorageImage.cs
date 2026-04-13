using OpenCvSharp;
using XXX.FrameGrabber.Features.Abstract;

namespace XXX.FrameGrabber.RtspFrameGrabber.Features;

public class StorageImage : IStorageImage
{
    /// <summary>
    /// Choose path for saved images
    /// You can change this path from UI
    /// </summary>
    /// <param name="image"></param>
    public async Task SaveImage(Mat image)
    {
        
        string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Captures", DateTime.Now.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(folder);
        
        string fileName = $"capture_{DateTime.Now:HH-mm-ss-fff}.png";
        string filePath = Path.Combine(folder, fileName);
        
        image.SaveImage(filePath);
    }
}