namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Countries");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCountryDTO[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            ImportCountryDTO[] countriesDTOs = (ImportCountryDTO[])xmlSerializer.Deserialize(sr);

            HashSet<Country> countries = new HashSet<Country>();

            foreach (var countryDto in countriesDTOs)
            {
                Country country = new Country()
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = countryDto.ArmySize
                };
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                countries.Add(country);
                sb.AppendLine(String.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }


            context.Countries.AddRange(countries);
            context.SaveChanges();


            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Manufacturers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportManufacturerDTO[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            ImportManufacturerDTO[] manufacturerDTOs = (ImportManufacturerDTO[])xmlSerializer.Deserialize(sr);

            HashSet<Manufacturer> manufacturers = new HashSet<Manufacturer>();

            var manufacturerNamesImported = new List<string>();

            foreach (var manufacturerDto in manufacturerDTOs)
            {
                if (!IsValid(manufacturerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = manufacturerDto.ManufacturerName,
                    Founded = manufacturerDto.Founded
                };

                if (manufacturerNamesImported.Contains(manufacturer.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                manufacturerNamesImported.Add(manufacturer.ManufacturerName);

                var townCountry = manufacturer.Founded.Split(", ").TakeLast(2).ToList();

                var townName = townCountry[0];

                var countryName = townCountry[1];

                manufacturers.Add(manufacturer);

                sb.AppendLine(String.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, $"{townName}, {countryName}"));
            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Shells");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportShellDTO[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            ImportShellDTO[] shellsDTO = (ImportShellDTO[])xmlSerializer.Deserialize(sr);

            HashSet<Shell> shells = new HashSet<Shell>();

            foreach (var shellDto in shellsDTO)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new Shell()
                {
                    ShellWeight = shellDto.ShellWeight,
                    Caliber = shellDto.Caliber
                };

                shells.Add(shell);

                sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            //Импортваме team-ове под формата на масив
            ImportGunsDTO[] gunsDTOs = JsonConvert.DeserializeObject<ImportGunsDTO[]>(jsonString);

            //var countriesWithValidId = context.Countries.Select(c => c.Id).ToHashSet();


            HashSet<Gun> guns = new HashSet<Gun>();

            foreach (var gunDTO in gunsDTOs)
            {

                if (!IsValid(gunDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var isValidGunType = Enum.TryParse<GunType>(gunDTO.GunType, out GunType gunType);

                if (!isValidGunType)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun currentGun = new Gun()
                {
                    ManufacturerId = gunDTO.ManufacturerId,
                    GunWeight = gunDTO.GunWeight,
                    BarrelLength = gunDTO.BarrelLength,
                    NumberBuild = gunDTO.NumberBuild,
                    Range = gunDTO.Range,
                    GunType = gunType,
                    ShellId = gunDTO.ShellId
                };

                foreach (var countryId in gunDTO.Countries.Select(c => c.Id).ToHashSet())
                {
                    /*                    CountryGun currentCountry = new CountryGun()
                                        {
                                            Gun = currentGun,
                                            CountryId = countryId
                                        };*/

                    currentGun.CountriesGuns.Add(new CountryGun
                    {
                        Gun = currentGun,
                        CountryId = countryId
                    });
                }

                /*                if (!IsValid(currentGun))
                                {
                                    sb.AppendLine(ErrorMessage);
                                    continue;
                                }*/

                guns.Add(currentGun);
                sb.AppendLine(String.Format(SuccessfulImportGun, currentGun.GunType, currentGun.GunWeight, currentGun.BarrelLength));
            }

            context.Guns.AddRange(guns);
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
