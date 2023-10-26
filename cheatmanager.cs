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
using HarmonyLib;
using HighlightingSystem;

namespace Unturned_Cheat
{
    public class cheatmanager : MonoBehaviour
    {
        //public Zombie[] zombielist = new Zombie[] { };
        public List<Zombie> zombielist = new List<Zombie>();
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
            var dist = Vector3.Distance(zombiepos, Player.player.transform.position);
            GUIContent gc = new GUIContent("Zombie\n" + Math.Round(dist, 2) + "m");            

            GUIStyle gs = GUI.skin.label;            
            gs.alignment = TextAnchor.MiddleCenter;
            gs.fontSize = 14;
            Vector2 size = gs.CalcSize(gc);

            pos.y = Screen.height - pos.y;

            Rect rectangle = new Rect(pos.x - size.x / 2, pos.y, size.x,size.y);

            GUI.Label(rectangle, gc);

            
            //Material zm = z.gameObject.GetComponent<SkinnedMeshRenderer>().material;

            //File.AppendAllText("UC.txt", "zm mat is" + zm + "Zombie render queue is: " + zm.renderQueue);

            //zm.renderQueue = 4000;

        }
        public void renderzombie (Zombie z)
        {
            //Animation za = Traverse.Create(z).Field("animator").GetValue() as Animation;
            //if (za == null)
            //{
            //    File.AppendAllText("UC.txt", "animation zm is null");
            //}

            //Transform zt0 = Traverse.Create(z).Field("attachmentModel_0").GetValue() as Transform;
            //Transform zt1 = Traverse.Create(z).Field("attachmentModel_1").GetValue() as Transform;
            //if (zt0 == null)
            //{
            //    File.AppendAllText("UC.txt", "zt0 is null");
            //}
            //if (zt1 == null)
            //{
            //    File.AppendAllText("UC.txt", "zt1 is null");
            //}


            //Component[] comps = z.GetComponents(typeof(Component));
            //foreach (Component c in comps)
            //{
            //    File.AppendAllText("UC.txt", "Comp: " + c.ToString() + "\n");
            //}

            Renderer[] r = z.GetComponentsInChildren<Renderer>();
            //File.AppendAllText("UC.txt", "R array size: " + r.Length + "\n");

            foreach (Renderer re in r)
            {

                //rm.renderQueue = 4000;
                //File.AppendAllText("UC.txt", "mat name: " + rm.ToString() + "\n");
                //File.AppendAllText("UC.txt", "rm renderqueue :" + rm.renderQueue + "\n");
                //File.AppendAllText("UC.txt", "mat array size: " + rm.Length + "\n");
                //File.AppendAllText("UC.txt", "shader:  " + rm.shader + "shader keywords: " + rm.shaderKeywords + "\n");

                //THIS WORKS!!!!
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                Material mat = new Material(shader);
                mat.renderQueue = 4000;
                mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                mat.SetInt("_ZWrite", 0);
                mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
                re.material = mat;
                //THIS WORKS!!!!3
                //Material rm = re.material;
                //rm.renderQueue = 4000;
                //rm.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                //rm.SetInt("_ZWrite", 0);
                //rm.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
                //re.material = rm;


            }

            Highlighter highlighter = z.transform.GetComponent<Highlighter>();
            if (highlighter == null)
            {
                highlighter = z.transform.gameObject.AddComponent<Highlighter>();
            }
            highlighter.ConstantOn(Color.yellow);

            //try to cheese the higlighter render?
            //Renderer[] stupidr = highlighter.GetComponentsInChildren<Renderer>();
            //foreach (Renderer stupidre in stupidr)
            //{
            //    Material srm = stupidre.material;
            //    srm.renderQueue = 4000;
            //    srm.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            //    srm.SetInt("_ZWrite", 0);
            //    srm.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
            //    stupidre.material = srm;
            //}

            //Renderer renderer = z.transform.GetComponent<Renderer>();
            //if (renderer == null)
            //{
            //    File.AppendAllText("UC.txt", "zombie rendewrer is null!");
            //    return;
            //}
            //File.AppendAllText("UC.txt", "Zombie render queue is: " + zm.renderQueue);

                //zm.renderQueue = 4000;




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
                renderzombie(z);

            }
        }

        //coroutine for checking zombie positions
        IEnumerator zombiecheck()
        {
            while (true)
            {
                
                if (Provider.isConnected)
                {
                    //List<Zombie> tempz = new List<Zombie>();
                    zombielist.Clear();
                    Vector3 playerpos = Player.player.transform.position;
                    Vector2 playerpos2d = new Vector2(playerpos.x, playerpos.z);

                    foreach (Zombie z in FindObjectsOfType<Zombie>())
                    {
                        //check if zombie is dead
                        //if so, don't add
                        if (z.isDead)
                        {
                            continue;
                        }
                        Vector3 zombiepos = z.gameObject.transform.position;
                        Vector2 zombiepos2d = new Vector2(zombiepos.x, zombiepos.z);
                        //check if zombie is too far, dont add to list
                        if (Vector2.Distance(playerpos2d,zombiepos2d) > zombiedistance)
                        {
                            continue;
                        }                        
                        zombielist.Add(z);                      
                    }


                    //zombielist = tempz.ToArray();
                    //GUI.color = Color.white;
                    //GUI.Label(new Rect(600f, 0f, 200f, 40f), "finding Zombies!");
                }
                Debug.Log("checking zombies!");
                yield return new WaitForSeconds(4f);
            }
        }
    }
}
