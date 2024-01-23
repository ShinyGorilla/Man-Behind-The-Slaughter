using System;
using System.IO;
using System.Reflection;
using BepInEx;
using Unity.Mathematics;
using UnityEngine;
using Utilla;

namespace ManBehindTheSlaughter
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public GameObject PlaneObj;

        public bool active;
        bool inRoom;

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            if (inRoom)
            {
                PlaneObj.SetActive(true);
            }

            active = true;

            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            PlaneObj.SetActive(false);

            active = false;

            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            var assetBundle = LoadAssetBundle("ManBehindTheSlaughter.slaughter");
            GameObject Obj = assetBundle.LoadAsset<GameObject>("PurpleObj");

            PlaneObj = Instantiate(Obj);
            PlaneObj.transform.position = new Vector3(-22.6647f, 4.2474f, -60.3607f);
            PlaneObj.transform.rotation = Quaternion.Euler(89.972f, 259.8621f, 0f);

            PlaneObj.SetActive(false);
        }

        AssetBundle LoadAssetBundle(string path)
        {
            try
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                AssetBundle bundle = AssetBundle.LoadFromStream(stream);
                stream.Close();
                Debug.Log("[" + PluginInfo.GUID + "] Success loading asset bundle");
                return bundle;
            }
            catch (Exception e)
            {
                Debug.Log("[" + PluginInfo.Name + "] Error loading asset bundle: " + e.Message + " " + path);
                throw;
            }
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            if (active)
            {
                PlaneObj.SetActive(true);
            }

            inRoom = true;
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            PlaneObj.SetActive(false);

            inRoom = false;
        }
    }
}