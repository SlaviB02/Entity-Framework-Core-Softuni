using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new SoftUniContext();
            string str = GetEmployeesByFirstNameStartingWithSa(context);
            Console.WriteLine(str);
        }



        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var emp = context.Employees.Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary,
                e.EmployeeId
            }).OrderBy(e => e.EmployeeId).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var e in emp)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
            }
            return sb.ToString().Trim();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var emp = context.Employees.Select(e => new { e.FirstName, e.Salary }).Where(e => e.Salary > 50000).OrderBy(e => e.FirstName).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var e in emp)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }
            return sb.ToString().Trim();
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var emp = context.Employees.Select(e => new { e.FirstName, e.LastName, DepName = e.Department.Name, e.Salary }).Where(e => e.DepName == "Research and Development")
                .OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var e in emp)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepName} - ${e.Salary:F2}");
            }
            return sb.ToString().Trim();
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAddress = new Address() { AddressText = "Vitoshka 15", TownId = 4 };
            var emp = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");
            emp.Address = newAddress;
            context.SaveChanges();
            var orderedAddresses = context.Employees.OrderByDescending(e => e.AddressId).Take(10).Select(e => new { addText = e.Address.AddressText }).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var e in orderedAddresses)
            {
                sb.AppendLine(e.addText);
            }
            return sb.ToString().Trim();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var emp = context.Employees.Include(x=>x.EmployeesProjects).ThenInclude(x=>x.Project).Take(10).Select(e => new
            {
                e.FirstName,
                e.LastName,
                MFirstName = e.Manager.FirstName,
                MLastName = e.Manager.LastName,
                e.EmployeesProjects
       
        
            }).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var e in emp)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.MFirstName} {e.MLastName}");
                foreach (var pr in e.EmployeesProjects.Where(pr => pr.Project.StartDate.Year>= 2001 && pr.Project.StartDate.Year <= 2003))
                {

                    if (pr.Project.EndDate != null)
                    {
                        sb.AppendLine($"--{pr.Project.Name} - {pr.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - {pr.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt")}");
                    }
                    else
                    {
                        sb.AppendLine($"--{pr.Project.Name} - {pr.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - not finished");
                    }
                }
            }
            return sb.ToString().Trim();
        }
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses.Select(e=> new {eCount=e.Employees.Count, eTown=e.Town.Name,e.AddressText}).
                OrderByDescending(e=>e.eCount).
                ThenBy(e=>e.eTown).
                ThenBy(e=>e.AddressText).
                Take(10)
                .ToList();
            StringBuilder sb=new StringBuilder();
            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.eTown} - {address.eCount} employees");
            }
            return sb.ToString().Trim();
        }
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees.
                    Where(e => e.EmployeeId == 147)
                    .Select(e => new {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        Projects = e.EmployeesProjects
                            .Select(ep => ep.Project.Name)
                            .OrderBy(p => p)
                            .ToList()
                    })
                    .First();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            foreach(var pr in employee.Projects)
            { 
                sb.AppendLine(pr); 
            }
            return sb.ToString().Trim();
            
          
        }
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments=context.Departments
                .Where(de=>de.Employees.Count>5)
                .OrderBy(de=>de.Employees.Count)
                .ThenBy(de=>de.Name)
                .Select(de=>new
                {
                   de.Name,
                   de.Manager,
                   de.Employees,
                })
                .ToList();
            StringBuilder sb=new StringBuilder();
            foreach(var de in departments)
            {
                sb.AppendLine($"{de.Name} - {de.Manager.FirstName} {de.Manager.LastName}");
                foreach(var emp in de.Employees.Select(e=>new { e.FirstName, e.LastName, e.JobTitle }).OrderBy(e=>e.FirstName).ThenBy(e=>e.LastName))
                {
                    sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                }
            }
            return sb.ToString().Trim();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects=context.Projects.OrderByDescending(x => x.StartDate).Take(10).OrderBy(e=>e.Name).Select(pr=> new
            {
                pr.Name,
                pr.Description,
                pr.StartDate,
            }).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var project in projects)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
            }
            return sb.ToString().TrimEnd();
        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var emp=context.Employees
                .Where(e=>e.Department.Name=="Engineering" || e.Department.Name=="Tool Design" || e.Department.Name=="Marketing" || e.Department.Name=="Information Services")
                .ToList();
            foreach(var e in emp)
            {
                e.Salary *= 1.12m;
            }
             context.SaveChanges();
            var employees=emp.Select(e=>new
            {
                e.FirstName,
                e.LastName,
                e.Salary
            }).OrderBy(e=>e.FirstName).ThenBy(e=>e.LastName).ToList();
            StringBuilder sb=new StringBuilder();
            foreach (var em in employees)
            {
                sb.AppendLine($"{em.FirstName} {em.LastName} (${em.Salary:F2})");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var emp=context.Employees
                .Where(e=>e.FirstName.Substring(0,2)=="Sa")
                .Select(e=>new { e.FirstName, e.LastName,e.JobTitle, e.Salary }).OrderBy(e=>e.FirstName).ThenBy(e=>e.LastName).ToList();
            StringBuilder sb = new StringBuilder();
            foreach(var em in emp)
            {
                sb.AppendLine($"{em.FirstName} {em.LastName} - {em.JobTitle} - (${em.Salary:F2})");
            }
            return sb.ToString().TrimEnd();
        }
        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectToRemove = context.Projects.Find(2);
            var empProjectsToRemove = context.EmployeesProjects.Where(x=>x.ProjectId==2).ToList();
           
            foreach(var em in empProjectsToRemove)
            {
                context.EmployeesProjects.Remove(em);
            }
            context.Projects.Remove(projectToRemove);
            context.SaveChanges();
            var projects = context.Projects.Take(10).Select(pr=>pr.Name).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var project in projects)
            {
                sb.AppendLine(project);
            }
            return sb.ToString().TrimEnd();
        }
        public static string RemoveTown(SoftUniContext context)
        {
            var townToDelete = context.Towns.FirstOrDefault(t => t.Name == "Seattle");
            var addressesToDelete=context.Addresses.Where(a=>a.TownId == townToDelete.TownId).ToList();
            var countAddresses=addressesToDelete.Count();
            var employees=context.Employees.Where(e=>addressesToDelete.Contains(e.Address)).ToList();
            foreach(var employee in employees)
            {
                employee.AddressId = null;
            }
            foreach(var add in addressesToDelete)
            {
               context.Addresses.Remove(add);
            }
            context.Towns.Remove(townToDelete);
            context.SaveChanges();
            return $"{countAddresses} addresses in Seattle were deleted";
        }





    }
}