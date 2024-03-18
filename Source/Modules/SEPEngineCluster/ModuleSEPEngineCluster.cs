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
        private const string MODULENAME = "ModuleSEPEngineCluster";

        private List<SEPEngineSet> EngineSets = new List<SEPEngineSet>();
        public override void OnLoad(ConfigNode val)
        {
            Debug.Log($"[{MODULENAME}] Present OnLoad");

            foreach (var node in val.GetNodes(nameof(SEPEngineSet)))
            {
                EngineSets.Add(new SEPEngineSet(pPart: part, pConfNode: node));
            }
   //         if (val.HasNode(nameof(ModuleSEPEngineSet)))
   //         {
			//	var tmp = val.GetNode(nameof(ModuleSEPEngineSet));
			//	Debug.Log(tmp);
   //             part.AddModule(tmp);
			//}
			//else
   //         {
   //             Debug.Log(val);
   //         }
            for (int i = 0; i < part.Modules.Count; i++)
			{
                Debug.Log(part.Modules.GetModule(i).moduleName);
			}

            Debug.Log($"[{MODULENAME}] Scene: {HighLogic.LoadedScene}");
        }

		public override void OnSave(ConfigNode node)
		{
            Debug.Log($"[{MODULENAME}] OnSave1 {node}");
            foreach(var engineSet in EngineSets)
            {
                node.AddNode(engineSet.ConstructToConfignode());
            }
			Debug.Log($"[{MODULENAME}] OnSave2 {node}");
		}
	}
}
