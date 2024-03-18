using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StarshipExpansionProject.Modules.SEPEngineCluster
{
    public class SEPEngineSet : ScriptableObject
    {
        private const string MODULENAME = "SEPEngineSet";

        public string id { get; private set; }

        private int startupDelay = 0;

        private int shutdownDelay = 0;

		private float SingleEngineMinThrust = 0;

		private float SingleEngineMaxThrust;

		private float SingleEngineHeatProduction;

		private float SingleEngineMass;

        public Dictionary<string, bool> engineTransforms = new Dictionary<string, bool>();

		public float EngineSetMass => SingleEngineMass * engineTransforms.Select(e => e.Value).Count();

		public void Construct(Part pPart, ConfigNode pConfNode)
        {
            if (pConfNode.HasValue(nameof(id))) id = pConfNode.GetValue(nameof(id));
            else Debug.LogError($"[{MODULENAME}] Failed to find key {nameof(id)} in ConfigNode: {pConfNode}");

            if (pConfNode.HasValue(nameof(startupDelay))) startupDelay = Convert.ToInt32(pConfNode.GetValue(nameof(startupDelay)));
            else Debug.Log($"[{MODULENAME}] Failed to find key {nameof(startupDelay)} using default");

			if (pConfNode.HasValue(nameof(shutdownDelay))) startupDelay = Convert.ToInt32(pConfNode.GetValue(nameof(shutdownDelay)));
			else Debug.Log($"[{MODULENAME}] Failed to find key {nameof(shutdownDelay)} using default");

			if (pConfNode.HasValue(nameof(SingleEngineMinThrust))) SingleEngineMinThrust = float.Parse(pConfNode.GetValue(nameof(SingleEngineMinThrust)));
			else Debug.Log($"[{MODULENAME}] Failed to find key {nameof(SingleEngineMinThrust)} using default");

			if (pConfNode.HasValue(nameof(SingleEngineMaxThrust))) SingleEngineMaxThrust = float.Parse(pConfNode.GetValue(nameof(SingleEngineMaxThrust)));
			else Debug.LogError($"[{MODULENAME}] Failed to find key {nameof(SingleEngineMaxThrust)} in ConfigNode: {pConfNode}");

			if (pConfNode.HasValue(nameof(SingleEngineHeatProduction))) SingleEngineHeatProduction = float.Parse(pConfNode.GetValue(nameof(SingleEngineHeatProduction)));
			else Debug.LogError($"[{MODULENAME}] Failed to find key {nameof(SingleEngineHeatProduction)} in ConfigNode: {pConfNode}");

			if (pConfNode.HasValue(nameof(SingleEngineMass))) SingleEngineMass = float.Parse(pConfNode.GetValue(nameof(SingleEngineMass)));
			else Debug.LogError($"[{MODULENAME}] Failed to find key {nameof(SingleEngineMass)} in ConfigNode: {pConfNode}");

			if (pConfNode.HasValue(nameof(engineTransforms)))
            {
				var tmpString = pConfNode.GetValue(nameof(engineTransforms)).Replace(" ", "").Split(',');

				engineTransforms = tmpString
					.Select(key =>
					{
						if (key.StartsWith("(") && key.EndsWith(")"))
						{
							var splitKey = key.Split(';');
							return new KeyValuePair<string, bool>(splitKey[0], Convert.ToBoolean(splitKey[1]));
						}
						else
						{
							return new KeyValuePair<string, bool>(key, true);
						}
					})
					.ToDictionary(kv => kv.Key, kv => kv.Value);

				foreach (var engineTransform in engineTransforms)
				{
					Debug.Log($"[{MODULENAME}] Transform: {engineTransform.Key} with value: {engineTransform.Value}");
				}
			}
			else Debug.LogError($"[{MODULENAME}] Failed to find key {nameof(engineTransforms)} in ConfigNode: {pConfNode}");

			ConfigNode tmpNode;
			if (pConfNode.HasNode("EngineNode"))
			{
				tmpNode = pConfNode.GetNode("EngineNode");

				tmpNode.AddValue("name", nameof(ModuleEnginesFX));
				tmpNode.AddValue("engineID", id);

				pPart.AddModule(tmpNode);

				SetThrustLevels(pPart);
			}
			else Debug.LogError($"[{MODULENAME}] Failed to find key EngineNode in ConfigNode: {pConfNode}");
		}

		public void SetThrustLevels(Part pPart)
		{
			var tmpEngineModule = pPart.Modules.GetModules<ModuleEnginesFX>().Where(m => m.engineID == id).FirstOrDefault();
			if (tmpEngineModule != null)
			{
				tmpEngineModule.minThrust = SingleEngineMinThrust * engineTransforms.Select(e => e.Value).Count();
				tmpEngineModule.maxThrust = SingleEngineMaxThrust * engineTransforms.Select(e => e.Value).Count();
				tmpEngineModule.heatProduction = SingleEngineHeatProduction * engineTransforms.Select(e => e.Value).Count();
				tmpEngineModule.minFuelFlow = tmpEngineModule.minThrust > 0 ? tmpEngineModule.minThrust / tmpEngineModule.atmosphereCurve.Curve.keys[0].value / 9.80665f : 0;
				tmpEngineModule.maxFuelFlow = tmpEngineModule.maxThrust / tmpEngineModule.atmosphereCurve.Curve.keys[0].value / 9.80665f;
			}
			else Debug.LogError($"[{MODULENAME}] Unable to find corrosponding ModuleEngineFx");
		}

		public void UpdateTransforms(Part pPart)
		{

		}

	}

}
