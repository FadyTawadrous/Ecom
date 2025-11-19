//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Tiers.BLL.Service.Abstraction;

//namespace Tiers.PL.Controllers
//{
//    public class EmployeeController : Controller
//    {
//        private readonly IEmployeeService _employeeService;

//        public EmployeeController(IEmployeeService employeeService)
//        {
//            _employeeService = employeeService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index()
//        {
//            var response = await _employeeService.GetAllAsync();

//            if (response.IsSuccess)
//            {
//                return View(response.Result); // Passes IEnumerable<GetEmployeeVM>
//            }

//            // Return an empty list on failure
//            return View(new List<GetEmployeeVM>());
//        }

//        [HttpGet]
//        public async Task<IActionResult> Details(int id)
//        {
//            if (id <= 0) return BadRequest();

//            var response = await _employeeService.GetByIdAsync(id);

//            if (response.IsSuccess)
//            {
//                return View(response.Result); // Passes GetEmployeeVM
//            }

//            return NotFound();
//        }

//        // Get create view
//        [HttpGet]
//        public async Task<IActionResult> Create()
//        {
//            // Call the service to get a VM with the dropdowns populated
//            var response = await _employeeService.GetCreateModelAsync();

//            if (response.IsSuccess)
//            {
//                return View(response.Result); // Passes CreateEmployeeVM
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        // Create employee
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(CreateEmployeeVM model)
//        {
//            if (ModelState.IsValid)
//            {
//                var response = await _employeeService.CreateAsync(model);

//                if (response.IsSuccess)
//                {
//                    TempData["SuccessMessage"] = "Employee has been created successfully";
//                    return RedirectToAction(nameof(Index));
//                }

//                ModelState.AddModelError(string.Empty, response.ErrorMessage);
//            }

//            // MUST reload the dropdowns before returning the view.
//            var dropdownsResponse = await _employeeService.GetCreateModelAsync();
//            model.Departments = dropdownsResponse.Result?.Departments ?? new List<SelectListItem>();
//            return View(model);
//        }

//        // Get edit view
//        [HttpGet]
//        public async Task<IActionResult> Edit(int id)
//        {
//            if (id <= 0) return BadRequest();

//            // Get the VM populated with employee data AND the dropdown list
//            var response = await _employeeService.GetUpdateModelAsync(id);

//            if (response.IsSuccess)
//            {
//                return View(response.Result); // Passes UpdateEmployeeVM
//            }

//            return NotFound();
//        }

//        // Edit department
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, UpdateEmployeeVM model)
//        {
//            if (id != model.Id) return BadRequest();

//            if (ModelState.IsValid)
//            {
//                var response = await _employeeService.UpdateAsync(model);

//                if (response.IsSuccess)
//                {
//                    TempData["SuccessMessage"] = "Employee has been updated successfully";
//                    return RedirectToAction(nameof(Index));
//                }

//                ModelState.AddModelError(string.Empty, response.ErrorMessage);
//            }

//            // Reload dropdowns if validation fails
//            var dropdownsResponse = await _employeeService.GetUpdateModelAsync(model.Id);
//            model.Departments = dropdownsResponse.Result?.Departments ?? new List<SelectListItem>();
//            return View(model);
//        }

//        // Get delete view
//        [HttpGet]
//        public async Task<IActionResult> Delete(int id)
//        {
//            if (id <= 0) return BadRequest();

//            var response = await _employeeService.GetDeleteModelAsync(id);

//            if (response.IsSuccess)
//            {
//                return View(response.Result); // Passes DeleteEmployeeVM
//            }

//            return NotFound();
//        }

//        // Delete employee
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Delete(DeleteEmployeeVM model)
//        {
//            var response = await _employeeService.DeleteAsync(model);

//            if (response.IsSuccess)
//            {
//                TempData["SuccessMessage"] = "Employee has been deleted successfully";
//                return RedirectToAction(nameof(Index));
//            }

//            // If it failed, redisplay the confirmation page with an error
//            ModelState.AddModelError(string.Empty, response.ErrorMessage);
//            return View(model);
//        }


//    }
//}