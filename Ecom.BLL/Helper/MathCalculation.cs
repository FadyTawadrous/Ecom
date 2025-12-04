
namespace Ecom.BLL.Helper
{
    public static class MathCalculation
    {
        // Calculate Cosine Similarity between two vectors
        // Returns a value between -1 and 1
        // 1 means identical, 0 means orthogonal, -1 means opposite
        public static double CosineSimilarity(double[] a, double[] b)
        {
            if (a == null || b == null || a.Length == 0 || b.Length == 0 || a.Length != b.Length)
                return 0;

            double dot = 0;
            double magA = 0;
            double magB = 0;

            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                magA += a[i] * a[i];
                magB += b[i] * b[i];
            }

            magA = Math.Sqrt(magA);
            magB = Math.Sqrt(magB);

            if (magA == 0 || magB == 0)
                return 0;

            return dot / (magA * magB);
        }
    }
}
