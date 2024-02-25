using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly IEmployeeRepository _employeeRepo;
        //private readonly IDepartmentRepository _departmentRepo;

        public EmployeeController(
            IUnitOfWork unitOfWork,
            IMapper mapper
            /*,IEmployeeRepository employeeRepo, IDepartmentRepository departmentRepo*/)// Ask CLR For Creating An Object From Class Implementing IEmployeeRepository
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_employeeRepo = employeeRepo;
            //_departmentRepo = departmentRepo;
        }

        // /Employee/Index
        public IActionResult Index(string searchInp)
        {

            //TempData.Keep();
            // Binding Through View's Dictionary : Transfer Data From Action To View [One Way]

            // 1. ViewData is a Dictionary Type Property (introduced in ASP.NET Framework 3.5)
            //            => It Helps Us To Transfer Data From Controller [Action] To View

            // ViewData["Message"] = "Hello ViewData";
            // 2. ViewBag is a Dynamic Type Property (introduced in ASP.NET Framework 4.0)
            //            => It Helps Us To Transfer Data From Controller [Action] To View
            //ViewBag.Message = "Hello ViewBag";

            var employees = Enumerable.Empty<Employee>();

            if (string.IsNullOrEmpty(searchInp))
                employees = _unitOfWork.EmployeeRepository.GetAll();
            else
                employees = _unitOfWork.EmployeeRepository.SearchByName(searchInp.ToLower());

            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmps);
        }

        // /Employee/Create
        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepo.GetAll();
            //ViewBag.Departments = _departmentRepo.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "images");
                // Manual Mapping

                ///var mappedEmp = new Employee()
                ///{
                ///    Name = employeeVM.Name,
                ///    Age = employeeVM.Age,
                ///    Address = employeeVM.Address,
                ///    Salary = employeeVM.Salary,
                ///    Email = employeeVM.Email,
                ///    PhoneNumber = employeeVM.PhoneNumber,
                ///    IsActive = employeeVM.IsActive,
                ///    HireDate = employeeVM.HireDate,
                ///};

                ///Employee mappedEmp = (Employee)employeeVM;

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.EmployeeRepository.Add(mappedEmp);

                var Count = _unitOfWork.Complete();

                // 2. Update

                // 3. Delete

                // dbContext.SaveChanges();
                /// 3. TempData is A Dictionary Type Property (introduced in ASP.NET Framework /3.5)
                ///            => Is Used To Pass Data Between Two Consecutive requests
                ///if (Count > 0)
                ///    TempData["Message"] = "Employee Created Successfuly";
                ///else
                ///    TempData["Message"] = "An Error Occurd, Employee Not Created";

                if (Count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }

        [HttpGet]
        // /Employee/Details/1
        // /Employee/Details
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound();

            return View(viewName, mappedEmp);
        }

        // /Employee/Edit/1
        // /Employee/Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //ViewBag.Departments = _departmentRepo.GetAll();
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    if (employeeVM.Image is not null)
                    {
                        employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "images");
                        DocumentSettings.DeleteFile(mappedEmp.ImageName, "images");
                        mappedEmp.ImageName = employeeVM.ImageName;
                    }




                    _unitOfWork.EmployeeRepository.Update(mappedEmp);
                    _unitOfWork.Complete();
                }
                catch (Exception ex)
                {
                    // 1. Log Exception
                    // 2. Friendly Message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }
        // /Employee/Delte/1
        // /Employee/Delete

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.EmployeeRepository.Delete(mappedEmp);
                var Count = _unitOfWork.Complete();
                if (Count > 0)
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "images");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }

        }
    }
}
