namespace RPG.Abilitys.Perk {
    
    public abstract class AbstractAbilityPerk {
        /// <summary>
        /// An extra perk for an ability
        /// </summary>
        public AbstractAbilityPerk(int energy) { }

        /// <summary>
        /// Get storage information related to the perk
        /// </summary>
        /// <param name="formBehavior">The instance of the formBehavior</param>
        /// <returns></returns>
        public abstract object GetPerkStorage(AbstractFormBehavior formBehavior);


    }

}
