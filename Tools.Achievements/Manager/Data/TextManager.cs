using Stump.Core.I18N;
using Stump.Server.WorldServer.Database.I18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Achievements.Manager
{
    public class TextManager
    {
        private static Dictionary<uint, LangText> m_texts = new Dictionary<uint, LangText>();

        public static void Initialize()
        {
            m_texts = DatabaseManager.Database.Fetch<LangText>(LangTextRelator.FetchQuery).ToDictionary(entry => entry.Id);
        }

        public static Languages GetDefaultLanguage()
        {
            return Languages.French;
        }

        public static string GetText(int id)
        {
            return GetText(id, GetDefaultLanguage());
        }

        public static string GetText(int id, Languages lang)
        {
            return GetText((uint)id, lang);
        }

        public static string GetText(uint id)
        {
            return GetText(id, GetDefaultLanguage());
        }

        public static string GetText(uint id, Languages lang)
        {
            return !m_texts.TryGetValue(id, out LangText record) ? "(not found)" : GetText(record, lang);
        }

        public static string GetText(LangText record)
        {
            return GetText(record, GetDefaultLanguage());
        }

        public static string GetText(LangText record, Languages lang)
        {
            switch (lang)
            {
                case Languages.English:
                    return record.English ?? "(not found)";
                case Languages.French:
                    return record.French ?? "(not found)";
                case Languages.German:
                    return record.German ?? "(not found)";
                case Languages.Spanish:
                    return record.Spanish ?? "(not found)";
                case Languages.Italian:
                    return record.Italian ?? "(not found)";
                case Languages.Japanish:
                    return record.Japanish ?? "(not found)";
                case Languages.Dutsh:
                    return record.Dutsh ?? "(not found)";
                case Languages.Portugese:
                    return record.Portugese ?? "(not found)";
                case Languages.Russish:
                    return record.Russish ?? "(not found)";
                default:
                    return "(not found)";
            }
        }
    }
}
