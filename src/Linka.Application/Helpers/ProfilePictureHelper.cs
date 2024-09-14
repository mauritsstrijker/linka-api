namespace Linka.Application.Helpers;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.IO;
using System.Threading.Tasks;

public static class ProfilePictureHelper
{
    private const int MaxFileSize = 5 * 1024 * 1024; // 5 MB

    public static async Task<bool> ValidateImageAsync(byte[] imageBytes, int maxWidth, int maxHeight)
    {
        if (imageBytes == null || imageBytes.Length == 0 || imageBytes.Length > MaxFileSize)
        {
            return false;
        }

        try
        {
            using var stream = new MemoryStream(imageBytes);
            using var image = await Image.LoadAsync(stream);

            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                return false;
            }

            return image.Metadata.DecodedImageFormat is JpegFormat || image.Metadata.DecodedImageFormat is PngFormat;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string GetImageExtension(byte[] imageBytes)
    {
        if (imageBytes == null || imageBytes.Length == 0)
        {
            throw new ArgumentException("A imagem não pode ser nula ou vazia.", nameof(imageBytes));
        }

        try
        {
            using var stream = new MemoryStream(imageBytes);
            IImageFormat format = Image.DetectFormat(stream);

            if (format is JpegFormat)
            {
                return ".jpg";
            }
            else if (format is PngFormat)
            {
                return ".png";
            }

            throw new NotSupportedException("Formato não suportado.");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro detectando formato da imagem.", ex);
        }
    }
}
