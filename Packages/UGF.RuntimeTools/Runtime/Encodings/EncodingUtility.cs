using System;
using System.Text;

namespace UGF.RuntimeTools.Runtime.Encodings
{
    public static class EncodingUtility
    {
        public static Encoding GetEncoding(EncodingType type)
        {
            switch (type)
            {
                case EncodingType.Default: return Encoding.Default;
                case EncodingType.Unicode: return Encoding.Unicode;
                case EncodingType.BigEndianUnicode: return Encoding.BigEndianUnicode;
                case EncodingType.UTF7: return Encoding.UTF7;
                case EncodingType.UTF8: return Encoding.UTF8;
                case EncodingType.UTF32: return Encoding.UTF32;
                case EncodingType.ASCII: return Encoding.ASCII;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown encoding type.");
            }
        }
    }
}
