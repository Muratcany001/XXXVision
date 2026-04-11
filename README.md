Industrial Vision Engine
Bu proje, C# (.NET) tabanlı, modüler ve yüksek performanslı bir endüstriyel görüntü işleme yazılımı altyapısıdır. Endustriyel goruntu isleme mimarisi yaklaşımila; donanım bağımsızlığı, gerçek zamanlı analiz ve verimli bellek yönetimini hedefler.

Mimari Yapı
Yazılım, sorumlulukların net ayrılması için katmanlı bir yapıda tasarlanmıştır:

Core (Çekirdek) Katmanı: Sistemdeki temel iş akışını ve donanım kontratlarını tanımlar.

IGrabber: Farklı kamera türlerinin (RTSP, USB, Endüstriyel) sisteme aynı standartla bağlanmasını sağlayan arayüzdür.

Infrastructure (Altyapı) Katmanı: Dış kütüphaneler ve donanım sürücüleri ile iletişim kuran katmandır.

RtspGrabber: RtspFrameGrabber kütüphanesini kullanarak network üzerinden gelen akışı çözer ve OpenCV formatına (Mat) dönüştürür.

ImageStorage: Görüntülerin diske asenkron olarak kaydedilmesinden sorumlu sınıftır.

Processing (İşleme) Katmanı: OpenCV (OpenCvSharp) algoritmalarının koşturulduğu alandır.

Preprocessing: Görüntü iyileştirme (Blur, Thresholding, Grayscale) adımlarını içerir.

Analysis: Kenar bulma (Canny) ve istatistiksel veriler (Histogram) üretir.

Pipeline İş Akışı
Acquisition: RtspGrabber üzerinden gelen ham frame'ler yakalanır.

Dispatch: Yakalanan frame eş zamanlı olarak iki kola ayrılır:

Live View: UI üzerinde anlık izleme için ham görüntü basılır.

Analysis: Kullanıcı tetiklemesine (Manual Trigger) veya belirlenen fixed-rate hızına bağlı olarak işleme motoruna gönderilir.

Storage: Eğer kayıt modu aktifse veya bir "Snapshot" alınmışsa, ImageStorage sınıfı görüntüyü asenkron bir kuyrukta diske yazar.

 Teknik Detaylar ve Standartlar
Memory Management: Endüstriyel yüksek FPS akışlarında bellek sızıntısını önlemek için tüm Mat nesneleri işleme bittiği anda Dispose() edilir.

Thread Safety: Görüntü yakalama ve görüntü işleme süreçleri farklı iş parçacıklarında (Thread) çalıştırılarak arayüzün (UI) donması engellenir.

Latest Frame Logic: İşlemci darboğazını önlemek için sistem her zaman en güncel frame'e odaklanır, işlenemeyen eski frame'ler kuyrukta bekletilmeden temizlenir.

 Geliştirme Modülleri
Acquisition (Grabber): Kamera bağlantı ve kesilme yönetimi.

Preprocessing: Görüntü filtreleme ve eşikleme.

Analysis & Feature Extraction: Geometrik analiz ve piksel yoğunluğu hesaplama.

UI & Visualization: Parametrik kontrol paneli ve canlı izleme ekranı.
