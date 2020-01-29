using System;
using System.Linq;
using ABitNow.Effects;
using ABitNow.iOS.Effects;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("ABitNow.Effects")]
[assembly: ExportEffect(typeof(iOSLabelTextKerningEffect), nameof(LabelTextKerningEffect))]
namespace ABitNow.iOS.Effects
{
    public class iOSLabelTextKerningEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var effect = (LabelTextKerningEffect)Element.Effects.FirstOrDefault(e => e is LabelTextKerningEffect);
            ApplySpacing(effect.Kerning);
        }

        protected override void OnDetached() { }

        void ApplySpacing(float letterSpacing)
        {
            var lbl = Control as UILabel;
            if (lbl == null || string.IsNullOrEmpty(lbl.Text)) return;
            lbl.AttributedText = new NSAttributedString(lbl.Text, kerning: letterSpacing);
        }
    }
}