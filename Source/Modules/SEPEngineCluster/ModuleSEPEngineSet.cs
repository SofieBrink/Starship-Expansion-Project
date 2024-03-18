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

        public int startupDelay = 0;

        public int shutdownDelay = 0;

        public Dictionary<string, bool> engineTransforms = new Dictionary<string, bool>();

		public SEPEngineSet(Part pPart, ConfigNode pConfNode)
        {
			ConstructFromConfignode(pConfNode);
			if (pConfNode.HasNode("TmpTestAdd")) pPart.AddModule(pConfNode.GetNode("TmpTestAdd"));
        }

        public void ConstructFromConfignode(ConfigNode pConfNode)
        {
            if (pConfNode.HasValue(nameof(id))) id = pConfNode.GetValue(nameof(id));
            else Debug.LogError($"[{MODULENAME}] Failed to find key {nameof(id)} in ConfigNode: {pConfNode}");

            if (pConfNode.HasValue(nameof(startupDelay))) startupDelay = Convert.ToInt32(pConfNode.GetValue(nameof(startupDelay)));
            else Debug.LogError($"[{MODULENAME}] Failed to find key {nameof(startupDelay)} in ConfigNode: {pConfNode}");

			if (pConfNode.HasValue(nameof(shutdownDelay))) startupDelay = Convert.ToInt32(pConfNode.GetValue(nameof(shutdownDelay)));
			else Debug.LogError($"[{MODULENAME}] Failed to find key {nameof(shutdownDelay)} in ConfigNode: {pConfNode}");

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
		}

		public ConfigNode ConstructToConfignode()
        {
			ConfigNode returnNode = new ConfigNode(nameof(SEPEngineSet));

			returnNode.AddValue(nameof(id), id);
			returnNode.AddValue(nameof(startupDelay), startupDelay);
			returnNode.AddValue(nameof(shutdownDelay), shutdownDelay);

			//StringBuilder tmpEngineTransforms = new StringBuilder();
			//foreach (var engineTransform in engineTransforms)
			//{
			//	if (engineTransform.Value)
			//	{
			//		tmpEngineTransforms.Append($"{engineTransform.Key}, ");
			//	}
			//	else
			//	{
			//		tmpEngineTransforms.Append($"({engineTransform.Key}; false), ");
			//	}
			//}
			//returnNode.AddValue(nameof(engineTransforms), tmpEngineTransforms.ToString());

			string tmpEngineTransforms =
			    string.Join(", ", engineTransforms.Select(kv => kv.Value 
															  ? kv.Key 
															  : $"({kv.Key}; {kv.Value})"));

			returnNode.AddValue(nameof(engineTransforms), tmpEngineTransforms);
			return returnNode;
        }
    }
}
