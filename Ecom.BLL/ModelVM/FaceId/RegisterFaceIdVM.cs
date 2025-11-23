
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace Ecom.BLL.ModelVM.FaceId
{
    public class RegisterFaceIdVM
    {
        [JsonIgnore]
        public double[] Encoding { get; set; }
        public string CreatedBy { get; set; }
        public string AppUserId { get; set; }
        public IFormFile imageFile { get; set; }

    }
}
