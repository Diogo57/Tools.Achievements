using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Npcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Achievements.Data.Npcs;
using NpcMessage = Stump.DofusProtocol.D2oClasses.NpcMessage;
using NpcMessageRecord = Stump.Server.WorldServer.Database.Npcs.NpcMessage;

namespace Tools.Achievements.Manager
{
    public class NpcsManager
    {
        List<Npc> m_npcs = new List<Npc>();

        List<NpcTemplate> m_templatesRecords = new List<NpcTemplate>();
        List<NpcSpawn> m_spawns = new List<NpcSpawn>();
        List<NpcReplyRecord> m_replysRecords = new List<NpcReplyRecord>();
        List<NpcActionRecord> m_actionsRecords = new List<NpcActionRecord>();
        List<NpcItem> m_npcsItems = new List<NpcItem>();

        List<NpcData> m_npcsDatas = new List<NpcData>();
        List<NpcData> m_npcsRemove = new List<NpcData>();

        public void Load()
        {
            m_npcs = DataManager.GetAll<Npc>();

            m_templatesRecords = DatabaseManager.Database.Query<NpcTemplate>(NpcTemplateRelator.FetchQuery).ToList();
            m_spawns = DatabaseManager.Database.Query<NpcSpawn>(NpcSpawnRelator.FetchQuery).ToList();
            m_replysRecords = DatabaseManager.Database.Query<NpcReplyRecord>(NpcReplyRecordRelator.FetchQuery).ToList();
            m_actionsRecords = DatabaseManager.Database.Query<NpcActionRecord>(NpcActionRecordRelator.FetchQuery).ToList();
            m_npcsItems = DatabaseManager.Database.Query<NpcItem>(NpcItemRelator.FetchQuery).ToList();

            foreach(var npc in m_npcs)
            {
                NpcTemplate template = m_templatesRecords.FirstOrDefault(x => x.Id == npc.Id);
                if (template != null)
                    m_npcsDatas.Add(new NpcData(npc, template, m_spawns.Where(x => x.NpcId == npc.Id).ToList(), GetActionByNpc(npc.Id), GetNpcItems(npc.Id), GetReplies(npc.Id)));
            }
        }

        public NpcData GetNpcData(string name)
            => m_npcsDatas.FirstOrDefault(x => x.Name == name);

        public NpcData GetNpcData(int id)
            => m_npcsDatas.FirstOrDefault(x => x.Id == id);

        public List<NpcItem> GetNpcItems(int npcId)
        {
            List<NpcItem> m_items = new List<NpcItem>();
            foreach (var action in GetActionByNpc(npcId).Where(x => x.Type == "Shop"))
                m_items.AddRange(m_npcsItems.Where(x => x.NpcShopId == action.Id));

            return m_items;
        }

        public List<NpcReplyRecord> GetReplies(int npcId)
        {
            List<NpcReplyRecord> m_replies = new List<NpcReplyRecord>();
            foreach (var action in GetActionByNpc(npcId).Where(w => w.Type == "Talk"))
            {
                foreach(var reply in m_replysRecords.Where(x => x.MessageId == int.Parse(action.Parameter0)))
                {
                    if (!m_replies.Contains(reply))
                        m_replies.Add(reply);
                }
            }

            return m_replies;
        }

        public List<NpcActionRecord> GetActionByNpc(int npcId)
            => m_actionsRecords.Where(x => x.NpcId == npcId).ToList();

        public string[] GetNpcsIds()
            => m_npcsDatas.Select(x => x.Id.ToString()).ToArray();

        public string[] GetNpcsIds(string id)
            => m_npcsDatas.Where(x => x.Id.ToString().Contains(id)).Select(a => a.Id.ToString()).ToArray();

        public string[] GetNpcsNames(string name)
            => m_npcsDatas.Where(x => x.Name.Contains(name)).Select(x => x.Name).ToArray();
        public string[] GetNpcsNames()
            => m_npcsDatas.Select(x => x.Name).ToArray();

        public int? GetNpcIdByActionId(int actionId)
            => m_npcsDatas.FirstOrDefault(x => x.Actions.Any(a => a.Id == actionId))?.Id;

        public int? GetNpcIdByParameter0(string parameter0)
            => m_npcsDatas.FirstOrDefault(x => x.Actions.Any(a => a.Type == "Talk" && a.Parameter0 == parameter0))?.Id;

        public uint GetNextSpawnId()
            => m_spawns.OrderBy(x => x.Id).Last().Id + 1;

        public int GetNextShopId()
            => m_npcsItems.OrderBy(x => x.Id).Last().Id + 1;

        public int GetNextReplyId()
            => m_replysRecords.OrderBy(x => x.Id).Last().Id + 1;

        public uint GetNextActionId()
            => m_actionsRecords.OrderBy(x => x.Id).Last().Id + 1;

        public void AddNpcData(NpcData data)
            => m_npcsDatas.Add(data);
        
        public void DeleteNpc(NpcData data)
        {
            m_npcsDatas.Remove(data);
            m_npcsRemove.Add(data);
        }

        public void Save(NpcData data)
        {
            D2OWriter writer = new D2OWriter(@".\common\Npcs.d2o");

            writer.StartWriting(true);
            writer.Write(data.NpcD2O, data.NpcD2O.Id);
            writer.EndWriting();

            if (m_templatesRecords.Any(x => x.Id == data.Id))
                DatabaseManager.Database.Update(data.NpcRecord);
            else
                DatabaseManager.Database.Insert(data.NpcRecord);

            foreach (var action in data.Actions)
            {
                if (m_actionsRecords.Any(x => x.Id == action.Id))
                    DatabaseManager.Database.Update(action);
                else
                    DatabaseManager.Database.Insert(action);
            }

            foreach (var replies in data.NpcReplies)
            {
                if (m_replysRecords.Any(x => x.Id == replies.Id))
                    DatabaseManager.Database.Update(replies);
                else
                    DatabaseManager.Database.Insert(replies);
            }

            foreach (var spawn in data.Spawns)
            {
                if (m_spawns.Any(x => x.Id == spawn.Id))
                    DatabaseManager.Database.Update(spawn);
                else
                    DatabaseManager.Database.Insert(spawn);
            }

            foreach (var shop in data.Items)
            {
                if (m_npcsItems.Any(x => x.Id == shop.Id))
                    DatabaseManager.Database.Update(shop);
                else
                    DatabaseManager.Database.Insert(shop);
            }

            m_templatesRecords = DatabaseManager.Database.Query<NpcTemplate>(NpcTemplateRelator.FetchQuery).ToList();
            m_spawns = DatabaseManager.Database.Query<NpcSpawn>(NpcSpawnRelator.FetchQuery).ToList();
            m_replysRecords = DatabaseManager.Database.Query<NpcReplyRecord>(NpcReplyRecordRelator.FetchQuery).ToList();
            m_actionsRecords = DatabaseManager.Database.Query<NpcActionRecord>(NpcActionRecordRelator.FetchQuery).ToList();
            m_npcsItems = DatabaseManager.Database.Query<NpcItem>(NpcItemRelator.FetchQuery).ToList();
        }
    }
}
