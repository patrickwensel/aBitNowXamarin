using System;
using Xamarin.Forms;

namespace ABitNow.Effects
{
   
    public class LabelTextKerningEffect : RoutingEffect
    {
        public float Kerning { get; set; }

        public LabelTextKerningEffect() : base($"ABitNow.Effects.{nameof(LabelTextKerningEffect)}")
        {
        }
    }
}
