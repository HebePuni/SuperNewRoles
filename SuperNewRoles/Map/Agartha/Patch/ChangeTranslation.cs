﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnhollowerBaseLib;

namespace SuperNewRoles.Map.Agartha.Patch
{
    class ChangeTranslation
    {

        [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), new[] {
                typeof(StringNames),
                typeof(Il2CppReferenceArray<Il2CppSystem.Object>)
            })]
        private class ChangeTranslationPatch
        {
            public static void Postfix(TranslationController __instance,ref string __result, [HarmonyArgument(0)] StringNames name)
            {
                if (!Data.IsMap(CustomMapNames.Agartha)) return;
                if (name == StringNames.Cafeteria)
                {
                    __result = __instance.GetString(StringNames.MeetingRoom);
                } else if(name == StringNames.Reactor)
                {
                    __result = ModTranslation.getString("Agartha_WorkRoom");
                } else if (name == StringNames.Nav)
                {
                    __result = "全箇所共通";//ModTranslation.getString("Agartha_WorkRoom");
                }
            }
        }
    }
}
