//using Tiers.BLL.ModelVM.Employee;
//using Tiers.DAL.Entity;
//using Tiers.DAL.Repo.Abstraction;

//namespace Tiers.BLL.Service.Implementation
//{
//    public class EmployeeService : IEmployeeService
//    {
//        private readonly IEmployeeRepo _employeeRepo;
//        private readonly IDepartmentService _departmentService;
//        private readonly IMapper _mapper;

//        public EmployeeService(IEmployeeRepo employeeRepo, IDepartmentService departmentService, IMapper mapper)
//        {
//            _employeeRepo = employeeRepo;
//            _departmentService = departmentService;
//            _mapper = mapper;
//        }

//        public async Task<ResponseResult<IEnumerable<GetEmployeeVM>>> GetAllAsync()
//        {
//            try
//            {
//                var employees = await _employeeRepo.GetAllAsync(e => !e.IsDeleted, includes: e => e.Department);
//                var mappedEmployees = _mapper.Map<IEnumerable<GetEmployeeVM>>(employees);

//                return new ResponseResult<IEnumerable<GetEmployeeVM>>(mappedEmployees, null, true);
//            }
//            catch (Exception ex)
//            {
//                return new ResponseResult<IEnumerable<GetEmployeeVM>>(null, ex.Message, false);
//            }
//        }

//        public async Task<ResponseResult<GetEmployeeVM>> GetByIdAsync(int id)
//        {
//            try
//            {
//                var employee = await _employeeRepo.GetByIdAsync(id);
//                if (employee == null || employee.IsDeleted)
//                {
//                    return new ResponseResult<GetEmployeeVM>(null, "Employee not found.", false);
//                }

//                var mappedEmployee = _mapper.Map<GetEmployeeVM>(employee);
//                return new ResponseResult<GetEmployeeVM>(mappedEmployee, null, true);
//            }
//            catch (Exception ex)
//            {
//                return new ResponseResult<GetEmployeeVM>(null, ex.Message, false);
//            }
//        }

//        public async Task<ResponseResult<CreateEmployeeVM>> GetCreateModelAsync()
//        {
//            try
//            {
//                // 1- Get departments from department service
//                var result = await _departmentService.GetAllAsync();
//                var departments = result.Result;

//                // 2- Create the CreateEmployeeVM and populate the Departments property
//                // by mapping departments to SelectListItems for the dropdown
//                var model = new CreateEmployeeVM
//                {
//                    Departments = departments.Select(d => new SelectListItem
//                    {
//                        Value = d.Id.ToString(),
//                        Text = d.Name
//                    }).ToList()
//                };

//                return new ResponseResult<CreateEmployeeVM>(model, null, true);
//            }
//            catch (Exception ex)
//            {
//                return new ResponseResult<CreateEmployeeVM>(null, ex.Message, false);
//            }
//        }

//        public async Task<ResponseResult<bool>> CreateAsync(CreateEmployeeVM newEmployee)
//        {
//            try
//            {
//                // 1- Handle file upload
//                string? uploadedImageUrl = "default.png";
//                if (newEmployee.Image != null)
//                {
//                    try
//                    {
//                        uploadedImageUrl = await Upload.UploadFileAsync("Images", newEmployee.Image);
//                    }
//                    catch (Exception ex)
//                    {
//                        return new ResponseResult<bool>(false, $"File upload failed: {ex.Message}", false);
//                    }
//                }
//                newEmployee.ImageUrl = uploadedImageUrl;

//                // 2- Map VM -> Entity
//                var employee = _mapper.Map<Employee>(newEmployee);

//                // 3- Call the repo to add the new employee
//                var result = await _employeeRepo.AddAsync(employee);

//                //4- Return the response
//                if (result)
//                {
//                    return new ResponseResult<bool>(true, null, true);
//                }
//                return new ResponseResult<bool>(false, "Failed to save employee to the database.", false);
//            }
//            catch (Exception ex)
//            {
//                return new ResponseResult<bool>(false, ex.Message, false);
//            }
//        }

//        public async Task<ResponseResult<UpdateEmployeeVM>> GetUpdateModelAsync(int id)
//        {
//            try
//            {
//                // 1- Get the employee entity
//                var employee = await _employeeRepo.GetByIdAsync(id);
//                if (employee == null || employee.IsDeleted)
//                {
//                    return new ResponseResult<UpdateEmployeeVM>(null, "Employee not found.", false);
//                }

//                // 2. Map Entity -> VM
//                var mappedEmployee = _mapper.Map<UpdateEmployeeVM>(employee);

//                // 3. Get departments for the dropdown
//                var result = await _departmentService.GetAllAsync();
//                var departments = result.Result;

//                mappedEmployee.Departments = departments.Select(d => new SelectListItem
//                {
//                    Value = d.Id.ToString(),
//                    Text = d.Name
//                }).ToList();

//                return new ResponseResult<UpdateEmployeeVM>(mappedEmployee, null, true);
//            }
//            catch (Exception ex)
//            {
//                return new ResponseResult<UpdateEmployeeVM>(null, ex.Message, false);
//            }
//        }

//        public async Task<ResponseResult<bool>> UpdateAsync(UpdateEmployeeVM newEmployee)
//        {
//            try
//            {
//                // 1- Get the tracked entity from the repo
//                var oldEmployee = await _employeeRepo.GetByIdAsync(newEmployee.Id);
//                if (oldEmployee == null)
//                {
//                    return new ResponseResult<bool>(false, "Employee not found.", false);
//                }

//                // 2- Handle file upload
//                string? uploadedImageUrl = oldEmployee.ImageUrl;
//                if (newEmployee.Image != null)
//                {
//                    try
//                    {
//                        uploadedImageUrl = await Upload.UploadFileAsync("Images", newEmployee.Image); // Upload image to server
//                        if (!string.IsNullOrEmpty(oldEmployee.ImageUrl))
//                        {
//                            await Upload.RemoveFileAsync("Images", oldEmployee.ImageUrl); // Remove old image if exists
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        return new ResponseResult<bool>(false, $"File update failed: {ex.Message}", false);
//                    }
//                }
//                newEmployee.ImageUrl = uploadedImageUrl;

//                // 3- Map VM -> Entity
//                var employee = _mapper.Map<Employee>(newEmployee);

//                // 4- Call the repo to update the employee
//                var result = await _employeeRepo.UpdateAsync(employee); // Use the new repo method

//                return new ResponseResult<bool>(result, null, result);
//            }
//            catch (Exception ex)
//            {
//                return new ResponseResult<bool>(false, ex.Message, false);
//            }
//        }


//        public async Task<ResponseResult<DeleteEmployeeVM>> GetDeleteModelAsync(int id)
//        {
//            try
//            {
//                // 1- Get the employee entity
//                var employee = await _employeeRepo.GetByIdAsync(id);
//                if (employee == null || employee.IsDeleted)
//                {
//                    return new ResponseResult<DeleteEmployeeVM>(null, "Employee not found.", false);
//                }

//                // 2- Map Entity -> VM
//                var model = _mapper.Map<DeleteEmployeeVM>(employee);

//                return new ResponseResult<DeleteEmployeeVM>(model, null, true);
//            }
//            catch (Exception ex)
//            {
//                return new ResponseResult<DeleteEmployeeVM>(null, ex.Message, false);
//            }
//        }

//        public async Task<ResponseResult<bool>> DeleteAsync(DeleteEmployeeVM model)
//        {
//            try
//            {
//                // 1- Get the tracked entity
//                var employeeToDelete = await _employeeRepo.GetByIdAsync(model.Id);
//                if (employeeToDelete == null)
//                {
//                    return new ResponseResult<bool>(false, "Employee not found.", false);
//                }

//                // 2- Delete the employee using the repo
//                bool toggleResult = await _employeeRepo.ToggleDeleteStatusAsync(model.Id, model.DeletedBy);
//                if (!toggleResult)
//                {
//                    return new ResponseResult<bool>(false, "Failed to toggle delete status.", false);
//                }

//                // 3- Delete the image from disk (only if delete was successful)
//                if (toggleResult && !string.IsNullOrEmpty(employeeToDelete.ImageUrl))
//                {
//                    await Upload.RemoveFileAsync("Images", employeeToDelete.ImageUrl);
//                }

//                return new ResponseResult<bool>(toggleResult, null, toggleResult);
//            }
//            catch (Exception ex)
//            {
//                return new ResponseResult<bool>(false, ex.Message, false);
//            }
//        }

//    }
//}
