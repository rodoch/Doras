# Doras

This is a reference implementation of the excellent [ImageProcessor](http://imageprocessor.org/) library.

I use this implementation as an image server to resize, reformat and cache images on the fly for other web applications. I wanted to have a reference repository that could be dropped into new applications with minimal configuration. I originally created this implementation for my clients at [Fiontar & Scoil na Gaeilge, DCU](https://www.gaois.ie/).

Doras includes sample configurations for two scenarios:

- Retrieving images from Microsoft Azure blob storage and caching the processed images on disk, and;
- Retrieving images from Microsoft Azure blob storage and caching the processed images in the same or in a different blob storage container.

Refer to the ImageProcessor [documentation](http://imageprocessor.org/imageprocessor-web/) for API details.

Doras is a .NET Framework 4.6.1 application. I intend to upgrade to a .NET Core implementation once the successor to ImageProcessor, [ImageSharp](https://github.com/SixLabors/ImageSharp) becomes stable.
