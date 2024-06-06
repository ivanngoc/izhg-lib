using System.Threading.Tasks;
using IziHardGames.Apps.Abstractions.Lib;
using UnityEngine;

namespace IziHardGames.Apps.ForUnity
{
    public abstract class AbstractAppFactory : MonoBehaviour, IIziAppFactory
    {
        public abstract Task<IIziAppVersion1> CreateAsync(IIziAppBuilder builder);
    }
}

