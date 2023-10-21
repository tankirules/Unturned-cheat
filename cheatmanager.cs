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
        public float zombiedistance = 60;
        void Start()
        {
            File.WriteAllText("UC.txt", "Cheat Loaded!");
            Debug.Log("Cheat loaded!");
            //GUI.color = Color.cyan;
            //GUI.Label(new Rect(200f, 0f, 200f, 40f), "HACK INJCETED");
            StartCoroutine(zombiecheck());

        }

        public void drawzombielabel(Zombie z)
        {

            Vector3 zombiepos = z.gameObject.transform.position;
            //offset the y position since the gameobject is at their feet
            zombiepos.y = zombiepos.y + 2.5f;
            Vector3 pos = Camera.main.WorldToScreenPoint(zombiepos);
            //check if the label is "behind" the camera
            //if so, do not render!
            if (pos.z < 0)
            {
                return;
            }


            //IMPORTANT! World to sreenpoint puts 0,0 at bottom left!

            GUI.color = Color.yellow;
            GUIContent gc = new GUIContent("Zombie");            

            GUIStyle gs = GUI.skin.label;            
            gs.alignment = TextAnchor.MiddleCenter;
            gs.fontSize = 14;
            Vector2 size = gs.CalcSize(gc);

            pos.y = Screen.height - pos.y;

            Rect rectangle = new Rect(pos.x - size.x / 2, pos.y, size.x,size.y);

            GUI.Label(rectangle, gc);


            
        }

        void OnGUI()
        {
            GUI.color = Color.cyan;
            GUI.Label(new Rect(200f, 0f, 200f, 40f), "HACK INJCETED");

            if (!Provider.isConnected)
            {
                GUI.color = Color.red;
                GUI.Label(new Rect(400f, 0f, 200f, 40f), "NOT connected!");
                return;
            }
            

            foreach (Zombie z in zombielist)
            {
                drawzombielabel(z);

            }
        }

        //coroutine for checking zombie positions
        IEnumerator zombiecheck()
        {
            while (true)
            {
                
                if (Provider.isConnected)
                {
                    List<Zombie> tempz = new List<Zombie>();
                    Vector3 playerpos = Player.player.transform.position;
                    Vector2 playerpos2d = new Vector2(playerpos.x, playerpos.z);

                    foreach (Zombie z in FindObjectsOfType<Zombie>())
                    {
                        Vector3 zombiepos = z.gameObject.transform.position;
                        Vector2 zombiepos2d = new Vector2(zombiepos.x, zombiepos.z);
                        //check if zombie is too far, dont add to list
                        if (Vector2.Distance(playerpos2d,zombiepos2d) > zombiedistance)
                        {
                            continue;
                        }
                        tempz.Add(z);                      
                    }


                    zombielist = tempz.ToArray();
                    //GUI.color = Color.white;
                    //GUI.Label(new Rect(600f, 0f, 200f, 40f), "finding Zombies!");
                }
                Debug.Log("checking zombies!");
                yield return new WaitForSeconds(4f);
            }
        }
    }
}
