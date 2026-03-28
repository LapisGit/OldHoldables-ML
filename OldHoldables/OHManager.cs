using GorillaNetworking;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace OldHoldables
{
    public class OHManager : MonoBehaviour
    {
        bool IsSteamVR;
        bool initialized = false;
        
        void Awake()
        {
            GorillaTagger.OnPlayerSpawned(GameInitialized);
        }

        void GameInitialized()
        {
            IsSteamVR = Traverse.Create(PlayFabAuthenticator.instance).Field("platform").GetValue().ToString().ToLower() == "steam";
            initialized = true;
        }

        void OnDisable() => HarmonyPatches.RemoveHarmonyPatches();

        public static bool RightStickClick = false;
        private float DropTime;

        void LateUpdate()
        {
            if (!initialized) return;
            
            if (IsSteamVR)
                RightStickClick = SteamVR_Actions.gorillaTag_RightJoystickClick.GetState(SteamVR_Input_Sources.RightHand);
            else
                ControllerInputPoller.instance.rightControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out RightStickClick);

            if (!Plugin.disableDropping.Value)
            {
                if (RightStickClick && (DropTime + 3) < Time.time) { DropManually(); }
            }
        }
        
        void DropManually()
        {
            HarmonyPatches.SetGoingToChange = true;
            EquipmentInteractor.instance.ReleaseLeftHand();
            EquipmentInteractor.instance.ReleaseRightHand();
            HarmonyPatches.SetGoingToChange = false;
            DropTime = Time.time;
        }
    }
}