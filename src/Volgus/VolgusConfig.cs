using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using Klei.AI;
using UnityEngine;

namespace Volgus
{
	public class VolgusConfig : IEntityConfig
	{
		public const string ID = "Volgus";
		public const string BASE_TRAIT_ID = "VolgusBaseTrait";
		public static readonly Tag VolgusSpecies = TagManager.Create(nameof(VolgusSpecies));

		public const string Name = "Volgus";
		public const string Description = "A skittish mammalian creature.\n\nIt moves in herds for safety.";

		public GameObject CreatePrefab()
		{
			var an = Assets.GetAnim("gronehog_kanim");
			for (int i = 0; i < an.GetData().animCount; i++)
			{
				UnityEngine.Debug.Log(an.GetData().GetAnim(i).name);
			}

			GameObject placedEntity = EntityTemplates.CreatePlacedEntity(ID, Name, Description, 25f,
				Assets.GetAnim((HashedString) "gronehog_kanim"), "idle",
				Grid.SceneLayer.Creatures, 3, 2, TUNING.DECOR.BONUS.TIER0);

			Db.Get().CreateTrait(BASE_TRAIT_ID, Name, Name, (string) null, false, (ChoreGroup[]) null, true, true)
				.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, Name, false, false, true));

			KPrefabID component = placedEntity.GetComponent<KPrefabID>();
			component.AddTag(GameTags.Creatures.GroundBased);
			component.prefabInitFn +=
				(KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));

			EntityTemplates.ExtendEntityToBasicCreature(placedEntity, FactionManager.FactionID.Pest, "GlomBaseTrait",
				"HatchNavGrid", NavType.Floor, 32, 2f, "", 0, true, true, 293.15f, 393.15f, 273.15f, 423.15f);

			placedEntity.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0.0f);
			placedEntity.AddOrGet<Trappable>();
			placedEntity.AddOrGetDef<ThreatMonitor.Def>();
			placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();

			//ElementDropperMonitor.Def def = placedEntity.AddOrGetDef<ElementDropperMonitor.Def>();
			//def.dirtyEmitElement = SimHashes.ContaminatedOxygen;
			//def.dirtyProbabilityPercent = 25f;
			//def.dirtyCellToTargetMass = 1f;
			//def.dirtyMassPerDirty = 0.2f;
			//def.dirtyMassReleaseOnDeath = 3f;
			//def.emitDiseaseIdx = Db.Get().Diseases.GetIndex((HashedString) "SlimeLung");
			//def.emitDiseasePerKg = 1000f;

			placedEntity.AddOrGet<LoopingSounds>();
			placedEntity.GetComponent<LoopingSounds>().updatePosition = true;

			//SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_movement_short",
			//	TUNING.NOISE_POLLUTION.CREATURES.TIER2);
			//SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_jump", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
			//SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_land", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
			//SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_expel", TUNING.NOISE_POLLUTION.CREATURES.TIER4);
			//EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, false);

			

			ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def());
			EntityTemplates.AddCreatureBrain(placedEntity, chore_table, VolgusSpecies, (string)null);
			return placedEntity;
		}

		public void OnPrefabInit(GameObject prefab)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
