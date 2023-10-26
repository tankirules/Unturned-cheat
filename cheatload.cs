namespace Unturned_Cheat    
{
    using SDG.Unturned;
    using UnityEngine;
    public class cheatload
    {
        public static GameObject cheatobj;
        public static void init()
        {
            cheatobj = new GameObject();
            UnityEngine.Object.DontDestroyOnLoad(cheatobj);
            //then add component of our class to this
            cheatobj.AddComponent<cheatmanager>();
            
        }

    }
}