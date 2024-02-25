using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo.PL.Controllers
{
    // Inheritance : DepartmentController is A Controller
    // Association(Composition) : DepartmentController Has A DepartmentRepository

    [Authorize]
    public class DepartmentController : Controller
    {
        
        //private readonly IDepartmentRepository _departmentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork,IMapper mapper
            /*IDepartmentRepository departmentRepo*/) 
        {
            //_departmentRepo = departmentRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // /Department/Index
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();

            var mappedDept = _mapper.Map<IEnumerable<Department>,IEnumerable<DepartmentViewModel>>(departments);
            
           

            return View(mappedDept);
        }

        // /Department/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            if(ModelState.IsValid) // Server Side Validation
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                _unitOfWork.DepartmentRepository.Add(mappedDept);

                var Count = _unitOfWork.Complete();

                if (Count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }

        [HttpGet]
        // /Department/Details/10
        // /Department/Details
        public IActionResult Details(int? id , string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest(); // 400

            var department = _unitOfWork.DepartmentRepository.Get(id.Value);

            var mappedDept = _mapper.Map<Department,DepartmentViewModel>(department);

            if (department is null)
                return NotFound(); // 404
                                   // 
            return View(viewName,mappedDept);

        }

        //[HttpGet]
        // /Department/Edit/10
        // /Department/Edit
        public IActionResult Edit(int? id)
        {
            ///if (!id.HasValue)
            ///    return BadRequest(); // 400
            ///var department = _departmentRepo.Get(id.Value);
            ///if (department is null)
            ///    return NotFound(); // 404                
            ///return View(department);

            return Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,DepartmentViewModel departmentVM)
        {

            if(id!= departmentVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {

                    var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                    _unitOfWork.DepartmentRepository.Update(mappedDept);
                    _unitOfWork.Complete();
                }
                catch (Exception ex)
                {

                    // 1. Log Exeption
                    // 2. Friendly Message

                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return RedirectToAction(nameof(Index));
                
            }

            return View(departmentVM);
        }

        // [HttpGet]
        // /Department/Delete/10 
        // /Department/Delete 
        public IActionResult Delete(int? id) 
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id,DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();
            try
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);


                _unitOfWork.DepartmentRepository.Delete(mappedDept);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                // 1. Log Exception 
                // 2. Friendly Message
                ModelState.AddModelError(string.Empty,ex.Message);
                return View(departmentVM);
            }
        }
    
    
    }


}
