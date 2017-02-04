// Instructions:
// 1. Place script in programmable block
// 2. Assign programmable block to antenna named "Antenna"
// 3. Use programmable block with argument "OpenHangar" to open hangar
// 4. Use programmable block with argument "CloseHangar" to close hangar

public void Broadcast(string name, string message)
{ 
    var antenna = GridTerminalSystem.GetBlockWithName(name) as IMyRadioAntenna;
    antenna.TransmitMessage(message);
}

public void Main(string argument)
{
    Broadcast("Antenna", argument);  
}
