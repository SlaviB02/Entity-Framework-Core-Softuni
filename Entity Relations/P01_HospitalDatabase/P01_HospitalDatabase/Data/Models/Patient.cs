﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {

        public Patient()
        {
            Visitations=new List<Visitation>();
            Diagnoses=new List<Diagnose>();
            Prescriptions=new List<PatientMedicament>();
        }

        [Key]
        public int PatientId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        [Required]
        [Column(TypeName = "varchar(80)")]

        public string Email {  get; set; }

        [Required]
        public bool HasInsurance {  get; set; }

        public ICollection<Visitation> Visitations { get; set; }

        public ICollection<Diagnose> Diagnoses { get; set; }

        public ICollection<PatientMedicament> Prescriptions { get; set; }

    }
}
