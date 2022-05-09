﻿using HarmonyLib;
using Hazel;
using SuperNewRoles.CustomObject;
using SuperNewRoles.Helpers;
using SuperNewRoles.Patch;
using System;
using System.Collections.Generic;
using UnityEngine;
using static SuperNewRoles.EndGame.CheckGameEndPatch;
using static UnityEngine.UI.Button;

namespace SuperNewRoles.Sabotage.Blizzard
{
    public static class main
    {
        public static void StartSabotage()
        {
            IsLocalEnd = false;
            SuperNewRolesPlugin.Logger.LogInfo("スタートサボ！");
            SabotageManager.thisSabotage = SabotageManager.CustomSabotage.Blizzard;
            foreach (Arrow aw in ArrowDatas)
            {
                GameObject.Destroy(aw.arrow);
            }
            ArrowDatas = new List<Arrow>();
            IsYellow = true;
            foreach (Vector2 data in Datas)
            {
                Arrow arrow = new Arrow(Color.yellow);
                arrow.arrow.SetActive(true);
                ArrowDatas.Add(arrow);
            }
            UpdateTime = 0;// DefaultUpdateTime;
            DistanceTime = DefaultDistanceTime;
            ArrowUpdateColor = 0.25f;
            OKPlayers = new List<PlayerControl>();
            main.Timer = 5f;
            main.Timer = BlizzardDuration;
            if (PlayerControl.GameOptions.MapId == 0)
            {
                // Blizzard.main.ReactorTimer = main.BlizzardDuration;
                ReactorTimer = Options.BlizzardskeldDurationSetting.getFloat();
            }
            if (PlayerControl.GameOptions.MapId == 1)
            {
                // Blizzard.main.ReactorTimer = main.BlizzardDuration;
                ReactorTimer = Options.BlizzardmiraDurationSetting.getFloat();
            }
            if (PlayerControl.GameOptions.MapId == 2)
            {
             //   Blizzard.main.ReactorTimer = main.BlizzardDuration;
                ReactorTimer = Options.BlizzardpolusDurationSetting.getFloat();
            }
            if (PlayerControl.GameOptions.MapId == 4)
            {
               // Blizzard.main.ReactorTimer = main.BlizzardDuration;
                ReactorTimer = Options.BlizzardairshipDurationSetting.getFloat();
            }
            if (PlayerControl.GameOptions.MapId == 5)
            {
                // Blizzard.main.ReactorTimer = main.BlizzardDuration;
                ReactorTimer = Options.BlizzardagarthaDurationSetting.getFloat();
            }
        }
        public static float BlizzardDuration;
        public static float BlizzardSlowSpeedmagnification;
        private static float ArrowUpdateColor = 1;
        public static float UpdateTime;
        private static float DistanceTime;
        public static float DefaultDistanceTime = 5;
        private static bool IsYellow;
        private static List<Arrow> ArrowDatas = new List<Arrow>();
        private static Vector2[] Datas = new Vector2[] { new Vector2(-13.9f, -15.5f), new Vector2(-24.7f, -1f), new Vector2(10.6f, -15.5f) };
        public static List<PlayerControl> OKPlayers;
        public static bool IsLocalEnd;
        public static bool IsAllEndSabotage;
        public static float Timer;
        public static bool IsOverlay = false;
        public static float MaxTimer;
        public static DateTime OverlayTimer;
        public static DateTime ReacTimer;
        public static float ReactorTimer;
        public static void Create(InfectedOverlay __instance)
        {
            if (SabotageManager.IsOK(SabotageManager.CustomSabotage.Blizzard))
            {
                    int i = 0;
                    foreach (ButtonBehavior Sabotagebuttons in __instance.allButtons)
                    {
                        if (PlayerControl.GameOptions.MapId == 1)
                        {
                            if (i == 2)
                            {
                                Sabotagebuttons.transform.localPosition = new Vector3(-0.5f, 1.964f, -1);
                            }
                            SuperNewRolesPlugin.Logger.LogInfo(Sabotagebuttons.name + ":" + i);
                            i++;
                        }
                    if (PlayerControl.GameOptions.MapId == 0)
                    {
                        if (i == 6)
                        {
                            Sabotagebuttons.transform.localPosition = new Vector3(0.12f, - 0.36f, - 1);
                        }
                        SuperNewRolesPlugin.Logger.LogInfo(Sabotagebuttons.name + ":" + i);
                        i++;
                    }
                    }
                ButtonBehavior button = InfectedOverlay.Instantiate(__instance.allButtons[0], __instance.allButtons[0].transform.parent);
                if (PlayerControl.GameOptions.MapId == 0)
                {
                    button.transform.localPosition = new Vector3(-4.06f, -0.3f, -1);
                }
                if (PlayerControl.GameOptions.MapId == 1)
                {
                    button.transform.localPosition = new Vector3(0.55f, 4.3f, -1);
                }
                if (PlayerControl.GameOptions.MapId == 2)
                {
                    button.transform.localPosition = new Vector3(4.28f, 0.3251f, -1);
                }
                if (PlayerControl.GameOptions.MapId == 4)
                {
                    button.transform.localPosition = new Vector3(1.42f, 0f, -1);
                }
                if (PlayerControl.GameOptions.MapId == 5)
                {
                    button.transform.localPosition = new Vector3(1.42f, 0f, -1);//偽レイラーよろしく
                }
                button.spriteRenderer.sprite = IconManager.BlizzardgetButtonSprite();
                button.OnClick = new ButtonClickedEvent();

                button.OnClick.AddListener((Action)(() =>
                {
                    if (SabotageManager.InfectedOverlayInstance.CanUseSpecial)
                    {
                        SabotageManager.CustomSabotageRPC(PlayerControl.LocalPlayer, SabotageManager.CustomSabotage.Blizzard, true);
                    }
                }));
                __instance.allButtons.AddItem(button);
                SabotageManager.CustomButtons.Add(button);
            }
        }
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static void Update()
        {
            //   SuperNewRolesPlugin.Logger.LogInfo(main.ReactorTimer);
            var TimeSpanDate = new TimeSpan(0, 0, 0, (int)2f);
            main.Timer = (float)((main.OverlayTimer + TimeSpanDate) - DateTime.Now).TotalSeconds;
            var TimeSpanDate1 = new TimeSpan(0, 0, 0, (int)BlizzardDuration);
            main.ReactorTimer = (float)((main.ReacTimer + TimeSpanDate1) - DateTime.Now).TotalSeconds;
            Blizzard.Reactor.Postfix();
            if (SabotageManager.InfectedOverlayInstance != null)
            {
                if (IsAllEndSabotage)
                {
                    SabotageManager.InfectedOverlayInstance.SabSystem.Timer = SabotageManager.SabotageMaxTime;
                } else if (!IsLocalEnd)
                {
                    SabotageManager.InfectedOverlayInstance.SabSystem.Timer = SabotageManager.SabotageMaxTime;
                }
            }
            bool IsOK = true;
            /*foreach (PlayerControl p3 in PlayerControl.AllPlayerControls)
            {
                if (p3.isAlive() && !OKPlayers.IsCheckListPlayerControl(p3)) {
                    IsOK = false;
                    if (PlayerControl.LocalPlayer.isImpostor())
                    {
                        if (!(p3.isImpostor() || p3.isRole(CustomRPC.RoleId.MadKiller)))
                        {
                            SetNamesClass.SetPlayerNameColor(p3,new Color32(18, 112, 214,byte.MaxValue));
                        }
                    }
                }
            }*/
            static void EndGame(ShipStatus __instance)
            {
                if (SabotageManager.thisSabotage == SabotageManager.CustomSabotage.Blizzard)
                {
                    __instance.enabled = false;
                    ShipStatus.RpcEndGame(GameOverReason.ImpostorBySabotage, false);
                }
            }
            if (IsOK)
            {
                //  SabotageManager.thisSabotage = SabotageManager.CustomSabotage.None;
                return;
            }
            if (!IsLocalEnd)
            {
                int arrowindex = 0;
                ArrowUpdateColor -= Time.fixedDeltaTime;
                Color? SetColor = null;
                if (ArrowUpdateColor <= 0)
                {
                    if (IsYellow)
                    {
                        SetColor = Color.red;
                        IsYellow = false;
                    }
                    else
                    {
                        SetColor = Color.yellow;
                        IsYellow = true;
                    }
                    ArrowUpdateColor = 0.25f;
                }
                foreach (Arrow arrow in ArrowDatas)
                {
                    arrow.Update(Datas[arrowindex], SetColor);
                    arrowindex++;
                }
                bool IsOK2 = false;
                foreach (Vector2 data in Datas)
                {
                    if (!IsOK2)
                    {
                        if (Vector2.Distance(PlayerControl.LocalPlayer.GetTruePosition(), data) <= 1)
                        {
                            IsOK2 = true;
                        }
                    }
                }
                if (IsOK2)
                {
                    DistanceTime -= Time.fixedDeltaTime;
                    if (DistanceTime <= 0)
                    {
                        SabotageManager.CustomSabotageRPC(PlayerControl.LocalPlayer, SabotageManager.CustomSabotage.Blizzard, false);
                    }
                }
                else
                {
                    DistanceTime = DefaultDistanceTime;
                }
                UpdateTime -= Time.fixedDeltaTime;
            }
        }
        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
        public static void EndSabotage(PlayerControl p)
        {
            OKPlayers.Add(p);
            if (p.PlayerId == PlayerControl.LocalPlayer.PlayerId)
            {
                IsLocalEnd = true;
                /*if (PlayerControl.GameOptions.TaskBarMode != TaskBarMode.Invisible)
                {
                    SlowSpeed.Instance.gameObject.SetActive(IsLocalEnd);
                }*/
                foreach (Arrow aw in ArrowDatas)
                {
                    GameObject.Destroy(aw.arrow);
                }
                ArrowDatas = new List<Arrow>();
                /*foreach (PlayerControl p2 in PlayerControl.AllPlayerControls)
                {
                    p2.resetChange();
                }*/
            }
        }

        //会議開始後処理
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        class ClearSabotageMeeting
        {
            static void Postfix(MeetingHud __instance)
            {
                //SuperNewRolesPlugin.Logger.LogInfo("会議だぁぁぁぁ");
                SabotageManager.thisSabotage = SabotageManager.CustomSabotage.None;
                foreach (Arrow aw in ArrowDatas)
                {
                    GameObject.Destroy(aw.arrow);
                }
            }
        }
    }
}
