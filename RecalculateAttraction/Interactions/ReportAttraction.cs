using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.SimIFace;
using Sims3.UI;

namespace zoniventris.Attraction.Interactions
{
    [DoesntRequireTuning]
    public class ReportAttraction : ImmediateInteraction<Sim, Sim>
    {
        public static readonly InteractionDefinition Singleton = new Definition();

        public static void ShowNotification(Sim actor, Sim target)
        {
            Relationship relationship = actor.GetRelationship(target, false);
            string notification;
            if (relationship == null)
            {
                notification = LocalizeString(actor.IsFemale, "DisplayNoRelationship", new object[] { actor, target });
            }
            else if (relationship.AttractionScore == float.PositiveInfinity)
            {
                notification = LocalizeString(actor.IsFemale, "DisplayInfiniteScore", new object[] { actor, target });
            }
            else
            {
                notification = LocalizeString(actor.IsFemale, "DisplayScore", new object[] { actor, target, relationship.AttractionScore });
            }
            StyledNotification.Format format = new StyledNotification.Format(notification, StyledNotification.NotificationStyle.kGameMessagePositive);
            StyledNotification.Show(format);
        }

        protected override bool Run()
        {
            ShowNotification(Actor, Target);
            return true;
        }

        private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, ReportAttraction>
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

        private static readonly string sLocalizationKey = "ReportAttraction:";

        private static string LocalizeString(bool isFemale, string entryKey, object[] parameters)
        {
            return Instantiator.LocalizeString(isFemale, sLocalizationKey + entryKey, parameters);
        }
    }
}
