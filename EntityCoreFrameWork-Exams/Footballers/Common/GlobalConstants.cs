using System;
using System.Collections.Generic;
using System.Text;

namespace Footballers.Common
{
    public class GlobalConstants
    {
        //Footballer
        public const int FOOTBALLER_NAME_MINLENGTH = 2;
        public const int FOOTBALLER_NAME_MAXLENGTH = 40;

        //Team
        public const int TEAM_NAME_MINLEGTH = 3;
        public const int TEAM_NAME_MAXLENGTH = 40;
        public const int TEAM_NATIONALITY_MINLEGTH = 2;
        public const int TEAM_NATIONALITY_MAXLENGTH = 40;
        public const string TEAM_NAME_REGEX = @"[A-Za-z 0-9.-]+";
        public const int TEAM_TROPHIES_MINVALUE = 1;

        //Coach
        public const int COACH_NAME_MINLENGTH = 2;
        public const int COACH_NAME_MAXLENGTH = 40;

        //Best Skill type
        public const int BEST_SKILL_TYPE_MINLENGTH = 0;
        public const int BEST_SKILL_TYPE_MAXLENGTH = 4;

        public const int POSITION_TYPE_MINLENGTH = 0;
        public const int POSITION_TYPE_MAXLENGTH = 3;
    }
}
