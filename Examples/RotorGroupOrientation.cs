// Directions for use:
// 1. Set Programmable Block Custom Data to the name of the rotor group
// 2. Set the script to run on a timer
// 3. Run the block with the target angle (in degrees) as an argument

const float degToRad = (float)Math.PI / 180f;
 
private float targetAngle = 0;
 
private void AdjustRotor(IMyMotorStator rotor, float errorMargin) 
{ 
    var diff = (targetAngle - rotor.Angle); 
    rotor.TargetVelocity = (Math.Abs(diff) > degToRad * errorMargin) ? diff * 0.5f : 0.0f; 
} 
 
public void Main(string argument) 
{ 
    var rotorGroup = GridTerminalSystem.GetBlockGroupWithName(Me.CustomData); 
    var rotorList = new List<IMyMotorStator>(); 
    rotorGroup.GetBlocksOfType(rotorList); 
      
    var arg = 0f;  
    if(float.TryParse(argument, out arg))  
    { 
        this.targetAngle = arg * degToRad; 
    } 
     
    foreach(var rotor in rotorList) 
    { 
        AdjustRotor(rotor, 5); // 5 degree error margin
    } 
} 
 
