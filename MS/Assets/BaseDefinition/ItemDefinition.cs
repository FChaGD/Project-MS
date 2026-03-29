using System;

namespace ItemDefinition
{
    public abstract class ItemBase
    {
        private int itemId;
        
        private string name;

        


    }

    public abstract class UseableItem : ItemBase
    {
        protected abstract void Use();
    }

    [Serializable]
    public class EquipableItem : UseableItem
    {
        protected override void Use()
        {
            
        }
    }

    [Serializable]
    public class BattleUseItem : UseableItem
    {
        protected override void Use()
        {

        }
    }


}
