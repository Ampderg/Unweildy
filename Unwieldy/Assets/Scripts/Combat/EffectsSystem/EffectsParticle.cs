using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class EffectsParticle : BaseEffect
{
    [SerializeField]
    private new ParticleSystem particleSystem;
    public override void Trigger()
    {
        particleSystem.Play();
    }
}
