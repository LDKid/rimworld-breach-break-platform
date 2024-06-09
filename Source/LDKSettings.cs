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
                "SettingsBreachDestroy".Translate(), 
                ref LDKSettings.breakPlatform, 
                "SettingsBreachDestroyTooltip".Translate()
            );
            LDKSettings.damageMult = listingStandard.SliderLabeled(
                "SettingsBreachDamageMult".Translate() + LDKSettings.damageMult.ToString(),
                LDKSettings.damageMult,
                0.1f,
                10f,
                tooltip: "SettingsBreachDamageMultTooltip".Translate()
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
        public static bool breakPlatform = false;
        public static float damageMult = 2.5f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref breakPlatform, "breachingDestroyPlatforms", breakPlatform);
            Scribe_Values.Look(ref damageMult, "escapeDamageMultiplier", damageMult);

            base.ExposeData();
        }
    }
}
