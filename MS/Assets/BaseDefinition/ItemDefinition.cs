using Unity.VisualScripting;

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

    public class EquipableItem : UseableItem
    {
        protected override void Use()
        {
            
        }
    }

    public class BattleUseItem : UseableItem
    {
        protected override void Use()
        {

        }
    }


}
