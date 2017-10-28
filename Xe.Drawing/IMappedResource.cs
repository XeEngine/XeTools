using System;

namespace Xe.Drawing
{
    public interface IMappedResource : IDisposable
    {
        IntPtr Data { get; }

        int Stride { get; }

        int Length { get; }
    }
}
