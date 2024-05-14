using RimWorld;
using Verse;
using HarmonyLib;
using System.Threading.Tasks;

namespace BreachBreaksPlatform
{
    [StaticConstructorOnStartup]
    public static class LDK_BreachBreak_Patch
    {
        static readonly LDKSettings settings;

        static readonly string LCM_PACKAGE_ID = "com.SYSMY.LCEM.Patch";
        static readonly string LCM_CONTENT_PACK = "sysmy.lcem";

        static LDK_BreachBreak_Patch()
        {
            Harmony harmony = new Harmony("com.ldk.breachbreakplatform.patch");
            
            var originalMethod = typeof(CompHoldingPlatformTarget).GetMethod("Escape");
            
            if (Harmony.HasAnyPatches(LCM_PACKAGE_ID)) {
                Log.Message("LobotomyCorp music is loaded. LDK's BreachBreaksPlatform is overriding their patch for compatibility.");
                
                HarmonyMethod postfixPatch = new HarmonyMethod(typeof(patch1).GetMethod("Postfix"));
                harmony.Patch(originalMethod, postfix: postfixPatch);
            }


            HarmonyMethod prefixPatch = new HarmonyMethod(typeof(patch1).GetMethod("Prefix"));

            
            harmony.Patch(originalMethod, prefix: prefixPatch);
            Log.Message("LDK's BreachBreaksPlatform mod initialized.");
        }
        private class patch1
        {
            public static bool Prefix(ref CompHoldingPlatformTarget __instance)
            {
                __instance.isEscaping = true;

                Building_HoldingPlatform platform = __instance.HeldPlatform;
                if (platform == null)
                {
                    Log.Error("Im confused");
                }
                
                int platformHitpoints = platform.HitPoints;
                Pawn anomaly = (Pawn) __instance.parent;
                float escapeDamage = EscapeAttemptDamage(anomaly);


                if (platformHitpoints <= escapeDamage)
                {
                    if (LDKSettings.breakPlatform)
                    {
                        DestroyPlatform(platform, anomaly);
                    }
                    return true;
                }

                __instance.HeldPlatform.TakeDamage(new DamageInfo(DamageDefOf.Scratch, escapeDamage, 0f, -1f, anomaly));
                __instance.isEscaping = false;
                Messages.Message("An anomaly has damaged it's holding spot while trying to escape.", new LookTargets(anomaly), MessageTypeDefOf.ThreatSmall);

                return false;
            }

            public static void Postfix(ref CompHoldingPlatformTarget __instance) {
                if (__instance.isEscaping) return;


                if (Find.MusicManagerPlay.CurrentSong.modContentPack.PackageId == LCM_CONTENT_PACK)
                {
                    Find.MusicManagerPlay.Stop();
                }
            }

            public static async void DestroyPlatform(Building_HoldingPlatform platform, Pawn pawn)
            {
                await Task.Delay(10);
                platform.TakeDamage(new DamageInfo(DamageDefOf.Scratch, EscapeAttemptDamage(pawn), 0f, -1f, pawn));
            }
            public static float EscapeAttemptDamage(Pawn pawn)
            {
                return pawn.GetStatValue(StatDefOf.MinimumContainmentStrength) * LDKSettings.damageMult;
            }
        }
    }
}
