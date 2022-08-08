using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.SimIFace;

namespace zoniventris.Attraction.Interactions
{
    [DoesntRequireTuning]
    public class RecalculateAttraction : ImmediateInteraction<Sim, Sim>
    {
        public static readonly InteractionDefinition Singleton = new Definition();

        protected override bool Run()
        {
            Relationship relationship = Actor.GetRelationship(Target, false);
            relationship.AttractionScore = float.PositiveInfinity;
            relationship.CalculateAttractionScore();
            ReportAttraction.ShowNotification(Actor, Target);
            return true;
        }

        private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, RecalculateAttraction>
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
                return Cheats.sTestingCheatsEnabled && actor != target && !isAutonomous;
            }
        }

        private static readonly string sLocalizationKey = "RecalculateAttraction:";

        private static string LocalizeString(bool isFemale, string entryKey, object[] parameters)
        {
            return Instantiator.LocalizeString(isFemale, sLocalizationKey + entryKey, parameters);
        }
    }
}
