using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace DiscordBotLib.Core.Networking
{
    public class CompressionHandler
    {
        public byte[] CompressData(byte[] data)
        {
            using (var memorystream = new MemoryStream())
            {
                using (var gzipstream = new GZipStream(memorystream, CompressionMode.Compress))
                {
                    gzipstream.Write(data, 0, data.Length);
                }
                return memorystream.ToArray();
            }
        }

        public byte[] DecompressData(byte[] compressedData)
        {
            using (var memorystream = new MemoryStream(compressedData))
            using (var gzipstream = new GZipStream(memorystream, CompressionMode.Decompress))
            using (var resultstream = new MemoryStream())
            {
                gzipstream.CopyTo(resultstream);
                return resultstream.ToArray();
            }
        }

        public string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            byte[] compressed = CompressData(buffer);
            return Convert.ToBase64String(compressed);
        }

        public string DecompressString(string compressedText)
        {
            byte[] compressedData = Convert.FromBase64String(compressedText);
            byte[] decompressed = DecompressData(compressedData);
            return Encoding.UTF8.GetString(decompressed);
        }

        public bool IsCompressed(byte[] data)
        {
            if (data.Length < 2) return false;
            return data[0] == 0x1F && data[1] == 0x8B;
        }
    }
}