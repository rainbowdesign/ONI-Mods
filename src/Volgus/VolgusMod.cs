using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;
using Object = System.Object;

namespace Volgus
{
	class VolgusMod
	{
		[HarmonyPatch(typeof(CodexEntryGenerator), "GenerateCreatureEntries")]
		public class CodexPatch
		{
			private static void Postfix(ref Dictionary<string, CodexEntry> __result)
			{
				CodexEntry entry = new CodexEntry("CREATURES", new List<ContentContainer> { new ContentContainer(
					new List<CodexWidget>() { new CodexWidget(CodexWidget.ContentType.Spacer), new CodexWidget(CodexWidget.ContentType.Spacer) }, ContentContainer.ContentLayout.Vertical) }, VolgusConfig.Name);
				entry.parentId = "CREATURES";
				CodexCache.AddEntry(VolgusConfig.VolgusSpecies.ToString(), entry, (List<CategoryEntry>)null);
				__result.Add(VolgusConfig.VolgusSpecies.ToString(), entry);
			}
		}


		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class VolgusEntityConfigManagerPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.CREATURES.SPECIES.VOLGUS.NAME", VolgusConfig.Name);
				Strings.Add("STRINGS.CREATURES.SPECIES.VOLGUS.DESC", VolgusConfig.Description);
				Strings.Add("STRINGS.CREATURES.FAMILY.VOLGUS", VolgusConfig.Name);
				Strings.Add("STRINGS.CREATURES.FAMILY_PLURAL.VOLGUS", "Volgi");
			}

			private static void Postfix()
			{
				object volgus = Activator.CreateInstance(typeof(VolgusConfig));
				EntityConfigManager.Instance.RegisterEntity(volgus as IEntityConfig);
			}
		}
	}
}
