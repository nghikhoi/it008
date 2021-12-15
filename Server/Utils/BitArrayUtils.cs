using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Utils {
    public static class BitArrayUtils {

        public static byte[] ToBytes(bool[] bitArray) {
            int byteSize = bitArray.Length / 8;
            if (bitArray.Length % 8 > 0)
                byteSize++;
            byte[] result = new byte[byteSize];
            for (var i = 0; i < bitArray.Length; i++) {
                int byteIndex = i / 8;
                result[byteIndex] |= (byte) ((bitArray[i] ? 1 : 0) << (7 - (i % 8)));
            }
            return result;
        }

        public static bool[] ToBits(byte[] byteArray) {
            int bitSize = byteArray.Length * 8;
            bool[] result = new bool[bitSize];
            for (int i = 0; i < byteArray.Length; i++) {
                bool[] bits = ToBits(byteArray[i]);
                int start = i * 8;
                for (int j = 0; j < bits.Length; j++) {
                    result[start + j] = bits[j];
                }
            }

            return result;
        }

        public static bool[] ToBits(byte b) {
            bool[] result = new bool[8];
            for (int i = 0; i < 8; i++) {
                result[7 - i] = ((b >> i) & 1) == 1;
            }
            return result;
        }

    }
}
