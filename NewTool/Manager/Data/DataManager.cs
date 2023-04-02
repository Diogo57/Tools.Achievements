using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Achievements
{
    public class DataManager
    {
        public static readonly Dictionary<Type, D2OReader> readers = new Dictionary<Type, D2OReader>();

        private static string Directory;

        public static void Initialize(string directory)
        {
            Directory = directory;
            foreach (var d2iFile in System.IO.Directory.EnumerateFiles(directory).Where(entry => entry.EndsWith(".d2o")))
            {
                try
                {
                    AddReader(new D2OReader(d2iFile));
                }
                catch
                {
                    var caca = d2iFile.Length;
                }
            }
        }

        public static void AddReader(D2OReader d2oFile)
        {
            var classes = d2oFile.Classes;

            foreach (var @class in classes)
            {
                if (readers.ContainsKey(@class.Value.ClassType))
                {
                    readers.Remove(@class.Value.ClassType);
                }
                else
                {
                    readers.Add(@class.Value.ClassType, d2oFile);
                }
            }
        }

        public static List<object> AddSingleReader(string file)
        {
            var d2oFile = new D2OReader(file);
            readers.Clear();

            return d2oFile.ReadObjects<object>(true).Values.ToList();
        }

        public static T Get<T>(uint key)
            where T : class
        {
            return Get<T>((int)key);
        }

        public static T Get<T>(int key, bool noExceptionThrown = false)
            where T : class
        {
            if (!readers.ContainsKey(typeof(T)))
            {
                throw new Exception("Missing D2O " + typeof(T).ToString().Replace("AmaknaProxy.API.Protocol.Data.", ""));
            }

            var reader = readers[typeof(T)];

            return reader.ReadObject<T>(key, true);
        }

        public static List<T> GetAll<T>()
            where T : class
        {
            if (!readers.ContainsKey(typeof(T)))
                throw new ArgumentException("Cannot find data corresponding to type : " + typeof(T));

            var reader = readers[typeof(T)];

            return reader.ReadObjects<T>(true).Values.ToList();
        }

        public static IEnumerable<Type> GetAllTypes()
        {
            return readers.Keys;
        }

        private static IEnumerable<object> EnumerateObjects(Type type)
        {
            if (!readers.ContainsKey(type))
                throw new ArgumentException("Cannot find data corresponding to type : " + type);

            var reader = readers[type];

            return reader.Indexes.Select(index => reader.ReadObject(index.Key, true)).Where(obj => obj.GetType().Name == type.Name);
        }

        public static IEnumerable<T> EnumerateObjects<T>() where T : class
        {
            if (!readers.ContainsKey(typeof(T)))
                throw new ArgumentException("Cannot find data corresponding to type : " + typeof(T));

            var reader = readers[typeof(T)];

            return reader.Indexes.Select(index => reader.ReadObject(index.Key, true)).OfType<T>().Select(obj => obj);
        }

        public static void Dispose()
        {
            readers.Clear();
        }
    }
}
