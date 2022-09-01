
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context
                .Shells
                .Where(s => s.ShellWeight > shellWeight)
                .ToArray()
                .Select(s => new ExportShellDTO()
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns
                                .Select(g => new ExportShellGunDTO()
                                {
                                    GunType = g.GunType.ToString(),
                                    GunWeight = g.GunWeight,
                                    BarrelLength = g.BarrelLength,
                                    Range = g.Range > 3000 ? "Long-range" : "Regular range"
                                })
                                .Where(g => g.GunType == GunType.AntiAircraftGun.ToString())//така се парсва към string
                                .OrderByDescending(g => g.GunWeight)
                                .ToArray()
                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();

            return JsonConvert.SerializeObject(shells, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Guns");
            XmlSerializer serializer = new XmlSerializer(typeof(ExportGunDTO[]), xmlRoot);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            using StringWriter stringWriter = new StringWriter(sb);

            ExportGunDTO[] gunDTOs = context
                        .Guns
                        .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                        .OrderBy(g => g.BarrelLength)
                        .Select(g => new ExportGunDTO()
                        {
                            Manufacturer = g.Manufacturer.ManufacturerName,
                            GunType = g.GunType.ToString(),
                            BarrelLength = g.BarrelLength,
                            GunWeight = g.GunWeight,
                            Range = g.Range,
                            Countries = g.CountriesGuns
                                        .Where(c => c.Country.ArmySize > 4500000)
                                        .Select(c => new ExportCountryDTO()
                                        {
                                            CountryName = c.Country.CountryName,
                                            ArmySize = c.Country.ArmySize
                                        })
                                        .OrderBy(c => c.ArmySize)
                                        .ToArray()
                        })
                        .ToArray();

            serializer.Serialize(stringWriter, gunDTOs, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
