
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Achievements;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Quests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools.Achievements.Manager;
using Point = Stump.DofusProtocol.D2oClasses.Point;

namespace Tools.Achievements
{
    public partial class Form1 : Form
    {
        #region Constructor
        public Form1()
        {
            InitializeComponent();
            DatabaseManager.Initialize();
            TextManager.Initialize();
            DataManager.Initialize(@".\common\");
            AchievementManager.Load();
            LoadInformation();
            LoadCategories();
        }
        #endregion
        #region Properties
        AchievementData AchievementSelected
        {
            get;
            set;
        }

        ObjectiveData ObjectiveSelected
        {
            get;
            set;
        }

        RewardData RewardSelected
        {
            get;
            set;
        }

        CategoryData CategorySelected
        {
            get;
            set;
        }

        NpcSpawn SpawnSelected
        {
            get;
            set;
        }

        NpcItem ShopSelected
        {
            get;
            set;
        }

        NpcActionRecord ActionSelected
        {
            get;
            set;
        }

        NpcReplyRecord ReplySelected
        {
            get;
            set;
        }

        #endregion
        #region Load/Refresh/Save
        void LoadInformation(bool search = false)
        {
            listBox1.Items.Clear();
            if (!search)
                listBox1.Items.AddRange(AchievementManager.GetAchievementName());
            else
                listBox1.Items.AddRange(AchievementManager.GetAchievementName(textBox1.Text.ToLower()));
        }

        void LoadCategories(bool search = false)
        {
            categoriesList.Items.Clear();
            if (!search)
                categoriesList.Items.AddRange(AchievementManager.GetCategoriesName());
            else
                categoriesList.Items.AddRange(AchievementManager.GetCategoriesName(textBox5.Text.ToLower()));
        }

        void LoadAchievementInformation()
        {
            listBox2.Items.Clear();
            listBox2.Items.AddRange(AchievementSelected.GetObjectiveId());
            listReward.Items.Clear();
            listReward.Items.AddRange(AchievementSelected.GetRewardsIds());

            numericId.Value = AchievementSelected.AchievementD2O.Id;
            numericUpDown1.Value = AchievementSelected.AchievementD2O.NameId;
            numericUpDown2.Value = AchievementSelected.AchievementD2O.CategoryId;
            numericDescription.Value = AchievementSelected.AchievementD2O.DescriptionId;
            numericIcon.Value = AchievementSelected.AchievementD2O.IconId;
            numericLevel.Value = AchievementSelected.AchievementD2O.Level;
            numericOrder.Value = AchievementSelected.AchievementD2O.Order;
            numericPoint.Value = AchievementSelected.AchievementD2O.Points;

            textBox2.Text = TextManager.GetText((int)numericUpDown1.Value);
            textBox3.Text = TextManager.GetText(AchievementManager.GetCategoryNameId((int)AchievementSelected.AchievementD2O.CategoryId));
            textBox4.Text = TextManager.GetText((int)numericDescription.Value);
        }

        void LoadObjectiveInformation()
        {
            numericObjectiveId.Value = ObjectiveSelected.ObjectiveD2O.Id;
            numericObjectiveAchievementId.Value = ObjectiveSelected.ObjectiveD2O.AchievementId;
            numericObjectiveName.Value = ObjectiveSelected.ObjectiveD2O.NameId;
            numericUpDown3.Value = ObjectiveSelected.ObjectiveD2O.Order;
            textObjectiveName.Text = TextManager.GetText((int)numericObjectiveName.Value);
            textObjectiveCriterion.Text = ObjectiveSelected.ObjectiveD2O.Criterion;
        }

        void LoadCategoryInformation()
        {
            categoryId.Value = CategorySelected.CategoryD2O.Id;
            categoryNameId.Value = CategorySelected.CategoryD2O.NameId;
            categoryParent.Value = CategorySelected.CategoryD2O.ParentId;
            categoryOrder.Value = CategorySelected.CategoryD2O.Order;
            categoryIcon.Text = CategorySelected.CategoryD2O.Icon;
            categoryColor.Text = CategorySelected.CategoryD2O.Color;
        }

        void LoadRewardInformation()
        {
            // Clear all rows of differents dataGrid
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();
            dataGridView5.Rows.Clear();

            // Print all informations about reward
            rewardId.Value = (int)RewardSelected.Id;
            rewardAchId.Value = (int)RewardSelected.AchievementId;
          

            // Print in gridView, the list of npcs
            for (int i = 0; i < RewardSelected.ItemsQuantities.Count; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = RewardSelected.ItemsReward[i];
                dataGridView1.Rows[i].Cells[1].Value = RewardSelected.ItemsQuantities[i];
            }

            for(int i = 0; i < RewardSelected.EmotesReward.Count; i++) 
                dataGridView2.Rows[i].Cells[0].Value = RewardSelected.EmotesReward[i];

            for (int i = 0; i < RewardSelected.SpellsRewards.Count; i++)
                dataGridView3.Rows[i].Cells[0].Value = RewardSelected.SpellsRewards[i];

            for (int i = 0; i < RewardSelected.TitlesReward.Count; i++)
                dataGridView4.Rows[i].Cells[0].Value = RewardSelected.TitlesReward[i];

            for (int i = 0; i < RewardSelected.OrnamentsReward.Count; i++)
                dataGridView5.Rows[i].Cells[0].Value = RewardSelected.OrnamentsReward[i];
        }

        #endregion
        #region Events
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            var achievementData = AchievementManager.GetDataByName(listBox1.SelectedItem.ToString());
            if (achievementData != null)
            {
                AchievementSelected = achievementData;
                LoadAchievementInformation();
            }
            else
                MessageBox.Show("Impossible de trouver la data correspondante, le succès séléctionner doit être manquant dans une des parties. (D2O/SQL)", "", MessageBoxButtons.OK);
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            LoadInformation(true);
        }

        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
            LoadCategories(true);
        }

        private void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
                return;

            var objectiveData = AchievementManager.GetObjectiveData(int.Parse(listBox2.SelectedItem.ToString()));
            if (objectiveData != null)
            {
                ObjectiveSelected = objectiveData;
                LoadObjectiveInformation();
            }
            else
                MessageBox.Show("Impossible de trouver la data correspondate, l'objectif séléctionner doit être manquant dans une des parties. (D2O/SQL)", "", MessageBoxButtons.OK);
        }

        private void CategoriesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoriesList.SelectedItem == null)
                return;

            var categoryData = AchievementManager.GetCategoryData(categoriesList.SelectedItem.ToString());
            if(categoryData != null)
            {
                CategorySelected = categoryData;
                LoadCategoryInformation();
            }
            else
                MessageBox.Show("Impossible de trouver la data correspondate, la catégorie séléctionner doit être manquant dans une des parties. (D2O/SQL)", "", MessageBoxButtons.OK);
        }


        private void NumericId_ValueChanged(object sender, EventArgs e)
        {
            if (AchievementSelected != null)
            {
                AchievementSelected.Id = (uint)numericId.Value;
            }
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (AchievementSelected != null && AchievementSelected.NameId != numericUpDown1.Value) {
                AchievementSelected.NameId = (uint)numericUpDown1.Value;
                textBox2.Text = TextManager.GetText((int)numericUpDown1.Value);
                LoadInformation();
            }
        }

        private void NumericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (AchievementSelected != null)
            {
                AchievementSelected.CategoryId = (uint)numericUpDown2.Value;

                if (AchievementManager.GetCategoryData((int)numericUpDown2.Value) != null)
                {
                    AchievementManager.GetCategoryData((int)numericUpDown2.Value).AchievementsId = AchievementManager.GetAchievementByCategory(AchievementSelected.CategoryId);
                    textBox3.Text = TextManager.GetText(AchievementManager.GetCategoryNameId((int)numericUpDown2.Value));
                }
            }
        }

        private void NumericDescription_ValueChanged(object sender, EventArgs e)
        {
            if (AchievementSelected != null)
            {
                AchievementSelected.DescriptionId = (uint)numericDescription.Value;
                textBox4.Text = TextManager.GetText((int)numericDescription.Value);
            }
        }

        private void NumericIcon_ValueChanged(object sender, EventArgs e)
        {
            if (AchievementSelected != null)
                AchievementSelected.IconId = (int)numericIcon.Value;
        }

        private void NumericPoint_ValueChanged(object sender, EventArgs e)
        {
            if (AchievementSelected != null)
                AchievementSelected.Points = (uint)numericPoint.Value;
        }

        private void NumericLevel_ValueChanged(object sender, EventArgs e)
        {
            if (AchievementSelected != null)
                AchievementSelected.Level = (uint)numericLevel.Value;
        }

        private void NumericOrder_ValueChanged(object sender, EventArgs e)
        {
            if(AchievementSelected != null)
               AchievementSelected.Order = (uint)numericOrder.Value;
        }

        private void CategoryId_ValueChanged(object sender, EventArgs e)
        {
            if (CategorySelected != null)
                CategorySelected.Id = (uint)categoryId.Value;
        }

        private void CategoryNameId_ValueChanged(object sender, EventArgs e)
        {
            if (CategorySelected != null && CategorySelected.NameId != categoryNameId.Value)
            {
                CategorySelected.NameId = (uint)categoryNameId.Value;
                LoadCategories();
            }
        }

        private void CategoryParent_ValueChanged(object sender, EventArgs e)
        {
            if (CategorySelected != null)
                CategorySelected.ParentId = (uint)categoryParent.Value;
        }

        private void CategoryOrder_ValueChanged(object sender, EventArgs e)
        {
            if (CategorySelected != null)
                CategorySelected.Order = (uint)categoryOrder.Value;
        }

        private void CategoryIcon_TextChanged(object sender, EventArgs e)
        {
            if (CategorySelected != null)
                CategorySelected.Icon = categoryIcon.Text;
        }

        private void CategoryColor_TextChanged(object sender, EventArgs e)
        {
            if (CategorySelected != null)
                CategorySelected.Color = categoryColor.Text;
        }

        private void NumericObjectiveId_ValueChanged(object sender, EventArgs e)
        {
            if (ObjectiveSelected != null)
            {
                AchievementSelected.ObjectivesData[AchievementSelected.ObjectivesData.IndexOf(ObjectiveSelected)].Id = (uint)numericObjectiveId.Value;
                ObjectiveSelected.Id = (uint)numericObjectiveId.Value;
                if (!listBox2.Items.Contains(ObjectiveSelected.Id.ToString()))
                {
                    listBox2.Items.Clear();
                    listBox2.Items.AddRange(AchievementSelected.GetObjectiveId());
                    AchievementSelected.Objectives = AchievementSelected.ObjectivesData.Select(x => (int)x.Id).ToList();
                }
            }
        }

        private void NumericObjectiveName_ValueChanged(object sender, EventArgs e)
        {
            if (ObjectiveSelected != null)
            {
                ObjectiveSelected.NameId = (uint)numericObjectiveName.Value;
                textObjectiveName.Text = TextManager.GetText(ObjectiveSelected.NameId);
            }
        }

        private void NumericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (ObjectiveSelected != null)
                ObjectiveSelected.Order = (uint)numericUpDown3.Value;
        }

        private void TextObjectiveCriterion_TextChanged(object sender, EventArgs e)
        {
            if (ObjectiveSelected != null)
                ObjectiveSelected.Criterion = textObjectiveCriterion.Text;
        }

        private void ListReward_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listReward.SelectedItem == null)
                return;

            var categoryData = AchievementManager.GetRewardData(int.Parse(listReward.SelectedItem.ToString()));
            if (categoryData != null)
            {
                RewardSelected = categoryData;
                LoadRewardInformation();
            }
            else
                MessageBox.Show("Impossible de trouver la data correspondate, la récompense séléctionner doit être manquant dans une des parties. (D2O/SQL)", "", MessageBoxButtons.OK);
        }

        private void RewardId_ValueChanged(object sender, EventArgs e)
        {
            if (RewardSelected != null)
            {
                AchievementSelected.RewardsData[AchievementSelected.RewardsData.IndexOf(RewardSelected)].Id = (uint)rewardId.Value;
                if (!listReward.Items.Contains(RewardSelected.Id.ToString()))
                {
                    AchievementSelected.RewardsId = AchievementSelected.GetRewardsIds().Select(x => int.Parse(x)).ToList();
                    listReward.Items.Clear();
                    listReward.Items.AddRange(AchievementSelected.GetRewardsIds());
                }
            }
        }

        private void RewardKamasRatio_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void RewardExpRatio_ValueChanged(object sender, EventArgs e)
        {
        }

        private void RewardCriteria_TextChanged(object sender, EventArgs e)
        {
        }

        private void RewardScale_CheckedChanged(object sender, EventArgs e)
        {
        }
        #endregion

        private void Button2_Click(object sender, EventArgs e)
        {
            if(RewardSelected != null)
            {
                RewardSelected.EmotesReward = new List<uint>();
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {
                    RewardSelected.EmotesReward.Add(uint.Parse(dataGridView2.Rows[i].Cells[0].Value.ToString()));
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (RewardSelected != null)
            {
                RewardSelected.SpellsRewards = new List<uint>();

                for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
                    RewardSelected.SpellsRewards.Add(uint.Parse(dataGridView3.Rows[i].Cells[0].Value.ToString()));
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (RewardSelected != null)
            {
                RewardSelected.OrnamentsReward = new List<uint>();
                RewardSelected.TitlesReward = new List<uint>();

                for (int i = 0; i < dataGridView4.Rows.Count - 1; i++)
                    RewardSelected.TitlesReward.Add(uint.Parse(dataGridView4.Rows[i].Cells[0].Value.ToString()));

                for (int i = 0; i < dataGridView5.Rows.Count - 1; i++)
                    RewardSelected.OrnamentsReward.Add(uint.Parse(dataGridView5.Rows[i].Cells[0].Value.ToString()));
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (RewardSelected != null)
            {
                RewardSelected.ItemsQuantities = new List<uint>();
                RewardSelected.ItemsReward = new List<uint>();

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    RewardSelected.ItemsReward.Add(uint.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString()));
                    RewardSelected.ItemsQuantities.Add(uint.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()));
                }
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            NewTool.Records.AchievementTemplate record = new NewTool.Records.AchievementTemplate();
            AchievementData data = new AchievementData(record, new Achievement(), AchievementManager.GetObjectiveDatas(record.ObjectiveIds.ToList()), AchievementManager.GetRewardDatas(record.RewardIds.ToList()));
            AchievementManager.AddAchievementData(data);
            AchievementSelected = data;
            AchievementSelected.CategoryId = AchievementManager.GetFirstCategory().Id;
            LoadInformation();
            LoadAchievementInformation();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if (AchievementSelected != null)
            {
                AchievementManager.DeleteAchievement(AchievementSelected);
                LoadInformation();
                AchievementSelected = AchievementManager.GetFirstData();
                LoadAchievementInformation();
            }
            else
                MessageBox.Show("Impossible de supprimer ce qui n'existe pas ;)");
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (AchievementSelected == null)
                return;

            ObjectiveData data = new ObjectiveData(new NewTool.Records.AchievementObjectiveRecord(), new AchievementObjective());
            AchievementManager.AddObjectiveData(data);
            ObjectiveSelected = data;
            ObjectiveSelected.AchievementId = AchievementSelected.Id;
            AchievementSelected.ObjectivesData.Add(data);
            AchievementSelected.Objectives = AchievementSelected.GetObjectiveId().Select(x => int.Parse(x)).ToList();
            LoadAchievementInformation();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            if (ObjectiveSelected == null)
                return;

            AchievementManager.DeleteObjective(AchievementSelected, ObjectiveSelected);
            LoadAchievementInformation();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            CategoryData data = new CategoryData(new NewTool.Records.AchievementCategoryRecord(), new AchievementCategory() { AchievementIds = new List<uint>() });
            AchievementManager.AddCategoryData(data);
            CategorySelected = data;
            LoadCategories();
            LoadCategoryInformation();
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            if (CategorySelected == null)
                return;

            AchievementManager.DeleteCategory(CategorySelected);
            LoadCategories();
            LoadInformation();
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            RewardData data = new RewardData(new NewTool.Records.AchievementRewardRecord() { AchievementId = AchievementSelected.Id, EmotesReward = new uint[0], ItemsQuantityReward = new List<uint>(), ItemsReward = new List<uint>(), OrnamentsReward = new uint[0], SpellsReward = new uint[0], TitlesReward = new uint[0] }
                , new AchievementReward() { AchievementId = AchievementSelected.Id, EmotesReward = new List<uint>(), ItemsQuantityReward = new List<uint>(), ItemsReward = new List<uint>(), TitlesReward = new List<uint>(), OrnamentsReward = new List<uint>(), SpellsReward = new List<uint>()});
            AchievementManager.AddRewardData(data);
            RewardSelected = data;
            AchievementSelected.RewardsData.Add(data);
            AchievementSelected.RewardsId = AchievementSelected.GetRewardsIds().Select(x => int.Parse(x)).ToList();
            LoadAchievementInformation();
            LoadRewardInformation();
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            if (RewardSelected == null)
                return;

            AchievementManager.DeleteReward(AchievementSelected, RewardSelected);
            LoadAchievementInformation();
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            AchievementManager.Save();
            MessageBox.Show("Sauvegarder réussie !", "", MessageBoxButtons.OK);
        }

    }
}
