namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;
    using Theatre.DataProcessor.ImportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres=context.Theatres
                .Where(t=>t.NumberOfHalls>=numbersOfHalls && t.Tickets.Count()>=20)
                .Select(t => new
                {
                    t.Name,
                    Halls=t.NumberOfHalls,
                    TotalIncome=t.Tickets
                    .Where(t=>t.RowNumber>=1 && t.RowNumber<=5)
                    .Sum(t=>t.Price),
                    Tickets=t.Tickets
                    .Where(t => t.RowNumber >= 1 && t.RowNumber <= 5)
                    .Select(e => new
                    {
                        e.Price,
                        e.RowNumber
                    })
                    .OrderByDescending(t => t.Price)
                    .ToList()
                })
                .OrderByDescending(t=>t.Halls)
                .ThenBy(t=>t.Name)
                .ToList();

            return JsonConvert.SerializeObject(theatres,Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            var plays = context.Plays
                .Where(p => p.Rating <= raiting)
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .Select(p => new PlayExport
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c", CultureInfo.InvariantCulture),
                    Rating =p.Rating==0?"Premier":p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts
                    .Where(c => c.IsMainCharacter == true)
                    .Select(c => new ActorExport
                    {
                        FullName = c.FullName,
                        MainCharacter = $"Plays main character in '{p.Title}'."
                    })
                    .OrderByDescending(c => c.FullName)
                    .ToArray()

                })
                .ToList();

            StringBuilder sb=new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<PlayExport>), new XmlRootAttribute("Plays"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, plays, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
