namespace ExpensifyImporter.UnitTests.Modules.IO;

public class ImageDownloaderTests
{
    private readonly HttpClient _client;

    public ImageDownloaderTests()
    {
        _client = new HttpClient();
    }

    [Fact]
    public async Task When_Image_Download_It_Successfully_Downloads_Image()
    {
        //https://images.all-free-download.com/images/graphicwebp/cat_domestic_cat_sweet_269854.webp
        //https://images.all-free-download.com/images/graphicwebp/cat_feline_cats_eye_220526.webp
        //https://images.all-free-download.com/images/graphicwebp/cat_cats_eyes_cat_face_269574.webp
        
    }
}