using System;
using System.Collections.Generic;
using IziHardGames.Libs.NonEngine.Game.Abstractions;
using IziHardGames.Ticking.SlotBased;
using UnityEngine.LowLevel;

namespace IziHardGames.Ticking.Scheming
{
    public static class UpdateSchemeUtility
    {
        public static void UpdateScheme(TickingSchemePreset preset)
        {
            var loop = PlayerLoop.GetCurrentPlayerLoop();
            var list = new List<PlayerLoopSystem>();
            List<TickGroupe> groupes = new List<TickGroupe>();
            List<TickSlot> slots = new List<TickSlot>();

            foreach (var stage in preset.All)
            {
                TickGroupe tickGroupe = new TickGroupe();
                groupes.Add(tickGroupe);
                tickGroupe.title = stage.title;
                tickGroupe.key = stage.nameOfStage;
                tickGroupe.insertType = (int)stage.insertType;
                tickGroupe.typeName = stage.typeName;
                IziTicks.RegistGroupe(tickGroupe);

                if (stage.isUnityEngineGroup)
                {
                    foreach (var item in stage.All)
                    {
                        TickSlot tickSlot = new TickSlot();
                        tickSlot.SetKey(item.key);
                        slots.Add(tickSlot);
                        tickGroupe.AddSlot(tickSlot);
                        IziTicks.RegistSlot(tickSlot);
                    }
                }
            }
            int index = 0;
            foreach (var playerLoop in loop.subSystemList)
            {
                Insert(list, playerLoop, groupes, ref index);
            }
            loop.subSystemList = list.ToArray();
            PlayerLoop.SetPlayerLoop(loop);
        }

        private static void Insert(List<PlayerLoopSystem> newList, PlayerLoopSystem playerLoop, List<TickGroupe> groups, ref int indexOfGroups)
        {
            int index = newList.Count;
            newList.Add(new PlayerLoopSystem()
            {
                loopConditionFunction = playerLoop.loopConditionFunction,
                subSystemList = playerLoop.subSystemList,
                type = playerLoop.type,
                updateDelegate = playerLoop.updateDelegate,
                updateFunction = playerLoop.updateFunction,
            });

            for (int i = indexOfGroups; i < groups.Count; i++)
            {
                var grp = groups[i];
                if (grp.typeName == playerLoop.type.Name)
                {
                    var newPlayerLoop = new PlayerLoopSystem()
                    {
                        subSystemList = null,// Array.Empty<PlayerLoopSystem>(),
                        type = typeof(TickGroupe),
                        updateDelegate = grp.ExecuteSync,
                    };

                    if (grp.insertType == (int)EInsertType.After)
                    {
                        newList.Add(newPlayerLoop);
                    }
                    else if (grp.insertType == (int)EInsertType.Before)
                    {
                        newList.Insert(newList.Count - 2, newPlayerLoop);
                    }
                    else if (grp.insertType == (int)EInsertType.NoInsert)
                    {
                        //if (grp.typeName == typeof(FixedUpdate).Name)
                        //{
                        //    IziTicks.RegistGroupe("Fixed.TickGroupe.Execute", grp);
                        //}
                        //else if (grp.typeName == typeof(Update).Name)
                        //{
                        //    IziTicks.RegistGroupe("Update.TickGroupe.Execute", grp);
                        //}
                        //else if (grp.typeName == typeof(PreLateUpdate).Name)
                        //{
                        //    IziTicks.RegistGroupe("Late.TickGroupe.Execute", grp);
                        //}
                    }
                    else if (grp.insertType == (int)EInsertType.InsideFirst)
                    {
                        var current = newList[index];
                        var listInsideFirst = new List<PlayerLoopSystem>() { newPlayerLoop };
                        listInsideFirst.AddRange(newList[index].subSystemList);
                        current.subSystemList = listInsideFirst.ToArray();
                        newList[index] = current;
                    }
                    else if (grp.insertType == (int)EInsertType.InsideLast)
                    {
                        var current = newList[index];
                        var listInsideLast = new List<PlayerLoopSystem>();
                        listInsideLast.AddRange(current.subSystemList);
                        listInsideLast.Add(newPlayerLoop);
                        current.subSystemList = listInsideLast.ToArray();
                        newList[index] = current;
                    }
                    else throw new ArgumentOutOfRangeException(grp.insertType.ToString());

                    break;
                }
            }
            if (playerLoop.subSystemList != null)
            {
                foreach (var sub in playerLoop.subSystemList)
                {
                    Insert(newList, sub, groups, ref indexOfGroups);
                }
            }
        }

        private static void Test1()
        {
            var loop = PlayerLoop.GetCurrentPlayerLoop();
            var list = new List<PlayerLoopSystem>();
            var existed = loop.subSystemList;

            foreach (var item in existed)
            {
                list.Add(item);
                //if (item.type == typeof(Initialization))
                //{
                //    var init = new UpdateChannelForUnity($"IziLoop:{nameof(IziTicks.Initilization)}");
                //    IziTicks.Initilization = init;
                //    list.Add(new PlayerLoopSystem()
                //    {
                //        subSystemList = null,
                //        type = typeof(UpdateChannelForUnity),
                //        updateDelegate = init.ExecuteSync,
                //    });
                //}
                //else if (item.type == typeof(FixedUpdate))
                //{
                //    var early = new UpdateChannelForUnity($"IziLoop:{nameof(IziTicks.BeforePhysics)}");
                //    IziTicks.Initilization = early;

                //    list.Add(new PlayerLoopSystem()
                //    {
                //        subSystemList = null,
                //        type = typeof(UpdateChannelForUnity),
                //        updateDelegate = early.ExecuteSync,
                //    });
                //}
            }
            loop.subSystemList = list.ToArray();
            PlayerLoop.SetPlayerLoop(loop);
        }

        private static PlayerLoopSystem AddSystem<T>(in PlayerLoopSystem loopSystem, PlayerLoopSystem systemToAdd) where T : struct
        {
            var newPlayerLoop = new PlayerLoopSystem()
            {
                loopConditionFunction = loopSystem.loopConditionFunction,
                type = loopSystem.type,
                updateDelegate = loopSystem.updateDelegate,
                updateFunction = loopSystem.updateFunction
            };

            var newSubSystemList = new List<PlayerLoopSystem>();

            foreach (var subSystem in loopSystem.subSystemList)
            {
                newSubSystemList.Add(subSystem);

                if (subSystem.type == typeof(T))
                    newSubSystemList.Add(systemToAdd);
            }
            newPlayerLoop.subSystemList = newSubSystemList.ToArray();
            return newPlayerLoop;
        }
    }
}
