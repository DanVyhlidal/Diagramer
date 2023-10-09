using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using SkiaSharp;
using Svg.Skia;

namespace Diagramer.GUI.Core;

public class ByteToBitmapConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is byte[] imageData)
        {
            using (var stream = new MemoryStream(imageData))
            {
                var svg = new SKSvg();
                svg.Load(stream);

                SKRect dimensions = svg.Picture.CullRect;
                    
                var info = new SKImageInfo((int)dimensions.Width, (int)dimensions.Height);
                
                using (var skBitmap = new SKBitmap(info))
                {
                    using (var canvas = new SKCanvas(skBitmap))
                    {
                        canvas.Clear(SKColors.White);
                        canvas.DrawPicture(svg.Picture);
                    }
                    
                    using (var ms = new MemoryStream())
                    {
                        skBitmap.Encode(ms, SKEncodedImageFormat.Png, 100);
                        ms.Position = 0;
                        var bitmap = new Bitmap(ms);
                        return bitmap;
                    }
                }
            }
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IBitmap image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream);
                return stream.ToArray();
            }
        }

        return null;
    }
}