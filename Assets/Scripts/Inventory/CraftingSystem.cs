using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class CraftingSystem : MonoBehaviour
    {
        [SerializeField]
        private InventoryManager _inventoryManager;
        [SerializeField]
        private CraftingSlot[] _craftingSlots;
        [SerializeField]
        private InventorySlot _outputSlot;
        [SerializeField]
        private GameObject _inventoryItemPrefab;
        [SerializeField]
        private Transform _inventoryRoot;

        [SerializeField]
        private List<RecipeSO> _recipes;


        private void Start()
        {
            _outputSlot.OnItemTake += PickItemFromOutputSlot;
            foreach (CraftingSlot slot in _craftingSlots) slot.OnItemDropAction += CheckForRecipe;
        }
        private void OnDestroy()
        {
            _outputSlot.OnItemTake -= PickItemFromOutputSlot;
            foreach (CraftingSlot slot in _craftingSlots) slot.OnItemDropAction -= CheckForRecipe;
        }
        private void CheckForRecipe()
        {
            ItemDataSO[] itemsInCraftSlots = new ItemDataSO[_craftingSlots.Length];
            for (int i = 0; i < _craftingSlots.Length; i++)
            {
                InventoryItem item = _craftingSlots[i].GetComponentInChildren<InventoryItem>();
                if (item != null) itemsInCraftSlots[i] = item.GetItemData();
            }
            bool isRecipeEqual = true;

            foreach (RecipeSO recipe in _recipes)
            {
                isRecipeEqual = true;

                for (int i=1; i < recipe.input.Length; i++)  //recipe comparison with workbench
                {
                    if (recipe.input[i] != itemsInCraftSlots[i])
                    {
                        isRecipeEqual = false;
                        break;
                    }
                }

                if (isRecipeEqual) 
                {
                    CreateOutputItem(recipe.output);
                    break;
                }  
            }
        }
        private bool CheckForItemsInCraftingSlots()
        {
            foreach(CraftingSlot slot in _craftingSlots)
            {
                if (slot.transform.childCount > 0) return true;
            }
            return false;
        }
        private void CreateOutputItem(ItemDataSO itemData)
        {
            _inventoryManager.SpawnItemInSlot(itemData, _outputSlot);
        }

        private void PickItemFromOutputSlot()
        {
            foreach (CraftingSlot slot in _craftingSlots)
            {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null) itemInSlot.RemoveItem();
            }
            Invoke(nameof(CheckForRecipe), 0.02f);
        }
    }
}