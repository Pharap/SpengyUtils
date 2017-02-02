// For getting a readable name from an IMyInventoryItem
// Use like:
// var item = entity.GetInventory().GetItems()[0];
// var itemId = new ItemId(item.Content);

public struct ItemId  
{      
    private const string prefix = "MyObjectBuilder_";  
	  
    private readonly string type;  
    private readonly string subtype;  
      
    public ItemId(VRage.ObjectBuilders.MyObjectBuilder_Base content)  
    {  
        var typeId = content.TypeId.ToString();  
        this.type = (typeId.IndexOf(prefix) == 0) ? typeId.Substring(prefix.Length) : "";  
        this.subtype = content.SubtypeName;  
    }  
  
    public string Type { get { return type; } }	  
    public string Subtype { get { return subtype; } }  
      
    public override string ToString()  
    {  
        return string.Format("[{0}:{1}]", Type, Subtype);  
    }  
}
