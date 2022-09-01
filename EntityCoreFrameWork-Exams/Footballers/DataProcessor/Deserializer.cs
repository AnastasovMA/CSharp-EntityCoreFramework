namespace Footballers.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enum;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Coaches");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCoachesDTO[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString); // прочитаме xml-а като стринг

            ImportCoachesDTO[] coachesDTOs = (ImportCoachesDTO[])xmlSerializer.Deserialize(sr);  //десериализираме xml-а в DTO

            HashSet<Coach> coaches = new HashSet<Coach>(); // създаваме си един лист от треньори, в който ще пълним информацията от DTO-то

            foreach (var coachDto in coachesDTOs)
            {
                //Почваме да проверяваме дали подадената ни информация за DTO-то е валидна
                if (!IsValid(coachDto))
                {
                    //Ако е невалидна, подаваме error message и продължваме със следващия
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality
                };

                HashSet<Footballer> footballers = new HashSet<Footballer>();

                foreach (var footballerDTO in coachDto.Footballers)
                {
                    if (!IsValid(footballerDTO))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    //По този начин проверяваме дали контракта е с валидна дата и вадим отговора като footballerStartDate
                    bool isContractStartDateValid = DateTime.TryParseExact(footballerDTO.ContractStartDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime footballerStartDate);

                    if (!isContractStartDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isContractEndDateValid = DateTime.TryParseExact(footballerDTO.ContractEndDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime footballerEndDate);

                    if (!isContractEndDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (footballerEndDate < footballerStartDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer f = new Footballer()
                    {
                        Name = footballerDTO.Name,
                        ContractStartDate = footballerStartDate,
                        ContractEndDate = footballerEndDate,
                        BestSkillType = (BestSkillType)footballerDTO.BestSkillType,
                        PositionType = (PositionType)footballerDTO.PositionType
                    };

                    footballers.Add(f);
                }

                coach.Footballers = footballers;
                coaches.Add(coach);

                sb.AppendLine(String.Format(SuccessfullyImportedCoach, coach.Name, footballers.Count));
            }

            context.Coaches.AddRange(coaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            //Импортваме team-ове под формата на масив
            ImportTeamDTO[] teamDTOS = JsonConvert.DeserializeObject<ImportTeamDTO[]>(jsonString);

            var existingFootballersIds = context.Footballers.Select(f => f.Id).ToHashSet();

            HashSet<Team> teams = new HashSet<Team>();

            foreach (var teamDTO in teamDTOS)
            {
                if (!IsValid(teamDTO) || teamDTO.Trophies <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team currentTeam = new Team()
                {
                    Name = teamDTO.Name,
                    Nationality = teamDTO.Nationality,
                    Trophies = teamDTO.Trophies
                };

                HashSet<TeamFootballer> teamFootballers = new HashSet<TeamFootballer>();
                //Проверяваме дали id-to на футболиста в teamDTO е валидно и дали го има в нашите футболисти
                foreach (var footballerId in teamDTO.Footballers.Distinct())
                {
                    Footballer footballer = context.Footballers.Find(footballerId);

                    if (footballer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    //Създваме запис на teamFootballer с футболиста и неговото Id

                    TeamFootballer currentTeamFootballer = new TeamFootballer()
                    {
                        FootballerId = footballerId,
                        Team = currentTeam
                    };

                    //Добавяме в списъка футболиста с Id-то му
                    //Записваме списъка в базата с данни в teamFootballers

                    //teamFootballers.Add(currentTeamFootballer);
                    currentTeam.TeamsFootballers.Add(currentTeamFootballer);
                }
                teams.Add(currentTeam);

                sb.AppendLine(String.Format(SuccessfullyImportedTeam, currentTeam.Name, currentTeam.TeamsFootballers.Count()));
            }

            context.Teams.AddRange(teams);
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
