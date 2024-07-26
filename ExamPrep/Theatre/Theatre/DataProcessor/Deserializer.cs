namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ImportPlay>),new XmlRootAttribute("Plays"));

            var dtoPlays=(List<ImportPlay>)serializer.Deserialize(new StringReader(xmlString))!;

            var validPlays = new List<Play>();

            StringBuilder sb=new StringBuilder();

            foreach (var dto in dtoPlays)
            {

                string[] parts = dto.Duration.Split(":");

                string TempHours = parts[0];
                int hours = 0;

                if (TempHours[0]=='0')
                {
                    hours = int.Parse(TempHours[1].ToString());
                }
                else
                {
                    hours = int.Parse(TempHours);
                }

                if (!IsValid(dto) || hours<1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Play play = new Play
                {
                    Title = dto.Title,
                    Duration =TimeSpan.ParseExact(dto.Duration,"c",CultureInfo.InvariantCulture),
                    Rating =(float)dto.Rating,
                    Genre = Enum.Parse<Genre>(dto.Genre),
                    Description = dto.Description,
                    Screenwriter = dto.Screenwriter,
                };
                validPlays.Add(play);
                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre.ToString(), play.Rating));

            }

            context.Plays.AddRange(validPlays);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ImportCast>), new XmlRootAttribute("Casts"));

            var castDtos = (List<ImportCast>)serializer.Deserialize(new StringReader(xmlString))!;

            var validCasts = new List<Cast>();

            StringBuilder sb = new StringBuilder();

            foreach (var castdto in castDtos) 
            {
                  if(!IsValid(castdto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast
                {
                    FullName = castdto.FullName,
                    IsMainCharacter = castdto.IsMainCharacter,
                    PhoneNumber = castdto.PhoneNumber,
                    PlayId = castdto.PlayId,
                };

                validCasts.Add(cast);
                sb.AppendLine(string.Format(SuccessfulImportActor,cast.FullName,cast.IsMainCharacter==true?"main":"lesser"));
             }

            context.Casts.AddRange(validCasts);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var  theatreDtos=JsonConvert.DeserializeObject<List<ImportTheater>>(jsonString);

            var list = new List<Theatre>();

            StringBuilder sb = new StringBuilder();

            foreach(var dto in theatreDtos!)
            {
                if(! IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Theatre theatre = new Theatre
                {
                    Name = dto.Name,
                    NumberOfHalls = dto.NumberOfHalls,
                    Director = dto.Director,
                };

                foreach(var ticketDto in dto.Tickets)
                {
                    if(!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Ticket ticket = new Ticket
                    {
                        Price = ticketDto.Price,
                        RowNumber = ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId,
                    };
                    theatre.Tickets.Add(ticket);
                }

                list.Add(theatre);

                sb.AppendLine(string.Format(SuccessfulImportTheatre,theatre.Name,theatre.Tickets.Count()));
            }

            context.Theatres.AddRange(list);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
