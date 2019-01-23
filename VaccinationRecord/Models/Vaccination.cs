using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VaccinationRecord.Models
{
    public class Vaccination
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name ="Препарат")]
        [Required(ErrorMessage ="Обязательное поле")]
        public int DrugId { get; set; }
        public Drug Drug { get; set; }

        [UIHint("CheckBox")]
        [Display(Name = "Согласие на привику")]
        [Required(ErrorMessage = "Обязательное поле")]
        public bool Agreement { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата")]
        [Required(ErrorMessage = "Обязательное поле")]
        public DateTime Date { get; set; }

        [Display(Name = "Пациент")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}