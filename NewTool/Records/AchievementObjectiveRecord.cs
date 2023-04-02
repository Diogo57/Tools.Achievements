using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewTool.Records
{
    public class AchievementObjectiveRelator
    {
        public static string FetchQuery = "SELECT * FROM achievements_objectives";
    }

    [TableName("achievements_objectives")]
    public class AchievementObjectiveRecord 
    {
        [PrimaryKey("Id", false)]
        public uint Id { get; set; }

        public uint AchievementId { get; set; }

        public uint Order { get; set; }

        public uint NameId { get; set; }

        public string Criterion { get; set; }
    }
}
