using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.SimIFace;

namespace zoniventris.Attraction.Interactions
{
    [DoesntRequireTuning]
    public class ToggleAttraction : ImmediateInteraction<Sim, Sim>
    {
        public static InteractionDefinition Singleton = new Definition();

        [Tunable]
        public static float kIsAttractedScore = 150.0f;

        [Tunable]
        public static float kIsNotAttractedScore = 50.0f;

        protected override bool Run()
        {
            // Do not create a relationship if one does not already exist.
            // Earlier error checking should prevent us from reaching this point for "nonsensical" relationships (e.g. a Sim and a boat, a Sim and themself), but for now bail out here in case there's something strange going on.
            Relationship relationship = Actor.GetRelationship(Target, false);
            if (relationship == null)
            {
                return true;
            }

            if (relationship.AreAttracted)
            {
                relationship.AttractionScore = kIsNotAttractedScore;
            }
            else
            {
                relationship.AttractionScore = kIsAttractedScore;
            }

            ReportAttraction.ShowNotification(Actor, Target);

            return true;
        }

        private class Definition : ImmediateInteractionDefinition<Sim, Sim, ToggleAttraction>
        {
            public override string[] GetPath(bool isFemale)
            {
                return Instantiator.GetInteractionPath(isFemale);
            }

            protected override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair iop)
            {
                return LocalizeString(actor.IsFemale, "InteractionName", new object[] { actor, target });
            }

            protected override bool Test(Sim actor, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                if (!Cheats.sTestingCheatsEnabled || actor == target || isAutonomous)
                {
                    return false;
                }

                if (!actor.CanHaveRomanceWith(target))
                {
                    greyedOutTooltipCallback = new GreyedOutTooltipCallback(new InvalidRelationshipTooltip(actor, target).GetMessage);
                    return false;
                }

                return true;
            }
        }

        private class InvalidRelationshipTooltip
        {
            public InvalidRelationshipTooltip(Sim actor, Sim target)
            {
                this.actor = actor;
                this.target = target;
            }

            public string GetMessage()
            {
                return LocalizeString(actor.IsFemale, "InvalidRelationshipTooltip", new object[] { actor, target });
            }

            private readonly Sim actor;
            private readonly Sim target;

        }

        private static string sLocalizationKey = "ToggleAttraction:";

        private static string LocalizeString(bool isFemale, string entryKey, object[] parameters)
        {
            return Instantiator.LocalizeString(isFemale, sLocalizationKey + entryKey, parameters);
        }
    }
}
