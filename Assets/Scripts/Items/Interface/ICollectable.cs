namespace Inventory
{
    internal interface ICollectable
    {
        public ItemDataSO CollectItem();
        public void DestroyItem();
    }
}