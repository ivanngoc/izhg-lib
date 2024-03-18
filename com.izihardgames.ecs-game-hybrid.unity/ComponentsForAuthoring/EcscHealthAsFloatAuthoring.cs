using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace IziHardGames.Games.ForUnity.ForEcs.Hybrids
{
    public class EcscHealthAsFloatAuthoring : MonoBehaviour
    {     
        public class EcscHealthAsFloatAuthoringBaker : Baker<EcscHealthAsFloatAuthoring>
        {
			public override void Bake(EcscHealthAsFloatAuthoring authoring)
			{
                throw new System.NotImplementedException();
			}
		}
    }
}
