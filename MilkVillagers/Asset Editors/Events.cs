using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;

namespace MilkVillagers.Asset_Editors
{
    public enum Emote
    {
        question = 8,
        angry = 12,
        exclamation = 16,
        heart = 20,
        sleep = 24,
        sad = 28,
        happy = 32,
        x = 36,
        pause = 40,
        videogame = 52,
        music = 56,
        blush = 60,
    }

    public static class EventEditor
    {
        private static IDictionary<string, string> data;

        public static bool CanEdit(IAssetName asset)
        {
            bool result = asset.IsEquivalentTo("Data/Events/Seedshop")
                || asset.IsEquivalentTo("Data/Events/Hospital")
                || asset.IsEquivalentTo("Data/Events/LeahHouse")
                || asset.IsEquivalentTo("Data/Events/ArchaeologyHouse")
                || asset.IsEquivalentTo("Data/Events/Saloon")
                || asset.IsEquivalentTo("Data/Events/Sunroom")
                || asset.IsEquivalentTo("Data/Events/ScienceHouse")
                || asset.IsEquivalentTo("Data/Events/HaleyHouse")
                || asset.IsEquivalentTo("Data/Events/BathHouse_Pool");


            return result;
        }

        public static void Edit(IAssetData asset)
        {
            EditAsset(asset);
        }

        private static void EditAsset(IAssetData asset)
        {
            data = asset.AsDictionary<string, string>().Data;

            // Completed - moved to CP
            if (asset.Name.IsEquivalentTo("Data/Events/Seedshop") && false)
            {
                #region Abigail reward event 594801 - completed!
                if (!data.ContainsKey($"{TempRefs.EventAbi}")) //moved to CP version
                {
                    ModFunctions.LogVerbose($"Loading event {TempRefs.EventAbi} - finished.", LogLevel.Alert);
                    data[$"{TempRefs.EventAbi}" +                    // event id
                            $"/d Mon Wed Fri" +         //not Mon, Wed or Fri 
                            $"/f Abigail 2000" +        //affection level
                            $"/n MTV_AbigailQ4" +         //need her invitation mail first
                            $"/t 1900 2400"] =          //between 7pm and midnight
                                                        //data[$"598401/f Abigail 2000"] =
                            $"echos" +
                            $"/13 8" +
                            $"/farmer 13 10 0 Abigail 14 6 1" +
                            $"/skippable" +
                            $"/move farmer 0 -4 0" +
                            $"/faceDirection farmer 1" +
                            $"/message \"Abigail is on her computer setting up her stream. Unusually, she is wearing a skirt and a loose top, and you can see that she's not wearing a bra.\"" +
                            $"/pause 300" +
                            $"/speak Abigail \"Oh, hey there @. I'm just getting the stream all set up for tonight. Are you ready to have your world rocked?\"" +
                            $"/emote farmer 16" +
                            $"/pause 300" +
                            $"/speak Abigail \"Great, everything is all ready, I just need to make sure that you are good to go and then we can start the stream. Everyone is going to love this so much.\"" +
                            $"/emote farmer 16" +
                            $"/message \"Abigails swivels around on her chair, and looks you up and down. She reaches forward and adjusts your hair a little, brushes some dirt off of you, and generally gets you ready for an audience.\"" +
                            $"/pause 300" +
                            $"/faceDirection Abigail 3" +
                            $"/speak Abigail \"Of course, I'm gonna rip all of that off you as soon as I can, but that's part of the fun. My fans think I'm a wild girl, and I want to keep it that way.$h\"" +
                            $"/emote Abigail 16" +
                            $"/speak Abigail \"Ok, time to start the stream!\"" +
                            $"/fade" +
                            $"/viewport -300 -300" +
                            $"/pause 500" +
                            $"/faceDirection Abigail 1" +
                            $"/speak Abigail \"Ok everyone. You asked for it, so we have a special guest tonight to 'help' me out, and help me cum. This here is the wonderful gal who grows those veggies so darn big on her farm.\"" +
                            $"/speak Abigail \"Of course, everything that I've put inside myself for you has been in her hands, and tonight I want to see if i can get at least one of her hands inside me as well.\"" +
                            $"/emote farmer 32" +
                            $"/speak Abigail \"Anyway, time to get started. I'm already dripping and this pussy is going to start tightening up again if I wait too long.\"" +
                            $"/speak Abigail \"Everyone get a good long look at @ because this may be the only time you get to see me strip her on camera.\"" +
                            $"/message \"Abigail turns around and flashes the webcam, giving her viewers a view up her skirt as she crawls towards you on the floor.\"" +
                            $"/message \"She quickly loses her skirt, revealing a bare ass underneath, and wiggles it as she grabs ahold of your pants. She wastes no time in stripping you, making sure her viewers get the best view of the two of you.\"" +
                            $"/emote Abigail 16" +
                            $"/speak Abigail \"Well, what did I tell you? @ is smoking hot, and my pussy juices are dripping onto the floor right now. She makes me so hot that I can barely control myself.\"" +
                            $"/emote farmer 16" +
                            $"/message \"Abigail lies on her back, and pulls your hips above her head so the camera can see her licking your pussy. She uses her tongue to lick up your valley, and then starts stroking your clit with her finger.\"" +
                            $"/message \"You moan a little as she uses her other hand to spread your lips for the camera, and then inserts her first finger inside you, causing your fluid to squelch out and drip onto her face.\"" +
                            $"/message \"Abigail looks straight into the camera, opens her mouth, and pushes two fingers inside you to get more of your fluid to drip out.\"" +
                            $"/speak Abigail \"Only tasty things come from @'s farm, and she is no exception. Now that she's warmed up, I think it's time I switched places and got serious.\"" +
                            $"/message \"Abigail and you spend the next 45mins trying out each of her sex toys, in order to 'warm her up for the main event'. By this time both of you are panting away, and you've already cum twice.\"" +
                            $"/speak Abigail \"Ok, here we go. @, start off with three fingers, and lots of lube. I'm already soaking wet, but every little helps. That's it, keep pushing your fingers into my wet snatch, and try and get that fourth finger in there.\"" +
                            $"/pause 300" +
                            $"/playSound fishSlap" +
                            $"/pause 300" +
                            $"/playSound fishSlap" +
                            $"/speak Abigail \"Here it goes, viewers! @, I'm ready for you! Keep pushing and get that beautiful hand inside me...\"" +
                            $"/pause 300" +
                            $"/speak Abigail \"Ooooh yes! The stretch is sooo good! I'm cumming!\"" +
                            $"/message \"Abigail starts shaking on top of you, and you wriggle your fingers inside her to make her cum explosively. After a few minutes, she recovers, and you gently remove your hand from her loose pussy.\"" +
                            $"/speak Abigail \"Well viewers, I think I'm well and truly done for tonight. Look at this giant, gaping hole where my pussy was an hour ago! I hope you also came a ton and had a great time. Anyway, I'm signing off now - come back for my next show!\"" +
                            $"/mail MTV_AbigailQ4T" +
                            $"/end";
                }
                else { ModFunctions.LogVerbose($"{TempRefs.EventAbi} already loaded.", LogLevel.Alert); }
                #endregion
            }

            // Completed - moved to CP
            if (asset.Name.IsEquivalentTo("Data/Events/Hospital") && false)
            {
                #region Harvey Event 594802 - completed!
                if (!data.ContainsKey($"{TempRefs.EventHarvey}")) //moved to CP version
                {
                    ModFunctions.LogVerbose($"Loading event {TempRefs.EventHarvey}", LogLevel.Alert);
                    data[$"{TempRefs.EventHarvey}" +
                                        "/d Tue Thu" +
                                        "/f Harvey 2000" +
                                        "/p Harvey" +
                                        "/n MTV_HarveyQ3" +
                                        "/t 900 1500"] =

                                        //data["594802/f Harvey 2000"] =
                                        "Hospital_Ambient" +
                                        "/4 7" +
                                        "/farmer 5 8 0 Harvey 4 5 0" +
                                        "/skippable" +
                                        "/move farmer 0 -3 1" +

                                        // Farmer and Player face each other
                                        "/beginSimultaneousCommand" +
                                        "/faceDirection farmer 3" +
                                        "/faceDirection Harvey 1" +
                                        "/endSimultaneousCommand" +

                                        // Introduction
                                        "/speak Harvey \"Ah, @. Thank you for coming to see me. Your last physical went well, but I think we need to put you through some more rigorous testing.\"" +
                                        "/emote farmer 16" +
                                        "/speak Harvey \"Oh, there’s nothing to worry about. You look like you’ve been taking care of yourself, eating well and exercising.\"" +
                                        "/speak Harvey \"In fact, if you don’t mind me saying so, you look very fit...and attractive.\"" +
                                        "/message \"Harvey loosens his tie and glances at you nervously\"" +
                                        "/speak Harvey \"Do you...find me attractive too?\"" +
                                        "/emote farmer 32" +
                                        "/speak Harvey \"Wonderful. Anyway, We’ll begin the tests. Please remove your clothes.\"" +
                                        "/emote farmer 16" +
                                        "/faceDirection farmer 0" +
                                        "/fade" +
                                        "/viewport -300 -300" +
                                        "/speak Harvey \"Ah yes, we’ll start with some flexibility testing.\"" +

                                        // Move Harvey behind player

                                        // Next dialogue chunk
                                        "/speak Harvey \"Please try touching your toes.\"" +
                                        "/message \"As you bend over Harvey touches your back, slowly trailing his hand down towards your ass\"" +
                                        "/speak Harvey \"Very good, and your skin is remarkably soft. Please hold that position as long as you can.\"" +
                                        "/message \"His hand drifts lower, stroking your ass and cupping your pussy. You start to feel moisture gathering on your lips, and your breathing gets heavier\"" +
                                        "/emote farmer 16" +
                                        "/speak Harvey \"Don’t push yourself, but please hold that pose a little longer. I need to see how much you can take.\"" +
                                        "/message \"Harvey strokes between your pussy lips, and gently inserts a finger into your pussy. You gasp a little at the cold invader, and he removes his finger\"" +
                                        "/speak Harvey \"I see you are sensitive. Would you consider this reaction normal for you, or is this position increasing your sensitivity?\"" +
                                        "/question \"Is this sensitivity normal?#Yes#No#Moan\"" +
                                        //"/move farmer 0 0 2" +
                                        //"/emote farmer 32" +
                                        "/speak Harvey \"Well I guess we need some further tests. Please stand next to the bed and lean over it. I’m going to be doing some internal testing for sensation and stretching.\"" +          // cut off for length

                                        // Move to bed                                  
                                        "/message \"You stand up and move over to the bed, resting your weight on it and presenting yourself to the horny doctor\"" +
                                        "/message \"Harvey undoes his belt and lowers his pants, and slips on a condom with practised ease.\"" +
                                        "/message \"He presses his cock against your entrance and teases you with the head by rubbing it up and down your labia, coating it in your juices.\"" +
                                        "/message \"You start moaning, and he takes this as his cue, smoothly plunging deep inside you. You gasp, and he pauses to let you adjust.\"" +
                                        "/speak Harvey \"Are you feeling any discomfort or pain, @?\"" +
                                        "/question \"Are you comfortable?#Yes#Moan\"" +
                                        "/emote farmer 32" +
                                        "/speak Harvey \"I’ll continue with the test then.\"" +
                                        "/message \"Harvey starts thrusting into you again, the sounds of your intercourse filling the echoey examination room.\"" +
                                        "/message \"You settle into a rhythm of Harvey grunting and you moaning, and it’s not long before you are getting close to the edge.\"" +
                                        "/message \"You start cumming, leaning on the bed for support, as your orgasm racks your body, causing you to clench down on the cock deep inside your pussy.\"" +
                                        "/message \"Apparently Harvey is not too far behind you, as you feel him start to hammer away at you faster in order to try and finish.\"" +
                                        "/message \"In a few moments he pushes as deeply into you as he can, and you feel the condom start to inflate as his cum fills it inside you.\"" +
                                        "/speak Harvey \"@...your body is...fantastic. You are in...such good shape.\"" +
                                        "/message \"Harvey leans against your body, clearly exhausted from this session. You move a little to the side and he lies on the bed beside you, his cock slipping out of your pussy.\"" +
                                        "/message \"After a few minutes of half lying on the bed you both get up, and Harvey hands you some surgical wipes to clean yourself with while he disposes of the condom.\"" +
                                        "/speak Harvey \"Well, @. I’d say that you are in perfect health. Please feel free to return any time you want another check up.\"" +
                                        "/mail MTV_HarveyQ3T" +
                                        "/end";

                }
                else { ModFunctions.LogVerbose($"{TempRefs.EventHarvey} already loaded.", LogLevel.Alert); }
                #endregion

                #region Maru/Harvey Event 594803 - completed!
                if (false) //!data.ContainsKey($"{TempRefs.Event3HarMar}"))
                {
#pragma warning disable CS0162 // Unreachable code detected
                    ModFunctions.LogVerbose($"Loading event {TempRefs.Event3HarMar}", LogLevel.Alert);
#pragma warning restore CS0162 // Unreachable code detected
                    data[$"{TempRefs.Event3HarMar}" +
                                        "/d Mon Wed Fri Sat Sun" +
                                        "/f Harvey 2000" +
                                        "/f Maru 2000" +
                                        "/p Harvey" +
                                        "/p Maru" +
                                        "/n MTV_HarveyQ4" +
                                        "/t 900 1500"] =

                                        //data["594803/f Harvey 2000/f Maru 2000"] =
                                        "Hospital_Ambient" +                        // Soundtrack for this scene
                                        "/4 7" +                                    // Camera position at start
                                        "/farmer 6 17 0 Harvey 3 7 0 Maru 1 5 2" +   // Initial placing
                                        "/skippable" +                              // Can farmer skip this event? (good in case someone modifies the map
                                        "/pause 500" +
                                        "/speak Harvey \"Nurse Maru, can you please get the topical cream ready for the next patient. It should be in the cupboard beside you.\"" +
                                        "/speak Maru \"Of course, Dr. Harvey.\"" +
                                        "/move Maru 3 0 1" +
                                        "/pause 300" +
                                        "/faceDirection Maru 0" +
                                        "/pause 700" +
                                        "/textAboveHead Harvey \"I love the perks of this job\"" +
                                        "/pause 700" +
                                        //"/message \"As Maru bends down to the cupboard, Harvey stops what he's doing and takes a moment to admire Maru's ass\"" +
                                        "/message \"Maru sees what Harvey is doing, gives her ass a wiggle, and then flips her skirt up to show him her naked behind\"" +
                                        "/speak Maru \"Like what you see, *doctor* Harvey? I'm sure we can find some time for you to *examine* me afterwards.\"" +
                                        "/textAboveHead Harvey \"Such a tease\"" +
                                        "/pause 300" +
                                        "/speak Maru \"I think I heard someone enter, so that might be them right now. Anyway, let me go and see who is waiting at the front.\"" +

                                        "/beginSimultaneousCommand" +
                                        "/fade" +
                                        "/viewport 5 15" +
                                        "/warp Maru 6 15" +
                                        "/faceDirection Maru 2" +
                                        "/endSimultaneousCommand " +

                                        "/speak Maru \"Good morning @. I'm sorry you had to wait, we were just getting everything ready for you in the back. The doctor will be ready for you in a moment.\"" +
                                        "/emote farmer 16" +
                                        "/faceDirection Maru 0" +
                                        "/textAboveHead Maru \"Dr Harvey, the trial patient is here\"" +
                                        "/pause 3500" +
                                        "/textAboveHead Maru \"Ok, I'll bring them in\"" +
                                        "/pause 500" +
                                        "/faceDirection Maru 2" +
                                        "/speak Maru \"Ok, you can head in now. I hope you don't mind but Dr Harvey asked me to assist him today so that I can observe and learn.\"" +
                                        "/emote farmer 16" +
                                        "/pause 300" +

                                        "/beginSimultaneousCommand" +
                                        "/fade" +
                                        "/viewport 15 6" +
                                        "/warp Maru 14 6" +
                                        "/warp farmer 16 5" +
                                        "/warp Harvey 15 6" +
                                        "/faceDirection farmer 2" +
                                        "/faceDirection Harvey 0" +
                                        "/faceDirection Maru 0" +
                                        "/endSimultaneousCommand" +

                                        "/speak Harvey \"Ah, @. I'm really glad you decided to take part in this trial. We have so much to learn about the human body, and yours is just delightful.\"" +
                                        "/pause 400" +
                                        "/faceDirection Harvey 3" +
                                        "/pause 300" +
                                        "/speak Harvey \"Nurse Maru, Can you assist the patient and get them situated on the bed, please? Make sure you remove all of their clothing before we begin.\"" +

                                        "/beginSimultaneousCommand" +
                                        "/fade" +
                                        "/viewport -300 -300" +
                                        "/endSimultaneousCommand" +

                                        //"/pause 300" +
                                        //"/move farmer 15 5 2" +

                                        "/message \"Maru helps you remove your clothing, placing it on the nearby chair. You lie down down on the bed\"" +
                                        "/speak Maru \"Are you comfortable, @?\"" +
                                        "/question null \"Are you comfortable?#Yes\"" +
                                        "/message \"Maru looks down at your naked body and smiles, then reaches out her gloved hands and gently opens your legs.\"" +
                                        "/message \"She takes a jar of ointment and applies some directly to the outside of your labia, then starts rubbing some more into your breasts, causing your nipples to get hard. She stops far too soon\"" +
                                        "/speak Maru \"Dr Harvey, the patient is ready for you now. Do you want me to provide stimulation?\"" +
                                        "/pause 500" +
                                        "/speak Harvey \"Hmm, not at this time, Nurse Maru. Although you have a very good bedside manner, this is a teaching moment.\"" +
                                        "/message \"Dr Harvey switches places with Nurse Maru and starts gently massaging your breasts, causing your nipples to stand even more erect. You feel your lower lips start swelling without being touched\"" +
                                        "/speak Maru \"Doctor Harvey, I can see that she is getting aroused. Her labia majora are swelling up and her clitoral hood seems to be getting more prominent. Should I check for secretions?\"" +
                                        "/speak Harvey \"Please do, Nurse. Let me know what information you can glean from examing the patients fluids\"" +
                                        "/message \"Nurse Maru leans over while Dr Harvey is rubbing your chest, and slides a gloved finger up your slit, purposefully rubbing against your clitoris. She brings her finger to her face and gently sniffs the tip\"" +
                                        "/speak Maru \"Patient's secretions are mildly scented. Should I perform a taste test, doctor?\"" +
                                        "/speak Harvey \"Of course, Nurse Maru. The ointment that you applied is safe for ingestion, and was designed to be tasteless but may add a slightly oily texture\"" +
                                        "/message \"Maru looks you straight in the eyes, opens her mouth and starts sensually sucking on her finger. She uses her other hand to start groping her breast, and lets out a low moan as she withdraws her finger from her mouth.\"" +
                                        "/speak Maru \"Patient's fluid is sweet as expected, doctor.\"" +
                                        "/pause 400" +
                                        "/speak Harvey \"I think it is time for you to demonstrate what you have learned so far, Nurse. Please proceed with the next stage of the treatment.\"" +
                                        "/pause 400" +
                                        "/message \"Nurse Maru approaches you on the bed and starts rubbing some of the ointment on to her gloved hands. She reaches between your legs and gently seperates your lips with both hands\"" +
                                        "/message \"She uses the finger and thumb of each hand, sliding up and down, to massage your lips. The ointment seems to be causing you to feel a tingling sensation wherever she touches, and you start panting\"" +
                                        "/message \"Satisfied with your response, and the amount of liquid you are producing, she slips one of her fingers inside your entrance, using it to gently probe you\"" +
                                        "/speak Harvey \"Now, remember the aim of this trial, Nurse Maru. We are not directly trying to bring the patient to orgasm, merely to see how the ointment affects their liquid secretions.\"" +
                                        "/speak Harvey \"@, please try to hold off as long as possible.\"" +
                                        "/message \"You realise that you are going to be fighting a losing battle. Maru is definitely very adept at this, and during Dr Harvey's speech she had slipped a second finger inside you and is pumping away\"" +
                                        "/message \"Slightly chastised, she slows down and withdraws her fingers while Dr Harvey jots down a note about your gushing vagina\"" +
                                        "/message \"While he is distracted, Maru scoops up some of your juices in her off hand, and reaches down under her skirt. She gives you a sneaky smile, and starts rubbing your secretions into her sopping wet cunt\"" +
                                        "/speak Maru \"Doctor, I think that @ is being a very good patient, and I don't want to scare her off of any future trials. Perhaps this time we can speed up the process a little?\"" +
                                        "/pause 400" +
                                        "/speak Harvey \"Ah, good point. Perhaps we should put on a little show for her, as thanks?\"" +
                                        "/speak Harvey \"I also see that you have been a very naughty nurse while you thought I wasn't looking? It looks like that needs addressing immediately, you naughty nurse\"" +
                                        "/message \"Dr Harvey undoes his belt and drops his pants, then flips up Nurse Maru's skirt and gives her bare behind a playful smack\"" +
                                        "/message \"Maru gives out a little yelp, which quickly turns into a loud moan as she feels him enter her dropping hole. She tries to stay proffesional though and, with shaking hands, goes back to fingering you\"" +
                                        "/pause 300" +
                                        "/playSound fishSlap" +
                                        "/pause 300" +
                                        "/playSound fishSlap" +
                                        "/pause 300" +
                                        "/message \"With this erotic display happening right in front of you, you are on the edge in no time. You shout a warning to Maru that you are about to cum, and she she starts rapidly rubbing your clit, bringing you to a powerful orgasm\"" +
                                        "/message \"Harvey takes this as his cue, and starts firing his hot load deep inside Maru, who starts shaking on top of you as well\"" +
                                        "/pause 1000" +
                                        "/speak Harvey \"Well, Nurse Maru, even though we got a little distracted, I think we can call this trial a success. If the pool collecting under @ right now is any indication I think we are on to a winner.\"" +
                                        "/speak Harvey \"Once you have both taken a moment to recover, can you clean up the patient and debrief them on the experience? Make sure you document absolutely everything.\"" +
                                        "/pause 300" +
                                        "/speak Maru \"Thank you for that, @. I hope you'll take part if we have another trial in the future.\"" +
                                        "/mail MTV_HarveyQ4T" +
                                        "/end";
                }
                else { ModFunctions.LogVerbose($"{TempRefs.Event3HarMar} already loaded.", LogLevel.Alert); }
                #endregion
            }

            // Completed - moved to CP
            if (asset.Name.IsEquivalentTo("Data/Events/LeahHouse") && false) // moved to CP
            {
                #region Leah painting quest 594829 - completed
                ModFunctions.LogVerbose($"Loading event {TempRefs.EventLeah} - incomplete");
                data[$"{TempRefs.EventLeah}" +                    // event id
                        $"/n MTV_LeahQ1" +    //need her invitation mail first
                        $"/f Leah 2000" +           //Leah at 8 hearts
                                                    //$"/t 1100 1800" +           //between 11am and 6pm
                        $""] =

                        // Setup the scene
                        $"echos" +
                        $"/6 5" +
                        $"/farmer 7 9 0 Leah 6 5 0" +
                        $"/skippable" +

                        // Begin the scene
                        $"/move farmer 0 -4 0" +
                        $"/pause 300" +

                        $"/beginSimultaneousCommand" +
                        $"/faceDirection farmer 3" +
                        $"/faceDirection Leah 1" +
                        $"/endSimultaneousCommand" +

                        $"/pause 500" +
                        $"/textAboveHead Leah \"Oh, there you are\"" +
                        $"/emote farmer 32" +
                        $"/pause 1500" +
                        $"/speak Leah \"Thank you for helping me out with this, @. I don't know why I haven't painted you yet, but today that changes.\"" +
                        $"/move Leah -1 0 1" +
                        $"/message \"Leah takes a step back, and thoughtfully looks you up and down.\"" +
                        $"/speak Leah \"We have pretty good lighting in here, and I try to keep my cottage pretty warm all year round.\"" +

                        //gender branch
                        "/speak Leah \"I'm sure some people would appreciate a painting with the cold making your nipples stand on end, but I think we can make that happen anyway with the right...stimulation\"" +
                        //"/speak Leah \"I'd hate to have your friend down below hiding on me because he doesn't like the cold. I'm sure I could coax him out, but it would be hard to stay focussed on painting you if I spend half of the time with him in my mouth.\"" +
                        //remember how Xtardew used forks to load different events?

                        // Back to the scene
                        "/move Leah 1 0 2" +
                        "/message \"Leah steps close to you and places her hands on your chest, lightly caressing you through your work attire.\"" +
                        "/speak Leah \"I think it's time these clothes started covering up that chair over there instead of your glorious body, don't you?\"" +
                        "/emote farmer 60" +
                        "/faceDirection Leah 0" +
                        "/pause 300" +

                        //fade out
                        $"/beginSimultaneousCommand" +
                        $"/fade" +
                        $"/viewport -300 -300" +
                        $"/endSimultaneousCommand" +

                        // Begin graphic description
                        $"/message \"As Leah turns back to her easel, wrestling it in to position, you start undressing. You notice her glancing at you, and she keeps getting distracted while getting her paints and brushes ready.\"" +
                        $"/speak Leah \"Wow, I wish I could paint this scene, but I've never been very good at capturing motion on canvas. It's like I'm getting my very own striptease...\"" +
                        $"/message \"You slyly half turn, making sure Leah has a good view of your plump rump, and lock eyes with her over your shoulder.\"" +
                        $"/speak Leah \"As classic as that pose is, I was thinking we could go for something a little more...erotic, if you're up for it?\"" +

                        // Choice of style of painting
                        $"/question fork0 \"What pose do you want to strike for Leah?#Something classy#Something erotic\"" +
                        $"/fork 594804classy" + // Switch to classy painting instead.

                        // Continue on to erotic painting
                        $"/speak Leah \"Well, I guess I'd better get you ready. I want everyone who sees this painting to either get wet or get hard from looking at it.\"" +
                        $"/textAboveHead Leah \"Just like I am right now...\"" +
                        $"/pause 2000" +

                        // Gender split
                        $"/message \"Leah walks over to you, loosely holding a naked, paintless brush in her hand. She uses it's soft tip to slowly circle your left breast, causing your nipples to start hardening as she promised.\"" +

                        //$"/speak Leah \"I bet you didn't think I'd be using the brush on your body, did you? As much as I want to slowly paint every inch of you, I can't exactly ship you off to the competition, can I?\"" +

                        // Gender split
                        $"/message \"You shake your head, and she uses your moment of distraction to brush your nipple, sending a jolt straight through your body. She slowly drags the tip of the brush across your chest, and flicks it over your other nipple, causing your knees to buckle slightly.\"" +
                        $"/speak Leah \"Now, now, @. I'm going to need you to hold a pose. I can't exactly paint you if you're writhing on the floor in bliss. I mean, I could, but then I'd be on the floor with you.\"" +

                        $"/message \"Leah helps you pick an erotic but natural pose, and you do your best 'Venus de Milo' pose as Leah starts sketching your form. Every so often she notices you start to get restless and comes over to provide you with a 'helping hand' in the form of some gentle caresses or stimulating stroking.\"" +
                        $"/pause 1000" +
                        $"/speak Leah \"Ok, @. I think I'm almost done for today. I can fill in the rest later, but I think my wonderful muse deserves a treat. Staring at your sexy, naked form all day has caused me to paint a different kind of picture on the floor by my easel.\"" +
                        $"/message \"Leah cleans her brush and rests it on the stand, then walks over to you, pausing every couple of steps as she strips out of her clothing. She has smudge marks on her face from where she has wiped away her sweat, and her ponytail has come loose, but she looks as stunning as ever." +
                        $"#Leah presses her naked form against yours, and starts rubbing herself against you. She hooks one leg around yours, and slowly slides down your thigh, leaving a wet trail. She lets out a little moan.\"" +
                        $"/speak Leah \"Oh @, can you feel just how wet you make me?\"" +
                        $"/message \"Leahs hands stroke reach around your waist, grabbing a your butt and squeezing as she brings her mouth to your sex and inhales. She takes your lips her mouth, and starts sucking softly, licking up the inside and then circling the entrance to your secret place" +
                        $"#You rest your hands on her head for support as she goes to town on you, showing the same dedication and skill to your form as she did to her painting, bringing colour to your body as it flushes, and causing your lips to swell with lust. She pauses for a moment, and asks you to lie down on her rug, then dives back between your legs." +
                        $"#The light from the fireplace casts shadows on the wall, your form mingling with Leah's as she coaxes your closer to orgasm. She starts licking and sucking your clit as her fingers pump in and out of your leaking pussy, her other hand furious between her own legs.\"" +
                        $"/speak Leah \"@...@...I'm getting close. Cum for me, please?\"" +
                        $"/message \"Your orgasm washes over you, and you clench you legs together. Leah's body starts shaking as well, and the two of your writhe on the floor, sharing your bliss.\"" +

                        $"/pause 1500" +
                        $"/speak Leah \"Wow. I know the theme of the competition is 'The Beautiful Body', but I don't think there's any way for me to encompass just how beautiful you are in a painting. We might have to make you an interactive exhibit...$h" +
                        $"#Anyway, can we just lie here for a while? I want to cuddle with you in front of the fire, and I don't think I'll be able to stand up for a while anyway...\"" +
                        $"/pause 1500" +

                        // End the scene
                        $"/end";

                // classy variant
                data["594804classy" +
                    ""] = $"/speak Leah \"Well, I guess I'd better get you ready. I want everyone who sees this painting to really understand just how beautiful the human form is.\"" +

                        $"/message \"Leah helps you pick a classic and natural pose, and you do your best 'Venus de Milo' as Leah starts sketching your form. Every so often she notices you start to get restless and comes over to give you a little massage or rub to loosen you up again.\"" +
                        $"/pause 1000" +
                        $"/speak Leah \"Ok, @. I think I'm almost done for today. I can fill in the rest later, but I think my wonderful muse deserves a treat. Staring at your sexy, naked form all day has caused me to paint a different kind of picture on the floor by my easel.\"" +

                        $"/message \"Leah cleans her brush and rests it on the stand, then walks over to you, pausing every couple of steps as she strips out of her clothing. She has smudge marks on her face from where she has wiped away her sweat, and her ponytail has come loose, but she looks as stunning as ever." +
                        $"#Leah presses her naked form against yours, and starts rubbing herself against you. She hooks one leg around yours, and slowly slides down your thigh, leaving a wet trail. She lets out a little moan.\"" +
                        $"/speak Leah \"Oh @, can you feel just how wet you make me?\"" +
                        $"/message \"Leahs hands stroke reach around your waist, grabbing your butt and squeezing as she brings her mouth to your sex and inhales. She takes your lips her mouth, and starts sucking softly, licking up the inside and then circling the entrance to your secret place" +
                        $"#You rest your hands on her head for support as she goes to town on you, showing the same dedication and skill to your form as she did to her painting, bringing colour to your body as it flushes, and causing your lips to swell with lust. She pauses for a moment, and asks you to lie down on her rug, then dives back between your legs." +
                        $"#The light from the fireplace casts shadows on the wall, your form mingling with Leah's as she coaxes your closer to orgasm. She starts licking and sucking your clit as her fingers pump in and out of your leaking pussy, her other hand furious between her own legs.\"" +
                        $"/speak Leah \"@...@...I'm getting close. Cum for me, please?\"" +
                        $"/message \"Your orgasm washes over you, and you clench you legs together. Leah's body starts shaking as well, and the two of your writhe on the floor, sharing your bliss.\"" +

                        $"/pause 1500" +
                        $"/speak Leah \"Wow. I know the theme of the competition is 'The Beautiful Body', but I don't think there's any way for me to encompass just how beautiful you are in a painting. We might have to make you an interactive exhibit...$h" +
                        $"#Anyway, can we just lie here for a while? I want to cuddle with you in front of the fire, and I don't think I'll be able to stand up for a while anyway...\"" +
                        $"/pause 1500" +

                        // End the scene
                        $"/end";
                #endregion

                #region Leah sex show part 1
                #region Leah/player exhibitionism - being replaced
                if (false) //not finished
                {
#pragma warning disable CS0162 // Unreachable code detected
                    ModFunctions.LogVerbose($"Loading event Leah Exhibitionism {TempRefs.EventLeahExhibitOld} - need to write", LogLevel.Alert);
#pragma warning restore CS0162 // Unreachable code detected
                    data[$"{TempRefs.EventLeahExhibitOld}" +

                            // Conditions
                            "D <name>" +                // player is dating the given NPC name.
                            "J 	" +                     // player has finished the Joja Warehouse.
                            "L" +                       // Event is in the FarmHouse and the FarmHouse has been upgraded at least twice (has nursery but not necessarily cellar).
                            "M <number>" +              // player has at least this much money.
                            "O <name>" +                // player is married to that NPC.
                            "S <secret note ID>" +      // player has seen the Secret Note with the given ID.
                            "a <x> <y>" +               // player is on that tile position.
                            "b <number>" +              // player has reached the bottom floor of the Mines at least that many times.
                            "c <number>" +              // player has at least that many free inventory slots.
                            "e <event ID>" +            // player has seen the specified event (may contain multiple event IDs).
                            "f <name> <number>" +       // player has at least <number> friendship points with the <name> NPC. Can specify multiple name and number pairs, in which case the player must meet all of them.
                            "g <gender> 	" +         // player is the specified gender (male or female).
                            "h <pet> 	" +             // player does not already have a pet, and their preference matches <pet> ("cat" or "dog").
                            "i <item ID> 	" +         // player has specified item in their inventory.
                            "j <number> 	" +         // player has played more than <number> days.
                            "k <event ID> 	" +         // player has not seen that event (may contain multiple event IDs).
                            "l <letter ID> 	" +         // player has not received that mail letter or non-mail flag.
                            "m <number> 	" +         // player has earned at least this much money (regardless of how much they " + //ly have).
                            "n <letter ID> 	" +         // player has received that mail letter or non-mail flag.
                            "o <name> 	" +             // player is not married to that NPC.
                            "p <name>" +                // Specified NPC is in the player's location.
                            "q <dialogue ID>" +         // player has chosen the given answer in a dialogue. May contain multiple dialogue IDs, in which case they must all have been selected.
                            "s <item ID> <number>" +    // player has shipped at least <number> of the specified item. Can specify multiple item and number pairs, in which case all of them must be met.
                            "t <min time> <max time>" + // time is between between the specified times. Can range from 600 to 2600.
                            "u <day of month> 	" +     // day of month is one of the specified values (may contain multiple days).
                            "x <letter ID>"] =          // For the player: mark this event as seen, add the specified letter to tomorrow's mail, then return false (so that nothing further happens). Use the format "x letterid true" to send the letter immediately.     

                            //Leah needs a model for a live art piece, entitled 'freedom in/through bondage' (maybe misunderstanding?)

                            //farmer walks in
                            "/emote Leah 32" +
                            "/textAboveHead Leah \"hum hum\"" +
                            "/speak Leah \"I'll just be a minute, @.\"" +
                            //move farmer walks up to Leah
                            "/pause 700" +
                            "/speak Leah \"Thanks for waiting for me, @. I was just reading a letter from an old friend in Zuzu city about an upcoming exhibition.\"" +
                            "/speak Leah \"It seems that they want some of the previous artists to enter pieces around a common theme, and this one is...\"" +
                            "/emote Leah 8" +
                            "/message \"Leah starts reading the letter again with a puzzled look on her face\"" +
                            "/speak Leah \"The letter must have gotten wet, and the ink has a big raindrop right there!\"" +
                            "/speak Leah \"I'm not sure if it says 'Freedom in Bondage' or something else. Oh well, its the kind of contrasting title that these events usually have, and if I get it wrong then I'll just say it's 'artistic expressionism'\"" +
                            "/emote farmer 8" +
                            "/pause 700" +
                            "/speak Leah \"Anyway, I'm glad you came by. Do you feel like brainstorming with me? I'd really appreciate the company.$l\"" +
                            "/emote Leah 60" +
                            "/question null \"Do you want to help Leah brainstorm?#Try to come up with some helpful suggestions#Keep Leah company#Come back later when Leah has some ideas\"" +
                            //Result 1
                            //     Leah has had an idea, but needs farmer to help out
                            //     
                            //     
                            //Leah ends up asking farmer if they would be willing to be the model for the event.
                            //It would involve Leah dressing the farmer up in a hood to preserve her identity, but otherwise being mostly nude.
                            //The freedom side would be the anonymity, along with the model having freedom from societies restrictions
                            //
                            //After the event Leah confesses to the farmer that the whole thing turned her on a lot, and asks the farmer if they enjoyed it too.
                            //    Yes, and i'm really turned on too
                            //    I'm not sure, but I liked spending time with you
                            //    Not really my thing.
                            //     
                            //First two lead into Leah blindfolding the farmer again and eating her out
                            //Third option leads to Leah just offering to make it up to the farmer...and eating her out.
                            //
                            //

                            "";
                }
                #endregion

                #region 594810 Leah visit show quest 594831 - started by WAILT. ACE version
                bool finished = true;
                if (finished)
                {
                    ModFunctions.LogVerbose($"EventLeahExhibitA {TempRefs.EventLeahExhibitA} marked ready? {finished}");
                    data[$"{TempRefs.EventLeahExhibitA}" +

                    #region Conditions - template only
                            //"/D <name>" +                // player is dating the given NPC name.
                            //"/J" +                       // player has finished the Joja Warehouse.
                            //"/L" +                       // Event is in the FarmHouse and the FarmHouse has been upgraded at least twice (has nursery but not necessarily cellar).
                            //"/M <number>" +              // player has at least this much money.
                            //"/O <name>" +                // player is married to that NPC.
                            //"/S <secret note ID>" +      // player has seen the Secret Note with the given ID.
                            //"/a <x> <y>" +               // player is on that tile position.
                            //"/b <number>" +              // player has reached the bottom floor of the Mines at least that many times.
                            //"/c <number>" +              // player has at least that many free inventory slots.
                            //"/e <event ID>" +            // player has seen the specified event (may contain multiple event IDs).
                            "/f Leah 1500" +               // player has at least <number> friendship points with the <name> NPC. Can specify multiple name and number pairs, in which case the player must meet all of them.
                                                           //"/g <gender>" +              // player is the specified gender (male or female).
                                                           //"/h <pet>" +                 // player does not already have a pet, and their preference matches <pet> ("cat" or "dog").
                                                           //"/i <item ID>" +             // player has specified item in their inventory.
                                                           //"/j <number>" +              // player has played more than <number> days.
                                                           //"/k <event ID>" +            // player has not seen that event (may contain multiple event IDs).
                                                           //"/l <letter ID>" +           // player has not received that mail letter or non-mail flag.
                                                           //"/m <number>" +              // player has earned at least this much money (regardless of how much they really have).
                            "/n MTV_Ace" +                 // player has received that mail letter or non-mail flag.
                            "/n MTV_LeahQ3" +
                            //"/o <name>" +                // player is not married to that NPC.
                            //"/p <name>" +                // Specified NPC is in the player's location.
                            //"/q <dialogue ID>" +         // player has chosen the given answer in a dialogue. May contain multiple dialogue IDs, in which case they must all have been selected.
                            //"/s <item ID> <number>" +    // player has shipped at least <number> of the specified item. Can specify multiple item and number pairs, in which case all of them must be met.
                            //"/t <min time> <max time>" + // time is between between the specified times. Can range from 600 to 2600.
                            //"/u <day of month> 	" +   // day of month is one of the specified values (may contain multiple days).
                            //"/x <letter ID>"             // For the player: mark this event as seen, add the specified letter to tomorrow's mail, then return false (so that nothing further happens). Use the format "x letterid true" to send the letter immediately.     
                            ""] =
                    #endregion

                    #region set up event
                            $"playful" +
                            $"/14 20" +
                            $"/farmer 14 23 0 Leah 40 18 1" +
                            $"/skippable" +
                    #endregion

                    #region event text: A-sexual
                            // Leah Pt. 3 (We’ll need to credit Seattle Erotic Arts Festival for inspiration. It looks like it was amazing)\"" + 
                            "/viewport -100 -100" +
                            "/message \"The exhibit is in a building with blacked out windows. You’re a little nervous, but Leah confidently takes your hand and leads you inside." +
                                "#The person at the front desk checks your IDs before motioning for you to go back. You and Leah part the thick black curtain that hangs in the entryway and step through." +
                                "#The first thing you see is a small platform where a nude man kneels on a soft looking pillow, arms bound by leather cuffs in front of him. His eyes are covered by a dark blindfold. Around his neck hangs a sign that says ’No Photography’." +
                                "#Next to him is a throne, where another man, wearing a leather harness over a crisp white button up, sits with a goblet in one hand, talking to a few guests. His other hand is running through the kneeling man’s hair." +
                                "#You look at Leah unsure but she’s looking towards the stage where a large metal frame with loops and hooks hanging from the top bars. Hanging from the bar is a large hoop on which a woman in a sparkling leotard is doing aerial acrobatics.\"" +

                            "/speak Leah \"Let’s go find seats. I want to watch the aerial performances.\"" +

                            "/message \"You follow along, looking around as you do. There are a lot of different static pieces. People wrapped in leather and latex, bound in various positions, modeling harnesses, cuffs, masks, and more." +
                                "#Leah drags you to a seat, close to the front, squeezing your hand before pulling out a sketch pad. You both watch the show in awe. The strength of the hoop and silk artists bodies is incredible." +
                                "#When the next group comes on stage carrying a bag full of ropes, Leah gets an excited look in her eye. Two women wearing sheer colored lacy lingerie and gauzy butterfly wings begin setting up their rope." +
                                "#Their bodies are both wrapped in rope harnesses. You can see the way it presses into their skin and you can’t help but imagine how that would feel on you." +
                                "#They do a series of positions, the woman dressed in blue suspended in the air. The ropes have on her back, hanging with a leg tied underneath her to her thigh where they frame her figure beautifully." +
                                "#By the time the show is over, you can’t stop imagining how stunning rope art is. There’s something beautiful in the constriction. Leah takes your hand and you can’t help but grin at how flushed her cheeks are.\"" +

                            "/speak Leah \"Let’s go back to my place and I can tell you my ideas.\"" +

                            "/message \"You nod and follow her to the exit. On your way out, you see a person sitting on a motor bunny, arms bound behind their back, while another plays with the controls." +
                                "#The one riding the toy looks like they’re in miserable ecstasy, tears streaking their face and, by the mess on the floor, you’re pretty sure they’ve cum more than a few times already. There’s a strange beauty in the way they cry.\"" +

                            "/speak Leah \"I'm so excited about the coming exhibit. I can't wait to show you the ideas this has given me.\"" +

                            // End event and choose aftercare
                            "/quickquestion \"Do you want to head back with Leah and cuddle in her cottage?" +
                            "#Go home with Leah and cuddle." +
                            "#Say goodnight to Leah." +
                            "(break)switchEvent 594804R2" +// Switch to cuddling result if chosen
                            "(break)switchEvent 594804R3\"" +// Otherwise go to end of event.
                            "/end";
                    #endregion
                }
                #endregion

                #region 594811 Leah visit show quest 594831 - started by WAILT. Vagina version
                finished = true;
                if (finished)
                {
                    ModFunctions.LogVerbose($"EventLeahExhibitV {TempRefs.EventLeahExhibitV} marked ready? {finished}");
                    data[$"{TempRefs.EventLeahExhibitV}" +

                    #region Conditions - template only
                            //"/D <name>" +                // player is dating the given NPC name.
                            //"/J" +                       // player has finished the Joja Warehouse.
                            //"/L" +                       // Event is in the FarmHouse and the FarmHouse has been upgraded at least twice (has nursery but not necessarily cellar).
                            //"/M <number>" +              // player has at least this much money.
                            //"/O <name>" +                // player is married to that NPC.
                            //"/S <secret note ID>" +      // player has seen the Secret Note with the given ID.
                            //"/a <x> <y>" +               // player is on that tile position.
                            //"/b <number>" +              // player has reached the bottom floor of the Mines at least that many times.
                            //"/c <number>" +              // player has at least that many free inventory slots.
                            //"/e <event ID>" +            // player has seen the specified event (may contain multiple event IDs).
                            "/f Leah 1500" +               // player has at least <number> friendship points with the <name> NPC. Can specify multiple name and number pairs, in which case the player must meet all of them.
                                                           //"/g <gender>" +              // player is the specified gender (male or female).
                                                           //"/h <pet>" +                 // player does not already have a pet, and their preference matches <pet> ("cat" or "dog").
                                                           //"/i <item ID>" +             // player has specified item in their inventory.
                                                           //"/j <number>" +              // player has played more than <number> days.
                                                           //"/k <event ID>" +            // player has not seen that event (may contain multiple event IDs).
                                                           //"/l <letter ID>" +           // player has not received that mail letter or non-mail flag.
                                                           //"/m <number>" +              // player has earned at least this much money (regardless of how much they really have).
                            "/n MTV_Vagina" +              // player has received that mail letter or non-mail flag.
                            "/n MTV_LeahQ3" +
                            //"/o <name>" +                // player is not married to that NPC.
                            //"/p <name>" +                // Specified NPC is in the player's location.
                            //"/q <dialogue ID>" +         // player has chosen the given answer in a dialogue. May contain multiple dialogue IDs, in which case they must all have been selected.
                            //"/s <item ID> <number>" +    // player has shipped at least <number> of the specified item. Can specify multiple item and number pairs, in which case all of them must be met.
                            //"/t <min time> <max time>" + // time is between between the specified times. Can range from 600 to 2600.
                            //"/u <day of month> 	" +   // day of month is one of the specified values (may contain multiple days).
                            //"/x <letter ID>" +           // For the player: mark this event as seen, add the specified letter to tomorrow's mail, then return false (so that nothing further happens). Use the format "x letterid true" to send the letter immediately.     
                            ""] =
                    #endregion

                    #region set up event
                            $"playful" +
                            $"/6 5" +
                            $"/farmer 7 9 0 Leah 6 5 0" +
                            $"/skippable" +
                    #endregion

                    #region event text: Vagina
                            // Leah Pt. 3 (We’ll need to credit Seattle Erotic Arts Festival for inspiration. It looks like it was amazing)
                            "/message \"The exhibit is in a building with blacked out windows. You’re a little nervous, but Leah confidently takes your hand and leads you inside." +
                                "#The person at the front desk checks your IDs before motioning for you to go back. You and Leah part the thick black curtain that hangs in the entryway and step through." +
                                "#The first thing you see is a small platform where a nude man kneels on a soft looking pillow, arms bound by leather cuffs in front of him. His eyes are covered by a dark blindfold. Around his neck hangs a sign that says ’No Photography’." +
                                "#Next to him is a throne, where another man, wearing a leather harness over a crisp white button up, sits with a goblet in one hand, talking to a few guests. His other hand is running through the kneeling man’s hair." +
                                "#You look at Leah unsure but she’s looking towards the stage where a large metal frame with loops and hooks hanging from the top bars. Hanging from the bar is a large hoop on which a woman in a sparkling leotard is doing aerial acrobatics.\"" +

                            "/speak Leah \"Let’s go find seats. I want to watch the aerial performances.\"" +

                            "/message \"You follow along, looking around as you do. There are a lot of different static pieces. People wrapped in leather and latex, bound in various positions, modeling harnesses, cuffs, masks, and more." +
                                "#Leah drags you to a seat, close to the front, squeezing your hand before pulling out a sketch pad. You both watch the show in awe. The strength of the hoop and silk artists bodies is incredible." +
                                "#When the next group comes on stage carrying a bag full of ropes, Leah gets an excited look in her eye. Two women wearing shear colored lacy lingerie and gauzy butterfly wings begin setting up their rope." +
                                "#Their bodies are both wrapped in rope harnesses. You can see the way it presses into their skin and you can’t help but imagine how that would feel on you. A shiver crawls up your spine and your pussy clenches around nothing." +
                                "#They do a series of positions, the woman dressed in blue suspended in the air. The ropes have her on her back, hanging with a leg tied underneath her to her thigh where they frame her ass beautifully." +
                                "#By the time the show is over, you’re so wet you’re worried it’s seeped through the fabric of your bottoms. Leah’s squeezing her legs together so at least you know you’re not alone. Leah takes your hand and you can’t help but grin at how flushed her cheeks are.\"" +

                            "/speak Leah \"Let’s go back to my place and I can tell you my ideas.\"" +
                            "/message \"You nod and follow her to the exit. On your way out, you see a person sitting on a motor bunny, arms bound behind their back, while another plays with the controls." +
                                "#The one riding the toy looks like they’re in miserable ecstasy tears streaking their face and, by the mess on the floor, you’re pretty sure they’ve cum more than a few times already." +
                                "#When you get out, you’re not sure how much longer you can wait before you need to fuck yourself into orgasm. Or maybe, Leah would be willing to help you out.\"" +
                    #endregion

                    #region set up post event
                            //$"/resetVariable" +
                            //$"/question fork0 \"Do you want to head home or see if Leah is up for more?#Head home#Talk to Leah\"" +
                            //$"/fork LeahShowPt1R3" +

                            //$"/resetVariable" +
                            //$"/question fork0 \"Do you want to head back with Leah and cuddle in her cottage?#See if Leah is up for getting rid of that sexual frustration.#Go home with Leah and cuddle.\"" +
                            //$"fork LeahShowPt1R1" +

                            //$"/message \"picked option 2 - cuddles.\""+
                            //$"switchEvent LeahShowPt1R2" +


                            "/quickQuestion \"Do you want to head back with Leah and cuddle in her cottage?" +
                            "#See if Leah is up for getting rid of that sexual frustration." +
                            "#Go home with Leah and cuddle." +
                            "#Head home" +
                            "(break)switchEvent 594804R1" +
                            "(break)switchEvent 594804R2" +
                            "(break)switchEvent 594804R3\"" +

                            //"/fork LeahShowSex 594804R1" +
                            //"/fork LeahShowCuddle 594804R2" +
                            //"/fork LeahShowLeave 594804R3" +
                    #endregion

                            "/end";
                }
                #endregion

                #region Result 1
                data[$"594804R1"] =
                            // Map LeahHouse+

                            //"fade" +
                            "viewport 7 7" +

                            //"playful" +
                            //"/14 20" +
                            "/warp farmer 7 9 0" +
                            "/warp Leah 7 8 0" +
                            //"/skippable" +

                            //"/fade" +
                            //"/viewport 6 6" +

                            "/message \"Farmer and Leah enter Leah’s house.\"" +
                            "/move Leah 0 -1 2" +
                            "/pause 300" +

                            "/beginSimultaneousCommand" +
                            "/move Leah 0 -2 2" +
                            "/move farmer 0 -3 0" +
                            "/endSimultaneousCommand" +

                            "/speak Leah \"Well, thank you for coming. That show was definitely...enlightening.\"" +
                            "/question null \"#Did you gain anything from it?\"" +
                            "/speak Leah \"I got a lot of new ideas, though that’s not all.\"" +
                            "/emote Leah 60" +
                            "/speak Leah \"Everything was so erotic, I got really wet from watching it. Would you want to give me a hand?\"" +
                            "/message \"Trigger Warning: If you choose to help Leah the scene involves consensual rope bondage.\"" +
                            "/quickquestion \"Do you want to help Leah get off?" +
                            "#Definitely, let’s give her a private show." +
                            "#That’s not really my thing, but we can cuddle." +
                            "#I’m too tired to do anything tonight." +
                            "(break)Fade" +                             // Result 1: Go to her bed (continue) 
                            "(break)switchEvent 594804R2" +             // Result 2: cuddle
                            "(break)switchEvent 594804R3\"" +           // Result 3: leave

                            "/beginSimultaneousCommand" +
                            "/viewport 5 5" +
                            "/move Leah -3 0 0" +
                            "/move farmer 0 -1 3" +
                            "/endSimultaneousCommand" +

                            "/move farmer -2 0 3" +

                            "/message \"You step close to Leah and brush her hair back behind her ear. She looks stunning, her gray brown eyes sparkling from the low firelight.\"" +
                            "/message \"A soft blush has spread high on her cheeks and you can't help but kiss it. Her eyes flutter closed and she lets out a soft gasp. You pull back after a quick peck to her nose.\"" +
                            "/message \"You look at her bed and notice there are cotton ropes tied to the bedposts and the shears on the side table. Leah notices you looking and follows your gaze. Her flush gets brighter.\"" +

                            "/question null \"#Would you like me to use those on you?\"" +
                            "/Speak Leah \"Yes, please.\"" +
                            "/pause 300" +
                            "/fade" +
                            "/viewport -100 -100" +

                            "/message \"You kiss her hard and filthily, tongues slicking together before you pull back abruptly. You strip her of her top and bra, trailing kisses up her stomach as you do, then sit her on the bed.\"" +
                            "/message \"You pull off her shoes and socks, tucking them under the bed. You stand her up, undo the buttons of her pants, and slide them down her legs until she can step out. You fold them neatly and place them next to her shoes.\"" +
                            "/message \"You undress yourself and she watches you, her blush spreading down her chest." +

                            "/question \"#Let me know if you need me to stop at any time. Do you have a safeword? Do you have any limits that we haven’t talked about before?\"" +
                            "/Speak Leah \"The light system. Red for stop, yellow for slow down, pause, talk it out and green for keep going. Same limits as before, no teeth being the biggest.\"" +
                            "/message \"You smile softly and give her a gentle kiss before you use a single column tie (thank you, college experimentation) to secure the cotton rope around each wrist and ankle, checking in with her as you do, until she’s spread eagle.\"" +
                            "/message \" You climb onto the bed between her legs, rubbing your hands up and down her thighs, her skin soft with the slight tickle of hair.\"" +
                            "/question null \"#Color?\"" +
                            "/speak Leah \"Green.\"" +

                            "/message \"You finally allow yourself to look at her fully. You curse softly under your breath. You don’t know if you’ve ever seen anything so beautiful. The color of the ropes contrasts beautifully with her skin." +
                                "#Your eyes fall on her underwear, a pair of dark green boyshorts. She wasn’t exaggerating when she said she soaked them through. The crotch is stained darker, spread all the way to the where the shorts stop an inch down her thighs." +
                                "#You lay on your stomach and press your face against the fabric covering her pussy. She smells of clean sweat. You drag your tongue against the fabric tasting the slight saltiness of it mixing with the mild taste of her slick." +
                                "#She moans as you continue to lick then gasps when you pull the fabric into your mouth and suck. You pull away once you’ve gotten all the flavor from it. Leah’s looking at you half lidded, with a small smirk." +
                                "#You notice the small buttons on the sides of her underwear and snap them open. She must’ve been hoping for something like this if she wore those. You pull them off, and drop them on the pile of her clothes before your gaze falls to her pussy." +
                                "#Leah’s dripping slick, the hair matted down and shiny. You meet her eyes, giving a devilish smirk, before dropping down, parting her lips with your nose, wrapping your mouth around her clit and sucking hard.\"" +

                            "/speak Leah \"Oh Yoba!\"" +
                            "/message \"You’re careful to keep your lips over your teeth as you suck at her, then dip your tongue low and slurp up her juices. You’ve never tasted anything so good. She tastes fresh and natural and you continue using your tongue to clean up every bit of slick.\"" +
                            "/message \"Leah is moaning, her arms and legs jerking as they try to close around your head.\"" +
                            "/question null \"#Color?\"" +
                            "/speak Leah \"Green! More! Please, more!\"" +

                            "/message \"You dip your head further, using your nose to rub against her clit as you lick around her entrance until you finally bury your tongue inside her." +
                                "#You fuck your tongue inside, making sure to curl it up to try and hit her g-spot while the tip of your nose rubs her clit. Leah’s moans are beautiful and you can hear her breath getting quicker." +
                                "#You thrust in a few more times before licking up to her clit, using your tongue to get under its hood and then you suck on it. The strong pressure pulls Leah over into an explosive orgasm, slick drenching her thighs." +
                                "#You work her clit until her orgasm tapers off and then slowly clean her up with your mouth, making sure to be as gentle as possible." +
                                "#When you’ve licked everything up, you crawl up Leah’s body and kiss her filthily, letting her taste herself. She moans into it, lips slow and dragging against yours." +
                                "#You pull back enough to see her face. Her eyes are mostly closed, cheeks red, and hair messy. She licks her lips.\"" +

                            "/question null \"#Are you ready for me to untie you?\"" +
                            "/Speak Leah \"Yes, then cuddles.\"" +

                            "/message \"You smile, kissing her softly one more time before you start untying her. You rub her wrists and ankles where the ropes were, admiring the way the light rope marks look against her skin." +
                                "#You help her sit up, and shuffle underneath the blankets. There’s a reusable water bottle on the bedside table and you grab it, helping her drink a little." +
                                "#Leah smiles sleepily at you and you can’t help but press soft kisses to her forehead, cheeks, and nose. You scoot down and wrap her in your arms.\"" +

                            "/question null \"#How was it? What did you like? Anything you want to do different?\"" +
                            "/Speak Leah \"It was perfect. Though, next time, I want you to cum too. Maybe while sitting on my face.\"" +
                            "/message \"You squeeze your thighs together and you groan, burying your face in her neck. She laughs at you quietly and you let out a contented sigh. You can’t wait to see what she’s going to come up with for the exhibit.\"" +
                            "/end";
                #endregion

                #region Result 2 - Cuddles!
                data[$"594804R2"] = "pause 500" +
                            "/speak Leah \"Sure, that sounds really nice! Let me go take care of myself real quick.\"" +
                            "/message \"Leah gives you a pair of pajamas to borrow before she goes to change into her own. When you’re both dressed you shuffle underneath the blankets." +
                                "#Leah smiles sleepily at you and you can’t help but press soft kisses to her forehead, cheeks, and nose. You scoot down and wrap her in your arms.\"" +
                            "/question \"#What was your favorite part of the show?\"" +
                            "/speak Leah \"I loved the rope artists with the fairy wings. The way the blue fairy’s dark skin contrasted with the rope was stunning.\"" +
                            "/message \"You agree and continue to talk until your voices are just murmurs, eyes fluttering shut. You love the warmth of her skin against yours, how safe and loved you feel, from the simple contact. You can’t wait to see what she’s going to come up with for the exhibit.\"" +
                            "/end";
                #endregion

                #region Result 3 - Say goodbye
                data[$"594804R3"] =

                "speak Leah \"Okay! No worries. I’ll let you know when I’ve figured out exactly what I want to do. Thanks for all your help.\"" +
                "/emote Leah 20" +
                "/message \"You give her a hug goodbye.\"" +
                "/face direction 2" +
                "/move farmer 3 0 2" +
                "/move farmer 0 5 2" +
                "/playSound doorOpen" +
                "/fade" +
                "/end";
                #endregion
                #endregion

            }

            // 1 done, 1 ideas only - moved to CP
            if (asset.Name.IsEquivalentTo("Data/Events/ArchaeologyHouse") && false)
            {
                #region Penny Library event 594805 - Need to playtest
                ModFunctions.LogVerbose($"Loading event {TempRefs.EventPenny} (need to make mail and quest)");
                data[$"{TempRefs.EventPenny}" +                    // event id
                        $"/n MTV_PennyQ1" +        // need her invitation mail first. This is a temporary way of adding the quest
                        $"/f Penny 2000" +          // Penny at 8 hearts
                                                    //$"/p Penny" +               // Penny present
                        $""] =

                        // Setup the scene
                        $"echos" +
                        $"/13 9" +
                        $"/farmer 8 14 1 Penny 17 5 0 Gunther 3 8 2" +
                        $"/skippable" +

                        // Begin scene  looking for Penny
                        $"/move farmer 3 0 1" +   // 11 14 1
                        $"/move farmer 0 -2 0" +  // 11 12 0
                        $"/move farmer 3 0 1" +   // 14 12 1
                        $"/pause 500" +
                        $"/emote farmer 8" +
                        $"/faceDirection farmer 1" +
                        $"/pause 500" +
                        $"/faceDirection farmer 3" +

                        // Penny makes noise
                        $"/TextAboveHead Penny \"*moan*\"" +
                        $"/pause 500" +
                        $"/emote farmer 16" +
                        $"/pause 500" +

                        // Move farmer to beside Penny
                        $"/move farmer 0 -7 1" +  // 14 5 1
                        $"/move farmer 1 0 1" +   // 15 5 1
                                                  //$"/faceDirection farmer 1" +
                        $"/pause 300" +
                        //make penny do sit down and shake animation.
                        $"/faceDirection Penny 3" +
                        $"/pause 300" +
                        $"/message \"Penny quickly pulls her hand out of her skirt and tries to hide a book behind the stool.\"" +
                        $"/speak Penny \"Oh, @. I didn't hear...I mean see you there.\"" +

                        $"/quickquestion \"What do you say to Penny?" +
                            $"#I heard you making a noise in the shelving" +
                            $"#I heard someone moaning" +
                            $"#I just wandered over here." +
                            $"(break)emote Penny 60" +
                            $"(break)textAboveHead Penny \"Oh no...\"" +
                            $"(break)textAboveHead Penny \"Phew\"" +

                        $"/speak Penny \"Oh...that. I guess you're wondering what I'm doing over here..." +
                        $"#$b#The last time I was here I found a book in the shelves that I hadn't seen before, and I started to read it. It was a kind of romance novel, but after a couple of chapters it became clear that 'romance' was just a loose metaphor for..." +
                        $"#$b#For sex. And other things. To be fair, the book did a good job of describing everything in detail, but I hadn't even heard of most of the things that were described\"" +
                        $"/pause 300" +
                        $"/emote Penny 60" +
                        $"/pause 500" +
                        $"/speak Penny \"Not that my mom ever really explained anything to me. Not while sober, anyway.\"" +

                        $"/beginSimultaneousCommand" +
                        //$"/emote Penny 28" +
                        $"/emote farmer 28" +
                        $"/endSimultaneousCommand" +

                        $"/speak Penny \"Anyway, the chapter was getting pretty graphic, and I...got carried away and *really* into it.\"" +

                        // Question Penny about her feelings
                        $"/pause 600" +

                        $"/quickquestion \"What do you want to ask Penny?" +
                            "#What kind of things did it make you do?" +                                           // Question 1
                            "#Did it turn you on?" +
                            "#Did you touch yourself?" +
                            "(break)speak Penny \"It didn't really make me do anything. I know it was all me, but I just got sooo wet down there, and before I knew what was happening I was rubbing away, and I moaned, and...you appeared.\"" +
                            "(break)speak Penny \"I guess, though I didn't really get that far into the book yet.\"" +
                            "(break)speak Penny \"I...may have touched myself a little just before you got here.\"" +

                        $"/speak Penny \"Anyway, I really want to continue reading this book, but I can't exactly do that here in the library. What if Gunther had found me instead!? Or one of the kids!?\"" +
                            "/speak Penny \"I want to ask you a favour. Could you check out this book for me? I would die of embarrassment if Gunther knew what it was, or if someone else saw me carrying it.\"" +

                        $"/question fork1 \"Will you check out the book for Penny?#Of course.#No." +
                        $"/fork 594805refused" +
                        //$"/addQuest 594810" +

                        $"/speak Penny \"Thank you so much, @. Please bring me the book once you have checked it out. You are the best! $h\"" +
                        $"/emote farmer 32" +

                        // Fade to next scene
                        $"/fade" +
                        $"/viewport -100 -100" +

                        $"/beginSimultaneousCommand" +
                        $"/move farmer -1 0 3" +  // 14 5 3
                        $"/warp Penny 0 0 1" +
                        $"/endSimultaneousCommand" +
                        $"/warp farmer 6 12 3" +  // 6 12 3

                        $"/viewport 4 11" +

                        // farmer goes to walk out, but Gunther calls out to them
                        $"/move farmer -3 0 3" +    // 3 12 3
                        "/textAboveHead Gunther \"Do you have a moment?\"" +
                        "/emote farmer 16" +
                        "/pause 500" +
                        "/move farmer 0 -2 0" +      // 3 10 1
                        $"/speak Gunther \"Good day, @. I don't mean to pry, but have you noticed Penny acting a little...stressed?\"" +
                        $"#$b#The last couple of times she's come here she has always been in a rush to get in, and then a rush to leave. I am concerned that something is bothering her." +

                        $"/quickQuestion \"What do you think is bothering Penny" +
                            $"#I think she's just very busy" +
                            $"#She's been reading erotic books" +
                            $"#I'm think she has a crush on you." +
                            $"(break)switchEvent 594805helped" +
                            $"(break)switchEvent 594805snitch" +
                            $"(break)switchEvent 594805crush" +

                        //$"/speak Gunther \"Ah, that is probably it. I did recommend that she try some light reading rather than always working on lessons for the children.\"" +
                        //$"#$b#Anyway, please take care and give my regards to Miss Penny.\"" +
                        //$"/addQuest 594826" +
                        $"/end";

                // Initially refused
                data["594805refused"] = $"speak Penny \"I'm too ashamed to ask anyone else, @! Please, if you change your mind, bring me the book at home.\"" +
                        $"/addQuest 594826" +
                        $"/end";

                #region Helped Penny: Done
                data["594805helped"] = "pause 300" +

                    "/speak Gunther \"Ah, that is probably it. I did recommend that she try some light reading rather than always working on lessons for the children." +
                        "#$b#Anyway, please take care and give my regards to Miss Penny.\"" +

                    //transition to outside
                    "/fade" +
                    "/viewport -300 -300" +

                    "/speak Penny \"Thank you, @. How did it go?\"" +
                    "/question null \"Did Gunther say anything about the book?" +
                        "#No, but he asked after you." +
                    "/speak Penny \"Thank you for helping me out, @. You are a real friend. Perhaps in the future we could read together...\"" +
                    "/friendship Penny 50" +
                    "/end";
                #endregion

                #region Snitched: Done
                data["594805snitch"] = $"pause 50" +

                    "/speak Gunther \"Oh dear, I wasn't aware we had such books in the library!" +
                        "#$b#Imagine if the children had gotten a hold of them? I suppose I'll have to through our bookshelves now and quarantine any books I find." +
                        "#$b#If Miss Penny wants to continue to read those books, she'll have to book them out and read them at home.\"" +

                    //transition to outside
                    "/fade" +
                    "/viewport -300 -300" +

                    "/speak Penny \"I can't believe you would say that to Gunther! I don't know how I'm going to be able to look him in the eye now!" +
                        "#$b#It is going to be so awkward now every time I see him, or when I teach the children there." +
                        "#$b#Anyway. I guess I should take that book, seeing as I cannot be any more embarrassed today.\"" +

                    "/message \"Penny appears pretty disappointed.\"" +
                    "/friendship Penny -50" +

                    "/end";
                #endregion

                #region Crush: Done
                ModFunctions.LogVerbose("Loading 594805crush", LogLevel.Alert);
                data["594805crush"] = "pause 50" +
                    "/speak Gunther \"Oh. I wasn't aware that Miss Penny had those kinds of feelings for me. I must confess that I'm a little flustered." +
                        "#$b#In hindsight I see that it I should have noticed those feelings sooner, seeing as I am here all of the time." +
                        "#$b#Do you REALLY think she has a crush on me? Actually, please don't answer that. I'd rather ask her myself. Take care, @.\"" +

                    //transition to outside
                    "/fade" +
                    "/viewport -300 -300" +

                    "/speak Penny \"I can't believe you would say that to Gunther! I don't know how I'm going to be able to look him in the eye now!" +
                        "#$b#It is going to be so awkward now every time I see him, or when I teach the children there." +
                        "#$b#Anyway. I guess I should take that book, seeing as I cannot be any more embarrassed today.\"" +

                    "/message \"Penny is very embarrassed.\"" +

                    "/end";
                #endregion
                #endregion

                #region Gunther event writing ideas down
                /* Encourage Gunther to spend some time outside of the Library/Museum
                 * He's not against being outside he just has his priorities set on fixing/cleaning it up, so the farmer tries to show them that other things are more important.
                 * Gunther is surprisingly charming and flirtateous, and wants to go up to the mountains.
                 * 
                 * Good afternoon, Mr/Mrs @. I trust your day is going well?
                 * emote farmer happy
                 * TextAboveHead farmer *nods*
                 * \"Today has been relatively quiet, though rebuilding, restocking and reindexing the works here is a monumental task. Sometimes I forget just how much has been accomplished and just see the mountain ahead of me
                 * question null \"#Perhaps you need some perspective?#This stuff isn't that important.#I am so grateful for all you have done.\""+
                 * \"Well, I guess it is easy to get lost in a task when it is all around you all the time. I live here, work here, and think about the exhibits and books a lot.\""+
                 * \"Sometimes I feel like I am 'The librarian' or 'The museum curator', rather than 'Gunther'. When people like yourself come in and talk to me about other things, it really helps me feel more 'human'.\""+
                 * pause 500
                 * \"Would you...like to go somewhere? I done most of the work I had set aside for myself today, and I trust the inhabitants of the valley will forgive me for closing up for the rest of the day.\""+
                 * emote farmer happy
                 * fade to black
                 * Load mountain map
                 * farmer and Gunther walk together
                 * emotes
                 * player looks at Gunther as he? looks around
                 * \"Mr/Mrs @, it is so beautiful up here. Not in the same way that you are beautiful, but nature is truly remarkable.
                 * 
                 * maybe introduce Penny or someone watching the couple from a distance
                 * 
                 * //I want the farmer to make Gunther lose his cool, calm and collected persona.
                 * emote farmer smirk/idea
                 * move farmer behind Gunther.
                 * TextAboveHead farmer ...
                 * message \"Your quietly creep close to Gunther, and reacha round his? waist from behind, sliding your hands between his legs\""+
                 * jump Gunther amount
                 * TextAboveHead Gunther \"Oh!\""+
                 * \"I...think that is a little innapropriate, Mr/Mrs @.\""+
                 * emote farmer questionmark
                 * \"I didn't mean that I don't appreciate the sentiment, just that I was...not expecting it.\""+
                 * \"Er...was it a joke?\""+
                 * \emote farmer no
                 * pause 700
                 * question null \"#I'm helping you find 'perspective'.#I want you to feel like a human again#I wanted to see if I could find any 'rocks' down there.\""+
                 * \"I don't know what to say. I...Thank you.\""+
                 * 
                 * //segue into scene
                 * fade to black
                 * message \"You firmly guide Gunther to sit down on a nearby rock, and slide down his pants. His dick flops into view, quickly growing in size.\""+
                 * \"It's a little...chilly up here. It may be a little hard for him to gain his full size...\""+
                 * message \"You ignore his comments, and take his prick into your mouth in one go. The warm, wet sensation causes Gunther to gasp, and you feel him jerk as you start sucking on him.\""+
                 * message \"You start off slow, swirling your tongue around the head, sucking it every so often to encourage it to swell up. It's not long before you start bobbing your head back and forth, locking eyes with Gunther.\""+
                 * \"Oh Yoba, this is heavenly!\""+
                 * message \"You start to get into the rhythm, and it's not long before Gunther is on the edge, fighting to control himself as his fingers curl in your hair.
                 */
                #endregion
            }

            // need to check - moved to CP
            if (asset.Name.IsEquivalentTo("Data/Events/Saloon") && false)
            {
                #region Haley Saloon event 594806 complete?
                if (true) // not complete?
                {
                    ModFunctions.LogVerbose($"Loading event {TempRefs.Event3HaleyAlex} - need to check");
                    data[$"{TempRefs.Event3HaleyAlex}" +
                            $"/n 5948HaleySaloon" +
                            $"/f Haley 2000" +
                            $"/f Alex 2000" +
                            $"/t 1800 2400" +
                            $""] =

                            // Setup the scene
                            $"playful" +
                            $"/14 20" +
                            $"/farmer 14 23 0 Haley 40 18 1 Gus 14 18 2 Alex 42 18 3" +
                            $"/skippable" +

                            // Start the scene
                            $"/Speak Gus \"Ah, @. It's going to be a busy night tonight.\"" +
                            $"/move farmer 0 -3 0" +
                            $"/emote farmer 8" +
                            $"/pause 1000" +
                            $"/speak Gus \"Haley won some photography contest, and she asked me if she could hold a celebration party here. I'm letting her use the other room for her friends. Did she send you a letter inviting you?\"" +
                            $"/emote farmer 20" +
                            $"/pause 800" +
                            $"/speak Gus \"She's over by the couches. Let me know what you want to drink, and I'll bring it over when it's ready.\"" +
                            $"/pause 300" +
                            $"/move farmer 0 1 2" +

                            // Fade to next scene
                            $"/beginSimultaneousCommand" +
                            $"/move farmer 10 0 1" +
                            $"/fade" +
                            $"/endSimultaneousCommand" +
                            $"/viewport -300 -300" +
                            $"/warp farmer 31 19 1" +

                            // Begin part 2 - talking about the photos
                            $"/pause 800" +
                            $"/fade" +
                            $"/viewport 38 19" +
                            $"/speak Haley \"...and then they sent sent me a copy of the journal a week early with a letter and the cheque!\"" +
                            $"/move farmer 8 0 1 true" +
                            $"/speak Haley \"Oh hey there @. I'm glad you got my invitation - I was worried that you wouldn't get it in time. Without that photography session on Marnie's farm I wouldn't even have entered this competition.\"" +
                            $"/emote farmer 60" +
                            $"/speak Alex \"Haley was just saying that they loved her photos in the farmyard, and they're going to post it on their website as well.\"" +
                            $"/emote farmer 16" +
                            $"/speak Haley \"Of course, I didn't send them ALL of the photos. They had a limit of two per photographer, and I wasn't about to show them the photos we took afterwards, cleaning up.\"" +
                            $"/jump Alex 8" +
                            $"/speak Alex \"You didn't tell me about THAT, Haley! Do you...still have those photos? I'm just asking because, well..." +
                            $"#$b#You're both super hot.\"" +
                            $"/emote Alex 60" +
                            $"/pause 300" +

                            // Haley and farmer look at each other
                            $"/beginSimultaneousCommand" +
                            $"/faceDirection Haley 3" +
                            $"/faceDirection farmer 1" +
                            $"/endSimultaneousCommand" +

                            $"/pause 300" +

                            $"/speak Haley \"Well, @. What do you think? Should we give Alex a little show?\"" +
                            $"/question null \"Fool around with Haley?#Of course\"" +

                            // Fade to black
                            $"/fade" +
                            $"/viewport -300 -300" +

                            // Warm up act
                            $"/message \"Haley checks to make sure the door to the lounge is closed, then winks at Alex and rubs up against you. You can feel her nipples poking into your arm as she leans into your neck and breathes into your ear.\"" +
                            $"/speak Haley \"You know, I wonder which one of us is wetter right now, me or you?\"" +
                            $"/message \"Haley's hand slips inside your clothing, and starts rubbing your slit through your panties. Your juices quickly soak through, and your legs go weak as Haley nibbles on your ear." +
                            $"#$b#She pauses for a moment, letting you get your breath back, and in one swift moment bends down and pulls your pants to the ground. She quickly removes her skirt, and throws it playfully at Alex.\"" +
                            $"/speak Haley \"What do you think, Alex? @ has clearly soaked right through her panties, and mine are pretty much the same. I can see you're also pitching a tent, so how about we get rid of these pointless clothes and start enojoying ourselves?\"" +

                            // Alex joins in
                            $"/speak Alex \"You're such a tease, Haley. I know you're just caught up in her craziness, @, but the two of you have me harder than a rock.\"" +
                            $"/message \"Alex slowly strips down to his under and walks over to you, while Haley locks the door. He pulls you in for a deep kiss, and you feel his manhood pressing against your belly.\"" +
                            $"/speak Haley \"I may have said that this was a party, but...it's just the three of us. I've been wanting to do this for a long time and I really just needed an excuse. Alex, I NEED you inside me.\"" +
                            $"/message \"Alex looks at you questioningly, and you nod to let him know that it's ok. Haley has already removed her panties and sat down on the couch, and spreads her legs for him." +
                            $"#$b#With a hungry look on both of their faces, Alex removes his underwear, freeing his prick which points skyhigh. Haley spreads her lips for Alex and he slides inside her, resting for a moment when he bottoms out inside her.\"" +

                            $"/speak Haley \"Ooh, I feel so full, Alex. Pound me hard, ok?\"" +
                            $"/message \"Alex starts jackhammering away at Haley's dripping wet pussy, and you sit down on the couch next to them and watch for a while, fingering yourself. Haley's is struggling to stifle her moans so you lean over and kiss you, your fingers buried to the hilt inside you, and Alex balls deep in Haley.\"" +

                            $"/speak Haley \"Alex, I think @ is going to feel left out soon. Let switch it up so I can eat her out.\"" +
                            $"/message \"\" you lay down on one end of the couch while Alex and Haley shuffle around, ending with Haley's face buried in your cunt, and Alex's dick still embedded in hers\"" +

                            // Play sex sounds
                            "/pause 300" +
                            "/playSound fishSlap" +
                            "/pause 300" +
                            "/playSound fishSlap" +
                            "/pause 300" +
                            "/playSound fishSlap" +

                            // finish them!
                            "/message \"Haley's tongue is magical, and she licks and nibbles every part of your special area like a pro. Alternating between burying her tongue inside you, wiggling it to stimulate the sides of your hole, and flicking and sucking on your clit, she brings you right to the edge of orgasm.\"" +
                            "/speak Alex \"I'm going to cum!$a\"" +
                            "/speak Haley \"Me too! Make sure you cover me and @!\"" +
                            "/message \"Alex quickly pulls out and sprays a jet of cum over Haleys back, and onto your chest, coating you in his seed. Haley sucks hard on your clit, causing a waterfall of pleasure to wash over you, and sinks three fingers inside her gaping hole as she cums with you." +
                            "#$b#All of you lay on the couch, exhausted. Alex's cock slowly softens as you shake in bliss, and Haley just rests her face between your legs.\"" +

                            "/pause 500" +
                            "/speak Haley \"Wow...that was awesome. We TOTALLY have to do that again sometime. Maybe next time I can bring some toys and we can fill you front and back, @?\"" +
                            "/pause 500" +
                            "/message \"The three of you use some napkins to try and clean up a bit, then spend the rest of the evening drinking and joking about the future.\"" +


                            // End
                            $"/end dialogue Haley \"Wow, that was exhausting!\"";
                }
                #endregion

            }

            // not started
            if (asset.Name.IsEquivalentTo("Data/Events/Sunroom"))
            {
                #region Caroline sunroom event 594807 not started
                ModFunctions.LogVerbose($"Loading event {TempRefs.EventCaroline} - need to write");
                if (false)
                {
#pragma warning disable CS0162 // Unreachable code detected
                    data[$"{TempRefs.EventCaroline}" +
                            // Conditions
                            "D <name>" +                // player is dating the given NPC name.
                            "J 	" +                     // player has finished the Joja Warehouse.
                            "L" +                       // Event is in the FarmHouse and the FarmHouse has been upgraded at least twice (has nursery but not necessarily cellar).
                            "M <number>" +              // player has at least this much money.
                            "O <name>" +                // player is married to that NPC.
                            "S <secret note ID>" +      // player has seen the Secret Note with the given ID.
                            "a <x> <y>" +               // player is on that tile position.
                            "b <number>" +              // player has reached the bottom floor of the Mines at least that many times.
                            "c <number>" +              // player has at least that many free inventory slots.
                            "e <event ID>" +            // player has seen the specified event (may contain multiple event IDs).
                            "f <name> <number>" +       // player has at least <number> friendship points with the <name> NPC. Can specify multiple name and number pairs, in which case the player must meet all of them.
                            "g <gender> 	" +         // player is the specified gender (male or female).
                            "h <pet> 	" +             // player does not already have a pet, and their preference matches <pet> ("cat" or "dog").
                            "i <item ID> 	" +         // player has specified item in their inventory.
                            "j <number> 	" +         // player has played more than <number> days.
                            "k <event ID> 	" +         // player has not seen that event (may contain multiple event IDs).
                            "l <letter ID> 	" +         // player has not received that mail letter or non-mail flag.
                            "m <number> 	" +         // player has earned at least this much money (regardless of how much they " + //ly have).
                            "n <letter ID> 	" +         // player has received that mail letter or non-mail flag.
                            "o <name> 	" +             // player is not married to that NPC.
                            "p <name>" +                // Specified NPC is in the player's location.
                            "q <dialogue ID>" +         // player has chosen the given answer in a dialogue. May contain multiple dialogue IDs, in which case they must all have been selected.
                            "s <item ID> <number>" +    // player has shipped at least <number> of the specified item. Can specify multiple item and number pairs, in which case all of them must be met.
                            "t <min time> <max time>" + // time is between between the specified times. Can range from 600 to 2600.
                            "u <day of month> 	" +     // day of month is one of the specified values (may contain multiple days).
                            "x <letter ID>"] =          // For the player: mark this event as seen, add the specified letter to tomorrow's mail, then return false (so that nothing further happens). Use the format "x letterid true" to send the letter immediately.     

                            // Setup the scene
                            $"playful" +
                            $"/14 20" +
                            $"/farmer 14 23 0 Haley 40 18 1 Gus 14 18 2 Alex 42 18 3" +
                            $"/skippable" +

                            // Begin the scene
                            "" +

                            // End the scene
                            "/end";
#pragma warning restore CS0162 // Unreachable code detected
                }
                #endregion

            }

            // 1 needs checking, 1 is idea only - moved to CP
            if (asset.Name.IsEquivalentTo("Data/Events/ScienceHouse") && false)
            {
                #region Maru sex machine event 594808 complete?
                ModFunctions.LogVerbose($"Loading event {TempRefs.EventMaru} - need to write event and letter", LogLevel.Alert);
                data[$"{TempRefs.EventMaru}" +

                #region Conditions
                        //"/D Maru" +                    // player is dating the given NPC name.
                        //"/J" +                         // player has finished the Joja Warehouse.
                        //"/L" + 	                     // Event is in the FarmHouse and the FarmHouse has been upgraded at least twice (has nursery but not necessarily cellar).
                        //"/M <number>" +                // player has at least this much money.
                        //"/O <name>" +                  // player is married to that NPC.
                        //"/S <secret note ID>" +        // player has seen the Secret Note with the given ID.
                        //"/a <x> <y>" +                 // player is on that tile position.
                        //"/b <number>" +                // player has reached the bottom floor of the Mines at least that many times.
                        //"/c <number>" +                // player has at least that many free inventory slots.
                        //"/e <event ID>" +              // player has seen the specified event (may contain multiple event IDs).
                        "/f Maru 2000" +                 // player has at least <number> friendship points with the <name> NPC. Can specify multiple name and number pairs, in which case the player must meet all of them.
                        "/g female" +                    // player is the specified gender (male or female).
                                                         //"h <pet>" +                   // player does not already have a pet, and their preference matches <pet> ("cat" or "dog").
                                                         //"i <item ID>" +               // player has specified item in their inventory.
                                                         //"j <number>" +                // player has played more than <number> days.
                                                         //"k <event ID>" +              // player has not seen that event (may contain multiple event IDs).
                                                         //"l <letter ID>" +             // player has not received that mail letter or non-mail flag.
                                                         //"m <number>" +                // player has earned at least this much money (regardless of how much they have).
                        "/n MTV_MaruQ4" +             // player has received that mail letter or non-mail flag.
                                                         //"o <name>" +                  // player is not married to that NPC.
                        "/p Maru" +                      // Specified NPC is in the player's location.
                                                         //"q <dialogue ID>" +           // player has chosen the given answer in a dialogue. May contain multiple dialogue IDs, in which case they must all have been selected.
                                                         //"s <item ID> <number>" +      // player has shipped at least <number> of the specified item. Can specify multiple item and number pairs, in which case all of them must be met.
                        $"/t 1800 2400" +                //"t <min time> <max time>" +   // time is between between the specified times. Can range from 600 to 2600.
                                                         //"/u <day of month>" +            // day of month is one of the specified values (may contain multiple days).
                                                         //"x <letter ID>"] =            // For the player: mark this event as seen, add the specified letter to tomorrow's mail, then return false (so that nothing further happens). Use the format "x letterid true" to send the letter immediately.     
                        ""] =
                #endregion

                #region Scene setup
                        $"echos" +                // set music
                        $"/7 7" +
                        $"/farmer 11 12 0 Maru 7 6 1 Demetrius 19 12 3" +
                        $"/skippable" +
                #endregion


                #region Begin the scene
                        "/move farmer -4 0 3" +
                        //"/pause 300" +
                        "/move farmer 0 -1 0" +
                        $"/emote farmer {(int)Emote.question}" +
                        $"/jump Maru 8" +
                        "/speak Maru \"I'll just be one moment, @\"" +
                        "/faceDirection Maru 1" +
                        $"/emote Maru {(int)Emote.videogame}" +
                        "/faceDirection Maru 3" +
                        "/pause 500" + //while maru finishes up
                        "/faceDirection farmer 1" +
                        "/pause 300" +
                        "/faceDirection farmer 3" +
                        "/pause 300" +
                        "/faceDirection farmer 0" +
                        "/move Maru 0 3 2" +
                        "/doAction 7 10" + // open the door
                        "/speak Maru \"Sorry I kept you, I've been...busy. I must look a real mess.\"" +
                        $"/emote farmer {(int)Emote.sad}" +
                        "/speak Maru \"I see you got my letter. It's not something I can ask anyone else to help me with, and it's...embarassing. Hey, can you come inside so I can show you what I've been working on?\"" +
                        $"/emote farmer {(int)Emote.happy}" +

                        //Move into the room for 'the secret'
                        //"/beginSimultaneousCommand" +
                        "/move Maru 0 -2 0 true" +
                        "/move farmer 0 -2 0 true" +
                        //"/endSimultaneousCommand" +

                        "/pause 800" +

                        //"/doAction 7 10" + // can't shut the door
                        //"/beginSimultaneousCommand" +
                        "/move Maru -1 0 3" +
                        "/move farmer 0 -2 0" +
                        //"/endSimultaneousCommand" +

                        "/faceDirection Maru 1" +
                        "/faceDirection farmer 3" +

                        //Maru explains the problem
                        "/message \"Maru walks into the middle of the room, looking fidgety and embarrassed.\"" +
                        "/speak Maru \"So, I've been working on another invention, and it's going pretty well, but...pretty well isn't good enough.\"" +
                        "/question null \"#I'm sure anything you're working on is great.\"" +
                        $"/emote Maru {(int)Emote.happy}" +
                        "/pause 300" +
                        "/speak Maru \"I think that is a great idea, but please don't judge me, ok?\"" +
                        $"/emote farmer {(int)Emote.question}" +
                        "/speak Maru \"I've been really struggling with...my desires...lately, and I thought that by working on a machine I'd be able to get everything under control.\"" +
                        "/speak Maru \"And get off easier as well.\"" +
                        $"/emote farmer {(int)Emote.heart}" +
                        "/pause 500" +
                        "/speak Maru \"Even thought it's very good at getting me in the mood at the beginning, it just feels too...clinical.\"" +
                        "/question fork1 \"What do you want to say to Maru?#Maybe I can try and help you work out what you like#Maybe I can try out the machine and give you some feedback?\"" +

                        // ######## The big split! ########
                        $"/fork {TempRefs.EventMaru}machine" +

                #endregion

                #region Route A - Inspiration
                        // ROUTE A - INSPIRATION
                        $"/emote Maru {(int)Emote.question}" +
                        $"/pause 500" +
                        "/speak Maru \"I'm willing to give it a try if you are. Maybe between the two of us we can work out what turns me on the most? I have one condition though, that you use the attachments I have for my invention. It's going to be hard enough without trying to reinvent fingers...or a tongue.\"" +

                        "/message \"Maru turns around and starts unbuttoning her overalls, undoing the front flap and stepping out of them. You take a moment to admire her plump ass as she bends over and removes her shorts, with her shirt swiftly following, revealing her bare back.\"" +
                        "/speak Maru \"I...don't like to wear a bra if I don't have to. They're so uncomfortable, and if I'm not leaving the house I don't see the point.\"" +
                        "/question null \"Do you...think I look ok?#You look beautiful\"" +
                        $"/emote Maru {(int)Emote.blush}" +
                        "/speak Maru \"You have this, Maru.$h\"" +
                        "/speak Maru \"Thank you, @. I'm not very confident about my body, and you always make me feel so safe and wonderful. I know I can trust you." +

                        // OBLIGATORY PARENTAL INTERRUPTION" +
                        "/move Demetrius -12 0 3" +
                        //"/pause 300" +
                        "/move Demetrius 0 -1 0" +
                        "/speak Demetrius \"Is everything ok in there, honey? I think I heard someone come in earlier.\"" +
                        $"/emote Maru {(int)Emote.exclamation}" +
                        "/pause 300" +
                        "/speak Maru \"Yeah, dad. Everything is fine. @ came over to help me with a project, so we may be a while.\"" +
                        $"/emote Maru {(int)Emote.blush}" +
                        "/speak Demetrius \"Let me know if you need a hand with anything, ok?\"" +
                        "/speak Maru \"Ok Dad. Are you feeling left out because I have a new helper? I promise that you aren't being replaced...completely\"" +
                        $"/emote Demetrius {(int)Emote.exclamation}" +
                        "/speak Demetrius \"Ouch. Ok honey. Don't forget to eat and stay hydrated, ok?\"" +
                        "/move Demetrius 0 1 2" +
                        //"/pause 300" +
                        "/move Demetrius 5 0 2 true" +

                        // BACK TO THE SCENE/NEW SCENE
                        "/speak Maru \"Of course my dad would choose that moment to make an appearance. I think we'd better find somewhere more private to continue this. I guess we'd better head into the lab? I have everything down there and I REALLY don't want my dad to start asking questions through the door again.\"" +
                        $"/emote farmer {(int)Emote.blush}" +

                        //transition to MaruBasement? may need to use fork.
                        "/fade" +
                        "/viewport -300 -300" +

                        "/message \"Maru unlocks the door to her lab in the basement\"" +
                        "/speak Maru \"Hey there, @. Won't you step into my lab? I have *all* sorts of wonderful things to try with you.$h\"" +
                        "/message \"As she starts climbing down the ladder, she flashes you a nervous, but slightly coy smile\"" +
                        "/pause " +
                        "/speak Maru \"I'm sorry about the mess. When I'm in the middle of things I tend to accumulate a lot of stuff. Anyway, I plan on making the area a lot messier before we're done.$h\"" +
                        "/message \"As you enter the lab, you see that Maru wasn't exagerating. There are various half finished projects strewn around the room, and The workbench is covered in wires and electrical components.\"" +
                        "/speak Maru \"There is a bottle of lube sitting beside a stool, and Maru walks past it to the workbench. After a couple of minutes she stands, triumphant and naked, having removed several items for you.\"" +
                        "/speak Maru \"See? I made it all modular so that I can try different things and see what works the best. I plan on upgrading some of them in the future.\"" +
                        "/message \"Maru stands before you, completely naked and holding a dildo in one hand, and a couple of small objects shaped like neko cat faces. She see you staring at them and clumsily uses her other hand to press a button, making them vibrate.\"" +
                        "/speak Maru \"They had a sale on these...and I spent too much money on the servos.$s\"" + // Farmer looks puzzled at toys, but maru is very attractive

                        "/message \"Maru quickly makes some space on the workbench, wipes the worst of the oil up with a rag, and then hops up on top and lies down with her knees over the edge. She props herself up on her elbows and looks at you coyly.\"" + // makes space on workbench and lies down on it" +

                        "/speak Maru \"So, I usually start off with the little vibrators between my legs, but I kind of struggle to get going from there. The sensations are nice, but I just end up feeling a little turned on without any hunger or anything." +
                        "/speak Maru \"I kind of get a little wet pretty quickly, but I end up drying up and then have to stop to apply lube...and it just ends up being pretty unfulfilling, even when I use the big boy there.\"" +
                        "/message \"You have a pretty good idea where she is going strong, so you ask her to lie down, close her eyes, and focus on the sensations. You start off by gently rubbing her breasts to get some feeling in them, and then turn on the little vibes and trace them around her breasts slowly.\"" +
                        "/message \"Her nipples responds quickly, and you bring the vibes closer and closer to them, causing Maru to start breathing heavier. You firmly hold them against her nipples, and her breath catches in her throat.\"" +
                        "/message \"Taking this as a good sign, you keep one hand there with a vibe, and use the other to start tracing patterns on the inside of her thigh. You make a path up the inside of her thigh, rubbing it up the outside of her labia, and then move to the other side and repeat.\"" +
                        "/speak Maru \"I see what you're doing there, trying to increase my focus on other parts of my body to turn me on first. I'm sure that I can do something so that it circles around my breasts, or up my thighs, but...\"" +
                        "/message \"You continue your path up her thigh, and this time end up sliding the vibe between her wet lips, pressing it lightly against her opening. She breaks off mid-sentence, clearly caught out, and her mouth lets out a low moan of pleasure.\"" +
                        "/message \"You slowly start moving it around her lips, touching lightly against her clit (which causes her to arch her back) and then back down to her opening to press slightly inside her. You do this several times, noticing just how wet she is getting, and also how close.\"" +
                        "/message \"As Maru starts panting, you remove the vibe from her lips and move it back up to her breasts, keeping some stimulation but denying her release.\"" +
                        "/speak Maru \"What are you doing? I was almost there!$a\"" +
                        "/message \"You smile down at her, completely helpless before you, and remind her that the goal is not just to get her off, but to give her ideas for the future. Edging can help bring her more enjoyable orgasms, and is something that she might be able to do with the machines.\"" +
                        "/message \"Maru pouts at you, but can't really do anything right now. After a moment you see she has come back from the edge, and you pick up the dildo that she grabbed earlier, and apply some lube. You rub it between your hands a few times to warm it up, then bring it to her entrance and start gently nudging it with the tip.\"" +
                        "/message \"It isn't long before her hips are bucking against the dildo, trying to get it inside her, so you oblige by slowly sliding it inside, her juices and the lube helping it to slip deeply inside her. You bring it back out again, clearly slow than Maru wants, but her juices are starting to pool on the workbench anyway, showing just how good it must feel.\"" +
                        "/speak Maru \"I'm getting really close again, @. That vibrator feels so good.\"" +
                        "/message \"You hadn't noticed before, but you can see a button on the bottom of the 'vibrator'. You pause for a moment to switch it on, and Maru almost bucks off the bench from the shock. The orgasm that has been building crashes into her as you plunge the vibrator fully inside her, and start pumping away.\"" +
                        "/speak Maru \"@....I'm cumming!$h\"" +
                        "/pause 800" +
                        "/speak Maru \"That was amazing, @. I never would have considered focussing on my breasts first. I guess I was thinking too hard about one part of my body, when I have sensation in all of it. And warming up the lube first stopped it from distracting me.\"" +
                        "/speak Maru \"I was alittle annoyed that you didn't warn me you were going to keep me on the edge, but...it did feel amazing when I finally came. I'm sure I can rig something up in software to stop me from cumming right away, but I'll probably try and get around it the first few times.\"" +
                        "/speak Maru \"I guess there's no substitution for a 'helping hand' though. You were amazing. Let me get cleaned up and we can head back upstairs. I'd hate for my dad to see me naked, covered in oil, and my juices dripping down my leg. I'd NEVER be able to live that down.$l\"" +

                        // End the scene
                        "/end";
                #endregion

                #region Route B - Machine Sex
                data[$"{TempRefs.EventMaru}machine"] = "" +

                //Maru encourages farmer to get undressed, and says she will 'take notes'
                //farmer starts undressing


                // ROUTE A - INSPIRATION
                $"/emote Maru {(int)Emote.question}" +
                $"/pause 500" +
                "/speak Maru \"I'm willing to give it a try if you are. Maybe you'll have some insights afterwards, or be able to suggest something for me to try. And of course, I'll get to see your...sexy...body...getting fucked by my invention.\"" +
                "/speak Maru \"Erm...I hope you don't mind, but this is probably the most erotic thing I've thought about. Making a machine and having it fuck you while I watch? I'm getting hot already.\"" +
                "/message \"Maru turns around and starts unbuttoning her overalls, undoing the front flap and stepping out of them. You take a moment to admire her plump ass as she bends over and removes her shorts, with her shirt swiftly following, revealing her bare back.\"" +
                "/speak Maru \"I...don't like to wear a bra if I don't have to. They're so uncomfortable, and if I'm not leaving the house I don't see the point.\"" +
                "/question null \"Do you...think I look ok?#You look beautiful\"" +
                $"/emote Maru {(int)Emote.blush}" +
                "/speak Maru \"You have this, Maru.$h\"" +
                "/speak Maru \"Thank you, @. I'm not very confident about my body, and you always make me feel so safe and wonderful. I know I can trust you." +

                // OBLIGATORY PARENTAL INTERRUPTION" +
                "/move Demetrius -12 0 3" +
                //"/pause 300" +
                "/move Demetrius 0 -1 0" +
                "/speak Demetrius \"Is everything ok in there, honey? I think I heard someone come in earlier.\"" +
                $"/emote Maru {(int)Emote.exclamation}" +
                "/pause 300" +
                "/speak Maru \"Yeah, dad. Everything is fine. @ came over to help me with a project, so we may be a while.\"" +
                $"/emote Maru {(int)Emote.blush}" +
                "/speak Demetrius \"Let me know if you need a hand with anything, ok?\"" +
                "/speak Maru \"Ok Dad. Are you feeling left out because I have a new helper? I promise that you aren't being replaced...completely\"" +
                $"/emote Demetrius {(int)Emote.exclamation}" +
                "/speak Demetrius \"Ouch. Ok honey. Don't forget to eat and stay hydrated, ok?\"" +
                "/move Demetrius 0 1 2" +
                //"/pause 300" +
                "/move Demetrius 5 0 2 true" +

                // BACK TO THE SCENE/NEW SCENE
                "/speak Maru \"Of course my dad would choose that moment to make an appearance. I think we'd better find somewhere more private to continue this. I guess we'd better head into the lab? I have everything down there and I REALLY don't want my dad to start asking questions through the door again.\"" +
                $"/emote farmer {(int)Emote.blush}" +

                //maru suggests they go into her 'lab'
                //sexy, seductive maru; 'step into my lab'
                //transition to MaruBasement?
                "/fade" +
                "/viewport -300 -300" +


                //makes space on workbench and farmer lies down on it
                //sorry about the mess -are you comfortable
                //farmer ok

                "/message \"Maru unlocks the door to her lab in the basement\"" +
                "/speak Maru \"Hey there, @. Won't you step into my lab? I have *all* sorts of wonderful things to try with you.$h\"" +
                "/message \"As she starts climbing down the ladder, she flashes you a nervous, but slightly coy smile\"" +
                "/pause " +
                //need to strip farmer
                "/speak Maru \"I'm sorry about the mess. When I'm in the middle of things I tend to accumulate a lot of stuff. Anyway, I plan on making the area a lot messier before we're done.$h\"" +
                "/message \"As you enter the lab, you see that Maru wasn't exagerating. There are various half finished projects strewn around the room, and the workbench is covered in wires and electrical components.\"" +
                "/speak Maru \"There is a bottle of lube sitting beside a stool, and Maru walks past it to the workbench.\"" +
                "/speak Maru \"See? I made it all modular so that I can try different things and see what works the best. I plan on upgrading some of them in the future.\"" +

                "/message \"Maru quickly clears off the workbench, wipes the worst of the oil up with a rag, and cleans it up as best as she can. She helps you up on to the table, then fetches a bundle of wires and servos, as well as some rather cute looking sex toys..\"" +
                "/speak Maru \"They had a sale on these...and I spent too much money on the servos. Here, let me set everyhing up. In my fantasies I would have bound you to the table, but I don't have anything like that ready...$s\"" + // Farmer looks puzzled at toys, but maru is very attractive
                "/message \"Maru sets up a rather small looking machine at the end of the workbench that looks suspiciously like a prototype of a robot, except for a piston protuding from the front. A purple dildo is attached the end, along with a spring mechanism to reduce the force, and cute little eyes are painted on it.\"" +

                //FARMER GETS SETTLED IN
                //maru joke about restraints for farmer
                //farmer emote concerned / exclamation
                //maru starts connecting various bits and pieces to the machine, and then 'strapping?' them to the player
                //maru: it's the only way i could think of keeping them on me. it's just velcro, so don't worry about being trapped
                //maru: i'd be mortified if i needed my mum or dad to get me out of them one day!
                //farmer emote laugh
                //maru switches on machine and farmer 'jumps?'
                //farmer emote suprised
                "/speak Maru \"I have 'Henry' all set up, and I usually start off by holding one of the the little vibrators between my legs, and the other on my breasts. I guess I'm rambling a bit, so I should probably just start everything off.\"" +
                "/message \"You close your eyes and wait for Maru and 'Henry' to begin. You hear a small buzzing sound, and Maru's hand presses one of the small vibrators against your breast, circling your nipple slowly.\"" +

                "/speak Maru \"Your nipple responds quickly, hardening and standing erect, and she brings the vibe closer and closer to it. Your breathing gets heavier, and your breath catches in your throat when Maru brings it to rest right on the tip.\"" +
                "/speak Maru \"Taking this as a good sign, you keep one hand there with a vibe, and use the other to start tracing patterns on the inside of her thigh. You make a path up the inside of her thigh, rubbing it up the outside of her labia, and then move to the other side and repeat.\"" +
                "/speak Maru \"Your breasts are perfect, @. I just want to suck on your nipples and make you cry out, but that will have to wait for another time.\"" +
                "/message \"You flinch a little as Maru places the other vibrator on your mound, and starts inching it downwards towards your sensitive place. It wanders slowly, never in a straight line, and teases you as it gets closer and closer to your nether lips. You can feel your juices running, and Maru slides the vibe up your slit, coating it heavily in your juices, and then pushes it firmly against your opening, causing you to buck off of the table.\"" +

                "/message\"Her hands leaves your breast, and Maru starts moving the vibrator up and down your pussy, making you jump every time it bumps against your clitoris, and moan every time she presses it slightly into your vagina. You feel your orgasm getting closer, and she suddenly removes that hand from you as well, leaving you unable to climax.\"" +
                "/speak Maru \"I'm sorry honey, but if you cum too soon I won't get much out of this experiment. Also, that was just the warm up to get you ready for Henry.\"" +
                "/message \"You pout at Maru, but you can't really do anything right now. After a moment of fiddling with her machine, you feel the dildo poking gently at your entrance, and Maru spreads your lips and inserts it slowly inside you as deep as it can go. She backs it off a little, and adjust 'Henry' a little.\"" +
                "/speak Maru \"Seems like you can take him a little deeper than me right now, but I've adjusted him for that. I think it won't be long before I can take him that deep as well, and I have been thinking about getting a larger attachment as well.$h\"" +
                "/message \"Maru flicks a switch, and the machine starts sliding in and out of you, building speed slowly. It isn't long before your hips are bucking against the dildo in time with it's thrusts, and you look over to see that Maru has her fingers deep inside her snatch as she watches you get pounded.\"" +
                "/speak Maru \"I hope it feels good for you, @. Watching your cunt take 'Henry's' dildo and stretch around it is the hottest thing I've seen. I wish I could record this and play it back when he's fucking me too. Here, let me turn on the last function.\"" +
                "/message \"There's a brief pause, and then the 'dildo' attachment starts vibrating as well with a buzzing noise much louder than it's little siblings earlier. Your eyes rolls back into your head at this extra stimulation, and you hear Maru shout out in pleasure as she squirts her cream all over her hand. Her legs are unsteady, but she leans against the workbench and french kisses you as your orgasm finally washes over you.\"" +
                "/pause 800" +
                "/speak Maru \"That was amazing, @. Here, let me turn off 'Henry' and I'll clean you up. I've never cum as hard in my life, and I'm starting to think that the machine is only part of it. Being here with you was wonderful, and I think that's a big part of what I've been missing.$l\"" +
                "/message \"After Maru helps wipe you clean (with a fresh cloth) she starts asking you questions and taking notes about which bits you enjoyed the most. After a while you both get dressed and head back upstairs.\"" +

                // End the scene
                "/end";

                #endregion
                #endregion

                #region Farmer taking up Maru on the option of providing milk directly.
                #endregion
            }

            // Partial - moved to CP
            if (asset.Name.IsEquivalentTo("Data/Events/HaleyHouse") && false)
            {
                #region Emily clothing model/groping event 594809 complete?
                ModFunctions.LogVerbose($"Loading event {TempRefs.EventEmily} - need to finish");
                data[$"594809" +

                        // Conditions
                        //"D <name>" +                // farmer is dating the given NPC name.
                        //"J 	" +                     // farmer has finished the Joja Warehouse.
                        //"L" + 	                    // Event is in the FarmHouse and the FarmHouse has been upgraded at least twice (has nursery but not necessarily cellar).
                        //"M <number>" +              // farmer has at least this much money.
                        //"O <name>" +                // farmer is married to that NPC.
                        //"S <secret note ID>" +      // farmer has seen the Secret Note with the given ID.
                        //"a <x> <y>" +               // farmer is on that tile position.
                        //"b <number>" +              // farmer has reached the bottom floor of the Mines at least that many times.
                        //"c <number>" +              // farmer has at least that many free inventory slots.
                        //"e <event ID>" +            // farmer has seen the specified event (may contain multiple event IDs).
                        "f Emily 1500" +            // farmer has at least <number> friendship points with the <name> NPC. Can specify multiple name and number pairs, in which case the farmer must meet all of them.
                                                    //"g <gender> 	" +         // farmer is the specified gender (male or female).
                                                    //"h <pet> 	" +             // farmer does not already have a pet, and their preference matches <pet> ("cat" or "dog").
                                                    //"i <item ID> 	" +         // farmer has specified item in their inventory.
                                                    //"j <number> 	" +         // farmer has played more than <number> days.
                                                    //"k <event ID> 	" +         // farmer has not seen that event (may contain multiple event IDs).
                                                    //"l <letter ID> 	" +         // farmer has not received that mail letter or non-mail flag.
                                                    //"m <number> 	" +         // farmer has earned at least this much money (regardless of how much they really have).
                        "n MTV_EmilyQ2" +         // farmer has received that mail letter or non-mail flag.
                                                    //"o <name> 	" +             // farmer is not married to that NPC.
                        "p Emily" +                 // Specified NPC is in the farmer's location.
                                                    //"q <dialogue ID>" +         // farmer has chosen the given answer in a dialogue. May contain multiple dialogue IDs, in which case they must all have been selected.
                                                    //"s <item ID> <number>" +    // farmer has shipped at least <number> of the specified item. Can specify multiple item and number pairs, in which case all of them must be met.
                                                    //"t <min time> <max time>" + // time is between between the specified times. Can range from 600 to 2600.
                                                    //"u <day of month> 	" +     // day of month is one of the specified values (may contain multiple days).
                                                    //"x <letter ID>" +          // For the farmer: mark this event as seen, add the specified letter to tomorrow's mail, then return false (so that nothing further happens). Use the format "x letterid true" to send the letter immediately.     
                        ""] =

                #region Setup the scene
                        $"playful" +
                        $"/14 6" +
                        $"/farmer 16 10 0 Emily 14 5 0" +
                        $"/skippable" +
                #endregion

                #region Start of scene
                        // Begin the scene
                        "/emote Emily 56" +
                        "/textAboveHead Emily \"Hum dee hum\"" +
                        "/pause 500" +
                        "/move farmer 0 -2 0" +
                        "/move farmer -2 0 3" +
                        "/move farmer 0 -2 0" +

                        // start introduction
                        "/faceDirection Emily 2" +
                        "/emote Emily 32" + // Happy
                        "/textAboveHead Emily \"Almost done\"" +
                        "/pause 300" +
                        "/emote farmer 16" +
                        "/pause 500" +
                        "/speak Emily \"There we go! It took a lot of work, but I think its ready. I'm really excited to see how it looks on a real person.\"" +
                        "/question null \"#What is it?\"" +
                        "/speak Emily \"Well, I got a commission for a ball gown, and they sent me some sketches of what they want it to look like, along with their measurements. I hope you don't mind, but I think you're actually the same size as the client.\"" +
                        "/emote farmer 8" +
                        "/pause 300" +
                        "/emote Emily 16" +
                        "/emote farmer 60" +
                        "/pause 400" +
                        "/speak Emily \"Oh, I hope you don't think I'm creepy. It's just very difficult to find models outside of Zuzu City, so I often ask Robin, Jodi and Caroline to model dresses for me, depending on what size I need to make it." +
                            "#$b#Here, let me just double check your waist and bust to make sure it's going to fit.\"" +
                        "/message \"Emily doesn't wait for you to respond, and has her tailors tape around your waist before you can protest. She clucks her tongue as she writes down your measurements on a notepad, and then quickly checks your chest size in a very professional manner.\"" +
                        "/emote farmer 16" +
                        "/speak Emily \"Oh, don't worry. I don't tell anyone about the measurements I take, and bodies come in all shapes and sizes. Don't let the magazines tell you that everyone needs to be the same size to be beautiful!\"" +
                        "/emote farmer 32" +
                        "/pause 300" +
                        "/message \"Emily checks her notes for the dress against yours, and seems happy.\"" +
                        "/speak Emily \"Just right. You're an inch or so thinner around the chest, but you have an extra couple of inches on the waist. Don't pout, I actually think you have a better body shape, and you work on the farm so it's to be expected.\"" +

                        "/fade" +
                        "/viewport -100 -100" +

                        // Try on the dress
                        "/message \"Emily removes the dress from her sewing machine, while you strip out of your outerwear, and hands it to you. She helps you step into it, and after a few minutes of wriggling and making sure the seams aren't going to pop, you are ready for her to zip you up.\"" +
                        "/speak Emily \"Oh wow, it looks so good on you! It really catches the light and accentuates your curves! Can you move around and walk in it fine?\"" +
                        //"/move farmer -2 0 1" +
                        //"/pause 200" +
                        //"/faceDirection farmer 3" +
                        //"/pause 200" +
                        //"/move farmer 2 0 3" +
                        //"/faceDirection farmer 0" +
                        "/speak Emily \"Wow, your ass looks so good in it! Here, it looks like it was hitching up a little. That won't be an issue with the right underwear, but I understand not wearing lingerie while farming. Here, let me smooth it out for you.\"" +
                        "/message \"Emily walks behind you and starts smoothing down your rump. She seems to get a distracted after a moment, and she gives your ass a couple of squeezes, causing you to yelp.\"" +
                        "/speak Emily \"Oh! I'm so sorry, @. You just look so good in the dress, and I get a little caught up in my head sometimes. I have to admit, I've been working non-stop on this dress for a while, and staring at reference pictures all day can get me a little...distracted.\"" +
                        "/emote Emily 32" +
                        "/emote farmer 32" +
                        "/pause 200" +

                        "/speak Emily \"Here, let me help you out of the dress, I don't think you can reach the zipper yourself. I have a couple of things to finish off and then I can ship it off to the client tomorrow morning.\"" +
                        "/fade" +
                        "/viewport -300 -300" +
                        "/message \"Emily walks around behind you and starts unzipping the dress. She helps slide it down around your waist, and kneels down behind you to pick up the dress as your step out of it. After a moment you realise she is still behind you, and feel her hand gently rest against your behind.\"" +
                        "/speak Emily \"You really do have such an attractive body, @. I promise I didn't ask you to do this just so I could see you in your underwear, but if you ever wanted me to design you something sexier I would love to.\"" +
                        "/message \"You smile at Emily as she straights up and places the dress on her work table, then turn around and pose for her." +
                            "#Emily walks over and wraps her arms around you, pulling you in close. Her body is pressed tightly against yours, and she lean into you, resting her head on your shoulder.\"" +
                        "/speak Emily \"I would love to lose myself with you right now, but you'd be suprised how tiring sitting at a sewing machine can be.\"" +

                        "/mail MTV_EmilyQ1T" +

                        "/quickquestion \"What would you like to do?" +
                            "#Give her a relaxing massage" +
                            "#Offer to help her work out her sexual frustrations" +
                            "(break)switchevent 594809boring" +
                            "(break)switchevent 594809Sexytime" +

                        "/end";
                #endregion

                #region Boring choice
                data["594809boring"] = "pause 50" +
                        "/speak Emily \"Oh, @. That would be wonderful. Here, give me a moment to take off my shirt and pants. There's some oil in the dresser over there.\"" +
                        "/message \"Emily quickly disrobes and lies on her bed, face down, quickly settling. Her body doesn't seem to be very relaxed, but you warm up the oil on your hands and start gently running your hands over her back. Her skin is soon glistening, and she is soon gently moaning into her pillow.\"" +
                        "/speak Emily \"Oh, @. That feels heavenly. Most of my tension is in my shoulders and lower back.\"" +
                        "/message \"You start rubbing her shoulders a little more intently, creating circles and spirals on her back that quickly fade from view. Her body is soon much more relaxed on the bed, and her moans and sighs of appreciation start getting quieter.\"" +
                        "/speak Emily \"@, if you keep this up then I'm going to fall asleep. As wonderful as this is we should probably stop so I can finish up the dress. Perhaps you can come back later after I've finished up and I can show you my appreciation?\"" +

                        "/addConversationTopic get_eaten 4" +
                        // End the scene
                        "/end";
                #endregion

                #region sexytime fork!
                data["594809Sexytime"] = "pause 50" +
                        "/speak Emily \"Oh! I wasn't touching you because I wanted to have sex with you, I just get distracted when I'm tired. It's not that I wouldn't love to, I just don't want to take advantage of you.\"" +
                        "/question null \"#It's ok, I want this too.\"" +
                        "/speak Emily \"Let's get a bit more comfortable then. I feel like I'm overdressed for this now.\"" +
                        "/message \"Emily removes her pants and top, her tired back clearly slowing her down. When she gets down to her underwear, you help her unclasp her bra, and sit her down on her bed so you can remove her panties." +
                            "#She leans back on the bed, so you start trailing kisses up her legs, pausing when you reach the top of her thigh." +
                            "#You hook your fingers in her underwear and slowly pull them down, taking the last of her reserve with them.\"" +
                        "/speak Emily \"Come join me on the bed, @. I don't have a lot of strength left, but I would love to be intimate with you.\"" +
                        "/message \"You quickly shed the last of your clothing and join Emily on the bed, cuddling against her naked body and gently stroking her skin." +
                            "#Emily sighs contentedly, and you start gently cupping and squeezing her breasts, watching her face to see her reactions." +
                            "#Before long you see that Emily's breath is getting shorter, and you take that as your cue to start gently brushing her nipples with your finger tips, squeezing them slightly, and eventually taking one of her nipples into your mouth and suckling.\"" +

                        "/speak Emily \"Don't get too rough with me today, @, I need some tender loving today.\"" +
                        "/question null \"#Ok, gentle, tender loving today.\"" +
                        "/message \"You suck on her nipple a couple more time, the sweet taste of her milk wetting your appetite, and reminding you that there are other juicey places to explore." +
                            "#You shimmy down the bed and seat yourself between her legs, knees bent to create space for you. You grab her slim butt and use it to hold yourself in place as you start planting kisses around her inner thighs" +
                            "#Not wanting to rush this, and knowing that she needs relaxing just as much as she needs an orgasm, you take your time making sure every spot of skin is loved, and then move inwards, building the suspense and arousal." +
                            "#The slight trickle of juices from when you started has now become a river snaking its way between Emily's lips. You place your tongue at the bottom of her slit and take one long, slow lick all the way up.\"" +

                        "/speak Emily \"oooooohhhhhh!\"" +
                        "/message \"You can feel Emily fighting not to close her legs right now, and her pussy is trembling as you reach the top, your tongue gently touching her clit momentarily." +
                            "#You sneak a glance upwards and see Emily blissfully gazing at the ceiling, a content look upon her face.\"" +

                        "/question null \"#Want me to keep going?\"" +
                        "/speak Emily \"Please, @. I didn't realise how much I needed this until you started kissing me down there.\"" +
                        "/message \"You start gently kneeding Emily's ass before going back to work licking all the juices from around her outer labia, and then taking slow deliberate licks between them." +
                            "#The flow isn't stopping any time soon, but you start tongueing her opening directly, probing the edge of it and then pushing your tongue in and wiggling it to stimulate her." +
                            "#The slow build at the beginning, and her overdue orgasm, soon has Emily's hips bucking as your tongue delves deep inside.\"" +

                        "speak Emily \"I'm getting close, @!\"" +
                        "/message \"You try and push as much of your tongue into Emily as you can as she goes over the edge. " +
                            "#Her hips close around your head in ecstasy and everything goes dark. You find her clit and start flicking it with your tongue, causing Emily to cry out loudly as she gushes." +
                            "#You ride out her orgasm until she collapses back on the bed, letting you see and breathe once more.\"" +

                        "/speak Emily \"That was amazing @, but I don't think I have any energy to do anything else today now. I'll have to pack up the dress later, but it was definitely worth it. Thank you.$l\"" +

                        "" +
                        "/end";
                #endregion

                #endregion

            }

            // idea only
            if (asset.Name.IsEquivalentTo("Data/Events/Beach"))
            {
                #region Shane Beach event
                /*
                 * 
                 *  Shane 8 hearts
                    requires seeing going sober event
                    farmer has vagina
                    ~craving a  drink~
                 *  Letter from Marnie that Shane hasn't been doing so good/speaking with Jas and she's worried about uncle Shane

                    Player finds Shane at the beach, staring at the sea
                    
                    Shane: sometimes i feel like i'm just trying to push back the tide. That no matter how hard and long i resist, eventually it will wash over me again.
                    Player:concern
                    Shane:I appreciate that you don't try and tell me to hang on for one more day. every day i hang on for one more day. some of them don't suck. some of them really do.
                    	I don't know if i'll ever have good days in the future, or if mediocre is the best i can hope for.
                    Player:*hug*
                    Player: q: what do you want to say? they will come in time. you just can't see them right now | it's the same for everyone. you just need to lower your expectations and then you can be happy. | most of life sucks, but the bits that don't suck are what makes it worthwhile.
                    shane: 
                    		1+3: everyone keeps telling me that, but I don't think they realise how much they are asking me to trust them. 
                    shane:I've had my expectations broken before and it hurt me so much. I end up so depressed that I need something to numb the pain, or take my mind off of it. that's where alcohol comes in...
                    shane:right now the tide is going out, but how long before it comes back in?
                    player: I can think of something else that can take your mind off of it...
                    shane:heart
                    player:heart
                    
                    fade to black
                    you flash shane a coy smile, and start undressing. you throw your clothes further up the beach away from the shoreline so they won't get wet from the spray.
                    You gently but firmly push shane on to his back, and climb aboard his body, careful not to kick sand over him.
                    He looks up into your eyes, and you brush your hair out of your face.
                    shane:are you sure about this? I have to know that this is what you want, not that you are doing it out of pity. I get enough of that from everyone else in the valley.
                    farmer:This isn't just about you feeling good, it's about me as well. I want this, and sharing it with you feels right.
                    As you say this you carefully spread your lips and guide him inside you, sliding down his penis as the waves crash in the background.
                    shane:oh, %name. you feel just right around me. I guess you weren't just being nice about wanting this too? you're as wet as the shoreline!
                    instead of replying, you start slowly sliding up and down on his pole, savouring the feeling of fullness that Shane's cock gives you.
                    It's not long before a little moan escapes your lips, but it's easily drowned out by the sounds of the beach.
                    shane:%name, you feel so good. This feels so much better sober than drunk, I can feel everything you are doing.
                    You feel Shane starting to move his hips more, trying to time his thrusts with yours to feel more pleasure.
                    It's not long before your juices are trickling down onto his groin, and the sounds of the two of you making love start to drown out the waves.
                    %fish slap noises%
                    %moan%
                    shane:I'm starting to get closer, %name.
                    farmer:q:do you want him to pull out? Yes|No
		            1: As Shane's orgasm nears, he pulls out of you and sprays the sand with his seed. You wait for his orgasm to subside, and he
		            2: You feel that Shane is getting close, and you flash him a grin to put his mind at ease, then start grinding your hips against his in short motions to push him over the edge. His cum spurts deep inside you, and the sensation, coupled with the knowledge that his seed is deep inside you now, brings you over the edge.
                 * */
                #endregion
            }

            // currently writing.
            if (asset.Name.IsEquivalentTo("Data/Events/BathHouse_Pool") && false)
            {

                #region BathHouse Pool event 5948## - Need to write
                #endregion

            }

            #region Example: Blank
            if (false)
            {
#pragma warning disable CS0162 // Unreachable code detected
                ModFunctions.LogVerbose($"Loading event {"ID"} - need to write");
#pragma warning restore CS0162 // Unreachable code detected
                data[$"{"ID"}" +

                        // Conditions
                        "D <name>" +                // player is dating the given NPC name.
                        "J 	" +                     // player has finished the Joja Warehouse.
                        "L" +                       // Event is in the FarmHouse and the FarmHouse has been upgraded at least twice (has nursery but not necessarily cellar).
                        "M <number>" +              // player has at least this much money.
                        "O <name>" +                // player is married to that NPC.
                        "S <secret note ID>" +      // player has seen the Secret Note with the given ID.
                        "a <x> <y>" +               // player is on that tile position.
                        "b <number>" +              // player has reached the bottom floor of the Mines at least that many times.
                        "c <number>" +              // player has at least that many free inventory slots.
                        "e <event ID>" +            // player has seen the specified event (may contain multiple event IDs).
                        "f <name> <number>" +       // player has at least <number> friendship points with the <name> NPC. Can specify multiple name and number pairs, in which case the player must meet all of them.
                        "g <gender> 	" +         // player is the specified gender (male or female).
                        "h <pet> 	" +             // player does not already have a pet, and their preference matches <pet> ("cat" or "dog").
                        "i <item ID> 	" +         // player has specified item in their inventory.
                        "j <number> 	" +         // player has played more than <number> days.
                        "k <event ID> 	" +         // player has not seen that event (may contain multiple event IDs).
                        "l <letter ID> 	" +         // player has not received that mail letter or non-mail flag.
                        "m <number> 	" +         // player has earned at least this much money (regardless of how much they " + //ly have).
                        "n <letter ID> 	" +         // player has received that mail letter or non-mail flag.
                        "o <name> 	" +             // player is not married to that NPC.
                        "p <name>" +                // Specified NPC is in the player's location.
                        "q <dialogue ID>" +         // player has chosen the given answer in a dialogue. May contain multiple dialogue IDs, in which case they must all have been selected.
                        "s <item ID> <number>" +    // player has shipped at least <number> of the specified item. Can specify multiple item and number pairs, in which case all of them must be met.
                        "t <min time> <max time>" + // time is between between the specified times. Can range from 600 to 2600.
                        "u <day of month> 	" +     // day of month is one of the specified values (may contain multiple days).
                        "x <letter ID>"] =          // For the player: mark this event as seen, add the specified letter to tomorrow's mail, then return false (so that nothing further happens). Use the format "x letterid true" to send the letter immediately.     


                        "";
            }
            #endregion
        }
    }

    public class EventShell
    {
        public bool Has_Vagina = false;
        public bool Has_Penis = false;
        public bool Is_Ace = false;

        public string EventConditions = "";
        public string EventDataV = "";  // Has Vagina
        public string EventDataP = "";  // Has Penis
        public string EventDataA = "";  // Is Ace
        public string EventDataB = "";  // Is Herm

        public EventShell()
        {

        }

        public string GetEventData()
        {
            if (Is_Ace) return EventDataA;
            if (Has_Penis && Has_Vagina) return EventDataB;
            if (Has_Vagina) return EventDataV;
            if (Has_Penis) return EventDataP;

            return "";
        }
    }

}
