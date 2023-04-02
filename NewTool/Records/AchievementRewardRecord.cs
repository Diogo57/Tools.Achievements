using Stump.Core.IO;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewTool.Records
{
    public class AchievementRewardRelator
    {
        public static string FetchQuery = "SELECT * FROM achievements_rewards";
    }

    [TableName("achievements_rewards")]
    public class AchievementRewardRecord 
    {
        private List<uint> m_itemsReward;
        private List<uint> m_itemsQuantityReward;
        private uint[] m_emotesReward;
        private uint[] m_spellsReward;
        private uint[] m_titlesReward;
        private uint[] m_ornamentsReward;
        private string m_itemsRewardCSV;
        private string m_itemsQuantityRewardCSV;
        private string m_emotesRewardCSV;
        private string m_spellsRewardCSV;
        private string m_titlesRewardCSV;
        private string m_ornamentsRewardCSV;

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get;
            set;
        }
        public uint AchievementId
        {
            get;
            set;
        }

        public int LevelMin
        {
            get;
            set;
        }
        public int LevelMax
        {
            get;
            set;
        }
        [Ignore]
        public List<uint> ItemsReward
        {
            get
            {
                return this.m_itemsReward;
            }
            set
            {
                this.m_itemsReward = value;
                this.m_itemsRewardCSV = value.ToCSV(",");
            }
        }
        public string ItemsRewardCSV
        {
            get
            {
                return this.m_itemsRewardCSV;
            }
            set
            {
                this.m_itemsRewardCSV = value;
                this.m_itemsReward = value.FromCSV<uint>(",").ToList();
            }
        }
        [Ignore]
        public List<uint> ItemsQuantityReward
        {
            get
            {
                return this.m_itemsQuantityReward;
            }
            set
            {
                this.m_itemsQuantityReward = value;
                this.m_itemsQuantityRewardCSV = value.ToCSV(",");
            }
        }
        public string ItemsQuantityRewardCSV
        {
            get
            {
                return this.m_itemsQuantityRewardCSV;
            }
            set
            {
                this.m_itemsQuantityRewardCSV = value;
                this.m_itemsQuantityReward = value.FromCSV<uint>(",").ToList();
            }
        }
        [Ignore]
        public uint[] EmotesReward
        {
            get
            {
                return this.m_emotesReward;
            }
            set
            {
                this.m_emotesReward = value;
                this.m_emotesRewardCSV = value.ToCSV(",");
            }
        }
        public string EmotesRewardCSV
        {
            get
            {
                return this.m_emotesRewardCSV;
            }
            set
            {
                this.m_emotesRewardCSV = value;
                this.m_emotesReward = value.FromCSV<uint>(",");
            }
        }
        [Ignore]
        public uint[] SpellsReward
        {
            get
            {
                return this.m_spellsReward;
            }
            set
            {
                this.m_spellsReward = value;
                this.m_spellsRewardCSV = value.ToCSV(",");
            }
        }
        public string SpellsRewardCSV
        {
            get
            {
                return this.m_spellsRewardCSV;
            }
            set
            {
                this.m_spellsRewardCSV = value;
                this.m_spellsReward = value.FromCSV<uint>(",");
            }
        }
        [Ignore]
        public uint[] TitlesReward
        {
            get
            {
                return this.m_titlesReward;
            }
            set
            {
                this.m_titlesReward = value;
                this.m_titlesRewardCSV = value.ToCSV(",");
            }
        }
        public string TitlesRewardCSV
        {
            get
            {
                return this.m_titlesRewardCSV;
            }
            set
            {
                this.m_titlesRewardCSV = value;
                this.m_titlesReward = value.FromCSV<uint>(",");
            }
        }
        [Ignore]
        public uint[] OrnamentsReward
        {
            get
            {
                return this.m_ornamentsReward;
            }
            set
            {
                this.m_ornamentsReward = value;
                this.m_ornamentsRewardCSV = value.ToCSV(",");
            }
        }
        public string OrnamentsRewardCSV
        {
            get
            {
                return this.m_ornamentsRewardCSV;
            }
            set
            {
                this.m_ornamentsRewardCSV = value;
                this.m_ornamentsReward = value.FromCSV<uint>(",");
            }
        }

    }
}
