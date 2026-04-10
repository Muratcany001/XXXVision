using OpenCvSharp;

namespace XXX.FrameGrabber.RtspFrameGrabber.Features;

public class Grabber
{
    private readonly SimpleCamera _camera;
    public Grabber(SimpleCamera camera)
    {
        _camera = camera;
    }
    public void grabVideo(string RtspUrl , string userName , string password)
    {
        int frameCount = 0;
        _camera.OnFrameReceived += (mat) =>
        {
            frameCount++;
            if (frameCount % 3 == 0)
            {
                Console.WriteLine($"{frameCount} frame received, {mat.Width}x{mat.Height} frame size");
            }
            mat.Dispose();
        };
        _camera.OnError += (msg) =>
        {
            Console.WriteLine("error found");
        };
        try
        {
            RtspUrl = "rtsp://95.3.35.42";
            userName = "admin";
            password = "12345";
            _camera.Start(RtspUrl, userName, password);
            Console.WriteLine($"{frameCount} frame started");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Baglanti hatasi {e.Message}");
        }
        finally
        {
            _camera.Dispose();
            Console.WriteLine($"{frameCount} frame stopped");
        }
    } 
}