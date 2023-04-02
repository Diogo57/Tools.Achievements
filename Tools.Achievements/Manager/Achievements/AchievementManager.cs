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
    public class AchievementManager
    {
        List<Achievement> m_achievements = new List<Achievement>();
        List<AchievementObjective> m_achievementObjectives = new List<AchievementObjective>();
        List<AchievementReward> m_achievementsReward = new List<AchievementReward>();
        List<AchievementCategory> m_categories = new List<AchievementCategory>();

        List<AchievementTemplateRecord> m_achievementsTemplate = new List<AchievementTemplateRecord>();
        List<AchievementObjectiveRecord> m_objectivesTemplate = new List<AchievementObjectiveRecord>();
        List<AchievementRewardRecord> m_rewardsTemplate = new List<AchievementRewardRecord>();
        List<AchievementCategoryRecord> m_categoryRecord = new List<AchievementCategoryRecord>();

        List<AchievementData> m_achievementsData = new List<AchievementData>();
        List<ObjectiveData> m_objectivesData = new List<ObjectiveData>();
        List<RewardData> m_rewardData = new List<RewardData>();
        List<CategoryData> m_categoryData = new List<CategoryData>();
        public void Load()
        {
            m_achievements = DataManager.GetAll<Achievement>();
            m_achievementObjectives = DataManager.GetAll<AchievementObjective>();
            m_achievementsReward = DataManager.GetAll<AchievementReward>();
            m_categories = DataManager.GetAll<AchievementCategory>();

            m_achievementsTemplate = DatabaseManager.Database.Query<AchievementTemplateRecord>(AchievementTemplateRelator.FetchQuery).ToList();
            m_objectivesTemplate = DatabaseManager.Database.Query<AchievementObjectiveRecord>(AchievementObjectiveRelator.FetchQuery).ToList();
            m_rewardsTemplate = DatabaseManager.Database.Query<AchievementRewardRecord>(AchievementRewardRelator.FetchQuery).ToList();
            m_categoryRecord = DatabaseManager.Database.Query<AchievementCategoryRecord>(AchievementCategoryRelator.FetchQuery).ToList();

            foreach (var reward in m_achievementsReward)
                m_rewardData.Add(new RewardData(m_rewardsTemplate.FirstOrDefault(x => x.Id == reward.Id), reward));

            foreach (var objective in m_achievementObjectives)
                m_objectivesData.Add(new ObjectiveData(m_objectivesTemplate.FirstOrDefault(x => x.Id == objective.Id), objective));
            
            foreach (var achievement in m_achievements)
                m_achievementsData.Add(new AchievementData(m_achievementsTemplate.FirstOrDefault(x => x.Id == achievement.Id), achievement, GetObjectiveDatas(achievement.objectiveIds), GetRewardDatas(achievement.rewardIds)));
            
            foreach (var category in m_categories)
                m_categoryData.Add(new CategoryData(m_categoryRecord.FirstOrDefault(x => x.Id == category.Id), category));
        }

        public string[] GetAchievementName()
            => m_achievementsData.Select(x => TextManager.GetText(x.NameId)).ToArray();

        public string[] GetAchievementName(string text)
            => m_achievementsData.Where(x => TextManager.GetText(x.NameId).ToLower().Contains(text)).Select(a => TextManager.GetText(a.NameId)).ToArray();

        public string[] GetCategoriesName()
            => m_categoryData.Select(x => TextManager.GetText(x.NameId)).ToArray();

        public string[] GetCategoriesName(string text)
          => m_categoryData.Where(x => TextManager.GetText(x.NameId).ToLower().Contains(text)).Select(a => TextManager.GetText(a.NameId)).ToArray();

        public AchievementData GetDataByName(string name)
            => m_achievementsData.FirstOrDefault(x => TextManager.GetText(x.Template.NameId) == name);

        public AchievementData GetFirstData()
            => m_achievementsData.FirstOrDefault();

        public ObjectiveData GetObjectiveData(int objectiveId)
            => m_objectivesData.FirstOrDefault(x => x.ObjectiveD2O.Id == objectiveId);

        public CategoryData GetCategoryData(string categoryName)
            => m_categoryData.FirstOrDefault(x => TextManager.GetText(x.CategoryD2O.NameId) == categoryName);

        public CategoryData GetCategoryData(int categoryId)
            => m_categoryData.FirstOrDefault(x => x.CategoryD2O.id == categoryId);

        public CategoryData GetFirstCategory()
            => m_categoryData.FirstOrDefault();

        public RewardData GetRewardData(int rewardId)
            => m_rewardData.FirstOrDefault(x => x.Id == rewardId);

        public List<ObjectiveData> GetObjectiveDatas(List<int> objectivesId)
            => m_objectivesData.Where(x => objectivesId.Contains((int)x.ObjectiveD2O.Id)).ToList();

        public List<RewardData> GetRewardDatas(List<int> rewardsId)
            => m_rewardData.Where(x => rewardsId.Contains((int)x.RewardD2O.Id)).ToList();

        public uint GetCategoryNameId(int categoryId)
            => m_categoryData.FirstOrDefault(x => x.Id == categoryId).NameId;

        public List<uint> GetAchievementByCategory(uint categoryId)
            => m_achievementsData.Where(x => x.CategoryId == categoryId).Select(a => a.Id).ToList();

        public List<AchievementData> GetDataByCategory(uint categoryId)
            => m_achievementsData.Where(x => x.CategoryId == categoryId).ToList();

        public void AddAchievementData(AchievementData data)
            => m_achievementsData.Add(data);

        public void AddObjectiveData(ObjectiveData data)
           => m_objectivesData.Add(data);

        public void AddCategoryData(CategoryData data)
            => m_categoryData.Add(data);

        public void AddRewardData(RewardData data)
            => m_rewardData.Add(data);

        public void DeleteAchievement(AchievementData data)
        {
            //m_achievementsData.Remove(data);
            //m_categoryData.FirstOrDefault(x => x.Id == data.CategoryId)?.AchievementsId.Remove(data.Id);

            //foreach (var objective in data.ObjectivesData)
            //    m_objectivesData.Remove(objective);

            //foreach (var reward in data.RewardsData)
            //    m_rewardData.Remove(reward);
        }

        public void DeleteObjective(AchievementData dataAchie, ObjectiveData objectiveData)
        {
            //m_objectivesData.Remove(objectiveData);
            //dataAchie.ObjectivesData.Remove(objectiveData);
            //dataAchie.Objectives = dataAchie.GetObjectiveId().Select(x => int.Parse(x)).ToList();
        }

        public void DeleteReward(AchievementData data, RewardData rewardData)
        {
            //m_rewardData.Remove(rewardData);
            //data.RewardsData.Remove(rewardData);
            //data.RewardsId = data.GetRewardsIds().Select(x => int.Parse(x)).ToList();
        }

        public void DeleteCategory(CategoryData data)
        {
          
        }

        public void Save()
        {
          
        }
    }
}
