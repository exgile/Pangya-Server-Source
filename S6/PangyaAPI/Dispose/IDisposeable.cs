using System;
namespace PangyaAPI.Dispose
{
    public interface IDisposeable : IDisposable
    {
        bool Disposed { get; set; }
    }
}