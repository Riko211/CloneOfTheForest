# CloneOfTheForest

## Inventory
The inventory is implemented following the example of the Minecraft game. Items can be moved in inventory slots, swapped, thrown out of inventory on the ground. When throwing an item, it will be created in front of the character and fall to the ground, it can be picked up and returned to inventory. When switching slots in the toolbar, if there is a tool in the slot, it will appear in the character's hands. Information about each item is stored in scriptable objects.  
![Inventory](https://github.com/Riko211/CloneOfTheForest/blob/main/Assets/2D/Screenshots/Inventory.jpg)
![Scriptable objects](https://github.com/Riko211/CloneOfTheForest/blob/main/Assets/2D/Screenshots/ScriptableObj1.jpg)

## Crafting system
Crafts are also implemented following the example of the Minecraft game. To create a new item, you need to lay out its recipe in 9 crafting slots. When picking up a created item, the items from which it can be crafted will disappear from the crafting slots. Each recipe is a scriptable object with 9 fields for the recipe items, 1 field for the output of the recipe, and 1 field for the number of output items.
![Crafting](https://github.com/Riko211/CloneOfTheForest/blob/main/Assets/2D/Screenshots/Craft.jpg)
![Recipe](https://github.com/Riko211/CloneOfTheForest/blob/main/Assets/2D/Screenshots/RecipeSO.jpg)

## Resource harvesting
Using tools such as an axe, you can extract resources by chopping trees on the island. Trees drop logs that can be used as material for crafting recipes and saplings that can be planted in the ground to grow a new tree.
![Harvesting](https://github.com/Riko211/CloneOfTheForest/blob/main/Assets/2D/Screenshots/Harvesting1.jpg)

## Building
The construction is based on the construction mechanics as in the game "The Forest" and "Rust". There are simple structures that can be built on the ground (fence, fire pit, small tent). And there are modular structures for which you need to put the foundation. On the foundation, you can put walls, floors and roofs.
![Building1](https://github.com/Riko211/CloneOfTheForest/blob/main/Assets/2D/Screenshots/Building1.jpg)
![Building2](https://github.com/Riko211/CloneOfTheForest/blob/main/Assets/2D/Screenshots/Building2.jpg)

## Weather

Rain:
![Rain](https://github.com/Riko211/CloneOfTheForest/blob/main/Assets/2D/Screenshots/Rain1.jpg)
