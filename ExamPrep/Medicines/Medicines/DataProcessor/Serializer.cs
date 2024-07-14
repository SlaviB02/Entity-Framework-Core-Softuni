namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
           

            

            DateTime givenDate = DateTime.Parse(date,CultureInfo.InvariantCulture);

            var patients = context.Patients
                .Where(p => p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate >= givenDate))
              
                .Select(e => new ExportPatientDto
                {
                    Gender = e.Gender.ToString().ToLower(),
                    Name = e.FullName,
                    AgeGroup = e.AgeGroup.ToString(),
                    Medicines = e.PatientsMedicines
                    .Where(p=>p.Medicine.ProductionDate>=givenDate)
                    .Select(m => m.Medicine)
                     .OrderByDescending(m => m.ExpiryDate)
                    .ThenBy(m => m.Price)
                    .Select(m => new ExportMedicineDto
                    {
                        Category = m.Category.ToString().ToLower(),
                        Name = m.Name,
                        Price = m.Price.ToString("F2"),
                        Producer = m.Producer,
                        BestBefore = m.ExpiryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                    })
           
                    .ToArray()


                })
                .OrderByDescending(p => p.Medicines.Count())
                .ThenBy(p => p.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<ExportPatientDto>), new XmlRootAttribute("Patients"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, patients, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicines = context.Medicines
                .Where(c => c.Category == (Category)medicineCategory && c.Pharmacy.IsNonStop == true)
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .Select(e => new
                {
                    Name = e.Name,
                    Price = e.Price.ToString("F2"),
                    Pharmacy = new ExportPharmacy
                    {
                        Name = e.Pharmacy.Name,
                        PhoneNumber = e.Pharmacy.PhoneNumber
                    }

                })
                .ToList();

            return JsonConvert.SerializeObject(medicines,Formatting.Indented);
        }
    }
}
