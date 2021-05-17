using MeetApi.MeetApiInterface;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MeetApi.MeetApiSpooler
{
    public class MeetApiPluginLoader
    {


        public static IMeetApiPluginFactory LoadPlugins(string FolderName)
        {
            Type[] pluginTypes = null;
           //Load the DLLs from the Plugins directory
            if (Directory.Exists(FolderName))
            {
                string[] files = Directory.GetFiles(FolderName);
                foreach (string file in files)
                {
                    if (file.EndsWith("Protocol.dll") )
                    {
                       var ass = Assembly.LoadFile(Path.GetFullPath(file));
                        pluginTypes = ass.GetTypes();
                    }
                }
            }

            Type interfaceType = typeof(IMeetApiPluginFactory);
            //Fetch all types that implement the interface IPlugin and are a class

           var factoryType = pluginTypes.Where(x => x.Name.EndsWith("Factory")).FirstOrDefault();
            if (factoryType != null)
                //Create a new instance of all found types
               return (IMeetApiPluginFactory)Activator.CreateInstance(factoryType);
            
            return null;
        }
        
    }
}
