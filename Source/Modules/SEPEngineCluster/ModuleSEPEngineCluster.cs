using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using StarshipExpansionProject.Modules.SEPEngineCluster;

namespace StarshipExpansionProject.Modules
{
    public class ModuleSEPEngineCluster : PartModule
    {
        private static string MODULENAME = "ModuleSEPEngineCluster";
        public override void OnLoad(ConfigNode val)
        {
            Debug.Log($"[{MODULENAME}] Present OnLoad");

            var tmp = val.GetNode(nameof(ModuleSEPEngineSet));
            Debug.Log(tmp);
            part.AddModule(tmp);
            for (int i = 0; i < part.Modules.Count; i++)
			{
                Debug.Log(part.Modules.GetModule(i).moduleName);
			}
        }
    }
}
