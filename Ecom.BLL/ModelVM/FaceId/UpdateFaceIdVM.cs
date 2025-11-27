
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Ecom.BLL.ModelVM.FaceId
{
    public class UpdateFaceIdVM
    {
        public int Id { get; set; }
        [JsonIgnore]
        public double[] Encoding { get; set; }
        public string UpdatedBy { get; set; }
        public string AppUserId { get; set; }
        public IFormFile imageFile { get; set; }
    }
}
