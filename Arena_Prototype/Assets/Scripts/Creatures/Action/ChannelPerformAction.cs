using UnityEngine;
using RPG.Creatures;
using RPG.Combat;
using RPG.Abilitys;

namespace RPG.Actions {
    public class ChannelPerformAction : AbilityPerformAction {

        int powerupEnergy = 0;
        bool isPoweringUp = false;

        public ChannelPerformAction(Ability ability, Creature user, IAbilityTargetingObject equipment) : base(ability, user, equipment) {

            animationInfo.boolens.Add(("Channel", true));
        }

        //Powerup

        public override UtilityAnimations.AnimationInfo Perform(Creature creature) {

            //Listen for Channel message

            SubToAnimatorMessage(OnChannelMessage);
            user.EquipmentManager.SubToChannelRelese(OnChannelControlRelease);

            return base.Perform(creature);
        }

        public override void Update() {

            //Increase Energy
            if (isPoweringUp) {
                powerupEnergy += (int)(Time.deltaTime * 100);
            }

        }

        public override void Finish() {
            Cancel();
        }

        public override void Cancel() {

            user.EquipmentManager.UnsubToChannelRelese(OnChannelControlRelease);
            base.Cancel();
        }

        //When channel button is released. Ex Right mouse button
        private void OnChannelControlRelease() {

            SetEnergy(energy + powerupEnergy);
            user.ActionHandler.AnimatorHandler.SetAnimatorBool("Channel", false);
        }

        private void OnChannelMessage(AbilityPerformAction abilityPerformAction, int layerIndex, string message) {

            if (UtilityAnimations.UPPERBODY_INDEX_LAYER != layerIndex &&
                    UtilityAnimations.AnimatorMessageType.ACTION_CHANNEL.ToString() != message) return;

            isPoweringUp = true;
        }
    }
}