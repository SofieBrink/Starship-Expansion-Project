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

        [SerializeField]
        private List<SEPEngineSet> EngineSets = new List<SEPEngineSet>();
        public override void OnLoad(ConfigNode val)
        {
            Debug.Log($"[{MODULENAME}] Present OnLoad in Scene: {HighLogic.LoadedScene}");
            if (HighLogic.LoadedScene == GameScenes.LOADING)
            {
                foreach (var node in val.GetNodes(nameof(SEPEngineSet)))
                {
                    var tmpEngineSet = ScriptableObject.CreateInstance<SEPEngineSet>();
                    tmpEngineSet.Construct(pPart: part, pConfNode: node);
                    EngineSets.Add(tmpEngineSet);
                }
            }
        }

        public override void OnStart(StartState state)
        {
            foreach (var node in EngineSets)
            {
                foreach (var engineTransform in node.engineTransforms)
                {
                    Debug.Log($"[{MODULENAME}] Transform: {engineTransform.Key} with value: {engineTransform.Value}");
                }
            }
        }
    }
}
