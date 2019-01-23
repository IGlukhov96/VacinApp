using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VaccinationRecord.Models
{

    public class Patient
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [RegularExpression(@"[А-яЁё -]+", ErrorMessage = "Неверный формат")]
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Firstname { get; set; }

        [RegularExpression(@"[А-яЁё -]+", ErrorMessage = "Неверный формат")]
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Lastname { get; set; }

        [RegularExpression(@"[А-яЁё -]+", ErrorMessage = "Неверный формат")]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата рождения")]
        [Required(ErrorMessage = "Обязательное поле")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Пол")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Gender { get; set; }

        [RegularExpression(@"[0-9]{3}[ -]?[0-9]{3}[ -]?[0-9]{3}\s?[0-9]{2}.?", ErrorMessage = "Неверный формат")]
        [Display(Name = "СНИЛС")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string SNILS { get; set; }

        
        
        //public ICollection<Vaccination> Vaccinations { get; set; }

        //public Patient()
        //{
        //    Vaccinations = new List<Vaccination>();
        //}
    }

    public class PatientsListViewModel
    {
        public IEnumerable<Patient> Patients { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Patronymic { get; set; }
        public string SNILS { get; set; }
    }
}