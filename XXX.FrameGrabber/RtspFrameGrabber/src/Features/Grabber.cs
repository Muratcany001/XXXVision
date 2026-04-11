using System.Diagnostics;
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
        int targetfps = 10;
        double frameInterval = 1000.0 / targetfps;
        int frameCount = 0;
        Stopwatch sw = new Stopwatch();
        Action<Mat> onFrameHandler = (mat) =>
        {
            if (sw.ElapsedMilliseconds > frameInterval)
            {
                // Processing area. (write,threshold,do preprocessing)
                frameCount++;
                if (frameCount % 3 == 0)
                {
                    Console.WriteLine($"{frameCount} frame received, {mat.Width}x{mat.Height} frame size");
                }
                mat.Dispose();
            }
        };
        Action<string> OnErrorhandler = (msg) =>
        {
            Console.WriteLine("error found");
        };
        _camera.OnFrameReceived += onFrameHandler;
        _camera.OnError += OnErrorhandler;
        try
        {
            RtspUrl = "rtsp://95.3.35.42";
            userName = "admin";
            password = "12345";
            _camera.Start(RtspUrl, userName, password);
            Console.WriteLine($"{frameCount} frame started");
            Console.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Baglanti hatasi {e.Message}");
        }
        finally
        {
            _camera.OnFrameReceived -= onFrameHandler;
            _camera.OnError -= OnErrorhandler;
            _camera.Dispose();
            Console.WriteLine($"{frameCount} frame stopped");
        }
    } 
}