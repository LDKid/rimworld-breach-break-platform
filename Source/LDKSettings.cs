using Verse;
using UnityEngine;

namespace BreachBreaksPlatform
{
    public class BreachBreaksPlatformMod: Mod
    {
        public readonly LDKSettings settings;

        public BreachBreaksPlatformMod(ModContentPack content) : base (content)
        {
            settings = GetSettings<LDKSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled(
                "Breaches destroy platforms:", 
                ref LDKSettings.breakPlatform, 
                "By default, anomalies won't be able to destroy platforms, instead will successfully escape if damaging the platform would break it. Turning this option on will make anomalies damage the platforms until they destroy it instead."
            );
            LDKSettings.damageMult = listingStandard.SliderLabeled(
                "Escape attempt damage multiplier: " + LDKSettings.damageMult.ToString(),
                LDKSettings.damageMult,
                0.1f,
                10f,
                tooltip: "The damage an escape attempt deals to the platform is the anomaly's Mininum Containment Strength mulitplied by this value. (Default: 2.5)"
            );
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Breach Breaks Platform";
        }
    }

    public class LDKSettings: ModSettings
    {
        public static bool breakPlatform;
        public static float damageMult;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref breakPlatform, "breachingDestroyPlatforms", false);
            Scribe_Values.Look(ref damageMult, "escapeDamageMultiplier", 2.5f);

            base.ExposeData();
        }
    }
}
