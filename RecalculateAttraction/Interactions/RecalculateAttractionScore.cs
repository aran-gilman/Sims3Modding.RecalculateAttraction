using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.SimIFace;

namespace zoniventris.Attraction
{
    [DoesntRequireTuning]
    public class RecalculateAttractionScore : ImmediateInteraction<Sim, Sim>
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

        private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, RecalculateAttractionScore>
        {
            public override string[] GetPath(bool isFemale)
            {
                return new string[] { Instantiator.kInteractionPath };
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
