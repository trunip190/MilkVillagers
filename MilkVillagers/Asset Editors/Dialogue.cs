using System.Collections.Generic;
using StardewModdingAPI;
using System.IO;
using StardewModdingAPI.Events;
using System.Linq;

namespace MilkVillagers.Asset_Editors
{
    public static class DialogueEditor // : IAssetEditor
    {
        private static IDictionary<string, string> data;
        public static bool ExtraContent = false;

        public static bool CanEdit<T>(IAssetInfo asset)
        {
            bool result =
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Abigail") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Emily") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Haley") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Leah") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Maru") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Penny") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Caroline") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Jodi") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Marnie") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Robin") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Pam") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Sandy") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Evelyn") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Alex") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Clint") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Demetrius") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Elliott") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/George") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Gil") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Harvey") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Sam") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Sebastian") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Shane") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Pierre") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Gunther") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Gus") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Kent") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Lewis") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Linus") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Marlon") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Morris") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Dwarf") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Mister Qi") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Willy") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Wizard") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Sophia") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Olivia") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Susan") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Claire") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Victor") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Andy") ||
                    asset.Name.IsEquivalentTo("Characters/Dialogue/Martin");

            return result;
        }

        public static bool CanEdit(IAssetName AssetName)
        {
            bool result =
                   AssetName.IsEquivalentTo("Characters/Dialogue/Abigail") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Emily") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Haley") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Leah") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Maru") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Penny") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Caroline") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Jodi") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Marnie") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Robin") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Pam") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Sandy") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Evelyn") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Alex") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Clint") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Demetrius") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Elliott") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/George") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Gil") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Harvey") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Sam") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Sebastian") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Shane") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Pierre") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Gunther") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Gus") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Kent") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Lewis") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Linus") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Marlon") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Morris") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Dwarf") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Mister Qi") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Willy") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Wizard") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Sophia") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Olivia") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Susan") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Claire") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Victor") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Andy") ||
                   AssetName.IsEquivalentTo("Characters/Dialogue/Martin");

            return result;
        }

        public static void Edit<T>(IAssetData asset)
        {
            EditAsset(asset);
        }

        public static void Edit(IAssetData asset)
        {
            EditAsset(asset);
        }

        private static void EditAsset(IAssetData asset)
        {
            data = asset.AsDictionary<string, string>().Data;
            bool Deploy = true;

            if (false) // Let CP do it's job
            {
                //TODO add way to update this on the fly.
                if (ExtraContent && asset.Name.IsEquivalentTo("Characters/Dialogue/Abigail"))
                {
                    ModFunctions.Log($"Loading in Abigail's extra dialogue", LogLevel.Trace);
                    data["Introduction"] = "Oh, that's right... I heard someone new was moving onto that old farm.#$e#I used to love exploring those old fields. I could hide in the weeds, strip naked and masturbate as much as I wanted. $9#Now I guess I'll have to do all of my camgirl streams in my room.";
                    data["spring_mon2"] = "Did you know that Monday night is my streaming night?";
                    data["spring_mon4"] = "Did I tell you that Monday night is my streaming night?#Have you checked out my site, 'Purple Showers'?";
                    data["spring_mon6"] = "People keep asking me to play with strawberries on my show!#Don't any of them listen to me when I say that I hate strawberries?#Even if they pay me I won't eat them, let alone put them in my pussy";
                    data["spring_mon8"] = "I'd love to have you join me on my show. You could tie me up and see how many sticks of rhubarb you can fit in my pussy.#Then I could eat them while you fuck my ass.";
                    data["summer_Thu8"] = "Hi.$l#$e#Do you feel like everything seems unreal lately?$l#$b#Not in a bad way, though, just like a fantasy. Not THAT kind of fantasy, unless you were thinking of something kinky? $h#$q 300123 Shop_no#Do you want to do something kinky?#$r 300121 0 sex_finger#Grope her under skirt until she cums#$r 300121 15 sex_suck#Get under her skirt and lick her#$r 300121 0 sex_tieup#Tie her up and tease her#$r 300123 -15 Shop_no#Not right now";
                    data["summer_Thu4"] = "If you get any blueberries you should bring me them.";
                    data["summer_Thu6"] = "If you get any blueberries you should bring me them.#I'd love to put them inside my pussy and push them out on my show.";
                    data["summer_Thu8"] = "If you get any blueberries you should bring me them.#I'd love to put them inside my pussy and have you fuck me with them inside";
                    data["summer_Mon"] = "When it's this hot all I can do is lie in my room naked. It's too hot to do ANYTHING right now.";
                    data["summer_Tue8"] = "The air is so thick with honey and nectar all summer, I almost feel dizzy.#I might need you to hold me to stop me swooning.$I might fall for you, but on my hands and knees, so you can fuck me doggy-style. $u";
                    data["summer_Wed"] = "Have you decorated your house at all? That's what I'd be doing if I had a house. That, and having sex in every room.";
                    data["summer_6"] = "I went to the beach last night, after dark... Sometimes you can see strange lights bobbing over the sea.#$e#Or maybe that was just a dream I had... It doesn't feel real anymore. All I know is that I woke up really sore between the legs. Must have been a really vivid dream.";
                    data["summer_Fri"] = "I kind of wish I had a cat. Unfortunately, my Dad is allergic to pretty much everything. My mum says there is only one pussy he's ever stroked in his whole life! ";
                    data["summer_Fri6"] = "I kind of wish I had a cat. Unfortunately, my Dad is allergic to pretty much everything.#Sometimes I dress up as a cat and lounge around in my room naked. If you come over remember to give me 'milk'.";
                    data["summer_Fri6"] = "Sometimes I dress up as a cat and lounge around in my room naked.# If you come over remember to give me 'milk'#You can pull on my tail and fuck me while I 'mew'";
                    data["summer_Sat"] = "I'm looking forward to fall... the cool mountain breeze, the swirling red petals, the smell of mushrooms... *sigh* and lets not forget mushroom cream. *wink*";
                    data["fall_mon2"] = "My show hasn't been getting many views. $s #$b#Do you have any suggestions on how I can get more views?";
                    data["fall_mon4"] = "I've been getting more views for my show lately. People don't seem to tip much, but it's better than I could earn in the valley.";
                    data["fall_mon6"] = "My show is one of the most popular on the channel! $h #I try not to think about it too much, but over 1,000 people watch me. # It makes me nervous thinking about all of those strange men...and women.";
                    data["fall_mon8"] = "How is the farm going? If business slows down let me know. #You could come on my show and let them see me get 'plowed' by a real farmer.";
                    data["fall_sat4"] = "I'm so glad it's fall. It's getting cooler, and there is still so much bumpy corn in your fields.";
                    data["fall_sat6"] = "I'm so glad it's fall. It's getting cooler, and there is still so much bumpy corn in your fields.#If you bring my some I can use it on y show on Monday night.";
                    data["fall_sat8"] = "I'm so glad it's fall. It's getting cooler, and there is still so much bumpy corn in your fields.#You should bring my a really big one so you can fuck me with it on my show $h";
                    data["fall_wed4"] = "I think my favourite vegetable has to be eggplant. Did you know in France they call it 'aubergine?";
                    data["fall_wed6"] = "If you want to make me smile bring me an eggplant. #I love how shiny and smooth they are. It's like a giant cock! $h";
                    data["fall_wed8"] = "Is that an eggplant in your pocket or are you just happy to see me?#Whichever one it is I'd love to have it stretch me out and fill me up";
                    data["winter_Sat6"] = "Did you come to visit @'s very own camgirl? There's nothing under my skirt today, and I start dripping any time I see you.#$q 300002 sex_choices#Want a quickie?#$r 300121 0 sex_finger#Grope her under skirt until she cums#$r 300121 15 sex_suck#Get under her skirt and lick her#$r 300121 30 sex_fuck#Fuck her right now#$r 300120 -15 sex_no#Not right now";
                    data["Mon8"] = "...Oh, @! Hi.$h#$e#Want to hang out for a while? Here! Let me read your palm. *giggle* I can have a look at other parts if you want. *wink* $h";
                    data["Sat6"] = "You came all this way to visit me? Someone must want me bad. ;)$h#$e#So have you been exploring the mountain caves at all?#$e#Interesting. I'd like to go there myself one of these days. Or you could explore my cave of wonders. It's pretty wet right now$l";
                    data["Tue4"] = "Hi, I'm glad to see you.$h#$e#I want to take my mind off things for a while... fancy a quickie?";
                    data["Wed2"] = "Oh, hi @. Taking a break from your work?#$e#Me too. Oh! Nothing physical... just some online classes I'm taking. I don't even have to get dressed for them, and I can play with myself as much as I want.";
                    data["Mon2"] = "Oh, hi!#$e#Do you ever hang out at the cemetery? It's a peaceful place to spend some time alone. Sometimes a girl needs to find somewhere...private... at night. *wink*";
                    data["Wed"] = "Hey. Sorry in advance if I say anything rude. I didn't get much sleep last night.#I won't be walking much today either.$e#What do you want?";
                    data["Sun"] = "Oh man... I've been pushing off my homework all weekend. Looks like I'll be pulling another all-nighter...#And not the playing with myself kind either.$s";
                    data["grave_choice"] = "I wonder what would happen if I spent all night in the graveyard?#$q 18 Sun_old#@, What would happen to me alone in the graveyard?#$r 17 0 grave_boring#You'd probably fall asleep.#$r 18 40 grave_ghosts#I think ghosts would watch you play with yourself.#$r 17 0 grave_boring#You might see a zombie.#$r 18 0 grave_boring#Your dad would come find you.#$r 17 30 grave_fuck#I'd tie you up and fuck you against the headstone.";
                    data["grave_boring"] = "That would be boring. Maybe I should just stay home and play porn online.$s";
                    data["grave_ghosts"] = "Hah! Maybe they'd shower me in ghost cum!$h";
                    data["grave_fuck"] = "Promise? The ghosts could masturbate to that and cover me in their ghostly cum.$8#$b#I could pretend I'd been at a rave. A naked rave.$h";
                    data["vacation_choice"] = "Do you ever get an urge to go exploring, @?#$q 300030 vacation_followup#Okay... pretend you just won a free vacation. Where would you take me?#$r 300025 30 vacation_bed#Anywhere, as long as it has a bed and you are with me.#$r 300025 0 vacation_cave#In a dark cave#$r 300025 30 vacation_forest#The old, gnarled forest#$r 300025 15 vacation_resort#A nudist resort#$r 300030 -30 vacation_skip#Don't ask me again";
                    data["vacation_bed"] = "Ooh! Should I bring some candles? $h #$b#And a blindfold? And handcuffs?";
                    data["vacation_cave"] = "We could go adventuring! You'd have to promise to let me use a sword though.";
                    data["vacation_forest"] = "A spooky forest? I'd strip naked and you could chase me through the trees. If you catch me you could tie me up and fuck me under the stars";
                    data["vacation_resort"] = "A nudist resort. Right. Your girlfriend is a camgirl and you want to go to a nudist resort? $s";
                    data["vacation_skip"] = "Fine then";
                    data["vacation_followup"] = "$p 300025 # I look forward to it. $h| I guess I'll have to find another vacation buddy. $s";
                    data["camgirl_choice"] = "Oh, hello.#$b##$q 300130 camgirl_followup#Which of these photos of me looks sexier?$e#$r 300128 20 camgirl_rope#The one with just a bit of rope hanging between your breasts.#$r 300129 5 camgirl_spread#The one with your legs spread and your finger in your mouth.#$r 300130 -15 camgirl_borrow#They're both pretty sexy, but I think I need to study them. Can I take these?";
                    data["camgirl_rope"] = "You can use that rope to lead me around like a puppy.#$b#If you want to, I mean.#$b#I could put a tailplug in and you could pull on it while you fuck me.";
                    data["camgirl_spread"] = "Do you have something I could suck on?#$b#Or something big that can fill me up?#$b#You could always be a guest star on my show.#$b#I'd let you cum on my tits, or in my pussy.";
                    data["camgirl_borrow"] = "Sorry, but if you're going to be looking at me naked then you'd better be making me feel good first.";
                    data["camgirl_followup"] = "$p 300128/300129#Remember those photos I showed you a while ago? I can model some in person if you want.#$p 300130#Did you have a good time with my camsite photos? $h";
                    data["sex_choices"] = "Okay, what are you waiting for?#$q 300123 Shop_no#$e#You can do whatever you want to me.#$r 300121 0 Shop_finger#Grope her under skirt until she cums#$r 300121 15 Shop_suck#Get under her skirt and lick her#$r 300121 30 Shop_fuck#Fuck her right now#$r 300123 -30 Shop_no#Stop asking me.";
                    data["sex_finger"] = "What are you doing with your hand?#$b#Someone is feeling frisky today! Ooh, you're rubbing my slit...Yeah...that's the spot. Keep rubbing there! Mmmm.#$b#Oh! You went inside! I can feel your fingers pushing inside and spreading me! *wet noises* Keep going! I'm going to cum soon!#$b#*Abigail moans loudly* Cumming! Thanks honey, it's always 10x better when you do.#$b#Here, let me lick your hand clean.";
                    data["sex_suck"] = "That's right. Keep licking my pussy#$b#Mmmm! I can feel your tongue inside me! That's so good! Keep licking there! Mmmmmmm!#$b#Did you hear that? Oooooh! I can hear someone! You have to stop! Please...Oh no! Here...I...CUM!#$b#*pants* Wow! I can't believe you licked me while someone was right there - it was so intense!#$b#I guess they know what we were doing. You are the best.#$b#Let me lick my cum off of your face, you dirty boy.^Let me lick my cum off of your face, you dirty girl.";
                    data["sex_fuck"] = "#$b#I want it rough today - push me against the wall while you ram it in. Oh god, it's in so deep! Keep ramming it in as hard as you can.#$b#*squelch* I can feel my juices running down my leg! *pound* *pound*  I'm getting closer...keep going!#$b#Mmmmm, fuck me, fuck me, FUCK ME! *Abigail goes limp as you cum deep inside her*#$b#Wow. Just...wow! I'm going to be leaving a trail for a while with all that cum inside me.";
                    data["sex_tieup"] = "#$b#Do you always carry rope? I could be your very own sex slave, and you could tie me up in the dungeons.#$b#*You tie Abigail's arms behind her back* Hmmm, I think I like where this is going. It feels so naughty when you grope my breasts, and there's nothing I can do to stop it.#$b#Where is your hand going? Do you think you can do anything you want to my pussy and I won't stop you? Just...like...that...#$b#Oooh yeah! You're going to make me ruin my panties! Actually, I think they're already soaking wet.#$b#*You pull her legs apart, take off her panties, then stuff them in her mouth* Mmmff! *You keep fingering her until she climaxes hard, then take the panties out of her mouth and untie her hands.*#$q 300123 panties_no#Do you give her panties back? $e#$r 300131 0 keep_panties#Keep her panties#$r 300132 15 keep_panties#Give her panties back";
                    data["panties_keep"] = "#$p 300131#What do you mean you're keeping them? Anyone could see up my skirt! /addObject 10 17 {{spacechase0.JsonAssets/ObjectId:[{{Abigails_Panties}}]}}|I might taste awesome, but you have some making up to do next time. I might blindfold you and tease YOU instead.";
                    data["sex_no"] = "Not right now. Someone might see us";
                }

                if (!data.ContainsKey("milk_start")) // skip if there is already Content Patcher dialogue loaded.
                {
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Abigail")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding milk_start for Abigail", LogLevel.Trace);
                        data["milk_start"] = "Please be gentle, they are really sore today." +
                        $"#$b#%*You sit down as she lies across your lap, letting her breast hang down. She gives you a bottle and you start kneeding her breasts as gently as you can*" +
                        $"#$b#%*Milk collects in the bottle as you expertly milk her, moving on to the second breast when the first runs dry*" +
                        $"#$b#Thank you. It's so much more erotic when you do it." +
                        $"#$b#Just think of this as taking care of one of your 'cows'. Here, you can keep this.";
                        //data["milk_start"] = "My breasts are so sore, I NEED someone to milk them.#$q 300006 milk_no#Will you help milk Abigail?#$r 300003 15 milk_yes#Milk her#$r 300003 -15 milk_no#Make her milk herself";
                        //data["milk_yes"] = "Please be gentle, they are really sore today.#$b#*You sit down as she lies across your lap, letting her breast hang down. She gives you a bottle and you start kneeding her breasts as gently as you can*#$b#*Milk collects in the bottle as you expertly milk her, moving on to the second breast when the first runs dry*#$b#Thank you. It's so much more erotic when you do it." + $"#$b#Just think of this as taking care of one of your 'cows'. Here, you can keep this. [{TempRefs.MilkAbig}]";
                        //data["milk_no"] = "But...I'm so sore. I'm going to have to try and suck the milk out myself now! $s#$b#Fine, then you have to watch me...as I lick my nipples, suck on them, feel the milk washing down my throat...#$b#*Abigail lifts her breast to her mouth and slowly circles the tip of her nipple. Milk starts leaking, and she carefully scoops it up with her tongue*#$b#*She starts sucking in ernest, and milk dribbles down her chin while she starts moaning softly. You move towards her and she puts a hand out to stop you*#$b#No! You made me do this, so you have to watch. *she switches to her other breast and takes big gulps while a river runs down her front and starts pooling on the floor*#$b#*She finally finishes, looks you in the eye, then turns around and leaves*";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Emily"))
                    {
                        ModFunctions.Log($"Adding milk_start for Emily", LogLevel.Trace);
                        data["milk_start"] = $"Oh! Did you know that human breast milk is a super food?#$b#It's way better for you than cows milk..." +
                        $"#$b#Not that your milk isn't great! Well, your cows milk. I'm sure your milk is just wonderful.$h" +
                        $"#$b#%*She quickly bares her breasts and sigh as the cool air hits them. Her nipples are already hardening, and you reach out gently and pinch them*" +
                        $"#$b#Oh, @! You always make me so wet when you touch me. Maybe I can 'milk' you some other time?$l" +
                        $"#$b#$*You hold her breast in one hand, and suck on her nipple to get the milk flowing. She moans loudly as the first bit of sweet liquid enters your mouth*" +
                        $"#$b#%*As you start milking her breast she reaches down and slips a hand inside her panties, and a heady smell wafts out*" +
                        $"#$b#Don't mind me, I just get so horny when you're around. I'm sure I'm going to have more than one kind of liquid staining my clothes soon.$h" +
                        $"#$b#%*You switch breasts, and her hand movements increase in speed. The sounds of her masturbating, mixed with the sound of her milk collecting in the bottle, get you very aroused*" +
                        $"#$b#I'm cumming!!!";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Haley"))
                    {
                        ModFunctions.Log($"Adding milk_start for Haley", LogLevel.Trace);
                        data["milk_start"] = $"Everyone always said my boobs are great. I guess I shouldn't be surprised that you love them too." +
                        $"#$b#I just LOVE it when guys play with my tits. They're just so sensitive, and my nipples feel heavenly when people lick or suck on them.$h" +
                        $"#$b#%You need no further encouragement, and immediately dive into her cleavage, coating them in your saliva as you try and get her nipples into your mouth*" +
                        $"#$b#Oh @, you don't need to go crazy. We have plenty of time for this." +
                        $"#$b#%*You slow down and suck teasingly on one of her nipplers, eliciting a wonderful moan from Haley. You reluctantly withdraw, and get back to the task at hand, pulling a bottle from your bag*" +
                        $"#$b#%*You start playing with her nipple with one hand, whilst using the other to set up a steady milking rhythm. Milk is soon collecting in the bottle, and Haley is making blissful sounds*" +
                        $"#$b#%*You switch breasts, and Haley is once again lost in her own world as you empty her breasts into your bottle. You soon finish up, and leave a drained Haley to cover up her milk-stained top*";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Leah"))
                    {
                        ModFunctions.Log($"Adding milk_start for Leah", LogLevel.Trace);
                        data["milk_start"] = $"I love the way your hands feel on me, @. You might be better with your hands than me, though I'd love to have a contest some day." +
                            $"#$b#%*Your hands wonder over her breasts, circling slowly closer to her nipple and then dancing away. Leah gasps as you flick her nipple, and pulls away so she can remove her garments*" +
                            $"#$b#%*You retrieve a bottle from your bag, and Leah gets into a comfortable position while you kneel beside her and start firmly squeezing her breast. Milk is soon flowing into the bottle*" +
                            $"#$b#If you are as confident milking your cows it's no wonder that they are so happy, and produce such wonderful milk.$l" +
                            $"#$b#%*You smirk a little at the thought of Leah dressed as a cow, with a cowbell around her neck, and tug a little harder on her nipples. You could have sworn she went 'moo'*" +
                            $"#$b#Wow. That was a wonderful experience. Please come see me again - we can get up to all sorts of mischief in my cabin. $h";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Maru"))
                    {
                        ModFunctions.Log($"Adding milk_start for Maru", LogLevel.Trace);
                        data["milk_start"] = $"I'm sure I could come up with a machine to help with this...I'd have to do some tests to see how the level of arousal affects milk quality..." +
                            $"#$b#%*You quickly unbutton her top to distract her, and give her nipples a quick nibble. Maru yelps and gives you a displeased look*" +
                            $"#$b#@! Treat me properly or you can go find someone else." +
                            $"#$b#%*You apologise and kiss her breasts better to get her in the mood. You start gently massaging her dark skin, and it's not long before she has a content look on her face*" +
                            $"#$b#%*I guess you aren't that bad after all, @. It looks like I'm starting to lactate - you should get a bottle ready so we can record how much fluid I'm producing." +
                            $"#$b#%*Maru gives you pointers, her reserved, scientific manner quickly turning into sexual encouragement. You're not sure when she started touching herself, but her panties are definitely soaked*" +
                            $"#$b#%*Eventually the stream of milk dries up, and you show Maru how much milk is in the bottle*" +
                            $"#$b#I'm not sure anything I make could be better than your touch.";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Penny"))
                    {
                        ModFunctions.Log($"Adding milk_start for Penny", LogLevel.Trace);
                        data["milk_start"] = $"I'm always self conscious about the size of my breasts. George may leer at me, but I think that's just because I'm young and female." +
                            $"#$b#But with you I feel like you see me as a woman. A hot, sexy woman. I want you to see all of me, and touch all of me, without these stupid clothes in the way." +
                            $"#$b#%*Penny tears open her blouse, and brazenly bares her chest. You tell her how beautiful she is, and she blushes. Soon, her nipples are turning a dark shade as well*" +
                            $"#$b#%*You start groping her chest, cupping them and squeezing them as much as you can. Penny is surprisingly sensitive, and she shyly turns her head away as you play with her*" +
                            $"#$b#No-one has ever made me feel such things. I feel just like the women in the romance novels I read, melting under your forbidden touch." +
                            $"#$b#%*You start pulling on her nipples, causing milk to jet out. You try and aim it into a bottle, but Penny is writhing around in pleasure and a lot of it ends up on your face and the floor*" +
                            $"#$b#%*As you finish up, Penny smiles at you breathlessly, clearly extremely turned on*" +
                            $"#$b#You awaken such carnal feelings in me, @. I'm sure I look like a wanton harlot right now...and I definitely feel like one.";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Caroline"))
                    {
                        ModFunctions.Log($"Adding milk_start for Caroline", LogLevel.Trace);
                        data["milk_start"] = $"My breasts are so sore, I NEED someone to milk them. Pierre is so obsessed with his 'business' that he doesn't pay attention to me any more." +
                            $"#$b#%*Caroline sits down on a nearby seat, and you can see that milk is already leaking through her top. She smiles embarrassedly, and pulls her vest to the side*" +
                            $"#$b#Don't be shy, @. It's perfectly natural to feel aroused in this situation, and it's not like I'm going to jump you or anything...This time at least." +
                            $"#$b#%*You bring the bottle to her chest, and start coaxing a steady stream of milk out of her overflowing tits*" +
                            $"#$b#%*You end up with a generous amount coating your hands, and Caroline spends some time using her mouth to suck all of the milk off of your fingers*" +
                            $"#$b#Aw, thanks honey, I feel much better now. I wouldn't want to waste anything, and all of my juices taste SO good. Maybe I can taste yours next time?";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Jodi"))
                    {
                        ModFunctions.Log($"Adding milk_start for Jodi", LogLevel.Trace);
                        data["milk_start"] = $"I've been so lonely since Kent first went off to war, and now that Vincent is growing up so fast I didn't think anyone would ever pay attention to me again.$s" +
                            $"#$b#Then you come along and make me feel so WANTED. I feel like I have a role to play again that is more than just cooking and cleaning. Like I'm a person again - desirable.$h" +
                            $"#$b#%*Your hands gently caress her breasts, and her nipples quickly get hard. Jodi leans into your body and starts rubbing her thighs against your leg*" +
                            $"#$b#%*Jodi is soon moaning alon to your ministrations, and the stream of milk starts filling the bottle. Your leg starts getting wet from her juices, and it's not long before Jodi cums hard*" +
                            $"#$b#@! Hold me tight as I cum! I want to feel your body pressed against mine.$l" +
                            $"#$b#%*You use your free arm to pull her in tight, the milking paused as her body is wracked with pleasure. She finally stops shaking, and rests limply in your arms. You place the bottle on the ground and kiss her deeply*" +
                            $"Thank you for that, @. I know who to come to if I feel lonely again. Or just horny.";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Marnie"))
                    {
                        ModFunctions.Log($"Adding milk_start for Marnie", LogLevel.Trace);
                        data["milk_start"] = $"I'm glad that Lewis isn't the only one to appreciate my big tits! He spends every moment he can in my cleavage, but he never thought to suck on them!" +
                            $"#$b#*Marnie's milk quickly fills the jar, and she sighs contentedly as she rearranges her clothing*" +
                            $"#$b#Make sure Lewis...I mean the Mayor...doesn't catch you! He might get jealous!";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Robin"))
                    {
                        ModFunctions.Log($"Adding milk_start for Robin", LogLevel.Trace);
                        data["milk_start"] = $"Demetrius is always so...clinical...when he talks about my breasts. I wish he was as romantic as you!" +
                            $"#$b#Of course you can collect my milk! Just...don't be surprised if I leave a damp spot on the chair when you're done!" +
                            $"#$b#*As you massage her breast with your hand, filling up the jar, you make sure to play with her other nipple.*" +
                            $"#$b#*Robin snakes a hand down her jeans and starts playing with herself, moaning and whimpering as her milk fills the jar. You finish milking her, but wait for her until she clenches her legs tightly and throws her head back.*" +
                            $"#$b#That was wonderful...Come back, any time.";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Pam"))
                    {
                        ModFunctions.Log($"Adding milk_start for Pam", LogLevel.Trace);
                        data["milk_start"] = $"Really? I...didn't know people were into that kind of thing. I guess it wouldn't hurt, but don't expect me too go 'moo'!$n" +
                            $"#$b#%*Pam looks around before opening her shirt, and pulling her breasts out. They sag without a bra to support them, but your magic hands soon have her nipples perking up*" +
                            $"#$b#*You give her nipple a quick flick with your tongue, and then suck on it to taste her milk. It's sourer then normal milk*" +
                            $"#$b#%*Milk starts to flow from her nipples into your bottle, and Pam looks down with interest as the liquid settles. She bites her lip, and you can see that your hands are having an effect on her whether she liks to admit it or not*" +
                            $"#$b#I never would have imagined you could find anything in these old tits of mine. You are full of surprises, @.$h";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Sandy"))
                    {
                        ModFunctions.Log($"Adding milk_start for Sandy", LogLevel.Trace);
                        data["milk_start"] = $"I knew you were too tempted to pass up this opportunity. There's a reason I'm called the flower of the desert, and I'd love to have you worship my breasts." +
                            $"#$b#%*She quickly sheds her top, baring her beautiful breasts to the hot desert air. Her skin glistens with moisture, and you can't help but lick a droplet of sweat that has caught on her nipple*" +
                            $"#$b#%*Her nipples are perky for their size, and you give them both a quick suck to get the milk flowing, earning you a slight gasp from Sandy." +
                            $"#$b#That's exactly why I've been trying to get you to visit me, @. I knew you would be more than enough to satisfy me, so milk away.$h" +
                            $"#$b#%*You start gently milking Sandy until she urges you to be more aggressive. You start tugging firmly on her nipples, causing her to stagger slightly as the pleasure rocks through her body*" +
                            $"#$b#*The bottle quickly fills up, and Sandy is breathing hard by the time you are done*" +
                            $"#$b#That was amazing, @. I'm going to go...relive myself. My pussy is gushing more than the Oasis, and I can't think straight when I'm this horny.$l";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Evelyn"))
                    {
                        ModFunctions.Log($"Adding milk_start for Evelyn", LogLevel.Trace);
                        data["milk_start"] = $"*Evelyn sits down on a nearby chair and unbottons her blouse. She deftly unhooks her bra, and you tenderly hold her mature breasts in your hands.*" +
                            $"#$b#Oh, @, dear. I fear I may not be able to provide you with much, but I'm grateful that you would try it with me. You are such a darling child.$l" +
                            $"%*You aren't able to coax much milk out, but Evelyn sighs contentedly, grateful for the tender way you are treating her*" +
                            $"#$b#This brings back memories of when I was MUCH younger...and prettier.$h";

                    }

                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Sophia"))
                    {
                        ModFunctions.Log($"Adding milk_start for Sophia", LogLevel.Trace);
                        data["milk_start"] = $"Oh, hey there @. I have the perfect cosplay outfit for that, but I've been too embarrassed to wear it out of the house...It's...a sexy maid outfit, but it doesn't cover...my..." +
                            $"#$b#My breasts very well. Oh god, I can't believe I told you. Please, if you promise not to laugh I'll change into it and you can milk me like a slutty maid.$s" +
                            $"#$b#%*She returns quickly, and the costume is everything she described. A sexy french maid, replete with short, frilly skirt that barely covers her ass, and her breasts are almost completely exposed*" +
                            $"#$b#%*She curtsies, and you command her to twirl for you. Her skirt flies up, and you are greeted with the typical anime panty shot." +
                            $"#$b#%*Wasting no time, you command her to stand still as you start to roughly milk her, eliciting feeble protests and a couple of requests to be gentler. You stop and ask her if she is ok" +
                            $"#$b#@, please don't stop. I've wanted to do this for a long time, and acting is part of cosplay.$l" +
                            $"#$b#%*You renew your milking, and her moans become louder. You notice her rubbing her thighs together, and as you finish milking her she shudders and climaxes*" +
                            $"#$b#Thank you, @. That was a fantasy finally come true. Please...come back again and enjoy my 'services'";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Olivia"))
                    {
                        ModFunctions.Log($"Adding milk_start for Olivia", LogLevel.Trace);
                        data["milk_start"] = $"OH! @, I'm not surprised that you find me attractive, but I am a lady, not someone who would bare their breasts for anyone...$s" +
                            $"#$b#However, you are right. You aren't just anyone to me, and it's been so long since I've felt the passion of anothers touch on my body. I'm flattered.$h" +
                            $"#$b#%*She seductively slips her dress off of her shoulders, revealing her pale, white breasts to your gaze. You tenderly graze them with your finger tips, then, more boldy, brush her nipples*" +
                            $"#$b#%*Olivia gasps as you lean in and gently suck on her nipple, causing a slightly sweet liquid to enter your mouth* You continue for a moment longer, and she leans back against a nearby wall*" +
                            $"#$b#%*You bring out a bottle, and start lovingly massaging her breasts, causing her breast milk to squirt inside. After you fully drain one, you move on to the other, noting that Olivia is feeling flushed*" +
                            $"#$b#Oh, @. You bring out the animal side of me. I never thought I would have these feelings again. I'm melting under your touch.$l" +
                            $"#$b#%*As the milk starts to dry up, you lean in and start suckling directly from her teat. You reach down with your free hand and stroke her panties, quickly bringing her over the edge*" +
                            $"#$b#Thank you, @, for remind me that I am still a woman, and a hot one at that.$l";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Susan"))
                    {
                        ModFunctions.Log($"Adding milk_start for Susan", LogLevel.Trace);
                        data["milk_start"] = $"@, in case you didn't notice I'm an agricultural farmer. I don't keep cows, so cannot provide you with any milk." +
                            $"#$b#Oh. I..I misunderstood you. You are welcome to try, but unless you have some kind of magic touch, I don't see how this will turn into more than groping." +
                            $"#$b#%*Determined to prove her wrong, you start rubbing her chest through her overalls. You quickly feel her nipples stiffen, standing proud and become visible even through the coarse fabric*" +
                            $"#$b#%*Surprising Susan, a pair of dark, wet spots start appearing, and she pulls down her top to see that milk is coming out of her erect mounds*" +
                            $"#$b#@!? I don't know how this is happening, but quickly, make sure you collect it all.$h" +
                            $"#$b#%*She settles into a comfortable position, and you start gently pulling on her nipples, causing thick streams of milk to jet into the bottle. Susan looks on incredulously as the bottle fills*" +
                            $"#$b#I never would have imagined that this was possible, but please come by again. My breasts will be ready for you any time.";

                    }
                    //if (asset.Name.IsEquivalentTo("Characters/Dialogue/Claire"))
                    //{
                    //    data["milk_start"] = $"";
                    //    
                    //}
                }

                if (!data.ContainsKey("BJ"))
                {
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Alex")) //Ver 1.0
                    {
                        ModFunctions.Log($"Adding BJ for Alex", LogLevel.Trace);
                        data["BJ"] = $"Ha! I knew you couldn't resist my dick. Sure you can have my cum. But I'm gonna need help, if you know what I mean.$h" +
                            $"#$b#%*Alex unzips his pants you you massage his cock. It swells under your touch. You bend down and begin to take in his member*" +
                            $"#$b#%*Slowly, moving back and forth until his tip is forcing its way further down your throat. You repress your gag reflex*" +
                            $"#$b#%*You use your tongue and swirl around the shaft, from base to tip, and Alex moans*" +
                            $"#$b#Almost there...little faster$l" +
                            $"#$b#%*Using your spit as lube, you work his shaft while artfully swirling the tip with your tongue." +
                            $"#$b#%Soon, hot salty fluid bursts into your mouth as Alex seizes, then lets out a sigh of relief." +
                            $"#$b#%*You spit your collection into a jar and cap it.*" +
                            $"#$b#Thanks, @. I can always count on you for a quick release." +
                            $"#split#" +
                            $"You think I'm hot? Well, I think you're really hot too, and if you want to take this further I'm sure we can find somewhere 'quiet' for a bit" +
                            $"#$b#%*He pulls you aside and unzips his pants as you drop to your knees. His dick is already rising, and he guides you with his hand on the back of your head.*" +
                            $"#$b#%*He cums rather quickly, and sags backwards as you fill your bottle with his semen, buttong up his pants and giving you a half dazed smile*";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Clint")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding BJ for Clint", LogLevel.Trace);
                        data["BJ"] = $"Wow, this must be my lucky day, @! I might be a little hot and sweaty down there. I've been busy all day and haven't had a chance to wash" +
                            $"#$b#*You pull his pants and underwear down and are lost in his thick, musky smell.* " +
                            $"#$b#*It makes you a little light headed, but when his prick bumps against your forehead you get to business*" +
                            $"#$b#Yeah, just like that, @. It feels really good when you, er, use your tongue like that. Oh! And you're sucking so hard I don't think I can last much longer!" +
                            $"#$b#*Clint cums down your throat";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Demetrius")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding BJ for Demetrius", LogLevel.Trace);
                        data["BJ"] = "Hmm...I guess I could help you out. For scientific research purposes, of course. Just don't let this get back to Robin." +
                            $"#$b#%*Demetrius looks over his shoulder a few times before pulling down the zipper of his pants. You reach in, and removed a semi-erect cock." +
                            $"#$b#You place it in your mouth and it begins to expand to its full size as you move along, until his tip has reached your throat and then some." +
                            $"#$b#You continue a back and forth motion, picking up the pace until you feel Demetrius tense up*" +
                            $"#$b#Uh--!$s" +
                            $"#$b#%*Quickly, you pull out a bottle and collect the scientist's cum inside of it*" +
                            $"#$b#Whew. You know, orgasms are a great way to clear the mind. Now I feel like I can focus on my project more. Thank you, @." +
                            $"#split#" +
                            $"I'm not really comfortable but with this, but I guess if you're collecting samples for scientific study then Robin won't mind...much#$b#" +
                            $"I'm not as large as the popular media would have you think based on my background, but I do make up for it in, er, volume.#$b#" +
                            $"*Demetrius seems to be unused to this much attention, and is quickly on the verge of cumming. You back off for a bit, and tell him that you want to make sure you get a 'proper sample'#$b#" +
                            $"But I was almost there... *You pick up the pace again, and in no time he is on the edge.*#$b#" +
                            $"*Demetrius wasn't lying and you struggle to keep up with his flow, your mouth filling up and his semen spilling down your face. You bottle it up and look up at him, covered in his cum*";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Elliott")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding BJ for Elliott", LogLevel.Trace);
                        data["BJ"] = "I've been writing all day. This will be a perfect release, my love! I do however, have some carpal tunnel. Could you...?" +
                            $"#$b#%*You pull down Elliot's waistband and lather his cock in thick saliva, and begin working his shaft, starting with a loose grip at the base and firmer at the tip*" +
                            $"#$b#%*He moans with pleasure and smiles down at you before tilting his head back and closing his eyes. You quicken your hand movements, and flick the tip with your tongue*" +
                            $"#$b#Almost...there...@" +
                            $"#$b#%*Elliott shudders with pleasure, and you aim his cock at the opening of your jar. His cock spurts out waves of the thick, sticky, fluid*" +
                            $"#$b#Thank you, @. I always enjoy our little trysts. Do come back later if you need any more, I'm very happy to oblige, and I always feel reinvigorated.$h" +
                            $"#split#" +
                            $"I could definitely use a break. I'm suffering from some dreadful writers block and this is exactly what I need to clear my head." +
                            $"#$b#If you could pretend to be a ship's boy then it would definitely help me with this part of my story I\"m working on." +
                            $"#$b#*You drop to your knees and pretend to be surprised by him pulling out his penis, faking a little bit of hesitancy as he encourages you*" +
                            $"#$b#*You pretend to be inexperienced at first, but once Elliott is into it you pick up the pace, and suck as much of his dick into your mouth as you can handle*" +
                            $"#$b#*As your eyes start to water, you try and swallow to massage his penis, and it's enough to send him over the edge*" +
                            $"#$b#@, that is incredible!";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/George")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding BJ for George", LogLevel.Trace);
                        data["BJ"] = $"I can't get out of this chair, and it's been so long since Evelyn did this for me. I wouldn't mind getting that Haley over here some time - she's such a tease." +
                            $"#$b#That's right, bend down and enjoy the taste. Bet you didn't expect to see such a large dick on a man in a wheelchair, huh?";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Gil")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding BJ for Gil", LogLevel.Trace);
                        data["BJ"] = $"Huh? What's going on? *Snore* Why are my pants off?#$b#Guess I must be dreaming again...having a beautiful face looking up at me from between my knees.#$b#*You quickly get to work, licking his balls while his penis hardens. It doesn't get fully erect at first, but after several minutes of soft sucking, and flicking his tip, he starts moaning and getting ready to cum*#$b#*Gil shoots his load into your mouth, and you quickly spit it into a bottle, wiping off any that dribbled down your chin*";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Harvey")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding BJ for Harvey", LogLevel.Trace);
                        data["BJ"] = "Well, a good orgasm IS good for your health. You know, it decrease blood pressure, helps with stress and sleep, and lowers your risk for health disease?" +
                            $"#$b#In fact, there was a study done by Philip Haake that analyzed the effects of the male orgasm on limphocyte subset circulation and cytokine production..." +
                            $"#$b#%*Before Harvey can continue, you unzip his pants and immediately place your mouth on his cock. He stops mid sentence in both surprise and pleasure." +
                            $"#$b#%*You slurp the whole thing in, and he all but quivers as you begin to move back and forth; slowly at first, but soon beginning to pick up the pace." +
                            $"#$b#%*Your tongue glides along his shaft; the faster you go, the less Harvey can maintain his composure. He begins to tremble, his knees threatening to buckle*" +
                            $"#$b#Keep...going...almost...there..." +
                            $"#$b#%*Harvey braces himself on you, and the grip on your shoulder becomes vice-like as he finishes. You don't have time to take out your bottle and take the full load into your mouth." +
                            $"#$b#%*You stand up, and as gracefully as you can, spit the contents in the container. Harvey is sweaty and weak*" +
                            $"#$b#That...took a lot more out of me than I expected. You're too skilled for my own good, @." +
                            $"#split#" +
                            $"It's very important to have a healthy sexual life, but have you have you been making sure to keep track of your sexual partners?" +
                            $"#$b#Perfect, then can I recommend you pay special attention to the underside of my glans? It's very sensitive." +
                            $"#$b#Yes...right there. Make sure you like just inside the tip every so often, but don't press too hard. Here we go, my orgasm is imminent." +
                            "#$b#*You grab a flask and hold it over the tip of his penis as he starts to ejaculate, collecting most in it, and giving his dick a quick suck to get the last little bit*";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Sam")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding BJ for Sam", LogLevel.Trace);
                        data["BJ"] = $"I've been horny as hell all day, this is JUST what I need. Besides, who could resist you anyway?$h" +
                            $"#$b#%*Sam, all too eager, unzips his jeans and pulls out his dick, which is already full mast. He looks at you with a mixture of excitement and desire*" +
                            $"#$b#%*You bend down to your knees and place your hands firmly on his thighs, and insert his throbbing cock into your mouth. Sam has a sharp intake of breath as you begin to move, adding more and more suction as you go along*" +
                            $"#$b#%*You run your hands around him, gripping his ass as you pull yourself into the depth of his groin, shoving his manhood into the cavern of your throat. A single word escapes his lips*" +
                            $"#$b#Fffffffffffuuuuuuuuuck$l" +
                            $"#$b#%Sam's nails dig into your shoulders as he convulses, climaxing. Hot sticky cum fills your mouth, spewing out and running down your chin. You deposit the stuff into a jar. Sam wipes your face with his sleeve.*" +
                            $"#$b#There's always more where that came from, @. You know where to find me." +
                            $"#split#" +
                            $"Sure, that sounds great! I love it when you wrap your sexy lips around my shaft" +
                            $"#$b#*You pull his pants down below his cute, white butt and give it a squeeze, then fondle his balls to get the blood flowing*" +
                            $"#$b#*This really gets him going and he's standing to attention in no time, with a slight deodorant smell tinging the air and masking his natural smell*" +
                            $"#$b#*You hold on tight to his behind as your head bobs back and forth, coating his cock with your saliva, and wringing the occasional gasp and moan from him*" +
                            $"#$b#*His hands rest lightly on your head, giving you encouragement, and you pick up the pace until his legs start to shake and he holds your head against his crotch hair*" +
                            $"#$b#*He explodes in your mouth and you pull back just in time to avoid having to swallow his precious load, getting the bottle in the way of a white shower to the face*";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Sebastian")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding BJ for Sebastian", LogLevel.Trace);
                        data["BJ"] = $"I could never say no to something like that from someone as attractive as you. I'm already in the palm of your hand figuratively, might as well make it literally, too." +
                            $"#$b#%When you reach down to Sebastian's groin, you find that his erection is already straining against his jeans, struggling to break free. You carefully pull down the zipper and his wood bursts out, free in the open air." +
                            $"#$b#%*Gently but firmly, you massage it, feeling it pulse under your touch. You crouch down and begin to trace your tongue around the shaft, swirling up to the tipe before fully inserting his cock into your warm mouth." +
                            $"#$b#%*As you move your head, you feel Sebastian's hands gingerly snake through your hair. His breathing becomes rapid and shaky.*" +
                            $"#$b#@...whatever you do...don't stop..." +
                            $"#$b#%You increase your tempo, faster and faster until suddenly Sebastian's hands grasp your hair in an intense moment of passion. He half-grunts, half-gasps as he fills you with his warm cum." +
                            $"#$b#%*You think about pulling away, but he holds you there, just for a few seconds, as if savoring the moment. Then he releases you and his cum spills out of your mouth, and you collect it into your jar.*" +
                            $"#$b#You really are something else, @. You have no idea how glad I am to have you.$l" +
                            $"#split#" +
                            $"Have you ever thought about doing this with anyone else? Like, at the same time? I'm sure Abigail or Sam would be down if you ever want to?" +
                            $"#$b#Not that I'm saying this isn't great. You're definitely really skilled...at this; your mouth is heavenly and so warm." +
                            $"#$b#*You and Sebastian spend several minutes locked together, mouth to groin, without only your sucking sounds being heard*" +
                            $"#$b#Here it comes! I'm going to cum in your mouth!" +
                            $"#$b#*You try and keep all of it in your mouth, but a lot of it explodes out of the side of your mouth and leaves you scraping the cum up with the bottle*";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Shane")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding BJ for Shane", LogLevel.Trace);
                        data["BJ"] = $"You know I'll never get over the idea that someone like you could be attracted to a guy like me. You're too good for me, you know.$h" +
                            $"#$b#%*Despite this, Shane unbuckles his belt and lowers his shorts. He looks at you, almost disbelieving, as if he had found himself in some impossible dream." +
                            $"#$b#%*You maintain soft, tender, eye contact as your hand finds itself grasping his cock." +
                            $"#$b#%*Then, slowly, you kneel before him, your gaze not leaving his eyes. You watch as his eyelids flutter shut when you place your mouth around him*" +
                            $"#$b#%*As you start using your tongue, your hands run down the front of his thighs; then upwards, under his shirt and onto his chest. Shane's hands find yours and he holds them there, against his torso*" +
                            $"#$b#%*You pick up your pace, and his grip tightens around your fingers, pinning your palms to him. He seems to want to hold you to him, grounding himself in this reality with you*" +
                            $"#$b#%Then, his eyes snap open, wide and alert. He trembles and you feel his cock pulsate in your mouth as it releases his ecstasy in waves. It's a lot, so you have to swallow some in order to make room." +
                            $"#$b#Holy fuck...@..." +
                            $"#$b#%*It tastes watery and somehow spicy on your tongue, and the remnants make their way into your jar.*" +
                            $"#$b#How did I get so lucky to find myself with you?$l" +
                            $"#split#" +
                            $"Are you joking? I don't know why you would offer that to me, it's not like I deserve it or anything. Are you sure?" +
                            $"#$b#Oh, ok then, just don't expect anything special from me." +
                            $"#$b#*You pull down Shane's jeans and free his member from his underwear, taking a moment to admire it.* " +
                            $"#$b#*He gets a little embarrassed, but when you lunge forward and take the whole thing in your mouth his insecurities vanish and he focusses on the sensations you are giving him*" +
                            $"#$b#*He gets lost in a world consisting solely of you, himself and the wonderful things your tongue and lips are doing to him, and you see a slightly dazed smile form on his face*" +
                            $"#$b#That feel amazing @, I can't believe you can give me such pleasure. I'm going to cum soon - get your bottle ready!";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Pierre")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding BJ for Pierre", LogLevel.Trace);
                        data["BJ"] = "Honestly, when you come into the store to buy seeds from me, I sometimes stare at you, imagining you on your knees, giving me all the pleasure in the world. It's nice to see I was right." +
                            $"#$b#I knew you were secretly into me. Don't worry, Caroline doesn't have to know. We don't do much together these days anyway. Well, not with each other.$s" +
                            $"#$b#%*Pierre fidgets the the button and zipper of his pants before pulling out his dick. It's not that impressive, but nothing to laugh at. It looks slightly sad. " +
                            $"#$b#%*You reach down and begin to preform a familiar back and forth motion. He smiles contently and leans his head back." +
                            $"#$b#%*As you start to increase the rhythm, Pierre immediately jerks his head back, his eyes wide*" +
                            $"#$b#I'm close, @!" +
                            $"#$b#%*You bring out the bottle but he finishes before you can get everything in. Only half of the white material manages to get into the jar*" +
                            $"#$b#Sorry, @. I'm kind of sensitive and over eager. Since I don't get much action with Caroline, I don't exactly have any stamina built up, but we can go again tomorrow?" +
                            $"#split#" +
                            $"What? Who told you I was into that? Does Caroline know...?" +
                            $"#$b#*You quickly calm down Pierre and kneel in front of him, waiting expectantly. He looks around and pulls out his hardening dick, accidentally poking you in the face*" +
                            $"#$b#*You open your mouth and use your tongue to guide it inside, licking the head as you do so and eliciting a small groan of pleasure from him.*" +
                            $"#$b#That's so good @. I had no idea you were such a great little cocksucker. I hope you're ready for this to become a regular thing because Caroline never does this for me." +
                            $"#$b#*It seems that Pierre likes talking dirty, so you lock eyes with him and use your hands to fondle his balls while he gets ready to ejaculate.*" +
                            $"#$b#*At the last minute you pull a bottle out of your bag and start pumping his cock to make sure you get everything out of him.*";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Gunther"))
                    {
                        ModFunctions.Log($"Adding BJ for Gunther", LogLevel.Trace);
                        data["BJ"] = $"If you are looking for books on that subject, I keep them in a private collection." +
                            $"#$b#Oh, a practical demonstration? I would be glad to be of assistance. This library is equipped with the finest tools for furthering your knowldge." +
                            $"#$b#%*He leads you between the shelves into a secluded area, and checks there is no-one following before dropping his pants and underwear*" +
                            $"#$b#You slowly trail your hands down his body as you sink to your knees, causing his body to react by becoming fully erect. He moans a little, and you shush im with a devilish grin*" +
                            $"#$b#%*He looks suitably chastised, so you decide to reward him by taking him entirely into your mouth in one go. It become a game of you seeing how loudly you can make him moan*" +
                            $"#$b#Oh...@...not so strongly! I... can't control myself!$l" +
                            $"#$b#%*It's not long before the librarian digs his hands into your shoulder and spews his cum down your throat*" +
                            $"#$b#%*Fortunately he cums like a firehose, and you manage to bottle a decent amount*";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Gus")) // Iliress dialogue
                    {
                        ModFunctions.Log($"Adding BJ for Gus", LogLevel.Trace);
                        data["BJ"] = "Honestly, @, this is unexpected. I didn't know you thought of me like that. You are an attractive person, so if you're up for it, I am too.$h" +
                            $"#$b#%*Gus undoes his belt and lowers his pants. You reach down and grasp his cock, and begin to move your hand steadily back and forth, but you struggle to get a rhythm going." +
                            $"#$b#%*Gus grabs some truffle oil he keeps nearby for cooking and pours some on your hands as they grip his cock, making your job much easier. As your hand movements quicken, so does his breathing*" +
                            $"#$b#I'm about to...c..c..$l" +
                            $"#$b#%*Before he can complete his word, he starts squirting on your face. You clumsily hold the jar with slippery hands and collect the rest. For some reason, his semen smells citrusy*" +
                            $"#$b#Always feels good to be touched like that.$h";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Kent")) // Iliress dialogue
                    {
                        ModFunctions.Log($"Adding BJ for Kent", LogLevel.Trace);
                        data["BJ"] = "Jodi and I...we haven't been connecting on that level lately. I'll admit, I've been seeing you around town and have fantasized about you approaching me like this." +
                             $"#$b#Didn't expect it to actually happen, though. Do you mind keeping this quiet from my wife? I don't think she'd be so understanding. Life is short, but I don't want to hurt her." +
                             $"#$b#%*Kent unbuckles his belt and removes his dick, already fully erect. He looks at you, a mixture of trepidation and eagerness in his eyes. You take the initiative and gently grasp it*" +
                             $"#$b#You make slow, deliberate movements, looking straight at him. Kent closes his eyes and breathes deeply, his muscular chest rising and falling in line with the movements of your hand." +
                             $"#$b#You kneel down and take him inside your mouth, and he trembles as he feels the soft warmth. As you accelerate your motions, his breathing becomes more and more irregular*" +
                             $"#$b#@...$h" +
                             $"#$b#%*Kent tenses up and holds your head in place, almost forcing you to take the entirety of his load in your mouth. You deposit the substance in your bottle, capping it*" +
                             $"#$b#Sorry...uh...got carried away at the end there. Hope you didn't mind. Stop by if you want to do this again sometime.";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Lewis"))
                    {
                        ModFunctions.Log($"Adding BJ for Lewis", LogLevel.Trace);
                        data["BJ"] = $"Ah, @. This isn't one of the funny games you kids play, is it?$a" +
                            $"#$b#I've seen you walking around town, causing all kinds of problems and flaunting that cute little ass in everyone's faces." +
                            $"#$b#On your knees then, like the cockslut you are - I'm sure this isn't the first time you've done this, and you look so good down there." +
                            $"#$b#%*Lewis undoes his belt, and drops his pants and underwear to his ankles. He looks at you hungrily and gestures impatiently to his crotch*" +
                            $"#$b#%*You hesitantly lean forward, contemplating whether this was a mistake, when Lewis grabs your head and plunges forward until his hair tickles your nose*" +
                            $"#$b#%*You gag a little, but he lets up and you get to work sucking and slurping away, while Lewis looks down at you smugly*" +
                            $"#$b#I bet you get off on this kind of thing, @? Being controlled by an older man and used as a recepticle for their semen? Well here is comes.$h" +
                            $"#$b#%*He explodes inside your mouth, and you choke a little until he lets go of your head and you can dribble his load into your jar, coughing all the time*";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Linus"))// Iliress dialogue
                    {
                        ModFunctions.Log($"Adding BJ for Linus", LogLevel.Trace);
                        data["BJ"] = "Pleasure is but a natural part of life. We are but humans, after all. Society puts too large of a stigma on such a thing." +
                            $"#$b#%*Linus casually lifts his outfit revealing an unsurprising lack of undergarments. He smiles kindly and gestures to his organ." +
                            $"#$b#%*For someone who lives in the mountains he smells surprisingly little, with a nice, manly mush. You shrug, lick your hand, and start to work his member." +
                            $"#$b#%*Linus relaxes to your touch. As you increase your handwork, he makes no indication of losing control, remaining completely calm*" +
                            $"#$b#I would get your bottle ready. I'm going to ejaculate soon." +
                            $"#$b#%*You do as you are told, and Linus releases himself in the glass container, maintaining a calm, almost serene composure. You cap the bottle*" +
                            $"#$b#It's nice to help one another out. Be one with each other as well as nature. Thank you for having such an open mind.";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Marlon"))
                    {
                        ModFunctions.Log($"Adding BJ for Marlon", LogLevel.Trace);
                        data["BJ"] = $"Ah, @. This life often ends unexpectedly, so we learn to take the pleasures where we can. I am grateful for this." +
                            $"#$b#%*Marlon quickly undoes his belt and drops his pants. He has lean but muscular legs, and every part of him is toned*" +
                            $"#$b#%*You lean in eagerly, and take his pleasntly firm cock into your mouth. He gives you gently affirming sounds, and you start pleasuring him intently*" +
                            $"#$b#That's it, @. You show the same dedication to acts of love as you do to protecting us. If this is a battle, I fear I am losing to you.$h" +
                            $"#$b#%*You take this encouragement and renew you attack, licking under his glans, fondling his balls with your hand, and giving everything to it*" +
                            $"#$b#%*He tenses up and you suck firmly, tipping him over the edge, earning his hot cum as a reward*" +
                            $"#$b#That was amazing, @. You have mastered this, and me, as well.";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Morris"))// Iliress dialogue
                    {
                        ModFunctions.Log($"Adding BJ for Morris", LogLevel.Trace);
                        data["BJ"] = "Okay, fine, but wear this JojaCorp uniform. It's the only way I can get off, and after my last store they won't let me do this with an employee...$s" +
                            $"#$b#%*You put on the blue outfit and Morris takes out his chode of a cock. He looks at you, but doesn't let you actually touch him." +
                            $"#$b#%*Using his thumb and forefinger, because his hand is too big, he begins to pleasure himself. His hand speeds up*" +
                            $"#$b#J..j...joja..." +
                            $"#$b#%*He finishes onto the floor and hands you a squeegee and a pan to clean it up yourself*" +
                            $"#$b#Have a good day. Cleanup in aisle 2, %rival. Er...@.$a";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Mister Qi"))// Iliress dialogue
                    {
                        ModFunctions.Log($"Adding BJ for Mister Qi", LogLevel.Trace);
                        data["BJ"] = $"$1 5948Q1#" + // First time switch
                            $"I knew you'd come to me for that one day. Your curiosity is very predictable. Be warned, @, this will not be like your experience you've had with others in town." +
                            $"#$b#%*Mr. Qi lifts up his shirt and begins to lower his waistband. But where any genitalia would be is instead a blinding light." +
                            $"#$b#%*Once your eyes adjust you see what may as well be a portal to some otherwordly beyond. You reach your hand towards it, checking his face to see if this is safe, and he nods." +
                            $"#$b#%*When your hand touches the vortex, visions and sensations you've never imagined and could never fully describe crash into your mind. You can see time, you can smell space." +
                            $"#$b#%*It's as if you've been brought into the fourth, fifth, sixth and seventh dimension all at once. You feel a great understanding of life itself*" +
                            $"#$b#That will do." +
                            $"#$b#%*As soon as it had started it was gone. You are left with spots in your vision and after images of some immense epiphany that is slowly fading from your mind." +
                            $"#$b#%*In your hand is a bottle containing what is possibly a rip in space-time itself. You do not recall ever taking out the container*" +
                            $"#$b#See you around, my dear @." +
                            $"#$e#" + // Later versions.
                            $"second time with Qi";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Willy"))
                    {
                        ModFunctions.Log($"Adding BJ for Willy", LogLevel.Trace);
                        data["BJ"] = $"The sea may be a beautiful mistress, but she does not do much when it comes to physical gratification. I'd appreciate you helping an old sailor out.$h" +
                            $"#$b#%Willy guides your hand as it snakes its way down his waistband and into his crotch hair. Though you cannot see through his pants, you feel his fleshy, erect cock; warm in your hands*" +
                            $"#$b#%*You use your other hand to pull his pants down and give yourself some wiggle room. You work his member back and forth, then you bend down and swirl your tongue around his tip*" +
                            $"#$b#%*You inhale a salty scent, like a warm sea breeze. You slurp his dick into your mouth, and a guttural grunt escapes Willy. His grunts transform into sharp gasps as your tempo increases." +
                            $"#$b#Prepare yerself, now." +
                            $"#$b#%Appreciative of his warning, you release your suction and grab the bottle just in time to catch a decent amount cum inside it. You cap the bottle and wipe your mouth.*" +
                            $"#$b#Aye, it feels good to have a proper release. Come back soon, I could use it again.$l";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Wizard"))
                    {
                        ModFunctions.Log($"Adding BJ for Wizard", LogLevel.Trace);
                        data["BJ"] = //$"$1 5948W1#" + // First time switch.
                            $"I am not used to the pleasures of the mortal world. It is a luxury I rarely partake in, as it keeps me distracted from communicating with the Elements. But...if you insist...I guess I could indulge myself." +
                            $"#$b#%The Wizard parts and lifts his robes, where his phallus hangs, flaccid. You gingerly reach forward and caress it, and it twitches under your touch. Then, in rising waves, it raises and extends." +
                            $"#$b#%*You massage it, increasing your firmness each stroke. When you bend down to suck his dick, something unexpected happens." +
                            $"#$b#%*You instantly find yourself connected to the Wizard's mind and consciousness, as if you had just plugged into his brain." +
                            $"#$b#%*You feel each sensation that he is experiencing at that moment. You can see yourself performing the movements, but instead of feeling it from your perspective, you are watching through his mind. You feel every flick of your tongue. Through his heavy breathing you hear the Wizard speak, as if from a distance...*" +
                            $"#$b#@...what in the...?" +
                            $"#$b#%Suddenly a massive tsunami of pleasure crashes into him, and through him, you. Next thing you know you are kneeling on the floor, shocked and stunned, with his cum on your face and yours in your pants." +
                            $"#$b#%*With a shaking hand you reach up to wipe off his climax, only to find it's not the usual white stuff. It's a prismatic liquid, so thin and light that you aren't sure if it's not a trick of the light.*" +
                            $"#$b#....Honestly, @, I did not predict this. But I am more than willing to try again later. To further....study this phenomenon." +
                            $"#$b#%*He mutters a spell, and a bottle appears in your hand*" +
                            //$"#$e#" + // Every other time.
                            $"#split#" +
                            $"I am but a scholar in search of knowledge. Please, let us begin our 'study session' of this symbiosis." +
                            $"#$b#%The Wizard moves to arrange his robes, but you get there first, diving into the cloth and searching hungrily for his cock. It only had time to get halfway erect before you had syphoned it into your mouth*" +
                            $"#$b#%*Then, the experience begins. You are melded into his mind, his body. Once again, you can perceive every minute motion your body makes." +
                            $"#$b#%*You feel your tongue making swirling motions around the base of the shaft, corkscrewing to the tip." +
                            $"#$b#%*You feel yourself suck the end, delicately, but intensely. You and he both sense the amount of pleasure rising, building, surging through your beings. It begins to peak.*" +
                            $"#$b#Here..it comes…$h" +
                            $"#$b#%Just as intense as the last time, you are plunged into a world of extreme pleasure. You are left breathless. You did not bother with a bottle before the climax, as the time it would take to bring it out was time wasted." +
                            $"#$b#%*You scrape the flowy, silky, colorful ejaculate into the container now. It takes time as you are still trembling. One of these days though, you will remember to bring a spare change of pants for yourself.*" +
                            $"#$b#I'm pleased to share this with you. It's an...odd connection. It does, however, require further examination. I know I will see you soon again, @.$l";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Dwarf")) // This isn't coming up for some reason.
                    {
                        ModFunctions.Log($"Adding BJ for Dwarf", LogLevel.Trace);
                        data["BJ"] = $"Most Dwarves never get this intimate with each other..." +
                            $"#$b#However, it has been a very long time since I've felt as close to someone as I do to you. I guess it's only natural to want to share our essence." +
                            $"#$b#%*You help the dwarf remove their garments, and a warm, earthy smell fills your nostrils as you lean in to their crotch*" +
                            $"#$b#%*They are already standing fully erect, and your warm, wet mouth surrounds them, using gentle sucking to start eliciting moans of pleasure*" +
                            $"#$b#You are so very good at this, @. I'm sorry, but I won't be able to last long." +
                            $"#$b#%*After a few minutes of gentle teasing and sucking, Dwarf tenses up start cumming, filling your mouth completely with their essence.#bIt has a slightly spiced taste*" +
                            $"#$b#Thank you, @. That was wonderful.$l";
                    }

                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Victor"))
                    {
                        ModFunctions.Log($"Adding BJ for Victor", LogLevel.Trace);
                        data["milk_start"] = $"That sounds wonderful, @. I think you are very attractive, and I often find myself fantasising about you...Here, let me help you." +
                            $"#$b#%*He unzips his pants, and frees his rather large cock, surprising you. A moment passes, and you come back to your senses, reaching out with a hand and starting to stroke his length*" +
                            $"#$b#%*You give the tip a tender kiss, and then lean underneath and lick the length of it, ending by engulfing the entire head with your mouth. Victor groans from this teasing stimulation, and you take that as a sign to continue*" +
                            $"#$b#@, This is better than anything I had imagined. That you are in front of me, performing fellatio, is a dream come true.$l" +
                            $"#$b#%*You slowly slide further down his cock, inch by inch, ending with your nose in his crotch hairs. Victor grunts, and you slowly slide back until just the tip is inside your mouth*" +
                            $"#$b#%*You repeat this, speeding up slightly every time, settling into a rhythm that causes Victor to become more vocal every time. Before too long he is on the edge, and you back off, looking up at him and not moving. He jerks his hips and you tut at him*" +
                            $"#$b#Please, @, I'm so close. I NEED to cum.$s" +
                            $"#$b#%*You give him a little lick, and this almost causes him to lose control. Decicing that he's had enough, you lean forward and start strongly sucking. This pushes him far over the edge, and he explodes into your mouth, filling you up with his hot cum*" +
                            $"#$b#%*As his eyes roll back into his head, you spit his cum into a jar and seal it up. He looks down at you with a dazed smile, a pulls up his pants*" +
                            $"#$b#Thankyou @. That was incredible.";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Andy"))
                    {
                        ModFunctions.Log($"Adding BJ for Andy", LogLevel.Trace);
                        data["milk_start"] = $"Damn right I'd love a blowjob! I know everyone looks down on me, but I have needs the same as everyone else.$s" +
                            $"#$b#He undoes his overalls and flops them down. He fumbles with the buttons, but you reach over and take his hands in yours momentarily before undoing the clasps*" +
                            $"#$b#Thank you @, I'm a little flustered. It's not every day someone like yourself does this for someone like me. I really appreciate this.$h" +
                            $"#$b#%*As his penis comes free, you reach forward and lightly trace you fingers up and down it, breathing life into Andy's sad little erection*" +
                            $"%*In no time you have coaxed it to life, and it stands proudly in front of you. You bend down and inhale Andy's slightly unwashed scent, then lick all around his head, coating it in your saliva*" +
                            $"#$b#Oh @, this is heavenly. I don't think I'll be able to last long, but please keep going.$h" +
                            $"#$b#%*You obey his wishes, and start seekig out his weak spots, licking, sucking and stroking him until you feel him about to blow. As he comes, you angle his penis so it sprays straight into the jar, and keep stroking him through his orgasm*" +
                            $"#$b#That...was...amazing, @. I definitely misjudged you when you first came here. You have made me a true friend.";
                    }
                    //if (asset.Name.IsEquivalentTo("Characters/Dialogue/Martin"))
                    //{
                    //    data["milk_start"] = $"";
                    //    
                    //}
                }

                if (!data.ContainsKey("eat_out"))
                {
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Abigail")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding eat_out for Abigail", LogLevel.Trace);
                        data["eat_out"] = $"You know I’m always down for an adventure...wanna adventure down on me?" +
                                            $"#$b#%You sweep in to kiss her tenderly, only to be met with ferocity and passion. She grabs your shirt and pulls you in, grabbing your hand and ushering it down the front of her pants. " +
                                            $"#$b#%Your fingers meet her soft, warm wetness. Gently, you stroke her clit with your middle finger, coating it in her juices." +
                                            $"#$b#%She leans backwards against a wall, pulling you with her. You crouch down and tear off her pants and underwear; using the wall as leverage, she wraps her legs around your head." +
                                            $"#$b#%Pressed between her thighs, you submerge yourself into her pussy. Despite the low light you can see that her hair there is purple too." +
                                            $"#$b#%Holding her hips agains the wall with your hands, you lick up her juices from between her folds, before arriving at her most sensitive area." +
                                            $"#$b#%There, you trace your tongue in a variety of motions. You hear her gasp and she pulls on your hair as if it’s her tether to earth." +
                                            $"#$b#Right there!Right there! Like that!$l" +
                                            $"#$b#%You repeat the motion, in the exact same tempo, and she writhes and bucks under your touch. The more you continue, the more vice-like her thighs become." +
                                            $"#$b#@!" +
                                            $"#$b#%Suddenly she all but screams as she clenches around you. A literal wave of warm liquid hits your face, soaking you as Abby moans and stares wide-eyed at nothing in particular." +
                                            $"#$b#%You lower her to the ground, and she slowly retrieves her pants as you wipe your face." +
                                            $"#$b#Where...the FUCK...did you learn that?";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Emily"))
                    {
                        ModFunctions.Log($"Adding eat_out for Emily", LogLevel.Trace);
                        data["eat_out"] = $"I’d love to feel our energies intertwine. Come, let’s explore them." +
                                            $"#$b#%In the blink of an eye Emily is sitting on a nearby table, smiling and lifting her dress. When you step towards her, she opens her legs and her smile turns coy." +
                                            $"#$b#%You crouch and duck into the blue hair of her privates. As you explore her soft folds she places a hand on your head, and affectionately strokes your hair." +
                                            $"#$b#%The affection turns into passion when you find her clit and make a variety of motions. Her nails comb your scalp." +
                                            $"#$b#%When you start a quick looping motion, her hand stops suddenly at the back of your head and she grasps a fistful of your hair." +
                                            $"#$b#Yes, that’s it. Keep going!" +
                                            $"#$b#%You loop around her clit, making a circle with your tongue but only adding pressure when you feel her grip tighten. You keep doing that motion, over and over, driving her closer to the edge." +
                                            $"#$b#%Emily squirms and begins to moan, louder and louder until she yelps and releases her pleasure. It washes over your face and down your chin." +
                                            $"#$b#%She smiles contently, her eyes closed. Then, after a moment, scoots off the table and smooths down her dress." +
                                            $"#$b#Wow! I really feel like my Sacral Chakra is aligned. I sense a good balance within me, and it's all down to you.";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Haley"))
                    {
                        ModFunctions.Log($"Adding eat_out for Haley", LogLevel.Trace);
                        data["eat_out"] = $"Oh, @, that sounds just about perfect right now. Come on then." +
                                            $"#$b#%You crouch down to lift Haley’s skirt, but she pushes you back and instructs you to lie down. You do as you’re told and lie on your back." +
                                            $"#$b#%Haley grins and climbs on your chest, then lifts her skirt and shuffles until she’s almost sitting on your chin. Her skirt falls and you’re in a blue tinted shadow." +
                                            $"#$b#%She shifts and you find yourself with a mouthful of her pussy, her thighs embracing your cheeks." +
                                            $"#$b#%You start to navigate your way in the dark and realize you found it when Haley moans aloud into the open air." +
                                            $"#$b#%You reach up and grab her ass, pulling her more onto you, and you more into her. She grinds and rocks on your face, leaning into every motion of your tongue." +
                                            $"#$b#%Her moans become louder and higher. Just when you think she’s close, she shifts again." +
                                            $"#$b#You don’t think you’d get out of this without something from me, do you ?" +
                                            $"#$b#%She has turned around, her ass now facing you. You’re still unable to see through her skirt, but you feel her tug at your pants." +
                                            $"#$b#%You’re about to resume your work when open air meets your groin, followed swiftly by the warmth of Haley’s own tongue." +
                                            $"#$b#%It’s now a race to see who can make the other cum first. You plunge your face back into her womanhood, determined to win." +
                                            $"#$b#%It’s difficult to concentrate, because Haley’s skill with her tongue matches and might even surpass your own." +
                                            $"#$b#%Wading through the fog she has created in your mind, you manage to resume the rhythm from before. She squeals and you feel her voice resonate through your genitals." +
                                            $"#$b#%You let a groan of your own escape your lips. Suddenly, her thighs clamp around your face, her hips dig into your neck, and she lets out an intense moan." +
                                            $"#$b#%She doesn't relent, and no sooner has your face been covered in her cum has she gone right back to work on you." +
                                            $"#$b#%Her face is soon covered as well. She rolls off of your body, lying on her back beside you, panting heavily." +
                                            $"#$b#Okay, I’ll admit it, you’re good. I thought I was in over my head for a second. *Giggle * No pun intended.";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Leah"))
                    {
                        ModFunctions.Log($"Adding eat_out for Leah", LogLevel.Trace);
                        data["eat_out"] = $"You know, when I was dating Kel they’d never go down on me. You’re such a breath of fresh air, @." +
                                            $"#$b#%Leah cocks her head at you, giving you a seductive look as she unclips her suspenders from her jeans. She lowers them to her ankles, and steps out of them while walking towards you." +
                                            $"#$b#%Her lips meet yours, smoothly and tenderly. You hold her to you and she puts a palm on your chest, gently guiding you to lie down on the ground." +
                                            $"#$b#%As she crawls up your body your hands run across her smooth, soft thighs. She settles on your face, adjusting to line up her warm, wet pussy with your mouth." +
                                            $"#$b#%Your hand explores her thighs and waist, while your tongue explores her privates, before settling on her ass. You give it a squeeze, finding it firm and taut." +
                                            $"#$b#%Leah begins to rock back and forth, grinding on your face and coating it in her warm succulent juices." +
                                            $"#$b#%You have to hold her still to be able to properly maintain contact with her clit, but once you find her rhythm she squirms above you even more." +
                                            $"#$b#%Your hands squeeze her thighs and firmly secure her to you." +
                                            $"#$b#Fuck fuck fuck fuuggllerrggpharlgarnngh" +
                                            $"#$b#%Her words get mingled and garbled as she completely loses control. She stops writhing around and freezes, every tendon and tissue in her body tensing and seizing." +
                                            $"#$b#%Her hands grasp the hair above your forehead and she continues to scream sounds to the open air, none of them actually forming coherant words despite her best efforts." +
                                            $"#$b#%Her cum bursts out of her, coating your face and soaking your hair. In contrast to her previous writhing, she is now perfectly still, staring up at the sky." +
                                            $"#$b#%The only movement is in her heaving breasts while she tries to regain a sense of composure. Then she leans off to the side and flops on the ground, completely drained." +
                                            $"#$b#Oh Yoba, I missed...how good that feels. If you could just..toss me my pants. I’m gonna...need some time...to recover here.";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Maru"))
                    {
                        ModFunctions.Log($"Adding eat_out for Maru", LogLevel.Trace);
                        data["eat_out"] = $"I mean, I’ve invented something that can do this for me. It isn’t as good as the real thing, however. No emotional connection, you know?" +
                                            $"#$b#%Maru unbuckles her overalls and shimmies out of them, pushing her panties down, too. For good measure, she also takes off her top and bra, standing before you, fully nude and confident." +
                                            $"#$b#%You stride towards her, and she rushes to meet your embrace. You feel every curve of her against your body." +
                                            $"#$b#%Your lips touch hers and your hands run down her back, across her ass, then around again to her front. You cup and tease her breasts, and she whimpers under your contact." +
                                            $"#$b#%She lies down on the floor and pulls you with her. Her hands usher your head towards her pussy, and her legs straddle your torso. You begin to lick and tease her, and she writhes underneath you." +
                                            $"#$b#%When you find her sweet spot, her hips buck. You wrap your arms around and under her thighs and continue, pausing occassionally to lap up her wetness." +
                                            $"#$b#No...focus on the first thing...no pauses...don’t stop..." +
                                            $"#$b#%You do as you’re told, and turn all your attention into licking her clit. Maru’s hands lay flat on the ground, pressing down as her pelvis jerks upward." +
                                            $"#$b#%You have to hold her down to continue your precise pattern. She releases a cry of pleasure and you find your face coated in her cum." +
                                            $"#$b#%You pull back and Maru grabs a cloth from the pocket of her overalls on the floor and uses a shaky hand to wipe her climax off your face. She smiles tenderly at you." +
                                            $"#$b#You’re a lot better than the machine. However, I would love to study your motions. Just to maybe get more ideas for an upgrade...come by again later, okay?";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Penny"))
                    {
                        ModFunctions.Log($"Adding eat_out for Penny", LogLevel.Trace);
                        data["eat_out"] = $"Oh, @, I don’t know...are you sure? I’m a little self-conscious about that area, but if you really want to, then okay." +
                                            $"#$b#%You hold Penny’s hands and gently guide her to a chair. You hike up her skirt and remove her panties in a slow, steady motion. You don’t want to be too forceful and rush this." +
                                            $"#$b#%You spread her legs with your hands and check Penny’s face to wordlessly ask permission. She nervously exhales and nods to you, giving you the go ahead. With her consent, you lean forwards." +
                                            $"#$b#%Her ginger pubic hair is wild, contrasting her usual primness. You dive right into the briar patch and lick her labia, swirling around the entrance to her vagina." +
                                            $"#$b#%Penny releases a sigh of contentment, and gently strokes the back of your head. You dip your tongue inside her and swirl it around." +
                                            $"#$b#%Her sighs turn to gasps; then when you head north, to give her clit attention, her gasps turn into moans." +
                                            $"#$b#Oh, @, how are you doing this to me?" +
                                            $"#$b#%The more you work her center, the louder her moans become. You never would have thought such a quiet, reserved girl could make such loud, untamed sounds." +
                                            $"#$b#%Her cries reverberate throughout the area, bouncing off the walls. The hand that was once gently stroking your hair is now tangled in it and grabbing it like a lifeline." +
                                            $"#$b#%She all but screams to the sky as a wave of clear fluid glazes your face. You lap up the fluid that remains coateing her, and then pull out and face her while wiping your mouth." +
                                            $"#$b#I..I..wow. You made me experience parts of my sexuality I didn’t even know were there. I’m going to have to read up on this...";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Caroline"))
                    {
                        ModFunctions.Log($"Adding eat_out for Caroline", LogLevel.Trace);
                        data["eat_out"] = $"Pierre refuses to go down on me. I could really use some attention down there, it’s been too long." +
                                            $"#$b#%Caroline guides you to a counter and hops up on it. You help her remove her pants, and gingerly pull down her underwear." +
                                            $"#$b#%Her panties are nothing special, showing that she hasn’t been expecting much action recently. She eagerly spreads her legs for you, revealing a soft pink pussy in a sea of green." +
                                            $"#$b#%You lean in and lick her womanhood gently, and begin exploring her warm, inviting nether regions with your tongue.You leave no crevice, no corner left alone." +
                                            $"#$b#%As you continue, she gets wetter and wetter until you don’t know where your saliva ends and her secretions begin. " +
                                            $"Then you find your way to her clit, gently teasing it with the tip of your tongue. " +
                                            $"#$b#%She lets out a low moan that resonates through your being." +
                                            $"#$b#Mmmm…@...don’t stop" +
                                            $"#$b#%Caroline throws her head back and closes her eyes in ecstasy when you tweak the rhythm and intensity. You found her medium, and refuse to change your pattern." +
                                            $"#$b#%She begins breathing harder and harder until she’s gasping for breath.In your peripheral vision you see her hands grasp the counter so hard her knuckles go white." +
                                            $"#$b#%She pulsates and quivers and lets out a choking gasp as warm fluid coats you and trails down your chin." +
                                            $"#$b#%She stays still for a few moments, recalibrating, and then hops down from the counter on shaky legs.*" +
                                            $"#$b#Th - th - thanks...I - I r - really n - needed that.Just need to c -catch my b-breath here.";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Jodi"))
                    {
                        ModFunctions.Log($"Adding eat_out for Jodi", LogLevel.Trace);
                        data["eat_out"] = $"Oh, it’s been too long since someone has pleasured me. Kent’s been dealing with his own problems lately, not much time for my needs. You wouldn’t mind?" +
                                            $"#$b#%To answer her question, you wordlessly guide her to the counter and hoist her up on top of it. You slide off both her jeans and panties, and discard them on the floor." +
                                            $"#$b#%Teasingly, you run your tongue up her thighs, circling but not yet touching her pussy. She twitches with anticipation every time you come near. Eventually..." +
                                            $"#$b#Oh, fuck it." +
                                            $"#$b#%Jodi grabs the back of your head and pushes your head right in. You’re surprised yet aroused by her sudden assertiveness. Apparently it HAS been too long." +
                                            $"#$b#%You place your hands on the outside of her thighs and begin to swirl your tongue around the opening, dipping in every now and then, before making your way upwards to her clit, adding pressure all the way there." +
                                            $"#$b#%As soon as you taste the round nub, Jodi gasps and leans backwards, bucking her hips. You begin to give it upward flicks of your tongue, and you feel her tense with each flick." +
                                            $"#$b#%Then you start a rapid but light up and down motion, and her kness find their way around your neck and lock behind you." +
                                            $"#$b#%You feel each muscle in her legs clench..." +
                                            $"#$b#Oh my goodness!!!" +
                                            $"#$b#%Shaking, Jodi releases her pleasure into your mouth. It spills down your chin and onto the floor. She doesn’t stop quivering, and takes a moment before easing herself onto the ground." +
                                            $"#$b#%Then she stumbles over and grabs her pants and underwear with shaking hands." +
                                            $"#$b#I’m so sorry for being so vulgar earlier; I don’t usually curse. I just really, really needed that. Now if you excuse me, I—um—have to go mop this up…";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Marnie"))
                    {
                        ModFunctions.Log($"Adding eat_out for Marnie", LogLevel.Trace);
                        data["eat_out"] = $"I never thought someone as young as you would be attracted to someone my age. Lewis does his best, but I don’t think his mind is exactly with us when we’re together." +
                                            $"#$b#I’d like to see what you have to offer." +
                                            $"#$b#%Marnie walks over to a nearby chair, hoists up her dress, and sits down. She spreads her thick thighs and presents her pussy." +
                                            $"#$b#%She looks at you with a mix of caution and anticipation in her eyes. You stride forward and crouch down in front of her then dive right in." +
                                            $"#$b#%She lets out a soft gasp and lets go of the hem of her dress so it falls over and covers your head. You continue your work now shrouded in darkness." +
                                            $"#$b#%Spreading her thighs with your hands, you explore every flap and fold. Your tongue flicks her clit, and Marnie’s gasps become sharper." +
                                            $"#$b#%You slip a finger in and curl it upwards and find a swelled bumpy patch about an inch inwards." +
                                            $"#$b#%When you tickle it, she lets out a cry of pleasure. Her thighs compress around you and you’re almost trapped; borderline smothered." +
                                            $"#$b#%You continue to simultaneously work her clit and the patch inside her and the force from her thighs becomes stronger and stronger." +
                                            $"#$b#@, I’m...I’m..!" +
                                            $"#$b#%Just when you think you’ll pass out from lack of oxygen, a stream of her cum blasts you in the face." +
                                            $"#$b#%You pull out from underneath her dress and fall backwards on the floor; soaked, breathing hard, and gasping for air." +
                                            $"#$b#%After the spots in your eyes clear up, you glance at Marnie. She’s looking at you with concern. You give her a smile to let her know you’re okay." +
                                            $"#$b#Sorry, I didn’t know it was possible for oral sex to feel that good. I guess Lewis didn’t set the bar very high…";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Robin"))
                    {
                        ModFunctions.Log($"Adding eat_out for Robin", LogLevel.Trace);
                        data["eat_out"] = $"Demetrius gets too wrapped up in the technicalities of it all; he never really just enjoys the moment. I'm game if you are?" +
                                            $"#$b#%Robin hops up on a counter and beckons you. You stride over and unbutton her jeans, and rip them and her panties right off, tossing them aside." +
                                            $"#$b#%Robin grabs your head by the hair and guides you down. You find yourself right in the middle of her delicious pussy, and she’s already soaking." +
                                            $"#$b#%Your tongue explores her, getting a taste of every nook and cranny, every fold. Robin sighs with pleasure. You find your way to her clit, and begin to tease it, flicking it with your tongue." +
                                            $"#$b#%She moans and flops backwards on the counter, going from sitting to lying down, legs akimbo. Her hands move to her chest and she massages her own breasts while you work." +
                                            $"#$b#%You slightly alter your rhythm, adding just a hint more pressure, and her moans become louder, almost evolving into screams." +
                                            $"#$b#Yes...yes...YES!!" +
                                            $"#$b#%Robin shouts her continuous affirmations while you maintain that exact motion and pattern. Her hands are pounding, slapping the counter, her legs squeezing you tightly as she climaxes under your attention." +
                                            $"#$b#%A stream of her fluids hit your face like a hose, making you start. Robin remains lying down, panting and smiling softly. After a moment she props herself up on her elbows and grins at you. " +
                                            $"#$b#Damn, you sure are a treat. Mind fetching my pants for me?";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Pam"))
                    {
                        ModFunctions.Log($"Adding eat_out for Pam", LogLevel.Trace);
                        data["eat_out"] = $"You really want to do that to me? I don’t get a lot of offers like that. Sure kid, go to town." +
                                            $"#$b#%Pam hobbles over to a chair and pulls down her sweatpants. She lifts up her gut and spreads her legs for you. As you get close to her crotch, an odor hits your nose." +
                                            $"#$b#%It’s a mixture of sweat, musk, and...is that booze? You have no clue how her pussy manages to smell like alcohol but dammit, your momma didn’t raise no quitter so you hold your breath and dive in." +
                                            $"#$b#%It somehow tastes even more rank than it smells. It feels like months of working with manure at the farm have managed to strengthen your nose and stomach for this moment." +
                                            $"#$b#%With her belly almost resting atop your head, you search for her clit through her unkempt nether hair. Once you find it, you lick it lightly and slowly. Pam responds by groaning with pleasure." +
                                            $"#$b#Holy shit, @. That’s the ticket." +
                                            $"#$b#%Happy that you managed to find her rhythm preference on the first try, you continue in that fashion. Pam groans louder and louder." +
                                            $"#$b#%Suddenly streams of clear liquid ebb their way out of her and coat you. It’s over. You retreat from her depths and sit on the ground." +
                                            $"#$b#Damn, that was a treat. You gotta do that again sometime, ya hear?";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Sandy"))
                    {
                        ModFunctions.Log($"Adding eat_out for Sandy", LogLevel.Trace);
                        data["eat_out"] = $"The desert may be dry, but I’m sure not. And no, my name is not indicative of anything, either." +
                                            $"#$b#%Sandy saunters over to her counter, casually pulling off her dress and tossing it aside. It appears that she had no underwear or bra underneath the blue gown." +
                                            $"#$b#%Her breasts jiggle as she hops backwards to a sitting position. She leans back on her elbows, tosses her hair, and spreads her legs for you. As you get closer you see that Sandy was true to her word." +
                                            $"#$b#%She’s definitely neither dry nor sandy; her pussy is shimmering wet. You’re not sure if that’s from arousal or wearing no panties in the heat of the desert, but you dive in anyway." +
                                            $"#$b#%Her juices taste salty and a bit tangy; you tongue every inch of her nonetheless. Sandy grabs your head and pulls you upwards and kisses you, tasting her own pleasure in the process." +
                                            $"#$b#Hmm, tasty. I hope you're enjoying the flavour as much as I do." +
                                            $"#$b#%She licks your mouth before inserting her own tongue inside, grazing your teeth. She pulls away and guides you back to her groin. You’re a bit thrown off by this, and have to remember what you were doing before." +
                                            $"#$b#%It doesn’t take too long to regain your senses and find a good rhythm on her clit. Sandy moans and brushes her hair back with her hand while you continue, and her moans increase in volume and frequency." +
                                            $"#$b#Oh, @! Fuck! Right there!" +
                                            $"#$b#%One bonus of her shop being mostly empty: total privacy. Sandy loses all inhibition as she moans and screams your name. Her beautiful breasts heave with every breath, every pant." +
                                            $"#$b#%You feel the muscles in her thighs contract as a warm clear liquid bursts out and covers you. You step back, leaving Sandy lying down, propped up on her elbows. She grins at you." +
                                            $"#$b#There’s some cloth in the drawers there if you want to clean up...better hand me one too. I think I might just spend the rest of the day in the shop naked; I’m too warm for clothes.";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Evelyn"))
                    {
                        ModFunctions.Log($"Adding eat_out for Evelyn", LogLevel.Trace);
                        data["eat_out"] = $"Oh honey, I'm flattered, but I don't think you I work down there like I used to." +
                            $"#$b#%You assure Evelyn that you're willing to try, and she blushes heavily, looking around for somewhere more private" +
                            $"#$b#%After a moments pause to remove her loose underwear, you sit her down on a chair and gently open her stick-thin legs" +
                            $"#$b#%It seems that Evelyn's concerns were valid, and she is as dry as the grave down there. You carefully seperate her labia, and load your tongue up with spit" +
                            $"#$b#%You use your tongue to stroke all the way from the bottom of her lips to her wrinkled hood, and Evelyn lets out a full body shudder" +
                            $"#$b#@, I haven't felt that sensation in DECADES! You are an angel for bringing me such a heavenly time!" +
                            $"#$b#%You start using you tongue to gently circle her clitoris, careful not to touch it too aggresively in case she faints, and slowly build up the intensity of your motions" +
                            $"#$b#%Evelyn starts stroking your hair absentmindedly, and you are pleasantly surprised to find your saliva taking on a different flavour" +
                            $"#$b#Oh my heavens! I'm...cumming!@l" +
                            $"#$b#%Evelyn tenses up, going completely rigid, and then lays back in her chair unmoving. After a moment, she stirs, and beams at you" +
                            $"#$b#@, you are simply delightful.";
                    }

                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Sophia"))
                    {
                        ModFunctions.Log($"Adding eat_out for Sophia", LogLevel.Trace);
                        data["eat_out"] = "Y-you’d really want to do that? For me? Okay…" +
                                            $"#$b#%Sophia shyly undoes her overalls and shimmies out of them, leaving them on the floor. You approach her, gently pick up her small frame, and place her on a table" +
                                            $"#$b#%You slide down her lacy pink panties and toss them aside. Sophia blushes at her vulnerable state. It takes a little physical coaxing from you but she cautiously opens her legs" +
                                            $"#$b#%You place a small but tender kiss on her lips, hoping to calming her nerves, but a deep blush spreads on her cheeks" +
                                            $"#$b#%You race the blush down her body, kissing her all the way, and she squirms adorably. You pause just below her waist, and give her a moment to prepare herself before continuing" +
                                            $"#$b#Oh, @, please don't make me wait any longer. I'm...ready for you." +
                                            $"#$b#%You take the cue, starting at the base of her slit and slowly licking your way up, stopping just shy of her clit. She mewls in anticipation, and you use the tip of your tongue to rub her clit" +
                                            $"#$b#%You spend a few moments gently coaxing it out of it's hood, and Sofia writhes above you, gripping the table to try and control the sensations coursing through her petite frame" +
                                            $"#$b#Oh, @, this is too much!" +
                                            $"#$b#%Sophia shakes and whimpers under each oscillation of your tongue, clenching her eyes shut. She uses one hand to cover her mouth, trying to stifle the moans, but they still manage to slip through her fingers, audible in the open air" +
                                            $"#$b#%Sophia shudders and clenches her legs together tightly, trying to stop you from overloading her senses as she cums hard. Your chin suddenly becomes very wet as Sophia bathes you in her climax" +
                                            $"#$b#%As she slowly recovers, and relaxes the grip of her trembling legs, you patiently lick her clean with your tongue" +
                                            $"#$b#%When you are done you place a gentle kiss on her forehead, eliciting a squeak of surprise, bringing her back from the world she was lost in" +
                                            $"#$b#@...I don’t know what just happened there. I didn't think it would be so overwhelming, but I kind of want to do it again sometime? Is that okay?";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Olivia"))
                    {
                        ModFunctions.Log($"Adding eat_out for Olivia", LogLevel.Trace);
                        data["eat_out"] = $"You want to eat me out? Be my guest, sweetie. Show me what you got." +
                                            $"#$b#%*Olivia gulps down the rest of her wine and sets the glass down. She looks at you with a sense of challenge in her eyes. She may be a widow, but she’s no spinster." +
                                            $"#$b#%Instead of lifting up her red dress, she unzips the back and lets the whole thing drop to the floor, revealing expensive black lace lingerie underneath." +
                                            $"#$b#%She puts one hand on her hip and beckons you with the other. You charge towards her, your lips clashing with hers; you lift her up and plant her on a counter." +
                                            $"#$b#%As you move to kiss her neck, she reaches behind her back and unhooks her bra. Your lips move to her breasts and you give her nips a little suck." +
                                            $"#$b#%She suppresses a whimper, then you continue your journey south and remove her dark panties with your teeth, revealing a smooth, clean shaven pussy." +
                                            $"#$b#%Olivia wraps her legs around your neck and pulls you in; in less than a second, you find yourself in her warm, silky smooth folds." +
                                            $"#$b#%You lick her clit every which way, every pattern, every frequency you can think of, but Olivia seems to remain pleasantly content at most." +
                                            $"#$b#%It is only when you start alternating between two particular sets of motions that she shows a change in her demeanor. Her eyes widen, she inhales a very sharp gasp, and her legs tighten around you." +
                                            $"#$b#For the love of all things holy and unholy don’t you dare stop doing that." +
                                            $"#$b#%You continue, careful not to change a single thing. You dare not mess up the rhythm and lose her satisfaction." +
                                            $"#$b#%One trip up, one lost beat, and you’re afraid all the pleasure you’ve built up in her will become undone and revoked." +
                                            $"#$b#%You concentrate so hard you barely notice that her breathing has become erratic, and her legs have clenched even more tightly around you." +
                                            $"#$b#%It isn’t until her juices are hitting you in the face that you know you’ve done your job right. Olivia gracefully eases herself down on the floor and goes to retrieve her clothing." +
                                            $"#$b#Hmmm..not bad, sweetie. But you should definitely stop by again sometime. For practice, I mean.";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Susan"))
                    {
                        ModFunctions.Log($"Adding eat_out for Susan", LogLevel.Trace);
                        data["eat_out"] = $"People tend to think that living in the mountains means I'm some kind of nun, but I love getting off just as the next person. Heck, I don't have to worry about the neighbours hearing me!" +
                                            $"#$b#It can get kind of boring with just me and my hands though, so I'm very appreciative of your offer" +
                                            $"#$b#%Susan undoes the snaps on her overalls and pushes them down, pausing when they catch on her full hips to remove her shirt and bra." +
                                            $"#$b#%She hooks her thumbs in her underwear and, in one smooth, practised motion, drops everything to the floor. She stands confidently before you, her gorgeously dark skin bare." +
                                            $"#$b#%She strides towards you, places her hands on your shoulders and firmly guides you to the ground, sitting astride your hips. She grins at you and tosses her curls, beautiful in the sun." +
                                            $"#$b#%You try to take back some control, and pull her towards you, grasping her firm thighs with your hands as you bury your head in her warm pussy, noticing just how wet she is already." +
                                            $"#$b#%You dig your tongue into her cunt and wiggle it, before moving up to her clit and giving it a little suck." +
                                            $"#$b#Oh, @! I didn't expect you to be so aggressive. Not that I'm complaining, I like it rough." +
                                            $"#$b#%You settle into a rhythm with Susan, and it's not long before she's moaning loudly, true to her word, letting you know just how much she is enjoying this." +
                                            $"#$b#%Her hands weave trails through your hair, stroking and massaging mindlessly as the waves of pleasure wash over her. Her thighs start pressing in on your, and your world shrinks to between her legs." +
                                            $"#$b#Keep going! I'm almost there, @. Make sure you suck my clit!" +
                                            $"#$b#%You barely hear her words of encouragement, but realise that she is very close. You rapidly flick your tongue over clit, and as she squeezes one last time you give it a slight nip." +
                                            $"#$b#%She responds immediately, and her legs go slack, her body falling backwards onto your legs as her juices spraying into the air." +
                                            $"#$b#Wow. I didn't know you had it in you, @. Just...wow.";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Claire"))
                    {
                        ModFunctions.Log($"Adding eat_out for Claire", LogLevel.Trace);
                        data["eat_out"] = $"I don’t really think I’ve let anyone do that to me before...I mean sure if you want to? You don’t want anything in return?" +
                                            "#$b#%You assure Claire that no, right now it’s all about her and her needs.Reluctantly, she inches her bottoms down, but seems hesitant to fully take them off. " +
                                            "#$b#%You gently lay her down on the ground and put her ankles over your shoulders.When your tongue gives an introductory swipe of her privates, she lets out a repressed squeak and covers her mouth with her hands as if to stifle her own pleasurous sounds.Almost challenged by this, you begin to tongue her clit in short flicks. Her breath rate increases, inhaling and exhaling through her nose as her eyes begin to cross and her eyelids start to flicker. You can tell she is struggling to maintain composure." +
                                            "#$b#Mmph...erngh…" +
                                            "#$b#%Noises manage to escape her hands despite her best efforts. You proceed with the tongue flicking, occasionally adding a swirl." +
                                            "#$b#%You sneak a finger into her and stroke her insides while your tongue handles her outsides. Claire bites her hand and shuts her eyes forcefully." +
                                            "#$b#%She writhes under you and locks her ankles behind your neck.With a high pitched squeal, she seizes and cums right in your face. " +
                                            "#$b#%She looks at you almost apologetically, and to show her it’s nothing to be ashamed of; you lap up every last drop of her pleasure." +
                                            "#$b#%Her breathing steadies and you retreat your head. She pulls up her pants and massages the bite marks on her hand." +
                                            "#$b#I...didn’t know that would feel so good.Can you do it again sometime ?";

                    }
                }

                if (!data.ContainsKey("get_eaten") && Deploy)
                {
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Abigail")) // ver 1.0
                    {
                        ModFunctions.Log($"Adding get_eaten for Abigail", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Emily"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Emily", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Haley"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Haley", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Leah"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Leah", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Maru"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Maru", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Penny"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Penny", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Caroline"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Caroline", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Jodi"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Jodi", LogLevel.Trace);
                        data["get_eaten"] = $"";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Marnie"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Marnie", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Robin"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Robin", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Pam"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Pam", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Sandy"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Sandy", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Evelyn"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Evelyn", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }

                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Sophia"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Sophia", LogLevel.Trace);
                        data["get_eaten"] = "";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Olivia"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Olivia", LogLevel.Trace);
                        data["get_eaten"] = $"";

                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Susan"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Susan", LogLevel.Trace);
                        data["get_eaten"] = $"";
                    }
                    if (asset.Name.IsEquivalentTo("Characters/Dialogue/Claire"))
                    {
                        ModFunctions.Log($"Adding get_eaten for Claire", LogLevel.Trace);
                        data["get_eaten"] = $"";

                    }
                }

                if (!data.ContainsKey("sex") && Deploy)
                {

                }
            }

            //DumpData(asset);

            #region add in dialogue hooks
            if (asset.Name.IsEquivalentTo("Characters/Dialogue/Haley") && !data.ContainsKey("HaleyPanties"))
            {
                data["HaleyPanties"] = "placeholder";
            }
            #endregion
        }

        public static void UpdateData(Dictionary<string, string> assetdata)
        {
            data = assetdata;
            ModFunctions.Log("Updating DialogueEditor: Data", LogLevel.Trace);
        }

        private static void DumpData(IAssetData asset)
        {
            //string FilePath = $"C:\\Users\\truni\\datadump\\{asset.NameWithoutLocale}.txt";
            //string DirectoryPath = Path.GetDirectoryName(FilePath);
            //if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);

            //List<string> lines = new List<string>();

            //foreach ( KeyValuePair<string, string> kvp in asset.AsDictionary<string, string>().Data)
            //{
            //    lines.Add($"\"{kvp.Key}\":\"{kvp.Value}\",");
            //}

            //File.WriteAllLinesAsync(FilePath, lines);
        }
    }


    public static class NPCGiftTastesEditor
    {
        private static IDictionary<string, string> data;
        public static List<string> Villagers = new List<string>();


        public static bool CanEdit(IAssetName AssetName)
        {
            return AssetName.IsEquivalentTo("Data/NPCGiftTastes");
        }


        public static void Edit(IAssetData asset)
        {
            EditAsset(asset);
        }


        private static void EditAsset(IAssetData asset)
        {
        }

        public static void UpdateData(Dictionary<string, string> assetdata)
        {
            data = assetdata;

            string[] banned = new string[] {
                "Universal_Love",
                "Universal_Like",
                "Universal_Neutral",
                "Universal_Dislike",
                "Universal_Hate"
            };

            foreach (KeyValuePair<string, string> kvp in data)
                if (!Villagers.Contains(kvp.Key))
                {
                    if (!banned.Contains(kvp.Key))
                        Villagers.Add(kvp.Key);
                }

        }

    }
}
