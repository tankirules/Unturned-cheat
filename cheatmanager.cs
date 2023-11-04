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
        public List<Player> players = new List<Player>();
        public List<InteractableVehicle> vehicles = new List<InteractableVehicle>();
        public List<InteractableItem> items = new List<InteractableItem>();


        public float zombiedistance = 60;
        public float vehicledistance = 120;
        public float itemdistance = 60;

        void Awake()
        {
            DontDestroyOnLoad(this);
        }
        void Start()
        {
            File.WriteAllText("UC.txt", "Cheat Loaded!\n");
            Debug.Log("Cheat loaded!");
            //GUI.color = Color.cyan;
            //GUI.Label(new Rect(200f, 0f, 200f, 40f), "HACK INJCETED");
            StartCoroutine(objectcheck());
            //StartCoroutine(playercheck());

        }

        

        void OnGUI()
        {
            GUI.color = Color.cyan;
            GUI.Label(new Rect(200f, 0f, 200f, 40f), "HACK INJCETED");

            if (!Provider.isConnected)
            {
                zombielist.Clear();
                vehicles.Clear();
                GUI.color = Color.red;
                GUI.Label(new Rect(400f, 0f, 200f, 40f), "NOT connected!");
                return;
            }
            else
            {

                //File.AppendAllText("UC.txt", "numzombie : " + zombielist.Count + " numveh: " + vehicles.Count + "\n");
                //draw zombies
                foreach (Zombie z in zombielist)
                {
                    ESP.drawzombielabel(z);
                    ESP.renderzombie(z);
                }

                //draw vehicles
                foreach (InteractableVehicle v in vehicles)
                {
                    ESP.drawvehiclelabel(v);
                    ESP.rendervehicle(v);
                }
                
                //draw items
                foreach (InteractableItem i in items)
                {
                    if (i == null)
                    {
                        continue;
                    }
                    ESP.drawitemlabel(i);
                    if (i.asset.rarity >= EItemRarity.RARE || i.asset.type == EItemType.GUN)
                    {
                        ESP.renderitem(i);
                    }
                    
                }

                foreach (Player p in players)
                {
                    ESP.drawplayerlabel(p);
                    ESP.renderplayer(p);
                }

                //TODO: HANDLE PLAYERS
                //File.AppendAllText("UC.txt", "clients size is: " + players.Count);
            }


        }

        //coroutine for checking objects
        IEnumerator objectcheck()
        {
            while (true)
            {
                //AppendAllText("UC.txt", "coroutine running\n");
                if (Provider.isConnected && Player.player != null)
                {
                    //File.AppendAllText("UC.txt", "checking objects");
                    
                    //try 
                    //{
                    //    Vector3 pp = Player.player.transform.position;
                    //}
                    //catch (Exception e)
                    //{
                    //    File.AppendAllText("UC.txt", "Exception: " + e + "\n");
                    //    continue;
                    //}


                    Vector3 playerpos = Player.player.transform.position;
                    if (playerpos == null)
                    {
                        //File.AppendAllText("UC.txt", "palyerpos is null!\n");
                        continue;
                    }
                    Vector2 playerpos2d = new Vector2(playerpos.x, playerpos.z);
                    //File.AppendAllText("UC.txt", "before zombie check\n");
                    #region Zombiecheck
                    //List<Zombie> tempz = new List<Zombie>();
                    zombielist.Clear();                    

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
                        //check if zombie is too far from player, and don't add to list if so
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
                    #endregion
                    //File.AppendAllText("UC.txt", "before vehicle check\n");
                    #region Vehiclecheck
                    vehicles.Clear();
                    foreach (InteractableVehicle v in VehicleManager.vehicles)
                    {
                        //check if vehicle is locked and inside distance
                        Vector3 vpos = v.transform.position;
                        Vector2 vpos2d = new Vector2(vpos.x, vpos.z);

                        //if vehicle is locked or outside range, ignore it
                        if (v.isLocked || (Vector2.Distance(playerpos2d, vpos2d) > vehicledistance))
                        {
                            continue;
                        }

                        vehicles.Add(v);
                    }

                    #endregion
                    //File.AppendAllText("UC.txt", "After Vehicle Check\n");
                    #region itemcheck
                    items.Clear();
                    ItemManager.findSimulatedItemsInRadius(playerpos, itemdistance * itemdistance, items);
                    File.AppendAllText("UC.txt", "item list size is: " + items.Count + "\n");
                    #endregion

                    #region playercheck

                    players.Clear();
                    foreach (SteamPlayer sp in Provider.clients)
                    {
                        if (sp.player != Player.player)
                        {
                            players.Add(sp.player);
                        }
                    }

                    #endregion


                }



                yield return new WaitForSeconds(4f);
            }

            
        }

       

    }
}
