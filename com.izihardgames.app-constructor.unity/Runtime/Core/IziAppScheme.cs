using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;


namespace IziHardGames.AppConstructor
{
    /*
    Services - singletons 100%
    Singletons
    Transients
    Scoped
    Objects
    */
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = nameof(IziAppScheme), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_MODULES + "/" + nameof(IziAppScheme))]
    public class IziAppScheme : ScriptableObject
    {
        /*
         Переводим все в MVC like архитектуру:
         1. объявляем модели (дата типы)
         2. объявляем контроллеры (каждый контроллер оперирует набором моделей как в ECS)
         3. объявляем пайплайны: последовательность выполнения контроллеров

            Прототипы:
            Загрузка сцены - Пул Объектов - Триггер (создание) - запуска контроллера AI - получение AI команлы - выполнение AI команды - получение урона от игрока - проверка жизней - смерть
         */

        public List<IziAppModuleScheme> modules = new List<IziAppModuleScheme>();
        public List<UnityEngine.Object> models = new List<UnityEngine.Object>();
        public List<UnityEngine.Object> controllers = new List<UnityEngine.Object>();
        public List<UnityEngine.Object> pipelienes = new List<UnityEngine.Object>();
        private IziAppModuled? app;
        public IziAppModuled App => app;

        [ContextMenu("Izi Validate")]
        private void Validate()
        {
            modules = modules.OrderBy(x => x.orderToStartup).ToList();
        }

        internal void StartapBegin()
        {
            IziAppModuled iziAppV2 = app = new IziAppModuled();

            foreach (var module in modules)
            {
                module.Begin(iziAppV2);
            }
        }
        internal void StartupEnd()
        {
            foreach (var module in modules)
            {
                module.End();
            }
        }
        public void CleanupStaticFields()
        {
            foreach (var item in modules)
            {
                item.CleanupStaticFields();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// <see langword="true"/> Itterating in progress - <br/>
        /// <see langword="false"/> - Itterating completed
        /// </returns>
        internal bool KeepItterateLoading()
        {
            foreach (var module in modules)
            {
                module.ItterateLoading();
            }

            foreach (var module in modules)
            {
                if (!module.IsLoadCompleted())
                {
                    return true;
                }
            }
            return false;
        }



        internal bool KeepResolveDependecies()
        {
            foreach (var module in modules)
            {
                if (!module.ResolveDependecies())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
