// To use:
// 1. Have a text panel named "Text Panel"
// 2. Supply the name of the cargo container as the run argument

public void Main(string argument) 
{
    IMyTextPanel textPanel = GridTerminalSystem.GetBlockWithName("Text Panel") as IMyTextPanel; // Acquire text panel
    IMyCargoContainer cargo = GridTerminalSystem.GetBlockWithName(argument) as IMyCargoContainer; // Acquire cargo

    IMyEntity cargoEntity = cargo as IMyEntity; // Cast cargo to entity to enable inventory access
    IMyInventory cargoInventory = cargoEntity.GetInventory(); // Get instance of IMyInventory
    List<IMyInventoryItem> cargoList = cargoIventory.GetItems(); // Get list of items in inventory

    textPanel.WritePublicText(""); // Clear text panel
    for(int i = 0; i < cargoList.Count; ++i)
    {
        IMyInventoryItem item = cargoList[i]; // Get item in list
        VRage.ObjectBuilders.MyObjectBuilder_Base content = item.Content; // Get content descriptor
        textPanel.WritePublicText(content.TypeId.ToString(), true); // Write name of builder type
        textPanel.WritePublicText(":", true);
        textPanel.WritePublicText(content.SubtypeName, true); // Write item's subtype
        textPanel.WritePublicText(" x", true);
        textPanel.WritePublicText(item.Amount.ToString(), true); // Write amount of item
        textPanel.WritePublicText("\n", true); // New line
    }
}
