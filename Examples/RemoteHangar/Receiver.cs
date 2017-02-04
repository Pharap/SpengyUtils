// Instructions:
// 1. Group all hangar doors together in a group called "Hangar"
// 2. Put this script in a progammable block
// 3. Assign that programmable block to an Antenna

void OpenHangarGroup(string groupName)
{
    var hangarGroup = GridTerminalSystem.GetBlockGroupWithName(groupName);
    var doors = new List<IMyAirtightHangarDoor>();
    hangarGroup.GetBlocksOfType(doors);

    for(int i = 0; i < doors.Count; ++i)
    {
        var door = doors[i];
        door.ApplyAction("Open_On");
    }
}

void CloseHangarGroup(string groupName)
{
    var hangarGroup = GridTerminalSystem.GetBlockGroupWithName(groupName);
    var doors = new List<IMyAirtightHangarDoor>();
    hangarGroup.GetBlocksOfType(doors);

    for(int i = 0; i < doors.Count; ++i)
    {
        var door = doors[i];
        door.ApplyAction("Open_Off");
    }
}

public void Main(string argument)
{
    if (argument == "OpenHangar")
    {
        OpenHangarGroup("Hangar");
    }
    if (argument == "CloseHangar")
    {
        CloseHangarGroup("Hangar");
    }   
}
