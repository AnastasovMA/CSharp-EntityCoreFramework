using System;
using System.Collections.Generic;
using System.Text;

namespace Artillery.Common
{
    public class GlobalConstants
    {
        //Country
        public const int COUNTRY_MIN_LENGTH = 4;
        public const int COUNTRY_MAX_LENGTH = 60;
        public const int COUNTRY_ARMYSIZE_MINLENGTH = 50000;
        public const int COUNTRY_ARMYSIZE_MAXLENGTH = 10000000;

        //Manufacturer
        public const int MANUFACURER_NAME_MINLENGTH = 4;
        public const int MANUFACTURER_NAME_MAX_LENGTH = 40;
        public const int MANUFACTURER_FOUNDED_MIN_LENGTH = 10;
        public const int MANUFACTURER_FOUNDED_MAX_LENGTH = 100;

        //Shell
        public const double SHELL_WEIGHT_MINLENGTH = 2;
        public const double SHELL_WEIGHT_MAXLENGTH = 1680;
        public const int SHELL_CALIBER_MINLEGTH = 4;
        public const int SHELL_CALIBER_MAXLENGTH = 30;

        //Gun
        public const int GUN_GUNWEIGHT_MINLENGTH = 100;
        public const int GUN_GUNWEIGHT_MAXLENGTH = 1350000;
        public const double GUN_BARRELLENGTH_MINLENGTH = 2;
        public const double GUN_BARRELLENGTH_MAXLENGTH = 35;
        public const int GUN_RANGE_MINLENGTH = 1;
        public const int GUN_RANGE_MAXLENGTH = 100000;

        public const int GUN_GUNTYPE_MINVALUE = 0;
        public const int GUN_GUNTYPE_MAXVALUE = 5;

    }
}
