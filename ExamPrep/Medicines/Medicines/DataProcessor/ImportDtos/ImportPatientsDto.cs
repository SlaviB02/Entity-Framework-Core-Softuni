using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientsDto
    {
        [MinLength(5)]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;
        [EnumDataType(typeof(AgeGroup))]
        public string AgeGroup {  get; set; } = null!;

        [EnumDataType(typeof(Gender))]

        public string Gender { get; set; } = null!;

        public int[]Medicines { get; set; }=null!;

    }
}
