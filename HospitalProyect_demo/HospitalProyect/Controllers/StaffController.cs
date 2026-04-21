using HospitalProyect.Models;
using HospitalProyect.Repositories;
using HospitalProyect.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalProyect.Controllers
{
	[Authorize]
	public class StaffController : Controller
	{
		private readonly StaffRepository _staffRepository;
		private readonly StaffCategoryRepository _staffCategoryRepository;
		private readonly SpecialtyRepository _specialtyRepository;
		private readonly IConfiguration _config;

		// Inyección de dependencias: El sistema nos da el repositorio automáticamente
		public StaffController(StaffRepository staffRepository, StaffCategoryRepository staffCategoryRepository, SpecialtyRepository specialtyRepository, IConfiguration configuration)
		{
			_staffRepository = staffRepository;
			_staffCategoryRepository = staffCategoryRepository;
			_specialtyRepository = specialtyRepository;
			_config = configuration;
		}

		// GET: Listar todo el personal
		public IActionResult Index()
		{
			var staffList = _staffRepository.GetAll();
			return View(staffList);
		}

		// GET: Formulario para crear
		public IActionResult Create()
		{
			var specialties = _specialtyRepository.GetAll();
			var staffCategories = _staffCategoryRepository.GetAll();

			ViewBag.Specialties = new SelectList(specialties, nameof(SpecialtyModel.Id), nameof(SpecialtyModel.Name));
			ViewBag.StaffCategories = new SelectList(staffCategories, nameof(StaffCategoryModel.Id), nameof(StaffCategoryModel.Name));


			return View();
		}

		// POST: Guardar el personal
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(StaffModel staff)
		{
			if (!ModelState.IsValid)
			{
				if(staff.PhotoFile != null && staff.PhotoFile.Length > 0)
				{
					string imageUrl = CloudinaryUtil.UploadImage(staff.PhotoFile, _config);
					staff.PhotoUrl = imageUrl;
				}

				_staffRepository.Add(staff);
				return RedirectToAction(nameof(Index));
			}
			return View(staff);
		}

		// GET: Editar
		public IActionResult Edit(int id)
		{
			var staff = _staffRepository.GetById(id);
			if (staff == null) return NotFound();

			var specialties = _specialtyRepository.GetAll();
			var staffCategories = _staffCategoryRepository.GetAll();

			ViewBag.Specialties = new SelectList(specialties, nameof(SpecialtyModel.Id), nameof(SpecialtyModel.Name), staff.SpecialtyId);
			ViewBag.StaffCategories = new SelectList(staffCategories, nameof(StaffCategoryModel.Id), nameof(StaffCategoryModel.Name), staff.StaffCategoryId);

			return View(staff);
		}

		// POST: Guardar cambios de edición
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(StaffModel staff)
		{
			if (!ModelState.IsValid)
			{
				if (staff.PhotoFile != null && staff.PhotoFile.Length > 0)
				{
					string newImageUrl = CloudinaryUtil.UploadImage(staff.PhotoFile, _config);
					staff.PhotoUrl = newImageUrl;
				}

				var specialties = _specialtyRepository.GetAll();
				var staffCategories = _staffCategoryRepository.GetAll();

				ViewBag.Specialties = new SelectList(specialties, nameof(SpecialtyModel.Id), nameof(SpecialtyModel.Name), staff.SpecialtyId);
				ViewBag.StaffCategories = new SelectList(staffCategories, nameof(StaffCategoryModel.Id), nameof(StaffCategoryModel.Name), staff.StaffCategoryId);

				_staffRepository.Update(staff);
				return RedirectToAction(nameof(Index));
			}
			return View(staff);
		}

		public IActionResult Delete(int id)
		{
			var staff = _staffRepository.GetById(id);
			if (staff == null) return NotFound();
			return View(staff);
		}

		// POST: Eliminar
		[HttpPost]
		public IActionResult Delete(StaffModel staffModel)
		{
			_staffRepository.Delete(staffModel.Id);
			return RedirectToAction(nameof(Index));
		}
	}
}
