using HospitalProyect.Models;
using HospitalProyect.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalProyect.Controllers
{
    public class StaffController : Controller
    {
        private readonly StaffRepository _staffRepository;

        // Inyección de dependencias: El sistema nos da el repositorio automáticamente
        public StaffController(StaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
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
            return View();
        }

        // POST: Guardar el personal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StaffModel staff)
        {
            if (ModelState.IsValid)
            {
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

            return View(staff);
        }

        // POST: Guardar cambios de edición
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StaffModel staff)
        {
            if (ModelState.IsValid)
            {
                _staffRepository.Update(staff);
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        // POST: Eliminar
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _staffRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}