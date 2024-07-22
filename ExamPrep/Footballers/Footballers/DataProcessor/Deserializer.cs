namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Text.Json;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            XmlSerializer serializer=new XmlSerializer(typeof(List<CoachDtoImport>),new XmlRootAttribute("Coaches"));

            var coachesDtos=(List<CoachDtoImport>)serializer.Deserialize(new StringReader(xmlString));

            var validCoaches = new List<Coach>();

            StringBuilder sb=new StringBuilder();

            foreach(var coachDto in coachesDtos)
            {
                if(!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality,
                };

                foreach(var footballerDto in coachDto.Footballers)
                {
                    if(!IsValid(footballerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime StartDate= DateTime
                        .ParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    DateTime EndDate = DateTime
                        .ParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if(StartDate > EndDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballer = new Footballer
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = StartDate,
                        ContractEndDate = EndDate,
                        BestSkillType = Enum.Parse<BestSkillType>(footballerDto.BestSkillType),
                        PositionType = Enum.Parse<PositionType>(footballerDto.PositionType)
                    };

                    coach.Footballers.Add(footballer);
                }

                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name,coach.Footballers.Count()));
                validCoaches.Add(coach);
            }

            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var teamDtos=JsonConvert.DeserializeObject<List<TeamDtoImport>>(jsonString);

            var validTeams=new List<Team>();

            StringBuilder sb=new StringBuilder();

            foreach(var teamDto in teamDtos)
            {
                if(!IsValid(teamDto) || teamDto.Trophies<=0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = new Team
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = teamDto.Trophies,
                };

                foreach(var footballerId in  teamDto.Footballers.Distinct())
                {
                    if(!context.Footballers.Any(f=>f.Id==footballerId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    TeamFootballer teamFootballer = new TeamFootballer
                    {
                        Team = team,
                        FootballerId = footballerId,
                    };
                    team.TeamsFootballers.Add(teamFootballer);
                }

                sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name,team.TeamsFootballers.Count()));
                validTeams.Add(team);
            }

            context.Teams.AddRange(validTeams);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
