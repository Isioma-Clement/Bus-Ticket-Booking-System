using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ticket_purchase_system
{
    public class TickettingConfigurations
    {

        public static string tickettingConfigFilePath = "tickettingConfigurationSettings.txt";

        // creates the configuration file only if previously non existent.
        public static void CreateConfigFile()
        {
            if (!File.Exists(tickettingConfigFilePath))
            {
                StreamWriter configWriter = new StreamWriter(tickettingConfigFilePath);
                configWriter.Close();
            }
            
        }
    }
}
