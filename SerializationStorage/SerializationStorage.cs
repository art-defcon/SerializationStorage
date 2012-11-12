using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace SerializationStorage
{
    /// <summary>
    /// SerializationStorage is a micro db that persists a xml serialized
    /// copy of it self. When disposed it persists it self again.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SerializationStorage<T> : List<T>, IDisposable
    {
        readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<T>));
        readonly string _file = typeof(T).Name + ".ses";

        public SerializationStorage()
        {
            if (!File.Exists(_file)) 
                return;
            
            using (var sr = new XmlTextReader(_file))
            {
                var list = _serializer.Deserialize(sr) as List<T>;
                if (list != null) 
                    AddRange(list);
            }
        }

        public void Dispose()
        {
            File.Delete(_file);
            if (this.Any())
            {
                using (var stream = new FileStream(_file, FileMode.OpenOrCreate))
                using (var fs = new StreamWriter(stream))
                    _serializer.Serialize(fs, this);
            }
        }
    }
}
