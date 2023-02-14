using System;
using RPG.Creatures;

namespace RPG.Actions.Requirement {
    public static class RequirementMethods {

        //Check
        /// <summary>
        /// Check actionHandler if its currently busy
        /// </summary>
        public static bool CheckIfBusy(Creature creature) => creature.ActionHandler.IsBusy;
        public static bool CheckIfIncapacitated(Creature creature) => creature.IsIncapacitated;
        public static bool CheckIfMoving(Creature creature) => creature.creatureControler.IsMoving;

        //Listen
        /// <summary>
        /// Listen if isBusy value changes
        /// </summary>
        /// <param name="listener"> What action to call when value changes </param>
        public static void ListenToBusy(Creature creature, Action<bool> listener) {

            PerformActionHandler actionHandler = creature.ActionHandler;
            actionHandler.IsBusyObserver.AddUpdateMethod(listener);
        }

        /// <summary>
        /// Listen if incapacitated value changes
        /// </summary>
        /// <param name="listener"> What action to call when value changes </param>
        public static void ListenToIncapacitated(Creature creature, Action<bool> listener) {

            creature.IsIncapacitatedObserver.AddUpdateMethod(listener);
        }

        /// <summary>
        /// Listen if moving value changes
        /// </summary>
        /// <param name="listener"> What action to call when value changes </param>
        public static void ListenToMoving(Creature creature, Action<bool> listener) {

            ICreatureControler creatureControler = creature.creatureControler;
            creatureControler.MovingObserver.AddUpdateMethod(listener);
        }

        //Ignore

        /// <summary>
        /// Ignore if isBusy value changes
        /// </summary>
        /// <param name="listener"> What action to ignore </param>
        public static void IgnoreBusy(Creature creature, Action<bool> listener) {

            PerformActionHandler actionHandler = creature.ActionHandler;
            actionHandler.IsBusyObserver.RemoveUpdateMethod(listener);
        }

        /// <summary>
        /// Ignore if Incapacitated value change
        /// </summary>
        /// <param name="listener"> What action to ignore </param>
        public static void IgnoreIncapacitated(Creature creature, Action<bool> listener) {

            creature.IsIncapacitatedObserver.RemoveUpdateMethod(listener);
        }

        /// <summary>
        /// Ignore if moving value change
        /// </summary>
        /// <param name="listener"> What action to ignore </param>
        public static void IgnoreMoving(Creature creature, Action<bool> listener) {

            ICreatureControler creatureControler = creature.creatureControler;
            creatureControler.MovingObserver.RemoveUpdateMethod(listener);
        }
    }
}
