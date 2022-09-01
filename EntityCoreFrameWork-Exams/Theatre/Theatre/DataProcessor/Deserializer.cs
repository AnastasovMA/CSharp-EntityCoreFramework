namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
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
            StringBuilder sb = new StringBuilder(); 

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Plays");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPlayDTO[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            ImportPlayDTO[] playDTOs = (ImportPlayDTO[])xmlSerializer.Deserialize(sr);

            HashSet<Play> plays = new HashSet<Play>();

            foreach (var playDTO in playDTOs)
            {
                var isEnumValid = Enum.TryParse<Genre>(playDTO.Genre, out Genre genreType);

                var isTimeSpanValid = TimeSpan.Parse(playDTO.Duration).TotalMinutes >= 60; // парсваме

                if (!IsValid(playDTO) || !isEnumValid || !isTimeSpanValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play currentPlay = new Play()
                {
                    Title = playDTO.Title,
                    Duration = TimeSpan.Parse(playDTO.Duration),
                    Rating = playDTO.Rating,
                    Genre = genreType,
                    Description = playDTO.Description,
                    Screenwriter = playDTO.Screnwriter
                };

                plays.Add(currentPlay);
                sb.AppendLine(String.Format(SuccessfulImportPlay, playDTO.Title, playDTO.Genre, playDTO.Rating));
            }

            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Casts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCastDTO[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            ImportCastDTO[] castDTOs = (ImportCastDTO[])xmlSerializer.Deserialize(sr);

            HashSet<Cast> casts = new HashSet<Cast>();

            foreach (var castDTO in castDTOs)
            {
                if (!IsValid(castDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                /*                var playId = context.Plays.Find(castDTO.PlayId);

                                if (playId == null)
                                {
                                    sb.AppendLine(ErrorMessage);
                                    continue;
                                }*/

                Cast currentCast = new Cast()
                {
                    FullName = castDTO.FullName,
                    PhoneNumber = castDTO.PhoneNumber,
                    IsMainCharacter = castDTO.IsMainCharacter,
                    PlayId = castDTO.PlayId
                };

                casts.Add(currentCast);

                sb.AppendLine(string.Format(SuccessfulImportActor, currentCast.FullName, currentCast.IsMainCharacter ? "main" : "lesser"));
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportProjectionDTO[] theatresDTOs = JsonConvert.DeserializeObject<ImportProjectionDTO[]>(jsonString);

            var existingPlaysId = context.Tickets.Select(p => p.PlayId).ToHashSet();

            HashSet<Theatre> theatres = new HashSet<Theatre>();

            foreach (var theatreDTO in theatresDTOs)
            {
                if (!IsValid(theatreDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                HashSet<Ticket> tickets = new HashSet<Ticket>();

                foreach (var ticketDTO in theatreDTO.Tickets)
                {
/*                    if (!existingPlaysId.Contains(ticketDTO.PlayId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }*/

                    if (!IsValid(ticketDTO))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket ticket = new Ticket()
                    {
                        Price = ticketDTO.Price,
                        RowNumber = ticketDTO.RowNumber,
                        PlayId = ticketDTO.PlayId
                    };

                    tickets.Add(ticket);
                }

                Theatre currentTheatre = new Theatre()
                {
                    Name = theatreDTO.Name,
                    NumberOfHalls = theatreDTO.NumberOfHalls,
                    Director = theatreDTO.Director,
                    Tickets = tickets
                };

                theatres.Add(currentTheatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, currentTheatre.Name, currentTheatre.Tickets.Count));
            }

            context.Theatres.AddRange(theatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        /*        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
                {
                    var theatreDtos = JsonConvert.DeserializeObject<ImportProjectionDTO[]>(jsonString);

                    var theatres = new List<Theatre>();

                    var sb = new StringBuilder();

                    foreach (var theatre in theatreDtos)
                    {
                        if (!IsValid(theatre))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        var tickets = new List<Ticket>();

                        foreach (var ticket in theatre.Tickets)
                        {
                            if (!IsValid(ticket))
                            {
                                sb.AppendLine(ErrorMessage);
                                continue;
                            }
                            tickets.Add(new Ticket
                            {
                                PlayId = ticket.PlayId,
                                Price = ticket.Price,
                                RowNumber = ticket.RowNumber
                            });
                        }

                        theatres.Add(new Theatre
                        {
                            Director = theatre.Director,
                            Name = theatre.Name,
                            NumberOfHalls = theatre.NumberOfHalls,
                            Tickets = tickets
                        });
                        sb.AppendLine(String.Format(SuccessfulImportTheatre, theatre.Name, tickets.Count));
                    }

                    context.AddRange(theatres);
                    context.SaveChanges();

                    return sb.ToString().TrimEnd();
                }*/

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
