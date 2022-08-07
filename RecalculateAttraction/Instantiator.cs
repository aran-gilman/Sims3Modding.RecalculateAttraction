using Sims3.Gameplay.Actors;
using Sims3.Gameplay.EventSystem;
using Sims3.SimIFace;
using System;

namespace zoniventris.Attraction
{
    public class Instantiator
    {
        [Tunable]
        private static bool kInstantiator = false;

        public static string kInteractionPath = "Attraction...";

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
                if (pair.InteractionDefinition.GetType() == Attraction.ReportAttraction.Singleton.GetType())
                {
                    return;
                }
            }
            sim.AddInteraction(ReportAttraction.Singleton);
            sim.AddInteraction(RecalculateAttractionScore.Singleton);
        }
    }
}
