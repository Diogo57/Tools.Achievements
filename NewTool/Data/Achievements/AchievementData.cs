using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.WorldServer.Database.Achievements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Achievements
{
    public class AchievementData
    {
        public AchievementData(NewTool.Records.AchievementTemplate record, Achievement d2o, List<ObjectiveData> data, List<RewardData> rewards)
        {
            Template = record;
            AchievementD2O = d2o;
            ObjectivesData = data;
            RewardsData = rewards;
        }

        public NewTool.Records.AchievementTemplate Template
        {
            get;
            set;
        }

        public Achievement AchievementD2O
        {
            get;
            set;
        }

        public List<ObjectiveData> ObjectivesData
        {
            get;
            set;
        }

        public List<RewardData> RewardsData
        {
            get;
            set;
        }

        public uint Id
        {
            get { return AchievementD2O.Id; }
            set
            {
                AchievementD2O.Id = value;
                Template.Id = value;

                foreach (var objective in ObjectivesData)
                    objective.AchievementId = value;

                foreach (var reward in RewardsData)
                    reward.AchievementId = value;
            }
        }

        public uint NameId
        {
            get { return AchievementD2O.NameId; }
            set
            {
                AchievementD2O.NameId = value;
                Template.NameId = value;
            }
        }

        public uint CategoryId
        {
            get { return AchievementD2O.CategoryId; }
            set
            {
                AchievementD2O.CategoryId = value;
                Template.CategoryId = value;
            }
        }

        public uint DescriptionId
        {
            get { return AchievementD2O.DescriptionId; }
            set
            {
                AchievementD2O.DescriptionId = value;
                Template.DescriptionId = value;
            }
        }

        public int IconId
        {
            get { return AchievementD2O.IconId; }
            set
            {
                AchievementD2O.IconId = value;
                Template.IconId = value;
            }
        }

        public uint Points
        {
            get { return AchievementD2O.Points; }
            set
            {
                AchievementD2O.Points = value;
                Template.Points = value;
            }
        }

        public uint Level
        {
            get { return AchievementD2O.Level; }
            set
            {
                AchievementD2O.Level = value;
                Template.Level = value;
            }
        }

        public uint Order
        {
            get => AchievementD2O.Order;
            set
            {
                AchievementD2O.Order = value;
                Template.Order = value;
            }
        }

        public List<int> Objectives
        {
            get => Template.ObjectiveIds.ToList();
            set
            {
                AchievementD2O.ObjectiveIds = value;
                Template.ObjectiveIds = value.ToArray();
            }
        }

        public List<int> RewardsId
        {
            get => Template.RewardIds.ToList();
            set
            {
                AchievementD2O.RewardIds = value;
                Template.RewardIds = value.ToArray();
            }
        }

        public string[] GetObjectiveId()
            => ObjectivesData.Select(x => x.Id.ToString()).ToArray();

        public string[] GetRewardsIds()
            => RewardsData.Select(x => x.Id.ToString()).ToArray();
    }

    public class RewardData
    {
        public RewardData(NewTool.Records.AchievementRewardRecord record, AchievementReward d2o)
        {
            Template = record;
            RewardD2O = d2o;
        }

        public NewTool.Records.AchievementRewardRecord Template
        {
            get;
            set;
        }

        public AchievementReward RewardD2O
        {
            get;
            set;
        }

        public uint Id
        {
            get => RewardD2O.Id;
            set
            {
                RewardD2O.Id = value;
                Template.Id = value;
            }
        }
        public uint AchievementId
        {
            get => RewardD2O.AchievementId;
            set
            {
                RewardD2O.AchievementId = value;
                Template.AchievementId = value;
            }
        }

        public List<uint> ItemsQuantities
        {
            get => RewardD2O.ItemsQuantityReward;
            set
            {
                RewardD2O.ItemsQuantityReward = value;
                Template.ItemsQuantityReward = value.ToList();
            }
        }
        public List<uint> ItemsReward
        {
            get => RewardD2O.ItemsReward;
            set
            {
                RewardD2O.ItemsReward = value;
                Template.ItemsReward = value.ToList();
            }
        }

        public List<uint> EmotesReward
        {
            get => RewardD2O.EmotesReward;
            set
            {
                RewardD2O.EmotesReward = value;
                Template.EmotesReward = value.ToArray();
            }
        }

        public List<uint> SpellsRewards
        {
            get => RewardD2O.SpellsReward;
            set
            {
                RewardD2O.SpellsReward = value;
                Template.SpellsReward = value.ToArray();
            }
        }

        public List<uint> TitlesReward
        {
            get => RewardD2O.TitlesReward;
            set
            {
                RewardD2O.TitlesReward = value;
                Template.TitlesReward = value.ToArray();
            }
        }
        public List<uint> OrnamentsReward
        {
            get => RewardD2O.OrnamentsReward;
            set
            {
                RewardD2O.OrnamentsReward = value;
                Template.OrnamentsReward = value.ToArray();
            }
        }
    }

    public class ObjectiveData
    {
        public ObjectiveData(NewTool.Records.AchievementObjectiveRecord record, AchievementObjective d2o)
        {
            Template = record;
            ObjectiveD2O = d2o;
        }

        public NewTool.Records.AchievementObjectiveRecord Template
        {
            get;
            set;
        }

        public AchievementObjective ObjectiveD2O
        {
            get;
            set;
        }

        public uint Id
        {
            get => ObjectiveD2O.Id;
            set
            {
                ObjectiveD2O.Id = value;
                Template.Id = value;
            }
        }

        public uint AchievementId
        {
            get => ObjectiveD2O.AchievementId;
            set
            {
                ObjectiveD2O.AchievementId = value;
                Template.AchievementId = value;
            }
        }

        public uint NameId
        {
            get => ObjectiveD2O.NameId;
            set
            {
                ObjectiveD2O.NameId = value;
                Template.NameId = value;
            }
        }

        public uint Order
        {
            get => ObjectiveD2O.Order;
            set
            {
                ObjectiveD2O.Order = value;
                Template.Order = value;
            }
        }

        public string Criterion
        {
            get => ObjectiveD2O.Criterion;
            set
            {
                ObjectiveD2O.Criterion = value;
                Template.Criterion = value;
            }
        }
    }

    public class CategoryData
    {
        public CategoryData(NewTool.Records.AchievementCategoryRecord template, AchievementCategory d2o)
        {
            Template = template;
            CategoryD2O = d2o;
        }
        public NewTool.Records.AchievementCategoryRecord Template
        {
            get;
            set;
        }

        public AchievementCategory CategoryD2O
        {
            get;
            set;
        }

        public uint Id
        {
            get => CategoryD2O.Id;
            set
            {
                CategoryD2O.Id = value;
                Template.Id = value;
            }
        }

        public uint NameId
        {
            get => CategoryD2O.NameId;
            set
            {
                CategoryD2O.NameId = value;
                Template.NameId = value;
            }
        }

        public uint ParentId
        {
            get => CategoryD2O.ParentId;
            set
            {
                CategoryD2O.ParentId = value;
                Template.ParentId = value;
            }
        }

        public uint Order
        {
            get => CategoryD2O.Order;
            set
            {
                CategoryD2O.Order = value;
                Template.Order = value;
            }
        }

        public string Icon
        {
            get => CategoryD2O.Icon;
            set
            {
                CategoryD2O.Icon = value;
                Template.Icon = value;
            }
        }

        public string Color
        {
            get => CategoryD2O.Color;
            set
            {
                CategoryD2O.Color = value;
                Template.Color = value; 
            }
        }

        public List<uint> AchievementsId
        {
            get => Template.AchievementsIds.ToList();
            set
            {
                CategoryD2O.AchievementIds = value;
                Template.AchievementsIds = value.ToArray();
            }
        }
    }
}
