using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.Server.WorldServer.Database.Achievements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Achievements.Manager
{
    public static class AchievementManager
    {
        static List<Achievement> m_achievements = new List<Achievement>();
        static List<AchievementObjective> m_achievementObjectives = new List<AchievementObjective>();
        static List<AchievementReward> m_achievementsReward = new List<AchievementReward>();
        static List<AchievementCategory> m_categories = new List<AchievementCategory>();

        static List<NewTool.Records.AchievementTemplate> m_achievementsTemplate = new List<NewTool.Records.AchievementTemplate>();
        static List<NewTool.Records.AchievementObjectiveRecord> m_objectivesTemplate = new List<NewTool.Records.AchievementObjectiveRecord>();
        static List<NewTool.Records.AchievementRewardRecord> m_rewardsTemplate = new List<NewTool.Records.AchievementRewardRecord>();
        static List<NewTool.Records.AchievementCategoryRecord> m_categoryRecord = new List<NewTool.Records.AchievementCategoryRecord>();

        static List<AchievementData> m_achievementsData = new List<AchievementData>();
        static List<ObjectiveData> m_objectivesData = new List<ObjectiveData>();
        static  List<RewardData> m_rewardData = new List<RewardData>();
        static  List<CategoryData> m_categoryData = new List<CategoryData>();
        public static void Load()
        {
            m_achievements = DataManager.GetAll<Achievement>();
            m_achievementObjectives = DataManager.GetAll<AchievementObjective>();
            m_achievementsReward = DataManager.GetAll<AchievementReward>();
            m_categories = DataManager.GetAll<AchievementCategory>();

            m_achievementsTemplate = DatabaseManager.Database.Query<NewTool.Records.AchievementTemplate>(NewTool.Records.AchievementTemplateRelator.FetchQuery).ToList();
            m_objectivesTemplate = DatabaseManager.Database.Query<NewTool.Records.AchievementObjectiveRecord>(NewTool.Records.AchievementObjectiveRelator.FetchQuery).ToList();
            m_rewardsTemplate = DatabaseManager.Database.Query<NewTool.Records.AchievementRewardRecord>(NewTool.Records.AchievementRewardRelator.FetchQuery).ToList();
            m_categoryRecord = DatabaseManager.Database.Query<NewTool.Records.AchievementCategoryRecord>(NewTool.Records.AchievementCategoryRelator.FetchQuery).ToList();

            foreach (var reward in m_achievementsReward)
                m_rewardData.Add(new RewardData(m_rewardsTemplate.FirstOrDefault(x => x.Id == reward.Id), reward));

            foreach (var objective in m_achievementObjectives)
                m_objectivesData.Add(new ObjectiveData(m_objectivesTemplate.FirstOrDefault(x => x.Id == objective.Id), objective));
            
            foreach (var achievement in m_achievements)
                m_achievementsData.Add(new AchievementData(m_achievementsTemplate.FirstOrDefault(x => x.Id == achievement.Id), achievement, GetObjectiveDatas(achievement.objectiveIds), GetRewardDatas(achievement.rewardIds)));
            
            foreach (var category in m_categories)
                m_categoryData.Add(new CategoryData(m_categoryRecord.FirstOrDefault(x => x.Id == category.Id), category));
        }

        public static string[] GetAchievementName()
            => m_achievementsData.Select(x => TextManager.GetText(x.NameId)).ToArray();

        public static string[] GetAchievementName(string text)
            => m_achievementsData.Where(x => TextManager.GetText(x.NameId).ToLower().Contains(text)).Select(a => TextManager.GetText(a.NameId)).ToArray();

        public static string[] GetCategoriesName()
            => m_categoryData.Select(x => TextManager.GetText(x.NameId)).ToArray();

        public static string[] GetCategoriesName(string text)
          => m_categoryData.Where(x => TextManager.GetText(x.NameId).ToLower().Contains(text)).Select(a => TextManager.GetText(a.NameId)).ToArray();

        public static AchievementData GetDataByName(string name)
            => m_achievementsData.FirstOrDefault(x => TextManager.GetText(x.Template.NameId) == name);

        public static AchievementData GetFirstData()
            => m_achievementsData.FirstOrDefault();

        public static ObjectiveData GetObjectiveData(int objectiveId) // sa met tjr l'auto increment, 
            => m_objectivesData.FirstOrDefault(x => x.ObjectiveD2O.Id == objectiveId);

        public static CategoryData GetCategoryData(string categoryName)
            => m_categoryData.FirstOrDefault(x => TextManager.GetText(x.CategoryD2O.NameId) == categoryName);

        public static  CategoryData GetCategoryData(int categoryId)
            => m_categoryData.FirstOrDefault(x => x.CategoryD2O.id == categoryId);

        public static CategoryData GetFirstCategory()
            => m_categoryData.FirstOrDefault();

        public static RewardData GetRewardData(int rewardId)
            => m_rewardData.FirstOrDefault(x => x.Id == rewardId);

        public static List<ObjectiveData> GetObjectiveDatas(List<int> objectivesId)
            => m_objectivesData.Where(x => objectivesId.Contains((int)x.ObjectiveD2O.Id)).ToList();

        public static List<RewardData> GetRewardDatas(List<int> rewardsId)
            => m_rewardData.Where(x => rewardsId.Contains((int)x.RewardD2O.Id)).ToList();

        public static uint GetCategoryNameId(int categoryId)
            => m_categoryData.FirstOrDefault(x => x.Id == categoryId).NameId;

        public static List<uint> GetAchievementByCategory(uint categoryId)
            => m_achievementsData.Where(x => x.CategoryId == categoryId).Select(a => a.Id).ToList();

        public static List<AchievementData> GetDataByCategory(uint categoryId)
            => m_achievementsData.Where(x => x.CategoryId == categoryId).ToList();

        public static void AddAchievementData(AchievementData data)
            => m_achievementsData.Add(data);

        public static void AddObjectiveData(ObjectiveData data)
           => m_objectivesData.Add(data);

        public static void AddCategoryData(CategoryData data)
            => m_categoryData.Add(data);

        public static void AddRewardData(RewardData data)
            => m_rewardData.Add(data);

        public static void DeleteAchievement(AchievementData data)
        {
            m_achievementsData.Remove(data);
            m_categoryData.FirstOrDefault(x => x.Id == data.CategoryId)?.AchievementsId.Remove(data.Id);

            foreach (var objective in data.ObjectivesData)
                m_objectivesData.Remove(objective);

            foreach (var reward in data.RewardsData)
                m_rewardData.Remove(reward);
        }

        public static void DeleteObjective(AchievementData dataAchie, ObjectiveData objectiveData)
        {
            m_objectivesData.Remove(objectiveData);
            dataAchie.ObjectivesData.Remove(objectiveData);
            dataAchie.Objectives = dataAchie.GetObjectiveId().Select(x => int.Parse(x)).ToList();
        }

        public static void DeleteReward(AchievementData data, RewardData rewardData)
        {
            m_rewardData.Remove(rewardData);
            data.RewardsData.Remove(rewardData);
            data.RewardsId = data.GetRewardsIds().Select(x => int.Parse(x)).ToList();
        }

        public static void DeleteCategory(CategoryData data)
        {
          
        }

        public static void Save()
        {
            DatabaseManager.Database.Execute("TRUNCATE TABLE achievements_templates");
            DatabaseManager.Database.Execute("TRUNCATE TABLE achievements_rewards");
            DatabaseManager.Database.Execute("TRUNCATE TABLE achievements_objectives");
            DatabaseManager.Database.Execute("TRUNCATE TABLE achievements_categories");
            D2OWriter writerAchievement = new D2OWriter(@".\common\Achievements.d2o", true);
            D2OWriter writerObjective = new D2OWriter(@".\common\AchievementObjectives.d2o", true);
            D2OWriter writerReward = new D2OWriter(@".\common\AchievementRewards.d2o", true);
            D2OWriter writerCategory = new D2OWriter(@".\common\AchievementCategories.d2o", true);


            writerAchievement.StartWriting(true);
            foreach (var achievement in m_achievementsData)
            {
                DatabaseManager.Database.Insert(achievement.Template);
                writerAchievement.Write(achievement.AchievementD2O, (int)achievement.Id);
            }
            writerAchievement.EndWriting();

            writerObjective.StartWriting(true);
            foreach (var objective in m_objectivesData)
            {
                DatabaseManager.Database.Insert(objective.Template);
                writerObjective.Write(objective.ObjectiveD2O, (int)objective.Id);
            }
            writerObjective.EndWriting();

            writerReward.StartWriting(true);
            foreach (var reward in m_rewardData)
            {
                reward.Template.ItemsRewardCSV = reward.RewardD2O.itemsReward.ToCSV(",");
                reward.Template.ItemsQuantityRewardCSV = reward.RewardD2O.itemsQuantityReward.ToCSV(",");
                DatabaseManager.Database.Insert(reward.Template);
                writerReward.Write(reward.RewardD2O, (int)reward.Id);
            }
            writerReward.EndWriting();

            writerCategory.StartWriting(true);
            foreach (var category in m_categoryData)
            {
                DatabaseManager.Database.Insert(category.Template);
                writerCategory.Write(category.CategoryD2O, (int)category.Id);
            }
            writerCategory.EndWriting();
        }
    }
}
