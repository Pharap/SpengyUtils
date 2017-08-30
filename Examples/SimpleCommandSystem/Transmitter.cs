// Instructions:
// 1. Place script in programmable block
// 2. Assign programmable block to antenna named "Antenna"

public void Broadcast(string name, string message)
{ 
    var antenna = GridTerminalSystem.GetBlockWithName(name) as IMyRadioAntenna;
    antenna.TransmitMessage(message);
}

public void Main(string argument)
{
    Broadcast("Antenna", argument);  
}
