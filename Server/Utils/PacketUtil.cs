using DotNetty.Buffers;

namespace ChatServer.Utils {

	public class PacketUtil {
		
		public static string encode(IByteBuffer buffer) {
			int length = buffer.ReadableBytes;
			byte[] array = new byte[length];   
			buffer.GetBytes(buffer.ReaderIndex, array);
			string result = HashUtils.Base64Encode(array);
			return result;
		}

		public static IByteBuffer decode(string encoded) {
			IByteBuffer buffer = ByteBufferUtil.DefaultAllocator.Buffer();
			byte[] array = HashUtils.Base64DecodeAsBytes(encoded);
			buffer = ByteBufferUtil.DefaultAllocator.Buffer();
			for (var i = 0; i < array.Length; i++) {
				buffer.WriteByte(array[i]);
			}
			return buffer;
		}
		
	}

}