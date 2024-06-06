using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IziHardGames.Ticking.SlotBased
{
    public class TickGroupe
    {
        public string title = "NoTitle";
        public string key = "NoKey";
        public string typeName = string.Empty;
        public int insertType;

        private List<TickSlot> tickSlots = new List<TickSlot>();

        public void AddSlot(TickSlot tickSlot)
        {
            tickSlots.Add(tickSlot);
        }

        public void Disable(int index)
        {
            tickSlots[index].Disable();
        }

        public int Enable(string key, Action action)
        {
            var slot = tickSlots.First(x => x.Key == key);
            int indexOf = tickSlots.IndexOf(slot);
            slot.Enable(action);
            return indexOf;
        }

        public void ExecuteSync()
        {
            foreach (var slot in tickSlots)
            {
                slot.Execute();
            }
        }
    }

    public class TickSlot
    {
        public string Key { get; private set; }
        private bool isBinded;
        public Action? Action { get; private set; }
        public bool IsBinded => isBinded;
        internal void Execute()
        {
            if (isBinded) Action!.Invoke();
        }
        public void SetKey(string key)
        {
            Key = key;
        }
        internal void Enable(Action action)
        {
            Action = action;
            isBinded = true;
        }

        internal void Disable()
        {
            Action = default;
            isBinded = false;
        }
    }
}
