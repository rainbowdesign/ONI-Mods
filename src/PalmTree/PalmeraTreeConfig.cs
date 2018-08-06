﻿using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;
using CREATURES = STRINGS.CREATURES;

namespace PalmeraTree
{
	public class PalmeraTreeConfig : IEntityConfig
	{
		public const float FERTILIZATION_RATE = 0.006666667f;
		public const string ID = "PalmeraTreePlant";
		public const string SEED_ID = "PalmeraTreeSeed";

		public GameObject CreatePrefab()
		{
			GameObject placedEntity = EntityTemplates.CreatePlacedEntity(ID, CREATURES.SPECIES.JUNGLEGASPLANT.NAME,CREATURES.SPECIES.JUNGLEGASPLANT.DESC, 1f,
				Assets.GetAnim((HashedString)"palmeratree_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 4, DECOR.BONUS.TIER1);
			EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 268.15f, 278.15f, 293.15f, 296.15f, 308.15f, 318.15f, new SimHashes[1] { SimHashes.ChlorineGas }, true, 0.0f, 0.15f, PalmeraBerryConfig.ID, true, true);

			placedEntity.AddOrGet<PalmeraTree>();

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Harvest, SEED_ID,
					"Palmera Tree Seed", "The " + UI.FormatAsLink("Seed", "PLANTS") + " of a " + CREATURES.SPECIES.JUNGLEGASPLANT.NAME + ".\n\nDigging up Buried Objects may uncover a Palmera Tree Seed.",
					Assets.GetAnim((HashedString)"seed_palmeratree_kanim"), "object", 0, new List<Tag> { GameTags.CropSeed },
					SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 6, CREATURES.SPECIES.JUNGLEGASPLANT.DOMESTICATEDDESC,
					EntityTemplates.CollisionShape.CIRCLE, 0.33f, 0.33f, null, string.Empty), "PalmeraTree_preview", Assets.GetAnim((HashedString)"palmeratree_kanim"), "idle_bloom_loop", 1, 4);

			SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
			SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);

			return placedEntity;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
