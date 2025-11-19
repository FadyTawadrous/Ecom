
//namespace Tiers.BLL.ModelVM.Employee
//{
//    public class UpdateEmployeeVM
//    {
//        [Required]
//        public int Id { get; set; }

//        [Required]
//        public string Name { get; set; } = string.Empty;

//        [Required]
//        [Range(18, 60)]
//        public int Age { get; set; }

//        [Required]
//        [Range(0, double.MaxValue)]
//        public decimal Salary { get; set; }

//        public string? ImageUrl { get; set; }
//        public IFormFile? Image { get; set; }

//        [Required]
//        public int DepartmentId { get; set; }

//        [Required]
//        public string UpdatedBy { get; set; } = string.Empty;

//        public DateTime UpdatedOn { get; set; }

//        public IEnumerable<SelectListItem> Departments { get; set; } = new List<SelectListItem>();

//    }
//}
