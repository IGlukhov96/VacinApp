using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VaccinationRecord.Models;

namespace VaccinationRecord.Controllers
{
    public class VaccinationController : Controller
    {
        VaccinationContext db = new VaccinationContext();

        public ActionResult Index()
        {
            var vaccnitions = db.Vaccinations
                .Include(v => v.Patient)
                .Include(v => v.Drug)
                .ToList();
            return View(vaccnitions);
        }

        private void InitViewBag()
        {
            var patients = db.Patients
              .Select(s => new
              {
                  Id = s.Id,
                  Description = !String.IsNullOrEmpty(s.Patronymic) ? s.Lastname + " " + s.Firstname + " " + s.Patronymic : s.Lastname + " " + s.Firstname
              })
             .ToList();
            ViewBag.Drugs = new SelectList(db.Drugs, "Id", "Name");
            ViewBag.Patients = new SelectList(patients, "Id", "Description");
        }

        [HttpGet]
        public ActionResult Create()
        {
            InitViewBag();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Vaccination vaccnition)
        {
            if (ModelState.IsValid)
            {
                Patient p = db.Patients.Find(vaccnition.PatientId);
                if (p != null)
                {
                    db.Vaccinations.Add(vaccnition);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }
            return View(vaccnition);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Vaccination vaccnition = db.Vaccinations.Find(id);
                if (vaccnition != null)
                {
                    InitViewBag();
                    return View(vaccnition);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(Vaccination vaccnition)
        {
            if (ModelState.IsValid)
            {
                Vaccination v = db.Vaccinations.Find(vaccnition.Id);
                Patient p = db.Patients.Find(vaccnition.PatientId);
                if (v != null && p != null)
                {
                    db.Entry(v).CurrentValues.SetValues(vaccnition);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }
            return View(vaccnition);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var vaccnitions = db.Vaccinations
                        .Include(v => v.Patient)
                        .Include(v => v.Drug)
                        .FirstOrDefault(v => v.Id == id);
                if (vaccnitions != null)
                {
                    return View(vaccnitions);
                }
            }
            return HttpNotFound();
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id != null)
            {
                Vaccination v = db.Vaccinations.Find(id);
                if (v != null)
                {
                    db.Vaccinations.Remove(v);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return HttpNotFound();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}

