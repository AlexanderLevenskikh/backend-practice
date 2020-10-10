using System;
using System.Collections.Generic;

namespace TodoApplication
{ 
    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        public LimitedSizeStack<ListModelAction<TItem>> Actions { get; set; }

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Actions = new LimitedSizeStack<ListModelAction<TItem>>(limit);
        }

        public void AddItem(TItem item)
        {
            Items.Add(item);
            
            Actions.Push(new ListModelAction<TItem>
            {
                Index = Items.Count - 1,
                Item = item,
                ActionType = ListModelActionTypeEnum.Add
            });
        }

        public void RemoveItem(int index)
        {
            var item = Items[index];
            Items.RemoveAt(index);
            
            Actions.Push(new ListModelAction<TItem>
            {
                Index = index,
                Item = item,
                ActionType = ListModelActionTypeEnum.Remove
            });
        }

        public bool CanUndo()
        {
            return Actions.Count > 0;
        }

        public void Undo()
        {
            var lastAction = Actions.Pop();

            switch (lastAction.ActionType)
            {
                case ListModelActionTypeEnum.Add:
                    Items.RemoveAt(lastAction.Index);
                    break;
                case ListModelActionTypeEnum.Remove:
                    Items.Insert(lastAction.Index, lastAction.Item);
                    break;
            }
        }
    }
    
    public enum ListModelActionTypeEnum
    {
        Remove,
        Add,
    }

    public class ListModelAction<TItem>
    {
        public ListModelActionTypeEnum ActionType { get; set; }
        public TItem Item { get; set; }
        public int Index { get; set; }
    }
}
