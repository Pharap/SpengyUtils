// Instructions:
// 1. Group all hangar doors together in a group called "Hangar"
// 2. Put this script in a progammable block
// 3. Assign that programmable block to an Antenna

 
void OpenDoorGroup(string groupName) 
{ 
    var hangarGroup = GridTerminalSystem.GetBlockGroupWithName(groupName); 
    var doors = new List<IMyDoor>(); 
    hangarGroup.GetBlocksOfType(doors); 
 
    foreach(var door in doors)
        door.OpenDoor();
} 
 
void CloseDoorGroup(string groupName) 
{ 
    var hangarGroup = GridTerminalSystem.GetBlockGroupWithName(groupName); 
    var doors = new List<IMyDoor>(); 
    hangarGroup.GetBlocksOfType(doors);  
  
    foreach(var door in doors) 
        door.CloseDoor();
} 
 
public void Main(string argument) 
{
    switch(argument)
    {
        case "OpenHangar": OpenDoorGroup("Hangar"); break; 
        case "CloseHangar": CloseDoorGroup("Hangar"); break;
    }  
}
