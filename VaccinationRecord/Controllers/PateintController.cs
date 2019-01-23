using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VaccinationRecord.Models;

namespace VaccinationRecord.Controllers
{
    public class PatientController : Controller
    {
        VaccinationContext db = new VaccinationContext();
        
        public ActionResult Index(string firstname, string lastname, string patronymic, string snils)
        {
            IQueryable<Patient> patients = db.Patients;
            if (!String.IsNullOrEmpty(firstname))
            {
                patients = patients.Where(p => p.Firstname.Contains(firstname));
            }
            if (!String.IsNullOrEmpty(lastname))
            {
                patients = patients.Where(p => p.Lastname.Contains(lastname));
            }
            if (!String.IsNullOrEmpty(patronymic))
            {
                patients = patients.Where(p => p.Patronymic.Contains(patronymic));
            }
            if (!String.IsNullOrEmpty(snils))
            {
                patients = patients.Where(p => p.SNILS.Contains(snils));
            }
            PatientsListViewModel viewModel = new PatientsListViewModel
            {
                Patients = patients.ToList(),
                Firstname = firstname,
                Lastname = lastname,
                Patronymic = patronymic,
                SNILS = snils
            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {           
            return View();
        }

        private void SnilsValidation(Patient patient) {
            string snils = CheckSnils(patient.SNILS);
            if (snils == "")
                ModelState.AddModelError("SNILS", "Не совпадает контрольная сумма");
            else
                patient.SNILS = snils;
        }

        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            SnilsValidation(patient);
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                Patient patient = db.Patients.Find(id);
                if (patient != null)
                {
                    var vaccnitions = db.Vaccinations
                        .Include(v => v.Drug)
                        .Where(v=>v.PatientId == patient.Id)
                        .ToList();
                    ViewBag.Vacctinations = vaccnitions;
                    return View(patient);
                }
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Patient patient = db.Patients.Find(id);
                if (patient != null)
                    return View(patient);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(Patient patient)
        {
            SnilsValidation(patient);
            if (ModelState.IsValid)
            {
                Patient p = db.Patients.Find(patient.Id);
                if (p != null)
                {
                    db.Entry(p).CurrentValues.SetValues(patient);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }
            return View(patient);           
        }


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Patient p = db.Patients.Find(id);
                if (p != null)
                    return View(p);
            }
            return HttpNotFound();
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id != null)
            {
                Patient p = db.Patients.Find(id);
                if (p != null)
                {
                    db.Patients.Remove(p);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return HttpNotFound();
        }

        private string CheckSnils(string Snils)
        {
            string pattern = @"\D";
            string target = "";
            Regex regex = new Regex(pattern);
            string snils = regex.Replace(Snils, target);
            string workSnils = snils.Substring(0, 9);
            string startWorkSnils = "001001998";
            //вычисления подходят для номеров больше 001-001-998
            if (String.Compare(workSnils, startWorkSnils) != 1)
                return snils;
            int contolSum = Int32.Parse(snils.Substring(9, 2));
            int workSum = 0;
            for (int i = 0; i < workSnils.Length; i++)
                workSum += Int32.Parse(workSnils[i].ToString()) * (9 - i);
            if (workSum > 101)
                workSum %= 101;
            if (workSum < 100 && workSum == contolSum)
                return snils;
            else if (contolSum == 0)
                return snils;
            return "";
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}