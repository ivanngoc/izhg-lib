using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

namespace IziHardGames.Query
{
    internal class EcsQueryExamples
    {
        public static void QueryEcs(World world)
        {
            var query = new EntityQueryBuilder(Allocator.Temp);
            var q = world.EntityManager.CreateEntityQuery(new ComponentType(typeof(EcsExampleComponentData)));
        }
    }

    public struct EcsExampleComponentData : IComponentData
    {
        public int valueInt;
        public float valueFloat;
        public uint valueUim;
    }
}
