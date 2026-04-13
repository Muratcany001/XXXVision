using OpenCvSharp;
using System;
using System.Threading;
using System.Threading.Tasks;
using XXX.FrameGrabber.Features.Abstract;
using Mat = OpenCvSharp.Mat;

public class SimpleCamera : IGrabber,IDisposable
{
    private readonly object _lock = new object();
    private VideoCapture _capture;
    private CancellationTokenSource _cts;
    private Task _task;
    private bool _isRunning = false;

    public event Action<Mat>? OnFrameReceived;
    public event Action<string>? OnError;
    
    public void GrabvVideo(string rtspUrl, string user, string pass)
    {
        if (_isRunning)
            throw new InvalidOperationException("Kamera zaten çalışıyor!");

        _cts = new CancellationTokenSource();
        
        string fullUrl = $"rtsp://{user}:{pass}@{new Uri(rtspUrl).Authority}{new Uri(rtspUrl).AbsolutePath}";
        
        Console.WriteLine($"Bağlanıyor: {rtspUrl.Replace(pass, "****")}");

        lock (_lock)
        {
            _capture = new VideoCapture(fullUrl);

            if (!_capture.IsOpened())
            {
                _capture?.Dispose();
                throw new Exception("Kamera açılamadı!");
            }

            Console.WriteLine($"Kamera açıldı");
            Console.WriteLine($"   Çözünürlük: {_capture.FrameWidth}x{_capture.FrameHeight}");
            Console.WriteLine($"   FPS: {_capture.Fps}");
        }
        
        Mat frame = new Mat();
        lock (_lock)
        {
            _capture.Read(frame);
        }

        if (!frame.Empty())
        {
            try
            {
                Cv2.ImShow("Test", frame);
                Cv2.WaitKey(1);
            }
            catch { }
        }

        frame.Dispose();
        
        _isRunning = true;
        _task = Task.Run(() => Loop(_cts.Token));
    }

    /// <summary>
    /// Take frame
    /// </summary>
    private void Loop(CancellationToken token)
    {
        Mat frame = new Mat();
        int errorCount = 0;
        const int maxErrors = 15;

        try
        {
            while (!token.IsCancellationRequested)
            {
                bool success = false;

                lock (_lock)
                {
                    if (_capture == null || !_capture.IsOpened())
                        break;

                    success = _capture.Read(frame);
                }
                if (success && !frame.Empty())
                {
                    errorCount = 0;
                    try
                    {
                        Mat cloned = frame.Clone();
                        OnFrameReceived?.Invoke(cloned);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Subscriber hata: {ex.Message}");
                        OnError?.Invoke($"Event handler error: {ex.Message}");
                    }
                }
                else
                {
                    errorCount++;
                    if (errorCount <= maxErrors)
                    {
                        Console.WriteLine($"Frame alınamadı ({errorCount}/{maxErrors})");
                    }
                    else
                    {
                        Console.WriteLine($"bağlantı koptu");
                        OnError?.Invoke("Connection lost: too many consecutive read failures");
                        break;
                    }

                    Thread.Sleep(50);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Loop hatası: {ex.Message}");
            OnError?.Invoke($"Loop error: {ex.Message}");
        }
        finally
        {
            frame?.Dispose();
        }
    }
    /// <summary>
    /// Stop taking frames
    /// </summary>
    public void Stop()
    {
        if (!_isRunning)
            return;

        Console.WriteLine("Kamera kapatılıyor...");

        _cts?.Cancel();

        try
        {
            _task?.Wait(TimeSpan.FromSeconds(5));
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Loop timeout ile iptal edildi");
        }

        lock (_lock)
        {
            _capture?.Release();
            _capture?.Dispose();
            _capture = null;
        }

        _isRunning = false;
        Console.WriteLine("Kamera kapatıldı");
    }

    public void Dispose()
    {
        Stop();
        _cts?.Dispose();
    }
}