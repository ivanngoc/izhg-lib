using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IziHardGames.AppConstructor
{
    public class IziBootstrap : MonoBehaviour
    {
        [SerializeField] public IziAppScheme? iziAppScheme;
        [Space]
        [SerializeField] private Status status;
        private IEnumerator<int> itterator;
        private readonly AppStateMachine appStateMachine = new AppStateMachine();
        private void Awake()
        {
#if UNITY_EDITOR
            Debug.Log(GetType().AssemblyQualifiedName);
            iziAppScheme.CleanupStaticFields();
#endif
            this.enabled = true;
            status.isLoaded = false;
            status.isResolvedDependecies = false;

            iziAppScheme?.StartapBegin();

            this.itterator = appStateMachine.Itterate().GetEnumerator();
        }

        private void Update()
        {
            itterator.MoveNext();
            var value = itterator.Current;

            if (!status.isLoaded)
            {
                if (!iziAppScheme?.KeepItterateLoading() ?? false)
                {
                    iziAppScheme!.StartupEnd();
                    status.isLoaded = true;
                }
            }
            else if (!status.isResolvedDependecies)
            {
                if (!iziAppScheme?.KeepResolveDependecies() ?? false)
                {
                    status.isResolvedDependecies = true;
                }
            }
            else
            {
                this.enabled = false;
            }
        }



    }

    public class Status
    {
        public bool isLoaded;
        public bool isResolvedDependecies;
    }

    public class AppStateMachine
    {
        public IEnumerable<int> Itterate()
        {
            int stage = default;

            while (!CreateBuilder())
            {
                yield return stage;
            }
            stage++;
            yield return stage;
        }
        private void RunApp()
        {
            // create builder
            // configure builder
            // build app
            // run app
        }

        private bool CreateBuilder()
        {
            return false;
        }
    }
}
