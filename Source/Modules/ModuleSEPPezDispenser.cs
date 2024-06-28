using B9PartSwitch;
using System.Collections.Generic;
using UnityEngine;

namespace StarshipExpansionProject.Modules
{
	public class ModuleSEPPezDispenser : PartModule
	{
		public const string MODULENAME = "ModuleSEPPezDispenser";

		// Fields that can be set in the part's config file and displayed in the part's information window in the game
		[KSPField]
		public string moduleID = "ModuleSEPPezDispenser"; // ID of this module

		[KSPField]
		public string OrginTransform = "PEZTransform";

		[KSPField]
		public Vector3 DispenseDirection;

		[KSPField]
		public float SatOffset;

		[KSPField]
		public float SatHeight;

		[KSPField]
		public int SatNodeSize;

		[KSPField]
		public bool ShowDispensing = false; // Whether to show the current state in the PAW or not

		[KSPField(isPersistant = false, guiActive = true, guiName = "Dispensing")] // Field that shows the current state of the dispenser
		public string CurrentSubtype;

		// Private members
		private Transform Origin;


		// Action to switch to the next subtype in the list, wrapping around to the beginning of the list if necessary
		[KSPAction(guiName = "Toggle Dispensing")]
		public void ToggleAction(KSPActionParam param)
		{

		}

		[KSPEvent(guiName = "Toggle Dispensing")]
		public void ToggleEvent()
		{
			var trmsfm = Origin.GetChild(0);
			var offset = Origin.up * SatHeight;
			trmsfm.localPosition -= offset;
			var node = part.attachNodes.Find(an => an.nodeTransform == trmsfm);
			node.position = trmsfm.position;
			if (node.attachedPart != null)
			{
				node.attachedPart.transform.localPosition -= offset;
			}

		}

		// Update the names of the action buttons to reflect the current value of the ActionName field
		private void UpdateOutfacingValues()
		{

		}

		public void FixedUpdate()
		{

		}
		
		public void Start()
		{
			Origin = part.FindModelTransform(OrginTransform);
			var newTransform = Origin.GetChild(0);

			ConfigNode TemporaryNode = new ConfigNode();
			TemporaryNode.AddValue("name", newTransform.name);
			TemporaryNode.AddValue("transform", newTransform.name);
			TemporaryNode.AddValue("size", SatNodeSize.ToString());
			TemporaryNode.AddValue("method", "FIXED_JOINT");
			part.AddAttachNode(TemporaryNode);

			//part.model
		}

		public override void OnLoad(ConfigNode node)
		{
			base.OnLoad(node);

		}
	}
}