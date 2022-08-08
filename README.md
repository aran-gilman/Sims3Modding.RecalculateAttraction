# Sims 3 Mod: Recalculate Attraction

This pure scripting mod adds new immediate interactions that give the player greater insight and control over Sims' attraction to one another.

This mod was originally designed for players who have a specific narrative in mind for their Sim(s) that conflicts with the results of the game's attraction calculation algorithm, which in an un-modded game only runs once, when Sims meet each other for the first time.

## Installation
Install this mod by placing it in your Mods/Packages folder. (If this folder doesn't exist, follow a tutorial for setting up a Mods folder.)

## Accessing the interactions in-game

1. Use SHIFT-CTRL-C to open the command prompt.
1. Enter "testingcheatsenabled true".
1. From Live Mode, click on the Sim whose attraction to the current Sim you want to see and/or modify.
1. Open the new "Attraction..." sub-menu and select the desired interaction.

## Added Interactions

* Report attraction between A and B: Displays a notification with the current attraction score.
* Calculate attraction between A and B: Recalculates the attraction score. This calls the base game's CalculateAttraction() method.
* Toggle attraction between A and B: Makes A and B attracted to each other if they are not, and removes attraction if they are. This option will be greyed out if the Sims cannot romance each other.

## Known Issues

* Calculating attraction does not work for Sims who are engaged or married to each other, though it should still work for engaged or married Sims when used on Sims they are *not* engaged or married to. For whatever reason, CalculateAttraction() exits without setting the relationship attraction score. The options for fixing this have unwanted side-effects (e.g. temporarily changing the relationship status) and/or are outside the scope of this mod. If you want a particular result for these sims, use the "Toggle attraction" interaction instead.

* Infinite values are displayed weirdly in the attraction reporting notification. This impacts pairs of Sims who have not had an attraction score calculated for them, since the game initializes the attraction score to positive infinity. This will be fixed in a future update.

* Checking for errors and disallowed relationships is fairly bare-bones right now. I don't *think* it will, for example, let a Sim be attracted to their car, but if it does...well, everything in this mod requires player direction, so at least it won't happen randomly. Please file a bug report and I'll try to have it fixed in a future update.

* While the package is setup for localization, all strings for all locales are currently in English.

## Compatibility

* Tuning mods: 100% compatible. Attraction (re-)calculation calls the base game's original method, so any tuning changes should be applied without issue.
* Script mods related to attraction and romance: I assume that "Calculate attraction" and "Toggle attraction" will not work as expected, if for no other reason than most mods will already re-run their own calculations. "Report attraction" may or may not work as expected, depending on whether the other mod simply provides its own calculation of the attraction score or replaces the attraction system with something else entirely. Notes on specific mods:
	+ [lizcandor's Harder Romance](https://modthesims.info/d/657983/harder-romance.html): Mostly incompatible. "Calculate attraction" will not call Harder Romance's attraction calculation, and Harder Romance will almost certainly overwrite whatever value gets set by my mod's "Calculate attraction" or "Toggle attraction" interactions. "Report attraction" should still work as expected.
* Custom content and script mods *not* related to attraction and romance: I do not expect there to be any issues with these.