using Sims3.Gameplay.Actors;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using System;

namespace zoniventris.Attraction
{
    public class Instantiator
    {
        public static string[] GetInteractionPath(bool isFemale)
        {
            return new string[] { LocalizeString(isFemale, "InteractionPath", new object[0]) };
        }

        public static string LocalizeString(bool isFemale, string entryKey, object[] parameters)
        {
            return Localization.LocalizeString(isFemale, sLocalizationKey + entryKey, parameters);
        }

        [Tunable]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Required for mod initialization")]
        private static bool kInstantiator = false;

        private static readonly string sLocalizationKey = "zoniventris/Attraction/";

        static Instantiator()
        {
            World.OnWorldLoadFinishedEventHandler += new EventHandler(OnWorldLoadFinished);
        }

        private static void OnWorldLoadFinished(object sender, EventArgs e)
        {
            EventTracker.AddListener(EventTypeId.kSimInstantiated, new ProcessEventDelegate(OnSimInstantiated));

            foreach (Sim sim in Sims3.Gameplay.Queries.GetObjects<Sim>())
            {
                AddInteractions(sim);
            }
        }

        private static ListenerAction OnSimInstantiated(Event e)
        {
            Sim sim = e.TargetObject as Sim;
            if (sim != null)
            {
                AddInteractions(sim);
            }
            return ListenerAction.Keep;
        }

        private static void AddInteractions(Sim sim)
        {
            foreach (var pair in sim.Interactions)
            {
                if (pair.InteractionDefinition.GetType() == Interactions.ReportAttraction.Singleton.GetType())
                {
                    return;
                }
            }
            sim.AddInteraction(Interactions.ReportAttraction.Singleton);
            sim.AddInteraction(Interactions.RecalculateAttractionScore.Singleton);
            sim.AddInteraction(Interactions.ToggleAttraction.Singleton);
        }
    }
}
