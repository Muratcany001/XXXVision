using System.Diagnostics;
using OpenCvSharp;
using XXX.FrameGrabber.Features.Abstract;

namespace XXX.FrameGrabber.RtspFrameGrabber.Features;

public class Grabber 
{
    private readonly SimpleCamera _camera;
    private readonly IStorageImage _save;
    public Grabber(SimpleCamera camera, StorageImage save)
    {
        _camera = camera;
        _save = save;
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
                //Example save section
                // if (_isRecordingAll)
                // {
                //     _save.SaveImage(mat);
                // }
                //
                // if (_isTriggerRecieved)
                // {
                //     _isTriggerRecieved = false;
                //     _save.SaveImage(mat);
                // }
                Cv2.CvtColor(mat,mat,ColorConversionCodes.BGR2GRAY);
                _save.SaveImage(mat);
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
            _camera.GrabvVideo(RtspUrl, userName, password);
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