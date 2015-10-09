using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS
{
    public class TSPlugin
    {
        #region singleton
        private readonly static Lazy<TSPlugin> _instance = new Lazy<TSPlugin>(() => new TSPlugin());

        private TSPlugin()
        {

        }

        public static TSPlugin Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        #endregion
        public TS3Functions Functions { get; set; }
        

        public string PluginName = "Testing Plugin";
        public string PluginVersion = "0.2";
        public int ApiVersion = 20;
        public string Author = "Birdboat";
        public string Description = "A plugin for some ts server";
        public string PluginID { get; set; }
        public int Init()
        {
            return 0;
        }
        public void Shutdown()
        {

        }
    }
}
