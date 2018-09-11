using System;
using STRINGS;
using UnityEngine;

namespace Volgus
{
	public class DeathStates : GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>
		{
			private GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State loop;
			private GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State pst;

			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = (StateMachine.BaseState)this.loop;
				GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State loop = this.loop;
				string name1 = (string)CREATURES.STATUSITEMS.DEAD.NAME;
				string tooltip1 = (string)CREATURES.STATUSITEMS.DEAD.TOOLTIP;
				StatusItemCategory main = Db.Get().StatusItemCategories.Main;
				string name2 = name1;
				string tooltip2 = tooltip1;
				string empty = string.Empty;
				int num1 = 0;
				int num2 = 4;
				int num3 = 0;
				int num4 = 0;
				int status_overlays = 63486;
				StatusItemCategory category = main;
				loop.ToggleStatusItem(name2, tooltip2, empty, (StatusItem.IconType)num1, (NotificationType)num2, num3 != 0, (SimViewMode)num4, status_overlays, null, null, category).Enter("EnableGravity", (StateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State.Callback)(smi => smi.EnableGravityIfNecessary())).PlayAnim("Death").OnAnimQueueComplete(this.pst).Exit("DisableGravity", (StateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State.Callback)(smi => smi.DisableGravity()));
				this.pst.TriggerOnEnter(GameHashes.DeathAnimComplete, (Func<DeathStates.Instance, object>)null).Enter("Butcher", (StateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State.Callback)(smi =>
				{
					if (!((UnityEngine.Object)smi.gameObject.GetComponent<Butcherable>() != (UnityEngine.Object)null))
						return;
					smi.GetComponent<Butcherable>().OnButcherComplete();
				})).Enter("Destroy", (StateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State.Callback)(smi => smi.gameObject.DeleteObject())).BehaviourComplete(GameTags.Creatures.Die, false);
			}

			public class Def : StateMachine.BaseDef
			{
			}

			public class Instance : GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.GameInstance
			{
				public Instance(Chore<DeathStates.Instance> chore, DeathStates.Def def)
				  : base((IStateMachineTarget)chore, def)
				{
					chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object)GameTags.Creatures.Die);
				}

				public void EnableGravityIfNecessary()
				{
					if (!this.HasTag(GameTags.Creatures.Flyer))
						return;
					GameComps.Gravities.Add(this.smi.gameObject, Vector2.zero, (System.Action)null);
				}

				public void DisableGravity()
				{
					if (!GameComps.Gravities.Has((object)this.smi.gameObject))
						return;
					GameComps.Gravities.Remove(this.smi.gameObject);
				}
			}
		}
}