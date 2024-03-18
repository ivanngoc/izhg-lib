using IziHardGames.Libs.NonEngine.Game.Abstractions;
using IziHardGames.Libs.NonEngine.Sorting;
using IziHardGames.Ticking.Abstractions.Lib;
using System;
using System.Collections.Generic;

namespace IziHardGames.Ticking.Lib
{
    /// <summary>
    /// Группа из <see cref="UpdateStep"/>. Каждая группа имеет горизонтальную иерархию и вертикальную общность. 
    /// Порядок выполнения по горизонтали - порядковый, согласно <see cref="order"/><br/>
    /// Порядок выполнения по вертикали (у <see cref="UpdateGroupe"/> c одинаковым <see cref="order"/>) - независимый (паралельный)
    /// Иными словами выполнение <see cref="UpdateGroupe"/> строго последовательно, а <see cref="UpdateStep."/> паралельно
    /// </summary>
    public class UpdateGroupe : IComparable<UpdateGroupe>, IUpdateGroupe
    {
        public const int DEFAULT_ID_GROUPE = (int)EUpdatePhase.GameMechanic;
        public const int DEFAULT_GROUPE_PRIORITY = 1;
        public const int DEFAULT_INITIAL_CAPACITY = 512;
        /// <summary>
        /// <see cref="EUpdatePhase"/>. ID as enum to int
        /// </summary>
        public int idGroupe;
        public int order;
        private bool isItemsToAdd;
        private bool isItemsToRemove;
        public UpdateChannelV1 updateChannel;
        public ITickChannel UpdateChannel { get => updateChannel; set { updateChannel = value as UpdateChannelV1; } }

        private List<UpdateStep> items;
        private List<UpdateStep> itemsAdd;
        private List<UpdateStep> itemsRemove;

        public UpdateGroupe(int capacity)
        {
            items = new List<UpdateStep>(capacity);
            itemsAdd = new List<UpdateStep>(capacity);
            itemsRemove = new List<UpdateStep>(capacity);
        }
        public UpdateStep GetOrCreateStep(UpdateControlToken token)
        {
            foreach (var item in itemsAdd)
            {
                if (item.priority == token.updateOptions.priority) return item;
            }
            foreach (var item in items)
            {
                if (item.priority == token.updateOptions.priority) return item;
            }
            UpdateStep updateStep = UpdateStep.Wrap(token);
            AddSheduled(updateStep);
            return updateStep;
        }

        public void Execute()
        {
            if (isItemsToAdd)
            {
                foreach (var item in itemsAdd)
                {
                    items.InsertWithBinaryTreeAscending(item);
                }
                itemsAdd.Clear();
                isItemsToAdd = false;
            }
            Run();

            if (isItemsToRemove)
            {
                foreach (var item in itemsRemove)
                {
                    items.Remove(item);
                }
                itemsRemove.Clear();
                isItemsToRemove = false;
            }
        }
        private void Run()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].trigger();
            }
        }

        public int IndexOf(UpdateStep item)
        {
            return items.IndexOf(item);
        }

        public int CompareTo(UpdateGroupe other)
        {
            if (idGroupe < other.idGroupe)
            {
                return -1;
            }
            if (idGroupe > other.idGroupe)
            {
                return 1;
            }
            return 0;
        }

        public void AddSheduled(UpdateStep item)
        {
#if DEBUG
            if (itemsAdd.Contains(item))
            {
                throw new ArgumentException($"{GetType().FullName} already contain {item.GetType().FullName} with id {item.id} and hash {item.GetHashCode()} in {nameof(itemsAdd)} list");
            }
            if (items.Contains(item))
            {
                throw new ArgumentException($"{GetType().FullName} already contain {item.GetType().FullName} with id {item.id} and hash {item.GetHashCode()} in {nameof(items)} list");
            }
#endif
            itemsAdd.Add(item);
            isItemsToAdd = true;
        }

        public void RemoveSheduled(UpdateStep item)
        {
#if DEBUG
            if (itemsRemove.Contains(item))
            {
                throw new ArgumentException($"{GetType().FullName} already contain {item.GetType().FullName} with id {item.id} and hash {item.GetHashCode()} in {nameof(itemsRemove)} list");
            }
#endif
            itemsRemove.Add(item);
            isItemsToRemove = true;
        }

#if UNITY_EDITOR
        public List<UpdateStep> DebugGetUpdateSteps() => items;
#endif

    }
}