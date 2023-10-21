using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

namespace Unturned_Cheat
{
    class cheat : MonoBehaviour
    {
        //this runs when the gameobject is initialized
        public static void start()
        {

        }

        void OnGUI()
        {
            GUI.color = Color.cyan;
            GUI.Label(new Rect(200f, 0f, 200f, 40f), "HACK INJCETED");
            
        }

        void getplayers()
        {
            
        }
    }
}
