using RPG.Abilitys.Form;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Creatures;
using System.Linq;

public class BehaviorExplosion : AbstractFormBehavior {

    
    float explosionRadius = 2f;
    int pointsAmount = 3;


    public override void StartForm(RPG.Abilitys.Ability.AbilityBaseInfo abilityBaseInfo, AbstractForm form) {
        base.StartForm(abilityBaseInfo, form);


        GameObject[] _Targets = Physics.OverlapSphere(transform.position, explosionRadius).Select(x => 
            x.gameObject).ToArray();

        SendGameObject(_Targets);
    }

    public override Vector3[] GetTargetPositions() =>
        RPG.UtilityForm.GetCircleRandomPoints(transform, explosionRadius, pointsAmount);


    public override GameObject[] GetTargetObjects() =>
        Physics.OverlapSphere(transform.position, explosionRadius).Select(x => x.gameObject).ToArray();

    /*---Protected---*/

    protected override Vector3 GetStartPosition() => transform.position;
    protected override Vector3 GetEndPosition() => transform.position;
    protected override Vector3 GetTickPosition() => transform.position;
    protected override void ExtraUpdate() { }

    /*---Private---*/

    private void Start() {
        _LifeTime = 2f;

    }
}
