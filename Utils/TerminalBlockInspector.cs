// Lists all actions and properties of a terminal block

public void Main(string argument)
{
    var block = GridTerminalSystem.GetBlockWithName(argument) as IMyTerminalBlock;

    var builder = new StringBuilder();

    var actions = new List<ITerminalAction>();
    block.GetActions(actions);
    foreach(var action in actions)
        builder.AppendFormat("{0} ({1})\n", action.Id, action.Name);

    builder.Append("\n");
    
    var properties = new List<ITerminalProperty>();
    block.GetProperties(properties);
    foreach(var prop in properties)
        builder.AppendFormat("{0} : {1}\n", prop.Id, prop.TypeName);

    Echo(builder.ToString());
}
