using System.Collections.Generic;
using UnityEngine;
using RPG.Creatures;

namespace RPG.Abilitys.Perk {

    //Filter out friendly units
    public class FriendlyPerk : AbstractAbilityPerk {
        public FriendlyPerk(int energy) : base(energy) {

        }

        public override object GetPerkStorage(AbstractFormBehavior formBehavior) =>
            new FriendlyPerkStorage(this, formBehavior.GetUser);
        

        public GameObject[] Filter(Creature user, GameObject[] list) {

            List<GameObject> listResult = new List<GameObject>(list);

            foreach (var target in list) {

                if (target.TryGetComponent(out Creature creatre) && creatre == user) {

                    listResult.Remove(target);
                }
            }

            return listResult.ToArray();
        }

        //Filters out user
        //In the future filter through factions of the user
        public class FriendlyPerkStorage : IFilter {

            FriendlyPerk perk;
            Creature user;

            public FriendlyPerkStorage(FriendlyPerk perk, Creature user) {

                this.perk = perk;
                this.user = user;
                 
            }

            public GameObject[] Filter(GameObject[] list) => perk.Filter(user, list);
        }
    }
}