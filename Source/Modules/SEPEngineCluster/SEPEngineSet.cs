using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StarshipExpansionProject.Modules.SEPEngineCluster
{
    public class ModuleSEPEngineSet : ModuleEnginesFX
    {
        private static string MODULENAME = "ModuleSEPEngineSet";

        public override void OnLoad(ConfigNode val)
		{
            base.OnLoad(val);
            Debug.Log($"[{MODULENAME}] Present OnLoad");
            Debug.Log(val);

        }
    }
}
