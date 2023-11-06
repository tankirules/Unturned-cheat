using HighlightingSystem;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace Unturned_Cheat
{
    public class ESP : MonoBehaviour
    {
        public static void drawzombielabel(Zombie z)
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

            //flip the y position 
            pos.y = Screen.height - pos.y;

            Rect rectangle = new Rect(pos.x - size.x / 2, pos.y, size.x, size.y);

            GUI.Label(rectangle, gc);


            //Material zm = z.gameObject.GetComponent<SkinnedMeshRenderer>().material;

            //File.AppendAllText("UC.txt", "zm mat is" + zm + "Zombie render queue is: " + zm.renderQueue);

            //zm.renderQueue = 4000;

        }
        public static void renderzombie(Zombie z)
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
                //File.AppendAllText("UC.txt", "Num of renderer for zombie: " + r.Length + "\n");
                //rm.renderQueue = 4000;
                //File.AppendAllText("UC.txt", "mat name: " + rm.ToString() + "\n");
                //File.AppendAllText("UC.txt", "rm renderqueue :" + rm.renderQueue + "\n");
                //File.AppendAllText("UC.txt", "mat array size: " + rm.Length + "\n");
                //File.AppendAllText("UC.txt", "shader:  " + rm.shader + "shader keywords: " + rm.shaderKeywords + "\n");

                //THIS WORKS!!!!
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                Material mat = new Material(shader);
                mat.SetColor("_Color", new Color32(115, 135, 105, 255));
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

            //draw a highlight outline around the zombies
            Highlighter highlighter = z.transform.GetComponent<Highlighter>();
            if (highlighter == null)
            {
                highlighter = z.transform.gameObject.AddComponent<Highlighter>();
            }
            highlighter.ConstantOn(Color.yellow);
            //these two settings make the highlight render through walls
            highlighter.occluder = true;
            highlighter.overlay = true;

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

        public static void drawvehiclelabel(InteractableVehicle v)
        {

            //get vehicle name and position
            Vector3 vpos = v.transform.position;
            //offset vpos since it is at the "floor" of the vehicle
            vpos.y = vpos.y + 2.5f;
            String vname = v.asset.vehicleName;

            //translate vehicle position to screen coordinates
            Vector3 pos = Camera.main.WorldToScreenPoint(vpos);

            //check if the label is "behind" the camera
            //if so, do not render!
            if (pos.z < 0)
            {
                return;
            }

            //set GUI colouor and content
            GUI.color = Color.red;
            GUIContent gc = new GUIContent("Vehicle: " + vname);

            GUIStyle gs = GUI.skin.label;
            gs.alignment = TextAnchor.MiddleCenter;
            gs.fontSize = 14;
            Vector2 size = gs.CalcSize(gc);

            //Invert y coords since WorldToScreenPoint returns inverted y coordinates
            pos.y = Screen.height - pos.y;

            Rect rectangle = new Rect(pos.x - size.x / 2, pos.y, size.x, size.y);
            //draw the label
            GUI.Label(rectangle, gc);


        }
        public static void rendervehicle(InteractableVehicle v)
        {

            //Renderer[] r = v.GetComponentsInChildren<Renderer>();
            ////File.AppendAllText("UC.txt", "num of renderer for vehicle: " + r.Length + "\n");
            //foreach (Renderer re in r)
            //{
            //    Shader shader = Shader.Find("Hidden/Internal-Colored");
            //    Material mat = new Material(shader);
            //    mat.SetColor("_Color", Color.green);
            //    mat.renderQueue = 4000;
            //    mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            //    mat.SetInt("_ZWrite", 0);
            //    mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
            //    re.material = mat;
            //}

            //var rp = v.GetComponent<Renderer>();
            //var rt = v.transform.GetComponent<Renderer>();
            //File.AppendAllText("UC.txt", "rp is n " + (rp == null) + " and rt is n " + (rt == null) + "\n");
            //foreach (Transform child in v.transform)
            //{
            //    File.AppendAllText("UC.txt", child.name + " is n " + (child.GetComponent<Renderer>() == null) + "\n");
            //}

            foreach (Transform t in v.transform)
            {
                if (t.name == "Model_0" || t.name == "Model_1")
                {
                    Renderer re = t.GetComponent<Renderer>();
                    if (re != null)
                    {
                        Shader shader = Shader.Find("Hidden/Internal-Colored");
                        Material mat = new Material(shader);
                        mat.SetColor("_Color", Color.yellow);
                        mat.renderQueue = 4000;
                        mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                        mat.SetInt("_ZWrite", 0);
                        mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
                        re.material = mat;
                    }


                }
            }





        }

        public static void drawitemlabel(InteractableItem i)
        {
            Vector3 itempos = i.gameObject.transform.position;
            Vector3 pos = Camera.main.WorldToScreenPoint(itempos);
            //check if the label is "behind" the camera
            //if so, do not render!
            if (pos.z < 0)
            {
                return;
            }
            GUI.color = Color.green;
            GUIContent gc = new GUIContent(i.asset.rarity + " Item: " + i.asset.itemName);
            GUIStyle gs = GUI.skin.label;
            gs.alignment = TextAnchor.MiddleCenter;
            gs.fontSize = 14;
            Vector2 size = gs.CalcSize(gc);
            pos.y = Screen.height - pos.y;


            Rect rectangle = new Rect(pos.x - size.x / 2, pos.y, size.x, size.y);

            GUI.Label(rectangle, gc);


        }

        public static void renderitem(InteractableItem i)
        {
            Renderer[] r = i.GetComponentsInChildren<Renderer>();

            foreach (Renderer re in r)
            {
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                Material mat = new Material(shader);

                Color raritycolour = ItemTool.getRarityColorHighlight(i.asset.rarity);

                mat.SetColor("_Color", raritycolour);
                mat.renderQueue = 4000;
                mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                mat.SetInt("_ZWrite", 0);
                mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
                re.material = mat;
            }
        }

        public static void drawplayerlabel(Player p)
        {
            Vector3 playerpos = p.gameObject.transform.position;
            //offset the y position since the transform is at their feet
            playerpos.y = playerpos.y + 2.5f;
            Vector3 pos = Camera.main.WorldToScreenPoint(playerpos);
            //check if the label is "behind" the camera
            //if so, do not render!
            if (pos.z < 0)
            {
                return;
            }

            GUI.color = Color.magenta;
            //get distance from other player to us
            var dist = Vector3.Distance(playerpos, Player.player.transform.position);

            //basic setup to draw a label
            GUIContent gc = new GUIContent(p.name + "\n" + Math.Round(dist, 2) + "m\n");
            GUIStyle gs = GUI.skin.label;
            gs.alignment = TextAnchor.MiddleCenter;
            gs.fontSize = 14;
            Vector2 size = gs.CalcSize(gc);

            //flip y coords since WorldToScreenPoint generates the y coordinate upside down
            pos.y = Screen.height - pos.y;

            Rect rectangle = new Rect(pos.x - size.x / 2, pos.y, size.x, size.y);

            GUI.Label(rectangle, gc);

        }

        public static void renderplayer(Player p)
        {
            Renderer[] r = p.GetComponentsInChildren<Renderer>();

            foreach (Renderer re in r)
            {
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                Material mat = new Material(shader);

                mat.SetColor("_Color", Color.cyan);
                mat.renderQueue = 4000;
                mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                mat.SetInt("_ZWrite", 0);
                mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
                re.material = mat;
            }

            //draw a highlight outline around the player
            Highlighter highlighter = p.transform.GetComponent<Highlighter>();
            if (highlighter == null)
            {
                highlighter = p.transform.gameObject.AddComponent<Highlighter>();
            }
            highlighter.ConstantOn(Color.red);
            //these two settings make the highlight render through walls
            highlighter.occluder = true;
            highlighter.overlay = true;
        }

        public static void zombietracer(Zombie z)
        {
            //setup zombie coordinates
            Vector3 zombiepos = z.gameObject.transform.position;
            //offset the y position since the gameobject is at their feet
            zombiepos.y = zombiepos.y + 2.5f;
            Vector3 pos = Camera.main.WorldToScreenPoint(zombiepos);
            //flip the y position 
            pos.y = Screen.height - pos.y;

            if (pos.z < 0)
            {
                return;
            }


            //basic material/shader setup
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            Material mat = new Material(shader);
            //mat.SetColor("_Color", new Color32(115, 135, 105, 255));
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            mat.SetInt("_ZWrite", 0);
            mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

            

            //Low level graphics rendering I don't fully understand
            mat.SetPass(0);
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            GL.Color(new Color32(115, 135, 105, 255));
            GL.Vertex3(pos.x,pos.y, 0f);
            GL.Vertex3(Screen.width / 2, Screen.height / 2, 0f);
            GL.End();
            GL.PopMatrix();

        }


    }
}
