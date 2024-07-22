namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context.Coaches
                .Where(c => c.Footballers.Count() > 0)
                .ToArray()
                .Select(c => new CoachDtoExport
                {
                    CoachName = c.Name,
                    Footballers = c.Footballers
                    .Select(f => new FootballerDtoExport
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString()
                    })
                    .OrderBy(f => f.Name)
                    .ToArray(),
                    FootballersCount=c.Footballers.Count(),
                })
                .OrderByDescending(c=>c.FootballersCount)
                .ThenBy(c=>c.CoachName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CoachDtoExport>), new XmlRootAttribute("Coaches"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, coaches, namespaces);

            return sb.ToString().TrimEnd();

        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Select(t => t.Footballer).Any(f => f.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                    .Select(t => t.Footballer)
                    .Where(t => t.ContractStartDate >= date)
                     .OrderByDescending(f => f.ContractEndDate)
                    .ThenBy(f => f.Name)
                    .Select(f => new
                    {
                        FootballerName = f.Name,
                        ContractStartDate = f.ContractStartDate.ToString("d",CultureInfo.InvariantCulture),
                        ContractEndDate = f.ContractEndDate.ToString("d",CultureInfo.InvariantCulture),
                        BestSkillType = f.BestSkillType.ToString(),
                        PositionType = f.PositionType.ToString(),
                    })
                  
                    .ToList()

                })
                .OrderByDescending(t => t.Footballers.Count())
                .ThenBy(t => t.Name)
                .Take(5)
                .ToList();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
