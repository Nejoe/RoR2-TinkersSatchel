﻿using RoR2;
using UnityEngine;
using System.Collections.ObjectModel;
using TILER2;
using static TILER2.MiscUtil;
using R2API;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace ThinkInvisible.TinkersSatchel {
	public class ShrinkRay : Item<ShrinkRay> {

		////// Item Data //////

		public override string displayName => "Shrink Ray";
		public override ItemTier itemTier => ItemTier.Tier3;
		public override ReadOnlyCollection<ItemTag> itemTags => new ReadOnlyCollection<ItemTag>(new[] { ItemTag.Utility });

		protected override string GetNameString(string langid = null) => displayName;
		protected override string GetPickupString(string langid = null) => "Suppress a single target's non-primary skills and damage.";
		protected override string GetDescString(string langid = null) => $"Once every {icd:N1} seconds, hitting an enemy disables their <style=cIsUtility>non-primary skills</style> and reduces their <style=cIsDamage>damage</style> by 50% for {duration:N1} seconds <style=cStack>(+{duration:N1} seconds per stack)</style>.";
		protected override string GetLoreString(string langid = null) => "";



		////// Config //////

		[AutoConfigRoOSlider("{0:N1} s", 0f, 30f)]
		[AutoConfigUpdateActions(AutoConfigUpdateActionTypes.InvalidateLanguage)]
		[AutoConfig("Effect duration per stack.",
			AutoConfigFlags.None, 0f, float.MaxValue)]
		public float duration { get; private set; } = 3f;

		[AutoConfigRoOSlider("{0:N1} s", 0f, 30f)]
		[AutoConfigUpdateActions(AutoConfigUpdateActionTypes.InvalidateLanguage)]
		[AutoConfig("Internal cooldown of applying the effect. Does not stack.",
			AutoConfigFlags.None, 0f, float.MaxValue)]
		public float icd { get; private set; } = 2.5f;



		////// Other Fields/Properties //////

		public BuffDef shrinkDebuff { get; private set; }
        public GameObject idrPrefab { get; private set; }



        ////// TILER2 Module Setup //////

        public ShrinkRay() {
			modelResource = TinkersSatchelPlugin.resources.LoadAsset<GameObject>("Assets/TinkersSatchel/Prefabs/Items/ShrinkRay.prefab");
			iconResource = TinkersSatchelPlugin.resources.LoadAsset<Sprite>("Assets/TinkersSatchel/Textures/ItemIcons/shrinkRayIcon.png");
            idrPrefab = TinkersSatchelPlugin.resources.LoadAsset<GameObject>("Assets/TinkersSatchel/Prefabs/Items/Display/ShrinkRay.prefab");
        }

        public override void SetupModifyItemDef() {
            base.SetupModifyItemDef();

            CommonCode.RetrieveDefaultMaterials(idrPrefab.GetComponent<ItemDisplay>());

            #region ItemDisplayRule Definitions

            /// Survivors ///
            displayRules.Add("Bandit2Body", new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab,
                childName = "Stomach",
                localPos = new Vector3(0.22045F, -0.06626F, 0.11193F),
                localAngles = new Vector3(359.0299F, 357.3219F, 25.2928F),
                localScale = new Vector3(0.3F, 0.3F, 0.3F)
            });
            displayRules.Add("CaptainBody", new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab,
                childName = "Stomach",
                localPos = new Vector3(0.38728F, 0.00965F, -0.06446F),
                localAngles = new Vector3(31.87035F, 332.9695F, 3.18838F),
                localScale = new Vector3(0.3F, 0.3F, 0.3F)
            });
            displayRules.Add("CommandoBody", new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab,
                childName = "Stomach",
                localPos = new Vector3(0.23353F, -0.00868F, -0.08696F),
                localAngles = new Vector3(27.00084F, 326.5775F, 4.93487F),
                localScale = new Vector3(0.3F, 0.3F, 0.3F)
            });
            displayRules.Add("CrocoBody", new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab,
                childName = "Stomach",
                localPos = new Vector3(-0.6739F, -1.47899F, 1.63122F),
                localAngles = new Vector3(354.4511F, 7.12517F, 355.0916F),
                localScale = new Vector3(3F, 3F, 3F)
            });
            displayRules.Add("EngiBody", new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab,
                childName = "Pelvis",
                localPos = new Vector3(0.24835F, 0.13692F, 0.12219F),
                localAngles = new Vector3(19.74273F, 338.7649F, 343.2596F),
                localScale = new Vector3(0.3F, 0.3F, 0.3F)
            });
            displayRules.Add("HuntressBody", new ItemDisplayRule {
                childName = "Stomach",
                localPos = new Vector3(0.17437F, -0.01902F, 0.11239F),
                localAngles = new Vector3(14.62809F, 338.0782F, 18.2589F),
                localScale = new Vector3(0.3F, 0.3F, 0.3F),
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab
            });
            displayRules.Add("LoaderBody", new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab,
                childName = "MechBase",
                localPos = new Vector3(0.28481F, -0.22564F, -0.12889F),
                localAngles = new Vector3(0.98176F, 51.91312F, 23.00177F),
                localScale = new Vector3(0.3F, 0.3F, 0.3F)
            });
            displayRules.Add("MageBody", new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab,
                childName = "Pelvis",
                localPos = new Vector3(0.16876F, -0.10376F, 0.02998F),
                localAngles = new Vector3(357.5521F, 355.006F, 105.9485F),
                localScale = new Vector3(0.25F, 0.25F, 0.25F)
            });
            displayRules.Add("MercBody", new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab,
                childName = "ThighR",
                localPos = new Vector3(-0.08794F, 0.03176F, -0.06409F),
                localAngles = new Vector3(350.6662F, 317.2625F, 21.97947F),
                localScale = new Vector3(0.3F, 0.3F, 0.3F)
            });
            displayRules.Add("ToolbotBody", new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab,
                childName = "Chest",
                localPos = new Vector3(2.33895F, -0.34548F, 0.80107F),
                localAngles = new Vector3(311.4177F, 7.89006F, 354.1869F),
                localScale = new Vector3(3F, 3F, 3F)
            });
            displayRules.Add("TreebotBody", new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab,
                childName = "PlatformBase",
                localPos = new Vector3(0.75783F, -0.10773F, 0.00385F),
                localAngles = new Vector3(308.2326F, 10.8672F, 329.0782F),
                localScale = new Vector3(1F, 1F, 1F)
            });
            displayRules.Add("RailgunnerBody", new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab,
                childName = "Backpack",
                localPos = new Vector3(0.28636F, -0.3815F, -0.06912F),
                localAngles = new Vector3(352.4358F, 63.85439F, 6.83272F),
                localScale = new Vector3(0.3F, 0.3F, 0.3F)
            });
            displayRules.Add("VoidSurvivorBody", new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = idrPrefab,
                childName = "Stomach",
                localPos = new Vector3(0.17554F, -0.13447F, -0.0436F),
                localAngles = new Vector3(15.08189F, 9.51543F, 15.89409F),
                localScale = new Vector3(0.3F, 0.3F, 0.3F)
            });
            #endregion
        }

        public override void SetupAttributes() {
			base.SetupAttributes();

			shrinkDebuff = ScriptableObject.CreateInstance<BuffDef>();
			shrinkDebuff.buffColor = Color.white;
			shrinkDebuff.canStack = true;
			shrinkDebuff.isDebuff = true;
			shrinkDebuff.name = "TKSATShrink";
			shrinkDebuff.iconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/texDifficultyEasyIcon.png")
				.WaitForCompletion();
			ContentAddition.AddBuffDef(shrinkDebuff);
		}

		public override void Install() {
			base.Install();

            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.OnBuffFinalStackLost += CharacterBody_OnBuffFinalStackLost;
            On.RoR2.CharacterBody.OnBuffFirstStackGained += CharacterBody_OnBuffFirstStackGained;
		}

        public override void Uninstall() {
			base.Uninstall();

			On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
			On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
			On.RoR2.CharacterBody.OnBuffFinalStackLost -= CharacterBody_OnBuffFinalStackLost;
			On.RoR2.CharacterBody.OnBuffFirstStackGained -= CharacterBody_OnBuffFirstStackGained;
		}



		////// Hooks //////

		private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim) {
			orig(self, damageInfo, victim);
			if(NetworkServer.active && damageInfo != null && damageInfo.attacker) {
				var count = GetCount(damageInfo.attacker.GetComponent<CharacterBody>());
				if(count > 0 && damageInfo.attacker != victim) {
					var sricd = damageInfo.attacker.GetComponent<ShrinkRayICDComponent>();
					if(!sricd)
						sricd = damageInfo.attacker.AddComponent<ShrinkRayICDComponent>();
					if(Time.fixedTime - sricd.lastHit > icd) {
						sricd.lastHit = Time.fixedTime;
						var stsd = victim.GetComponent<ServerTimedSkillDisable>();
						if(!stsd) stsd = victim.AddComponent<ServerTimedSkillDisable>();
						stsd.ServerApply(duration * count, SkillSlot.Secondary);
						stsd.ServerApply(duration * count, SkillSlot.Utility);
						stsd.ServerApply(duration * count, SkillSlot.Special);
						if(victim.TryGetComponent<CharacterBody>(out var vbody)) {
							vbody.AddTimedBuff(shrinkDebuff, duration * count);
						}
					}
				}
			}
		}

		private void CharacterBody_OnBuffFirstStackGained(On.RoR2.CharacterBody.orig_OnBuffFirstStackGained orig, CharacterBody self, BuffDef buffDef) {
			orig(self, buffDef);
			if(self && buffDef == shrinkDebuff && self.modelLocator) {
				self.modelLocator.modelTransform.localScale *= 0.5f;
			}
		}

		private void CharacterBody_OnBuffFinalStackLost(On.RoR2.CharacterBody.orig_OnBuffFinalStackLost orig, CharacterBody self, BuffDef buffDef) {
			orig(self, buffDef);
			if(self && buffDef == shrinkDebuff && self.modelLocator) {
				self.modelLocator.modelTransform.localScale *= 2f;
			}
		}

		private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self) {
			orig(self);
			if(self && self.HasBuff(shrinkDebuff)) {
				self.damage *= 0.5f;
			}
		}
	}

	public class ShrinkRayICDComponent : MonoBehaviour {
		public float lastHit = 0f;
    }
}