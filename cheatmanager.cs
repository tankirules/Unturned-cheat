using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.IO;
using JetBrains.Annotations;
using UnityEngine.UI;

namespace Unturned_Cheat
{
    public class cheatmanager : MonoBehaviour
    {
        public Zombie[] zombielist = new Zombie[] { };
        public static void start()
        {
            File.WriteAllText("UC.txt", "Cheat Loaded!");
            GUI.color = Color.cyan;
            GUI.Label(new Rect(200f, 0f, 200f, 40f), "HACK INJCETED");

        }

        void update()
        {
            if (!Provider.isConnected)
            {
                return;
            }
            //Vector3 playerpos = Player.player.transform.position;
            foreach (Zombie z in zombielist)
            {
                drawzombielabel(z);               

            }
        }

        public static void drawzombielabel(Zombie z)
        {
            //IMPORTANT! World to sreenpoint puts 0,0 at bottom left!
            Vector3 pos = Camera.main.WorldToScreenPoint(z.gameObject.transform.position);

            GUI.color = Color.black;
            GUIContent gc = new GUIContent("Zombie");            

            GUIStyle gs = GUI.skin.label;            
            gs.alignment = TextAnchor.MiddleCenter;
            Vector2 size = gs.CalcSize(gc);

            pos.y = Screen.height - pos.y;

            Rect rectangle = new Rect(pos.x, pos.y, size.x,size.y);

            GUI.Label(rectangle, gc);


            
        }

        void OnGUI()
        {
            GUI.color = Color.cyan;
            GUI.Label(new Rect(200f, 0f, 200f, 40f), "HACK INJCETED");
        }

        //coroutine for checking zombie positions
        IEnumerator zombiecheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(4f);
                if (Provider.isConnected)
                {
                    
                    zombielist = FindObjectsOfType<Zombie>();
                }
            }
        }
    }
}
