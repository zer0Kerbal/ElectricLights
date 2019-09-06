using System.Text;
using UnityEngine;
using KSP.Localization;

namespace ElectricLights
{
    class ModuleInteriorLight : PartModule, IModuleInfo
    {
        const float resourceRate = 1.0F;
        const string resourceType = "ElectricCharge";

        [KSPField]
        public double resourceAmount = 0.02;

        public bool interiorLight = false;

        private bool ready = false;

        public override void OnStart(StartState state)
        {
            if ((state != StartState.None) && (state != StartState.Editor))
            {
                ready = true;
#if DEBUG
                Debug.Log("ModuleInteriorLight.OnStart(): ready!");
#endif
            }
            base.OnStart(state);
        }

        public void FixedUpdate()
        {
            if (ready)
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    if (interiorLight)
                    {
                        double ecRequested = resourceAmount * resourceRate * TimeWarp.deltaTime;
                        if (part.RequestResource(resourceType, ecRequested) <= 0)
                        {
                            interiorLight = false;
#if DEBUG
                            Debug.Log("ModuleInteriorLight.FixedUpdate(): Electric Charge depleted!");
#endif
                        }
                    }
                }
            }
        }

        public override string GetInfo()
        {
            StringBuilder info = new StringBuilder(base.GetInfo());
            info.AppendLine(Localizer.Format("<color=#FF8C00><b><<1>></b></color>", Localizer.GetStringByTag("#autoLOC_244332")));
            info.Append(Localizer.Format(Localizer.GetStringByTag("#autoLOC_244201"), Localizer.GetStringByTag("#autoLOC_501004"), (resourceRate * 60 * resourceAmount).ToString()));
            return info.ToString();
        }

        public string GetModuleTitle()
        {
            return Localizer.GetStringByTag("#autoLOC_6003003");
        }

        public override string GetModuleDisplayName()
        {
            return Localizer.GetStringByTag("#autoLOC_6003003");
        }

        public string GetPrimaryField()
        {
            return null;
        }

        public Callback<Rect> GetDrawModulePanelCallback()
        {
            return null;
        }
    }
}
