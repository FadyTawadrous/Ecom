
namespace Ecom.BLL.Helper
{
    public class VectorConverter
    {
        public static byte[] DoubleArrayToBytes(double[] array)
        {
            if (array == null || array.Length == 0)
                return Array.Empty<byte>();

            var bytes = new byte[array.Length * sizeof(double)];
            Buffer.BlockCopy(array, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static double[] BytesToDoubleArray(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return Array.Empty<double>();

            var array = new double[bytes.Length / sizeof(double)];
            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
            return array;
        }
    }
}
