using System.Text;
using UnityEngine;

namespace ElectricLights
{
    class ModuleAnimateGenericConsumer : ModuleAnimateGeneric
    {
        ModuleInteriorLight moduleInteriorLight;

        private bool lastAnimSwitch = false;
        private bool ready = false;

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            if ((state != StartState.None) && (state != StartState.Editor))
            {
                moduleInteriorLight = part.Modules.GetModule<ModuleInteriorLight>();
                if (moduleInteriorLight != null)
                {
                    lastAnimSwitch = animSwitch;
                    moduleInteriorLight.interiorLight = !animSwitch; // Light animSwitch is reversed (ON=False, OFF=True)
                    ready = true;
#if DEBUG
                    Debug.Log("ModuleAnimateGenericConsumer.OnStart(): ready!");
#endif
                }
            }
        }

        public override void OnUpdate()
        {
            if (ready)
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    if (animSwitch != lastAnimSwitch)
                    {
                        // Animation Lights changed
                        if (animSwitch)
                        {
                            // Animation Lights are now OFF
                            moduleInteriorLight.interiorLight = false;
#if DEBUG
                            Debug.Log("ModuleAnimateGenericConsumer.OnUpdate(): Switched OFF interior lights.");
#endif
                        }
                        else
                        {
                            // Animation Lights are now ON
                            moduleInteriorLight.interiorLight = true;
#if DEBUG
                            Debug.Log("ModuleAnimateGenericConsumer.OnUpdate(): Switched ON interior lights.");
#endif
                        }
                        lastAnimSwitch = animSwitch;
                    }
                    else if (!moduleInteriorLight.interiorLight && !animSwitch)
                    {
                        // Interior Lights OFF (EC depleted) and Animation Lights ON (animSwitch is reversed: ON=False, OFF=True)
                        Toggle();
#if DEBUG
                        Debug.Log("ModuleAnimateGenericConsumer.OnUpdate(): Turned OFF interior lights. (EC depleted)");
#endif
                    }
                }
            }
        }
    }
}
