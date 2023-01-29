using UnityEngine;
using RPG.Inventory;

namespace RPG.Creatures {
    /// <summary>
    /// Manages equipment placement and Ability calls to the equipment
    /// </summary>
    public class EquipmentManager {

        Creature creature;
        Actions.PerformActionHandler performActionHandler;

        Transform rightEquipmentPlacement;
        Transform leftEquipmentPlacement;

        Equipment weaponEquipment;

        public EquipmentManager(Creature creature) {

            this.creature = creature;
            performActionHandler = creature.ActionHandler;

            SetItemPlacements();

            //During testing a weapon is already placed in its right hand
            weaponEquipment = rightEquipmentPlacement.GetComponentInChildren<Equipment>();
        }

        //Player/AI Input calls this method
        /// <summary>
        /// Sends Ability Performance Action (IPerformAction) for the creature to perform the action
        /// </summary>
        public void ActivateWeaponEquipmentQuick() {

            if (performActionHandler.isBusy) return;
            //Send action to action handler
            performActionHandler.StartAction(weaponEquipment.GetAbilityPerformAction(creature)); 
        }
        /// <summary>
        /// Currently not in use as Channel Ability isn't implemented yet
        /// </summary>
        public void ActivateWeaponEquipmentChannel() => weaponEquipment.ActivateChannelAbility(creature);
        public Equipment GetWeaponEquipment => weaponEquipment;
        //Main Weapon
        public void SetRightWeaponEquipment(Equipment equipment) {

            SetEquipmentPosition(equipment, rightEquipmentPlacement);
            weaponEquipment = equipment;
        }

        //Off hand weapon/Shield
        public void SetLeftWeaponEquipment(Equipment equipment) {

            SetEquipmentPosition(equipment, leftEquipmentPlacement);
        }
        private void SetEquipmentPosition(Equipment equipment, Transform equipmentPlacement) {

            equipment.transform.SetParent(equipmentPlacement, false);
            equipment.transform.position = equipmentPlacement.position;
        }

        private void SetItemPlacements() {

            foreach (var holder in creature.GetComponentsInChildren<ItemHolder>()) {

                switch (holder.GetPlacement) {
                    case ItemHolder.ItemPlacement.RightArm:
                        rightEquipmentPlacement = holder.transform;
                        break;
                    case ItemHolder.ItemPlacement.LeftArm:
                        leftEquipmentPlacement = holder.transform;
                        break;
                    default:
                        break;
                }
            }
            
            if (rightEquipmentPlacement == null) throw new System.Exception("Right Equipment Placement missing");
            if (leftEquipmentPlacement == null) throw new System.Exception("Left Equipment Placement missing");
        }
    }

}

