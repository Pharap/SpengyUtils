
public void Main(string argument)
{
	Command command = new Command();
	
	if(!Command.TryParse(argument, ref command))
	{
		return;
	}
	
	Process(command);
}

delegate bool GroupFunction(IMyBlockGroup group);
delegate bool BlockFunction(IMyTerminalBlock block);

static readonly Dictionary<string, BlockFunction> blockFunctions = new Dictionary<string, BlockFunction>()
{
	{ MakeIndex("door", "open"), MakeBlockFunction((IMyDoor d) => d.OpenDoor()) },
	{ MakeIndex("door", "close"), MakeBlockFunction((IMyDoor d) => d.CloseDoor()) },
	{ MakeIndex("door", "toggle"), MakeBlockFunction((IMyDoor d) => d.ToggleDoor()) },
	{ MakeIndex("piston", "extend"), MakeBlockFunction((IMyPistonBase p) => p.Extend()) },
	{ MakeIndex("piston", "retract"), MakeBlockFunction((IMyPistonBase p) => p.Retract()) },
	{ MakeIndex("piston", "reverse"), MakeBlockFunction((IMyPistonBase p) => p.Reverse()) },
	{ MakeIndex("functional", "enable"), MakeBlockFunction((IMyFunctionalBlock f) => f.Enabled = true) },
	{ MakeIndex("functional", "disable"), MakeBlockFunction((IMyFunctionalBlock f) => f.Enabled = false) },
	{ MakeIndex("remotecontrol", "enable"), MakeBlockFunction((IMyRemoteControl r) => r.SetAutoPilotEnabled(true)) },
	{ MakeIndex("remotecontrol", "disable"), MakeBlockFunction((IMyRemoteControl r) => r.SetAutoPilotEnabled(false)) },
};
	
static readonly List<IMyDoor> doors = new List<IMyDoor>();
static readonly List<IMyPistonBase> pistons = new List<IMyPistonBase>();
static readonly List<IMyFunctionalBlock> functionalBlocks = new List<IMyFunctionalBlock>();
static readonly List<IMyRemoteControl> remoteControls = new List<IMyRemoteControl>();

static readonly Dictionary<string, GroupFunction> groupFunctions = new Dictionary<string, GroupFunction>()
{
	{ MakeIndex("door", "open"), MakeGroupFunction(doors, d => d.OpenDoor()) },
	{ MakeIndex("door", "close"), MakeGroupFunction(doors, d => d.CloseDoor()) },
	{ MakeIndex("door", "toggle"), MakeGroupFunction(doors, d => d.ToggleDoor()) },
	{ MakeIndex("piston", "extend"), MakeGroupFunction(pistons, p => p.Extend()) },
	{ MakeIndex("piston", "retract"), MakeGroupFunction(pistons, p => p.Retract()) },
	{ MakeIndex("piston", "reverse"), MakeGroupFunction(pistons, p => p.Reverse()) },
	{ MakeIndex("functional", "enable"), MakeGroupFunction(functionalBlocks, f => f.Enabled = true) },
	{ MakeIndex("functional", "disable"), MakeGroupFunction(functionalBlocks, f => f.Enabled = false) },
	{ MakeIndex("remotecontrol", "enable"), MakeGroupFunction(remoteControls, r => r.SetAutoPilotEnabled(true)) },
	{ MakeIndex("remotecontrol", "disable"), MakeGroupFunction(remoteControls, r => r.SetAutoPilotEnabled(false)) },
};

static string MakeIndex(string typeName, string actionName)
{
	return string.Concat(typeName.ToLower(), "_", actionName.ToLower());
}

bool Process(Command command)
{
	switch(command.Qualifier)
	{
		case Qualifier.Block:
			return ProcessBlock(command.Name, command.Type, command.Action);
		case Qualifier.Group:
			return ProcessGroup(command.Name, command.Type, command.Action);
		default:
			return false;
	}
}

bool ProcessBlock(string name, string type, string action)
{
	var block = GridTerminalSystem.GetBlockWithName(name);
	
	if(block == null)
		return false;
	
	BlockFunction function;
	var index = MakeIndex(type, action);
	if(blockFunctions.TryGetValue(index, out function))
		return function(block);
	
	return false;
}

bool ProcessGroup(string name, string type, string action)
{
	var group = GridTerminalSystem.GetBlockGroupWithName(name);
	
	if(group == null)
		return false;
	
	GroupFunction function;
	var index = MakeIndex(type, action);
	if(groupFunctions.TryGetValue(index, out function))
		return function(group);
	
	return false;
}

static BlockFunction MakeBlockFunction<T>(Action<T> action)
{
	return (IMyTerminalBlock block) =>
	{
		if(!(block is T))
			return false;
			
		action((T)block);
		return true;
	};
}

static GroupFunction MakeGroupFunction<T>(List<T> list, Action<T> action) where T : class
{
	return (IMyBlockGroup group) =>
	{
		list.Clear();
		group.GetBlocksOfType<T>(list);
		
		if(list.Count == 0)
			return false;
			
		foreach(var item in list)
			action(item);
			
		return true;
	};
}

public enum Qualifier
{
	Block,
	Group,
}
	
public struct Command
{
	private string name;
	private string type;
	private string action;
	private Qualifier qualifier;

	public string Name { get { return this.name; } }
	public string Type { get { return this.type; } }
	public string Action { get { return this.action; } }
	public Qualifier Qualifier { get { return this.qualifier; } }
		
	public static bool TryParse(string input, ref Command command)
	{		
		Command result = new Command();
		Parser parser = new Parser(input);
		
		// Read Action
		result.action = parser.ReadIdentifier();		
		if(result.action == null)
			return false;
		parser.ConsumeWhitespace();
		
		// Read Type
		result.type = parser.ReadIdentifier();	
		if(result.type == null)
			return false;
		parser.ConsumeWhitespace();
		
		// Read Qualifier
		var qualifier = parser.ReadIdentifier();		
		if(qualifier == null)
			return false;
		if(!Enum.TryParse(qualifier, true, out result.qualifier))
			return false;	
		parser.ConsumeWhitespace();
		
		// Read Name
		result.name = parser.ReadStringOrIdentifier();
		if(result.name == null)
			return false;
		parser.ConsumeWhitespace();
			
		command = result;
		return true;
	}	
}

public class StringReader
{
	private readonly string source;
	private int index;
	
	public StringReader(string source)
	{
		if(source == null)
			throw new ArgumentNullException("source");
		this.source = source;
	}
	
	public int Peek()
	{
		return
			(this.index >= 0 && this.index < this.source.Length) ?
			(int)this.source[index] :
			-1;
	}
	
	public char Read()
	{
		if(this.index < 0 || this.index >= this.source.Length)
			throw new InvalidOperationException("Attempt to read past end of string");
			
		char result = this.source[index];
		++this.index;
		return result;
	}
	
	public int Read(char[] buffer, int index, int count)
	{
		int bytesRead = 0;
		for(int j = index; j < (index + count) && this.index < this.source.Length; ++j)
		{
			char c = this.source[this.index];			
			buffer[j] = c;
			++this.index;
			++bytesRead;
		}
		return bytesRead;
	}
	
	public string ReadLine()
	{
		int end = -1;
		int start = this.index;
		while(this.index < this.source.Length)
		{
			char c = this.source[this.index];
			if(c == '\r')
			{
				end = this.index;
				++this.index;
				c = this.source[this.index];
				if(c == '\n')
				{
					++this.index;
				}
				return this.source.Substring(start, end - start);
			}
			else if(c == '\n')
			{
				end = this.index;
				++this.index;
				return this.source.Substring(start, end - start);
			}
		}
		return null;
	}
	
	public string ReadToEnd()
	{
		return
			(this.index > 0) ? 
			this.source.Substring(index) :
			this.source;
	}
}

public class Parser
{
	readonly StringReader reader;
	readonly StringBuilder builder = new StringBuilder();
	readonly Dictionary<char, char> escapeChars = new Dictionary<char, char>()
	{
		{ '"', '"' }
	};
	
	public Parser(string source)
	{
		if(source == null)
			throw new ArgumentNullException("source");	
		this.reader = new StringReader(source);
	}
	
	public Parser(string source, Dictionary<char, char> escapeChars)
	{
		if(source == null)
			throw new ArgumentNullException("source");	
		this.reader = new StringReader(source);
		this.escapeChars = new Dictionary<char, char>(escapeChars);
	}
	
	public string ReadIdentifier()
	{
		builder.Clear();
		for(int i = reader.Peek(); i != -1; i = reader.Peek())
		{
			char c = (char)i;
			if(!char.IsLetter(c))
				break;
			builder.Append(c);
			reader.Read();
		}
		return builder.ToString();
	}

	public string ReadString()
	{
		builder.Clear();
		bool escapeMode = false;
		
		if(reader.Peek() != '"')
			return null;
		reader.Read();
		
		for(int i = reader.Peek(); i != -1; i = reader.Peek())
		{
			char c = (char)i;
			if(escapeMode)
			{
				char escapeChar = '\0';
				if(!escapeChars.TryGetValue(c, out escapeChar))
					return null;					
				c = escapeChar;
				escapeMode = false;
			}
			else
			{
				if(c == '\\')
				{
					escapeMode = true;
					reader.Read();
					continue;
				}
				else if(c == '"')
					return builder.ToString();
				else if(reader.Peek() == -1)
					return null;
			}
			builder.Append(c);
			reader.Read();
		}
		return builder.ToString();
	}
	
	public string ReadStringOrIdentifier()
	{
		builder.Clear();
		bool quoteMode = false;
		bool escapeMode = false;
		for(int i = reader.Peek(); i != -1; i = reader.Peek())
		{
			char c = (char)i;
			if(escapeMode)
			{
				char escapeChar = '\0';
				if(!escapeChars.TryGetValue(c, out escapeChar))
					return null;					
				c = escapeChar;
				escapeMode = false;
			}
			else if(quoteMode)
			{
				if(c == '\\')
				{
					escapeMode = true;
					reader.Read();
					continue;
				}
				else if(c == '"')
					break;
				else if(reader.Peek() == -1)
					return null;
			}
			else
			{
				if(c == '"')
				{
					quoteMode = true;
					reader.Read();
					continue;
				}					
				else if(!char.IsLetter(c))
					break;
			}
			builder.Append(c);
			reader.Read();
		}
		return builder.ToString();
	}
	
	public void ConsumeWhitespace()
	{
		for(int i = reader.Peek(); i != -1; i = reader.Peek())
		{
			char c = (char)i;
			if(!char.IsWhiteSpace(c))
				return;
			reader.Read();
		}
	}
	
	public void ReadWhitespace()
	{
		for(int i = reader.Peek(); i != -1; i = reader.Peek())
		{
			char c = (char)i;
			if(!char.IsWhiteSpace(c))
				return;
			builder.Append(c);
			reader.Read();
		}
	}
}
