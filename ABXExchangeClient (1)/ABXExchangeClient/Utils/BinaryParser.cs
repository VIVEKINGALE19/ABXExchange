using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABXExchangeClient.Utils
{
    public static class BinaryParser
    {
        public static int ReadInt32BigEndian(byte[] buffer, int offset)
        {
            return BinaryPrimitives.ReadInt32BigEndian(buffer.AsSpan(offset, 4));
        }

        public static string ReadAsciiString(byte[] buffer, int offset, int length)
        {
            return System.Text.Encoding.ASCII.GetString(buffer, offset, length);
        }
    }
}
