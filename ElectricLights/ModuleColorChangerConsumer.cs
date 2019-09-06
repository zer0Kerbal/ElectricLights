using System.Text;
using UnityEngine;

namespace ElectricLights
{
    class ModuleColorChangerConsumer : ModuleColorChanger
    {
        ModuleInteriorLight moduleInteriorLight;

        private bool lastAnimState = false;
        private bool ready = false;

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            if ((state != StartState.None) && (state != StartState.Editor))
            {
                moduleInteriorLight = part.Modules.GetModule<ModuleInteriorLight>();
                if (moduleInteriorLight != null)
                {
                    lastAnimState = animState;
                    moduleInteriorLight.interiorLight = animState;
                    ready = true;
#if DEBUG
                    Debug.Log("ModuleColorChangerConsumer.OnStart(): ready!");
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
                    if (animState != lastAnimState)
                    {
                        // Animation Lights changed
                        if (!animState)
                        {
                            // Animation Lights are now OFF
                            moduleInteriorLight.interiorLight = false;
#if DEBUG
                            Debug.Log("ModuleColorChangerConsumer.OnUpdate(): Switched OFF interior lights.");
#endif
                        }
                        else
                        {
                            // Animation Lights are now ON
                            moduleInteriorLight.interiorLight = true;
#if DEBUG
                            Debug.Log("ModuleColorChangerConsumer.OnUpdate(): Switched ON interior lights.");
#endif
                        }
                        lastAnimState = animState;
                    }
                    else if (!moduleInteriorLight.interiorLight && animState)
                    {
                        // Interior Lights OFF (EC depleted) and Animation Lights ON
                        ToggleEvent();
#if DEBUG
                        Debug.Log("ModuleColorChangerConsumer.OnUpdate(): Turned OFF interior lights. (EC depleted)");
#endif
                    }
                }
            }
        }
    }
}
