using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Scripting{
    class GlobalVarMap{
        public static Dictionary<string, int> ints;
        public static Dictionary<string, bool> bools;
        public static Dictionary<string, string> strings;
        public static Dictionary<string, char> chars;
        public static Dictionary<string, float> floats;
        public static Dictionary<string, object> objects; //Use at with caution!!

        public static void Initialize(){
            ints = new Dictionary<string, int>();
            bools = new Dictionary<string, bool>();
            strings = new Dictionary<string, string>();
            chars = new Dictionary<string, char>();
            floats = new Dictionary<string, float>();
            objects = new Dictionary<string, object>();
        }
    }
}
