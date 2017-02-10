static class GpsHelper 
{ 
    public static string ToString(string name, Vector3 vector) 
    { 
        return string.Format("GPS:{0}:{1}:{2}:{3}:", name, vector.X, vector.Y, vector.Z); 
    } 
     
    public static string ToString(string name, Vector3D vector) 
    { 
        return string.Format("GPS:{0}:{1}:{2}:{3}:", name, vector.X, vector.Y, vector.Z); 
    } 
     
    public static string ToString(string name, float x, float y, float z) 
    { 
        return string.Format("GPS:{0}:{1}:{2}:{3}:", name, x, y, z); 
    } 
     
    public static string ToString(string name, double x, double y, double z) 
    { 
        return string.Format("GPS:{0}:{1}:{2}:{3}:", name, x, y, z); 
    } 
     
    public static bool TryParse(string gpsString, out string name, out Vector3 vector) 
    { 
        var parts = gpsString.Trim().Split(':'); 
         
        name = null; 
        vector = new Vector3();
 
        if (parts.Length != 6) 
            return false; 
 
        name = parts[1]; 
 
        return 
            parts[0] == "GPS" && 
            float.TryParse(parts[2], out vector.X) && 
            float.TryParse(parts[3], out vector.Y) && 
            float.TryParse(parts[4], out vector.Z) && 
            parts[5] == ""; 
    } 
     
    public static bool TryParse(string gpsString, out string name, out Vector3D vector) 
    { 
        var parts = gpsString.Trim().Split(':'); 
         
        name = null; 
        vector = new Vector3();
 
        if (parts.Length != 6) 
            return false; 
 
        name = parts[1]; 
 
        return 
            parts[0] == "GPS" && 
            double.TryParse(parts[2], out vector.X) && 
            double.TryParse(parts[3], out vector.Y) && 
            double.TryParse(parts[4], out vector.Z) && 
            parts[5] == ""; 
    } 
     
    public static bool TryParse(string gpsString, out string name, out float x, out float y, out float z) 
    { 
        var parts = gpsString.Trim().Split(':'); 
         
        name = null; 
        x = 0; 
        y = 0; 
        z = 0; 
 
        if (parts.Length != 6) 
            return false; 
 
        name = parts[1]; 
 
        return 
            parts[0] == "GPS" && 
            float.TryParse(parts[2], out x) && 
            float.TryParse(parts[3], out y) && 
            float.TryParse(parts[4], out z) && 
            parts[5] == ""; 
    } 
     
    public static bool TryParse(string gpsString, out string name, out double x, out double y, out double z) 
    { 
        var parts = gpsString.Trim().Split(':'); 
         
        name = null; 
        x = 0; 
        y = 0; 
        z = 0; 
 
        if (parts.Length != 6) 
            return false; 
 
        name = parts[1]; 
 
        return 
            parts[0] == "GPS" && 
            double.TryParse(parts[2], out x) && 
            double.TryParse(parts[3], out y) && 
            double.TryParse(parts[4], out z) && 
            parts[5] == ""; 
    } 
}
