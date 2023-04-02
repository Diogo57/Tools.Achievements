using Nlog_net;
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
using Tools.Achievements.Data.Npcs;
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
            AchievementManager = new AchievementManager();
            AchievementManager.Load();
            NpcsManager = new NpcsManager();
            NpcsManager.Load();
            Logger = new Logger();
            Logger.Initialize();
            LoadInformation();
            LoadCategories();
            LoadNpcsInformations();
            comboBox1.SelectedIndex = 0;
        }
        #endregion
        #region Properties
        AchievementManager AchievementManager
        {
            get;
            set;
        }

        NpcsManager NpcsManager
        {
            get;
            set;
        }

        Logger Logger
        {
            get;
            set;
        }

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

        NpcData NpcSelected
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

        void LoadNpcsInformations()
        {
            listNpcs.Items.Clear();
            listNpcs.Items.AddRange(NpcsManager.GetNpcsIds());
        }

        void SearchNpcs()
        {
            listNpcs.Items.Clear();
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Name":
                    listNpcs.Items.AddRange(npcsSearchText.Text.Length > 0 ? NpcsManager.GetNpcsNames(npcsSearchText.Text) : NpcsManager.GetNpcsNames());
                    break;
                case "Id":
                    listNpcs.Items.AddRange(npcsSearchText.Text.Length > 0 ? NpcsManager.GetNpcsIds(npcsSearchText.Text) : NpcsManager.GetNpcsIds());
                    break;
            }

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

        void LoadNpcsInformation()
        {
            // Print all dependances (shop, spawns, actions and replies)
            listBox4.Items.Clear();
            listBox4.Items.AddRange(NpcSelected.Spawns.Select(x => x.Id.ToString()).ToArray());
            listBox5.Items.Clear();
            listBox5.Items.AddRange(NpcSelected.Items.OrderBy(x => x.Id).Select(x => x.ItemId.ToString()).ToArray());
            listActions.Items.Clear();
            listActions.Items.AddRange(NpcSelected.Actions.OrderBy(x => x.Id).Select(x => x.Id.ToString()).ToArray());
            listBox7.Items.Clear();
            listBox7.Items.AddRange(NpcSelected.NpcReplies.OrderBy(x => x.Id).Select(x => x.Id.ToString()).ToArray());

            // Print all informations about npc
            npcId.Value = NpcSelected.Id;
            nameId.Value = NpcSelected.NameId;
            textNpcName.Text = NpcSelected.Name;
            npcTokenShop.Value = NpcSelected.TokenShop;
            npcFastAnim.Checked = NpcSelected.FastAnim;
            textNpcLook.Text = NpcSelected.Look;

            // Clear all rows of differents dataGrid
            gridMessages.Rows.Clear();
            gridReplies.Rows.Clear();
            dataGridView6.Rows.Clear();

            // Print in gridView, the list of npcs
            for (int i = 0; i < NpcSelected.DialogMessages.Count(); i++)
            {
                gridMessages.Rows.Add();
                gridMessages.Rows[i].Cells[0].Value = NpcSelected.DialogMessages[i][0];
                gridMessages.Rows[i].Cells[1].Value = NpcSelected.DialogMessages[i][1];
            }

            for(int i = 0; i < NpcSelected.DialogReplies.Count(); i++)
            {
                gridReplies.Rows.Add();
                gridReplies.Rows[i].Cells[0].Value = NpcSelected.DialogReplies[i][0];
                gridReplies.Rows[i].Cells[1].Value = NpcSelected.DialogReplies[i][1];
            }

            for (int i = 0; i < NpcSelected.ActionsIds.Count; i++)
            {
                dataGridView6.Rows.Add();
                dataGridView6.Rows[i].Cells[0].Value = NpcSelected.ActionsIds[i];
            }
        }

        void LoadSpawnInformations()
        {
            numericUpDown4.Value = SpawnSelected.Id;
            numericUpDown5.Value = SpawnSelected.NpcId;
            numericUpDown6.Value = SpawnSelected.MapId;
            numericUpDown7.Value = SpawnSelected.CellId;
            numericUpDown8.Value = (int)SpawnSelected.Direction;
        }

        void LoadShopInformation()
        {
            shopId.Value = ShopSelected.Id;
            shopNpcId.Value = ShopSelected.NpcShopId;
            shopItemId.Value = ShopSelected.ItemId;
            shopPrice.Value = ShopSelected.CustomPrice != null ? (decimal)ShopSelected.CustomPrice : (decimal)0;
            shopBuyCriterion.Text = ShopSelected.BuyCriterion;
        }

        void LoadActionInformation()
        {
            actionId.Value = ActionSelected.Id;
            actionNpcId.Value = ActionSelected.NpcId;
            actionPriority.Value = ActionSelected.Priority;
            actionType.Text = ActionSelected.Type;
            actionCondition.Text = ActionSelected.Condition;
            actionParameter0.Text = ActionSelected.Parameter0;
            actionParameter1.Text = ActionSelected.Parameter1;
            actionParameter2.Text = ActionSelected.Parameter2;
            actionParameter3.Text = ActionSelected.Parameter3;
            actionParameter4.Text = ActionSelected.Parameter4;
            actionParameter5.Text = ActionSelected.AdditionalParameters;
        }

        void LoadReplyInformation()
        {
            replyId.Value = ReplySelected.Id;
            replyReplyId.Value = ReplySelected.ReplyId;
            replyMessageId.Value = ReplySelected.MessageId;
            replyCriteria.Text = ReplySelected.Criteria;
            replyType.Text = ReplySelected.Type;
            replyParameter0.Text = ReplySelected.Parameter0;
            replyParameter1.Text = ReplySelected.Parameter1;
            replyParameter2.Text = ReplySelected.Parameter2;
            replyParameter3.Text = ReplySelected.Parameter3;
            replyParameter4.Text = ReplySelected.Parameter4;
            replyParameter5.Text = ReplySelected.AdditionalParameters;
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
            AchievementTemplateRecord record = new AchievementTemplateRecord();
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

            ObjectiveData data = new ObjectiveData(new AchievementObjectiveRecord(), new AchievementObjective());
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
            CategoryData data = new CategoryData(new AchievementCategoryRecord(), new AchievementCategory() { AchievementIds = new List<uint>() });
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
            RewardData data = new RewardData(new AchievementRewardRecord() { AchievementId = AchievementSelected.Id, EmotesReward = new uint[0], ItemsQuantityReward = new uint[0], ItemsReward = new uint[0], OrnamentsReward = new uint[0], SpellsReward = new uint[0], TitlesReward = new uint[0] }
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

        private void NpcsSearchText_TextChanged(object sender, EventArgs e)
            =>  SearchNpcs();

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
           =>   SearchNpcs();

        private void ListNpcs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listNpcs.SelectedItem == null)
                return;

            switch (comboBox1.SelectedItem.ToString())
            {
                case "Id":
                    var npcData = NpcsManager.GetNpcData(int.Parse(listNpcs.SelectedItem.ToString()));
                    if (npcData != null)
                    {
                        NpcSelected = npcData;
                        LoadNpcsInformation();
                    }
                    else
                        MessageBox.Show("Impossible de trouver le data correspondant");
                    break;
                case "Name":
                    var data = NpcsManager.GetNpcData(listNpcs.SelectedItem.ToString());
                    if (data != null)
                    {
                        NpcSelected = data;
                        LoadNpcsInformation();
                    }
                    else
                        MessageBox.Show("Impossible de trouver le data correspondant");
                    break;
            }
        }

        private void ListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NpcSelected.Spawns.Any(x => x.Id == int.Parse(listBox4.SelectedItem.ToString())))
            {
                SpawnSelected = NpcSelected.Spawns.FirstOrDefault(x => x.Id == int.Parse(listBox4.SelectedItem.ToString()));
                LoadSpawnInformations();
            }
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            if(NpcSelected.Spawns.Contains(SpawnSelected))
            {
                NpcSelected.Spawns.Remove(SpawnSelected);
                LoadNpcsInformation();
            }
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            if (NpcSelected == null)
                return;

            NpcSpawn spawn = new NpcSpawn() { Id = NpcsManager.GetNextSpawnId() };
            NpcSelected.Spawns.Add(spawn);
            SpawnSelected = spawn;
            LoadNpcsInformation();
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox5.SelectedItem == null)
                return;

            if (NpcSelected.Items.Any(x => x.ItemId == int.Parse(listBox5.SelectedItem.ToString())))
            {
                ShopSelected = NpcSelected.Items.FirstOrDefault(x => x.ItemId == int.Parse(listBox5.SelectedItem.ToString()));
                LoadShopInformation();
            }
        }

        private void listActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(NpcSelected.Actions.Any(x => x.Id == int.Parse(listActions.SelectedItem.ToString())))
            {
                ActionSelected = NpcSelected.Actions.FirstOrDefault(x => x.Id == int.Parse(listActions.SelectedItem.ToString()));
                LoadActionInformation();
            }
        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(NpcSelected.NpcReplies.Any(x => x.Id == int.Parse(listBox7.SelectedItem.ToString())))
            {
                ReplySelected = NpcSelected.NpcReplies.FirstOrDefault(x => x.Id == int.Parse(listBox7.SelectedItem.ToString()));
                LoadReplyInformation();
            }
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (SpawnSelected != null)
                SpawnSelected.NpcId = (int)numericUpDown5.Value;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (SpawnSelected != null)
                SpawnSelected.MapId = (int)numericUpDown6.Value;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (SpawnSelected != null)
                SpawnSelected.CellId = (int)numericUpDown7.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (SpawnSelected != null)
                SpawnSelected.Direction = (DirectionsEnum)numericUpDown8.Value;
        }

        private void shopId_ValueChanged(object sender, EventArgs e)
        {
            if (ShopSelected != null)
                ShopSelected.Id = (int)shopId.Value;
        }

        private void shopPrice_ValueChanged(object sender, EventArgs e)
        {
            if (ShopSelected != null)
                ShopSelected.CustomPrice = (int)shopPrice.Value;
        }

        private void shopItemId_ValueChanged(object sender, EventArgs e)
        {
            if (ShopSelected != null)
                ShopSelected.ItemId = (int)shopItemId.Value;
        }

        private void shopBuyCriterion_TextChanged(object sender, EventArgs e)
        {
            if (ShopSelected != null)
                ShopSelected.BuyCriterion = shopBuyCriterion.Text;
        }

        private void shopNpcId_ValueChanged(object sender, EventArgs e)
        {
            if(ShopSelected != null)
            {
                NpcSelected.Items.Remove(ShopSelected);
                ShopSelected.NpcShopId = (int)shopNpcId.Value;
                var npcId = NpcsManager.GetNpcIdByActionId(ShopSelected.NpcShopId);
                if (npcId != null)
                    NpcsManager.GetNpcData((int)npcId).Items.Add(ShopSelected);

                LoadNpcsInformation();
            }
        }

        private void actionNpcId_ValueChanged(object sender, EventArgs e)
        {
            if (ActionSelected != null)
                ActionSelected.NpcId = (int)actionNpcId.Value;
        }

        private void actionPriority_ValueChanged(object sender, EventArgs e)
        {
            if (ActionSelected != null)
                ActionSelected.Priority = (int)actionPriority.Value;
        }

        private void actionType_TextChanged(object sender, EventArgs e)
        {
            if (ActionSelected != null)
                ActionSelected.Type = actionType.Text;
        }

        private void actionCondition_TextChanged(object sender, EventArgs e)
        {
            if (ActionSelected != null)
                ActionSelected.Condition = actionCondition.Text;
        }

        private void actionParameter0_TextChanged(object sender, EventArgs e)
        {
            if (ActionSelected != null)
                ActionSelected.Parameter0 = actionParameter0.Text;
        }

        private void actionParameter1_TextChanged(object sender, EventArgs e)
        {
            if (ActionSelected != null)
                ActionSelected.Parameter1 = actionParameter1.Text;
        }

        private void actionParameter2_TextChanged(object sender, EventArgs e)
        {
            if (ActionSelected != null)
                ActionSelected.Parameter2 = actionParameter2.Text;
        }

        private void actionParameter3_TextChanged(object sender, EventArgs e)
        {
            if (ActionSelected != null)
                ActionSelected.Parameter3 = actionParameter3.Text;
        }

        private void actionParameter4_TextChanged(object sender, EventArgs e)
        {
            if (ActionSelected != null)
                ActionSelected.Parameter4 = actionParameter4.Text;
        }

        private void actionParameter5_TextChanged(object sender, EventArgs e)
        {
            if (ActionSelected != null)
                ActionSelected.AdditionalParameters = actionParameter5.Text;
        }

        private void replyId_ValueChanged(object sender, EventArgs e)
        {
            if (ReplySelected != null)
                ReplySelected.Id = (int)replyId.Value;
        }

        private void replyReplyId_ValueChanged(object sender, EventArgs e)
        {
            if (ReplySelected != null)
                ReplySelected.ReplyId = (int)replyReplyId.Value;
        }

        private void replyType_TextChanged(object sender, EventArgs e)
        {
            if (ReplySelected != null)
                ReplySelected.Type = replyType.Text;
        }

        private void replyCriteria_TextChanged(object sender, EventArgs e)
        {
            if (ReplySelected != null)
                ReplySelected.Criteria = replyCriteria.Text;
        }

        private void replyParameter0_TextChanged(object sender, EventArgs e)
        {
            if (ReplySelected != null)
                ReplySelected.Parameter0 = replyParameter0.Text;
        }

        private void replyParameter1_TextChanged(object sender, EventArgs e)
        {
            if (ReplySelected != null)
                ReplySelected.Parameter1 = replyParameter1.Text;
        }

        private void replyParameter2_TextChanged(object sender, EventArgs e)
        {
            if (ReplySelected != null)
                ReplySelected.Parameter2 = replyParameter2.Text;
        }

        private void replyParameter3_TextChanged(object sender, EventArgs e)
        {
            if (ReplySelected != null)
                ReplySelected.Parameter3 = replyParameter3.Text;
        }

        private void replyParameter4_TextChanged(object sender, EventArgs e)
        {
            if (ReplySelected != null)
                ReplySelected.Parameter4 = replyParameter4.Text;
        }

        private void replyParameter5_TextChanged(object sender, EventArgs e)
        {
            if (ReplySelected != null)
                ReplySelected.AdditionalParameters = replyParameter5.Text;
        }

        private void replyMessageId_ValueChanged(object sender, EventArgs e)
        {
            if(ReplySelected != null)
            {
                NpcSelected.NpcReplies.Remove(ReplySelected);
                ReplySelected.MessageId = (int)replyMessageId.Value;
                var npcId = NpcsManager.GetNpcIdByParameter0(ReplySelected.MessageId.ToString());
                if (npcId != null)
                    NpcsManager.GetNpcData((int)npcId).NpcReplies.Add(ReplySelected);

                LoadNpcsInformation();
            }
        }

        private void npcId_ValueChanged(object sender, EventArgs e)
        {
            if (NpcSelected != null)
                NpcSelected.Id = (int)npcId.Value;
        }

        private void nameId_ValueChanged(object sender, EventArgs e)
        {
            if(NpcSelected != null)
            {
                NpcSelected.NameId = (uint)nameId.Value;
                textNpcName.Text = NpcSelected.Name;
            }
        }

        private void npcTokenShop_ValueChanged(object sender, EventArgs e)
        {
            if (NpcSelected != null)
                NpcSelected.TokenShop = (int)npcTokenShop.Value;
        }

        private void npcFastAnim_CheckedChanged(object sender, EventArgs e)
        {
            if (NpcSelected != null)
                NpcSelected.FastAnim = npcFastAnim.Checked;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if(NpcSelected != null)
            {
                List<uint> actions = new List<uint>();

                for (int i = 0; i < dataGridView6.Rows.Count - 1; i++)
                    actions.Add(Convert.ToUInt32(dataGridView6.Rows[i].Cells[0].Value));

                NpcSelected.ActionsIds = actions;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            // save messages
            if(NpcSelected != null)
            {
                List<List<int>> dialog = new List<List<int>>();

                for (int i = 0; i < gridMessages.Rows.Count - 1; i++)
                    dialog.Add(new List<int>() { Convert.ToInt32(gridMessages.Rows[i].Cells[0].Value), Convert.ToInt32(gridMessages.Rows[i].Cells[1].Value) });

                NpcSelected.DialogMessages = dialog;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            // save replies
            if(NpcSelected != null)
            {
                List<List<int>> replies = new List<List<int>>();

                for (int i = 0; i < gridReplies.Rows.Count - 1; i++)
                    replies.Add(new List<int>() { Convert.ToInt32(gridReplies.Rows[i].Cells[0].Value), Convert.ToInt32(gridReplies.Rows[i].Cells[1].Value) });

                NpcSelected.DialogReplies = replies;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (NpcSelected == null)
                return;

            NpcItem spawn = new NpcItem() { Id = NpcsManager.GetNextShopId() };
            NpcSelected.Items.Add(spawn);
            ShopSelected = spawn;
            LoadNpcsInformation();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (NpcSelected == null || ShopSelected == null)
                return;

            if (NpcSelected.Items.Contains(ShopSelected))
                NpcSelected.Items.Remove(ShopSelected);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (NpcSelected == null)
                return;

            NpcActionRecord action = new NpcActionRecord() { Id = NpcsManager.GetNextActionId() };
            NpcSelected.Actions.Add(action);
            ActionSelected = action;
            LoadNpcsInformation();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (NpcSelected == null || ActionSelected == null)
                return;

            if (NpcSelected.Actions.Contains(ActionSelected))
                NpcSelected.Actions.Remove(ActionSelected);

            LoadNpcsInformation();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (NpcSelected == null)
                return;

            NpcReplyRecord action = new NpcReplyRecord() { Id = NpcsManager.GetNextReplyId() };
            NpcSelected.NpcReplies.Add(action);
            ReplySelected = action;
            LoadNpcsInformation();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (NpcSelected == null || ReplySelected == null)
                return;

            if (NpcSelected.NpcReplies.Contains(ReplySelected))
                NpcSelected.NpcReplies.Remove(ReplySelected);

            LoadNpcsInformation();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            NpcData newData = new NpcData(new Npc() { DialogMessages = new List<List<int>>(), DialogReplies = new List<List<int>>(), Actions = new List<uint>() }, new NpcTemplate(), new List<NpcSpawn>(), new List<NpcActionRecord>(), new List<NpcItem>(), new List<NpcReplyRecord>()) { Look = "{}" };
            NpcsManager.AddNpcData(newData);
            NpcSelected = newData;
            LoadNpcsInformation();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (NpcSelected == null)
                return;

            NpcsManager.DeleteNpc(NpcSelected);
            LoadNpcsInformations();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (NpcSelected == null)
                return;

            if(MessageBox.Show("Etes vous de vouloir enregistrer les modifications du npc séléctionner ? (actions, replies, shop...)", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                NpcsManager.Save(NpcSelected);
            
        }

        private void textNpcLook_TextChanged(object sender, EventArgs e)
        {
            if (NpcSelected == null)
                return;

            NpcSelected.Look = textNpcLook.Text;
        }
    }
}
