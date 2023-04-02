using Stump.Core.IO;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewTool.Records
{
    public class AchievementTemplateRelator
    {
        public static string FetchQuery = "SELECT * FROM achievements_templates";
    }

    [TableName("achievements_templates")]
    public class AchievementTemplate 
    {
        private const double REWARD_SCALE_CAP = 1.5;
        private const double REWARD_REDUCED_SCALE = 0.7;

        private int[] m_objectiveIds = new int[0];
        private string m_objectiveIdsCSV;
        private int[] m_rewardIds = new int[0];
        private string m_rewardIdsCSV;

        [PrimaryKey("Id", false)]
        public uint Id { get; set; }

        public uint NameId { get; set; }

        public uint CategoryId { get; set; }

        public uint DescriptionId { get; set; }

        public int IconId { get; set; }

        public uint Points { get; set; }

        public uint Level { get; set; }

        public uint Order { get; set; }

        public float KamasRatio { get; set; }

        public float ExperienceRatio { get; set; }

        public bool KamasScaleWithPlayerLevel { get; set; }

        [Ignore]
        public int[] ObjectiveIds
        {
            get { return m_objectiveIds; }
            set
            {
                m_objectiveIds = value;
                m_objectiveIdsCSV = value.ToCSV(",");
            }
        }

        [Ignore]
        public AchievementObjectiveRecord[] Objectives { get; set; }

        public string ObjectiveIdsCSV
        {
            get { return m_objectiveIdsCSV; }
            set
            {
                m_objectiveIdsCSV = value;
                m_objectiveIds = value.FromCSV<int>(",");
            }
        }

        [Ignore]
        public int[] RewardIds
        {
            get { return m_rewardIds; }
            set
            {
                m_rewardIds = value;
                m_rewardIdsCSV = value.ToCSV(",");
            }
        }

        [Ignore]
        public AchievementRewardRecord[] Rewards { get; private set; }

        public string RewardIdsCSV
        {
            get { return m_rewardIdsCSV; }
            set
            {
                m_rewardIdsCSV = value;
                m_rewardIds = value.FromCSV<int>(",");
            }
        }

    }
}
