using Stump.DofusProtocol.D2oClasses;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Npcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Achievements.Manager;

namespace Tools.Achievements.Data.Npcs
{
    public class NpcData
    {
        public NpcData(Npc d2o, NpcTemplate record, List<NpcSpawn> spawns, List<NpcActionRecord> actions, List<NpcItem> items, List<NpcReplyRecord> replies)
        {
            NpcD2O = d2o;
            NpcRecord = record;
            Spawns = spawns;
            Actions = actions;
            Items = items;
            NpcReplies = replies;
        }

        public Npc NpcD2O
        {
            get;
            set;
        }

        public NpcTemplate NpcRecord
        {
            get;
            set;
        }

        public int Id
        {
            get => NpcD2O.Id;
            set
            {
                NpcD2O.Id = value;
                NpcRecord.Id = value;
            }
        }

        public uint NameId
        {
            get => NpcD2O.NameId;
            set
            {
                NpcD2O.NameId = value;
                NpcRecord.NameId = value;
            }
        }

        public int TokenShop
        {
            get => NpcD2O.TokenShop;
            set
            {
                NpcD2O.TokenShop = value;
                NpcRecord.TokenShop = value;
            }
        }

        public string Look
        {
            get => NpcD2O.Look;
            set
            {
                NpcD2O.Look = value;
                NpcRecord.LookAsString = value;
            }
        }

        public bool FastAnim
        {
            get => NpcD2O.FastAnimsFun;
            set
                => NpcD2O.FastAnimsFun = value;
        }

        public List<List<int>> DialogMessages
        {
            get => NpcD2O.DialogMessages;
            set
            {
                NpcD2O.DialogMessages = value;
                NpcRecord.DialogMessagesId = value.Select(x => x.ToArray()).ToArray();
            }
        }

        public List<List<int>> DialogReplies
        {
            get => NpcD2O.DialogReplies;
            set
            {
                NpcD2O.DialogReplies = value;
                NpcRecord.DialogRepliesId = value.Select(x => x.ToArray()).ToArray();
            }
        }

        public List<uint> ActionsIds
        {
            get => NpcD2O.Actions;
            set
            {
                NpcD2O.Actions = value;
                NpcRecord.ActionsIds = value.Select(x => x).ToArray();
            }
        }
        public string Name
            => TextManager.GetText(NameId);

        public List<NpcSpawn> Spawns
        {
            get;
            set;
        }

        public List<NpcItem> Items
        {
            get;
            set;
        }

        public List<NpcActionRecord> Actions
        {
            get;
            set;
        }

        public List<NpcReplyRecord> NpcReplies
        {
            get;
            set;
        }
    }
}