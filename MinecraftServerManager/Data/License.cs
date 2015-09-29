using System;
using System.IO;
using System.Xml.Serialization;

namespace MinecraftServerManager.Data
{
    public class License
    {
        public int Version = 0;

        public License() { }

        public License(int version)
        {
            this.Version = version;
        }

        public static License Deserialize()
        {
            License serverData = new License();
            XmlSerializer serializer = new XmlSerializer(typeof(License));
            StreamReader reader = new StreamReader(Utils.Main.DataDirectory + "License.xml");
            serverData = (License)serializer.Deserialize(reader);
            reader.Close();
            return serverData;
        }

        public void Save()
        {
            XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Data.License));
            StreamWriter file = new StreamWriter(Utils.Main.DataDirectory + "License.xml");
            writer.Serialize(file, this);
            file.Close();
        }
    }
}
