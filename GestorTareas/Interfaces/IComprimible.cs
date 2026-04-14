using System;

namespace GestorTareas.Interfaces;

public interface IComprimible
{
    public byte[] Compress();
    public void DesCompress(byte[] dataInfo);
}
