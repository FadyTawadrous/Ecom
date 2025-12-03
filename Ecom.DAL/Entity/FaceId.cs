using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DAL.Entity
{
    public class FaceId
    {
        public int Id { get; private set; }

        public byte[] Encoding { get; private set; } = Array.Empty<byte>();

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
        public string? CreatedBy { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }

        // Foreign key
        [ForeignKey("AppUser")]
        public string AppUserId { get; private set; } = null!;
        public virtual AppUser AppUser { get; private set; } = null!;

        public FaceId() { }
        public FaceId(double[] encoding, string appUserId, string createdBy)
        {
            Encoding = DoubleArrayToBytes(encoding);
            AppUserId = appUserId;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
        }

        public bool Update(double[] encoding, string updatedBy)
        {
            if (encoding != null && encoding.Length > 0 && !string.IsNullOrEmpty(updatedBy))
            {
                Encoding = DoubleArrayToBytes(encoding);
                UpdatedBy = updatedBy;
                UpdatedOn = DateTime.UtcNow;
                return true;
            }
            return false;
        }

        // Helper methods to convert between byte[] and double[]
        // Since face encodings are typically represented as arrays of doubles
        // we need to convert them to byte[] for storage in the database
        // and back when retrieving
        // because EF Core does not natively support double[] storage in a single column.
        public static byte[] DoubleArrayToBytes(double[] array)
        {
            var bytes = new byte[array.Length * sizeof(double)];
            Buffer.BlockCopy(array, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public double[] GetEncodingAsDouble()
        {
            var array = new double[Encoding.Length / sizeof(double)];
            Buffer.BlockCopy(Encoding, 0, array, 0, Encoding.Length);
            return array;
        }
    }
}
