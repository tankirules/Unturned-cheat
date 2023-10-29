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
        public List<Zombie> zombielist = new List<Zombie>();
        public List<SteamPlayer> players = new List<SteamPlayer>();


        public float zombiedistance = 60;
        void Start()
        {
            File.WriteAllText("UC.txt", "Cheat Loaded!\n");
            Debug.Log("Cheat loaded!");
            //GUI.color = Color.cyan;
            //GUI.Label(new Rect(200f, 0f, 200f, 40f), "HACK INJCETED");
            StartCoroutine(zombiecheck());
            StartCoroutine(playercheck());

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
            
            //handle zombies
            foreach (Zombie z in zombielist)
            {
                ESP.drawzombielabel(z);
                ESP.renderzombie(z);
            }


            //TODO: HANDLE PLAYERS
            File.AppendAllText("UC.txt", "clients size is: " + players.Count);
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
                        Vector3 zombiepos = z.transform.position;
                        Vector2 zombiepos2d = new Vector2(zombiepos.x, zombiepos.z);
                        //check if zombie is too far, dont add to list
                        if (Vector2.Distance(playerpos2d,zombiepos2d) > zombiedistance)
                        {
                            continue;
                        }                        
                        zombielist.Add(z);
                    }


                    //TODO:REMOVE
                    //no instance of zombiemanager
                    List<Zombie> lz0 = new List<Zombie>();
                    ZombieManager.getZombiesInRadius(playerpos, zombiedistance * zombiedistance, lz0);
                    //TODO: COMPARE Vector3 playerpos = Player.player.transform.position; to Player.player.base.transform.position;

                    //File.AppendAllText("UC.txt", "lz0 size is : " + lz0.Count() + "\n");
                    //File.AppendAllText("UC.txt", "zombie region size is : " + ZombieManager.regions.Length + "\n");
                    //bool lv = LevelNavigation.tryGetNavigation(playerpos, out var nav);
                    //File.AppendAllText("UC.txt", "lv is : " + lv + "\n" + "nav is : " + nav + "\n");
                    //File.AppendAllText("UC.txt", "is regions[nav] null? " + (ZombieManager.regions[nav] == null) + "\n");
                    //File.AppendAllText("UC.txt", "is regions[nav].zombies null? " + (ZombieManager.regions[nav].zombies == null) + "\n");

                }
                //Debug.Log("checking zombies!");


                yield return new WaitForSeconds(4f);
            }

            
        }

        IEnumerator playercheck()
        {
            while (true) {
                players.Clear();
                if (Provider.isConnected) {
                    foreach (SteamPlayer SP in Provider.clients)
                    {
                        players.Add(SP);
                    }
                }
                
                yield return new WaitForSeconds(4f);
            }
        }

    }
}
