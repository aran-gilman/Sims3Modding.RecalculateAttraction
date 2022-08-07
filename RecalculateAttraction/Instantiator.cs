using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.SimIFace;
using Sims3.UI;
using System;

namespace zoniventris.Attraction
{
    public class Instantiator
    {
        [Tunable]
        protected static bool kInstantiator = false;

        public static string[] kInteractionPath = new string[] { "Attraction..." };


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
                if (pair.InteractionDefinition.GetType() == DEBUG_ReportAttraction.Singleton.GetType())
                {
                    return;
                }
            }
            sim.AddInteraction(DEBUG_ReportAttraction.Singleton);
            sim.AddInteraction(DEBUG_RecalculateAttractionScore.Singleton);
        }

        private static void ReportAttraction(Sim actor, Sim target)
        {
            Relationship relationship = actor.GetRelationship(target, false);
            string notification;
            if (relationship == null)
            {
                notification = $"DEBUG No relationship exists between {actor.Name} and {target.Name}";
            }
            else
            {
                notification = $"DEBUG Attraction score between {actor.Name} and {target.Name}: {relationship.AttractionScore}";
            }
            StyledNotification.Format format = new StyledNotification.Format(notification, StyledNotification.NotificationStyle.kGameMessagePositive);
            StyledNotification.Show(format);
        }

        [DoesntRequireTuning]
        private class DEBUG_ReportAttraction : ImmediateInteraction<Sim, Sim>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            protected override bool Run()
            {
                ReportAttraction(Actor, Target);
                return true;
            }

            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, DEBUG_ReportAttraction>
            {
                public override string[] GetPath(bool isFemale)
                {
                    return kInteractionPath;
                }

                protected override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair iop)
                {
                    return $"Report attraction score between {actor.Name} and {target.Name}";
                }

                protected override bool Test(Sim actor, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return Cheats.sTestingCheatsEnabled && actor != target && !isAutonomous;
                }
            }
        }

        [DoesntRequireTuning]
        private class DEBUG_RecalculateAttractionScore : ImmediateInteraction<Sim, Sim>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            protected override bool Run()
            {
                Relationship relationship = Actor.GetRelationship(Target, false);
                relationship.AttractionScore = float.PositiveInfinity;
                relationship.CalculateAttractionScore();
                ReportAttraction(Actor, Target);
                return true;
            }

            private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, DEBUG_RecalculateAttractionScore>
            {
                public override string[] GetPath(bool isFemale)
                {
                    return kInteractionPath;
                }

                protected override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair iop)
                {
                    return $"Recalculate attraction score between {actor.Name} and {target.Name}";
                }

                protected override bool Test(Sim actor, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return Cheats.sTestingCheatsEnabled && actor != target && !isAutonomous;
                }
            }
        }
    }
}
