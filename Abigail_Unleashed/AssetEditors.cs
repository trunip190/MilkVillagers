using StardewModdingAPI;
using System.Collections.Generic;

namespace MilkVillagers
{
    public class RecipeEditor : IAssetEditor
    {
        public IDictionary<string, string> data;

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Data/CookingRecipes");
        }

        public void Edit<T>(IAssetData asset)
        {
            TempRefs.Monitor.Log("Loading recipes", LogLevel.Trace);
            if (asset.AssetNameEquals("Data/CookingRecipes"))
            {
                data = asset.AsDictionary<string, string>().Data;
                data["'Protein' Shake"] = $"{TempRefs.CumType} 1/10 10/{TempRefs.ProteinShake}/default/'Protein' shake";
                data["Milkshake"] = $"{TempRefs.MilkType} 1/10 10/{TempRefs.MilkShake}/default/Milkshake";
            }
        }
    }

    public class DialogueEditor : IAssetEditor
    {
        public IDictionary<string, string> data;

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Characters/Dialogue/Abigail") || asset.AssetNameEquals("Characters/Dialogue/Caroline") || (asset.AssetNameEquals("Characters/Dialogue/Abigail") || asset.AssetNameEquals("Characters/Dialogue/Emily")) || (asset.AssetNameEquals("Characters/Dialogue/Haley") || asset.AssetNameEquals("Characters/Dialogue/Leah") || (asset.AssetNameEquals("Characters/Dialogue/Maru") || asset.AssetNameEquals("Characters/Dialogue/Penny"))) || (asset.AssetNameEquals("Characters/Dialogue/Caroline") || asset.AssetNameEquals("Characters/Dialogue/Jodi") || (asset.AssetNameEquals("Characters/Dialogue/Marnie") || asset.AssetNameEquals("Characters/Dialogue/Robin")) || (asset.AssetNameEquals("Characters/Dialogue/Pam") || asset.AssetNameEquals("Characters/Dialogue/Sandy"))) || asset.AssetNameEquals("Characters/Dialogue/Evelyn");
        }

        public bool Replace(IAssetData asset, string keyString, string oldString, string newString)
        {
            this.data = asset.AsDictionary<string, string>().Data;
            if (!data.ContainsKey(keyString))
                return false;
            this.data[keyString] = this.data[keyString].Replace(oldString, newString);
            return true;
        }

        public void Edit<T>(IAssetData asset)
        {
            #region Log item codes
            if (false)
            {
                TempRefs.Monitor.Log($"MilkAbig: {TempRefs.MilkAbig  }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkEmil: {TempRefs.MilkEmil }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkHale: {TempRefs.MilkHale }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkLeah: {TempRefs.MilkLeah }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkMaru: {TempRefs.MilkMaru }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkPenn: {TempRefs.MilkPenn }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkCaro: {TempRefs.MilkCaro }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkJodi: {TempRefs.MilkJodi }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkMarn: {TempRefs.MilkMarn }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkRobi: {TempRefs.MilkRobi }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkPam: {TempRefs.MilkPam }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkSand: {TempRefs.MilkSand }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkEvel: {TempRefs.MilkEvel }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkGeneric: {TempRefs.MilkGeneric }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkSpecial: {TempRefs.MilkSpecial }", LogLevel.Trace);

                TempRefs.Monitor.Log($"ProteinShake: {TempRefs.ProteinShake }", LogLevel.Trace);
                TempRefs.Monitor.Log($"MilkShake: {TempRefs.MilkShake    }", LogLevel.Trace);

                TempRefs.Monitor.Log($"MilkType: {TempRefs.MilkType }", LogLevel.Trace);
                TempRefs.Monitor.Log($"CumType: {TempRefs.CumType }", LogLevel.Trace);
            }
            #endregion

            TempRefs.Monitor.Log("Loading messages", LogLevel.Trace);

            // Girls
            if (asset.AssetNameEquals("Characters/Dialogue/Abigail"))
            {
                data = asset.AsDictionary<string, string>().Data;
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
                data["milk_start"] = "My breasts are so sore, I NEED someone to milk them.#$q 300006 milk_no#Will you help milk Abigail?#$r 300003 15 milk_yes#Milk her#$r 300003 -15 milk_no#Make her milk herself";
                data["milk_yes"] = "Please be gentle, they are really sore today.#$b#*You sit down as she lies across your lap, letting her breast hang down. She gives you a bottle and you start kneeding her breasts as gently as you can*#$b#*Milk collects in the bottle as you expertly milk her, moving on to the second breast when the first runs dry*#$b#Thank you. It's so much more erotic when you do it." + $"#$b#Just think of this as taking care of one of your 'cows'. Here, you can keep this. [{TempRefs.MilkAbig}]";
                data["milk_no"] = "But...I'm so sore. I'm going to have to try and suck the milk out myself now! $s#$b#Fine, then you have to watch me...as I lick my nipples, suck on them, feel the milk washing down my throat...#$b#*Abigail lifts her breast to her mouth and slowly circles the tip of her nipple. Milk starts leaking, and she carefully scoops it up with her tongue*#$b#*She starts sucking in ernest, and milk dribbles down her chin while she starts moaning softly. You move towards her and she puts a hand out to stop you*#$b#No! You made me do this, so you have to watch. *she switches to her other breast and takes big gulps while a river runs down her front and starts pooling on the floor*#$b#*She finally finishes, looks you in the eye, then turns around and leaves*";
                data["sex_choices"] = "Okay, what are you waiting for?#$q 300123 Shop_no#$e#You can do whatever you want to me.#$r 300121 0 Shop_finger#Grope her under skirt until she cums#$r 300121 15 Shop_suck#Get under her skirt and lick her#$r 300121 30 Shop_fuck#Fuck her right now#$r 300123 -30 Shop_no#Stop asking me.";
                data["sex_finger"] = "What are you doing with your hand?#$b#Someone is feeling frisky today! Ooh, you're rubbing my slit...Yeah...that's the spot. Keep rubbing there! Mmmm.#$b#Oh! You went inside! I can feel your fingers pushing inside and spreading me! *wet noises* Keep going! I'm going to cum soon!#$b#*Abigail moans loudly* Cumming! Thanks honey, it's always 10x better when you do.#$b#Here, let me lick your hand clean.";
                data["sex_suck"] = "That's right. Keep licking my pussy#$b#Mmmm! I can feel your tongue inside me! That's so good! Keep licking there! Mmmmmmm!#$b#Did you hear that? Oooooh! I can hear someone! You have to stop! Please...Oh no! Here...I...CUM!#$b#*pants* Wow! I can't believe you licked me while someone was right there - it was so intense!#$b#I guess they know what we were doing. You are the best.#$b#Let me lick my cum off of your face, you dirty boy.^Let me lick my cum off of your face, you dirty girl.";
                data["sex_fuck"] = "#$b#I want it rough today - push me against the wall while you ram it in. Oh god, it's in so deep! Keep ramming it in as hard as you can.#$b#*squelch* I can feel my juices running down my leg! *pound* *pound*  I'm getting closer...keep going!#$b#Mmmmm, fuck me, fuck me, FUCK ME! *Abigail goes limp as you cum deep inside her*#$b#Wow. Just...wow! I'm going to be leaving a trail for a while with all that cum inside me.";
                data["sex_tieup"] = "#$b#Do you always carry rope? I could be your very own sex slave, and you could tie me up in the dungeons.#$b#*You tie Abigail's arms behind her back* Hmmm, I think I like where this is going. It feels so naughty when you grope my breasts, and there's nothing I can do to stop it.#$b#Where is your hand going? Do you think you can do anything you want to my pussy and I won't stop you? Just...like...that...#$b#Oooh yeah! You're going to make me ruin my panties! Actually, I think they're already soaking wet.#$b#*You pull her legs apart, take off her panties, then stuff them in her mouth* Mmmff! *You keep fingering her until she climaxes hard, then take the panties out of her mouth and untie her hands.*#$q 300123 panties_no#Do you give her panties back? $e#$r 300131 0 keep_panties#Keep her panties#$r 300132 15 keep_panties#Give her panties back";
                data["panties_keep"] = "#$p 300131#What do you mean you're keeping them? Anyone could see up my skirt! /addObject 10 17 {{spacechase0.JsonAssets/ObjectId]=PantiesAbigail}}|I might taste awesome, but you have some making up to do next time. I might blindfold you and tease YOU instead.";
                data["sex_no"] = "Not right now. Someone might see us";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Emily"))
            {
                data = asset.AsDictionary<string, string>().Data;
                data["milk_start"] = $"Oh! Did you know that human breast milk is a super food?#$b#It's way better for you than cows milk...Not that your milk isn't great! [{TempRefs.MilkEmil}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Haley"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"Everyone always said my boobs were great. I guess I shouldn't be surprised that you love them too. [{TempRefs.MilkHale}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Leah"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"I love the way your hands feel on me.#$b#I could get used to this... [{TempRefs.MilkLeah}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Maru"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"I'm sure i could come up with a machine to help with this...#$b#I'm not sure it would be better than your touch though. [{TempRefs.MilkMaru}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Penny"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"I'm always self conscious about the size of my breasts.#$b#But you make me feel like a woman every time. [{TempRefs.MilkPenn}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Caroline"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"My breasts are so sore, I NEED someone to milk them.#$b#Aw, thanks honey. You're much firmer than Pierre. Sometimes he's so afraid of hurting me that he can't make me feel very good.#$b#Here's something for your trouble. [{TempRefs.MilkCaro}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Jodi"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"I've been so lonely since Kent first went off to war. Now that Vincent is growing up so fast, I didn't think anyone would ever pay attention to me again.#$b#You make me feel so WANTED, even if it is only for my milk. I feel like I have a role to play again.#$b#*Your hands gently caress her breasts, and her nipples quickly get hard. Milk dribbles from them, and you angle them over a jar so that you can aim the stream better* [{TempRefs.MilkJodi}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Marnie"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"I'm glad that Lewis isn't the only one to appreciate my big tits! He spends every moment he can in my cleavage, but he never thought to suck on them!#$b#*Marnie's milk quickly fills the jar, and she sighs contentedly as she rearranges her clothing*#$b#Make sure Lewis...I mean the Mayor...doesn't catch you! He might get jealous! [{TempRefs.MilkMarn}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Robin"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"Demetrius is always so...clinical...when he talks about my breasts. I wish he was as romantic as you!#$b#Of course you can collect my milk! Just...don't be surprised if I leave a damp spot on the chair when you're done!#$b#*As you massage her breast with your hand, filling up the jar, you make sure to play with her other nipple.*#$b#*Robin snakes a hand down her jeans and starts playing with herself, moaning and whimpering as her milk fills the jar. You finish milking her, but wait for her until she clenches her legs tightly and throws her head back.*#$b#That was wonderful...Come back, any time. [{TempRefs.MilkRobi}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Pam"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"Really? I...didn't know people were into that kind of thing. I guess it wouldn't hurt, but don't expect me too go 'moo'!#$b#*You give her nipple a quick flick with your tongue, and then suck on it to taste her milk. It's sourer then normal milk, but you dutifully kneed her breasts to fill a jar.* [{TempRefs.MilkPam}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Sandy"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"I knew you were too tempted to pass up this opportunity. I'd love to have you worship my breasts.#$b#*She quickly sheds her top, baring her beautiful breasts to the air.*#$b#*Her nipples are perky for their size, and you give them a quick suck to get the milk flowing. You collect a lot of her sweet milk in a jar.[{TempRefs.MilkSand}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Evelyn"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"*Evelyn sits down on a nearby chair and unbottons her blouse. She deftly unhooks her bra, and you tenderly hold her mature breasts in your hands.*#$b#*You aren't able to coax much milk out, but she sighs contentedly.*#$b#This brings back memories of when I was MUCH younger...and prettier. [{TempRefs.MilkEvel}]";
            }

            // Guys
            if (asset.AssetNameEquals("Characters/Dialogue/Alex"))  
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"You think I'm hot? Well, I think you're really hot too, and if you want to take this further I'm sure we can find somewhere 'quiet' for a bit#$b#*He pulls you aside and unzips his pants as you drop to your knees. His dick is already rising, and he guides you with his hand on the back of your head.*#$b#*He cums rather quickly, and sags backwards as you fill your bottle with his semen, buttong up his pants and giving you a half dazed smile* [{TempRefs.MilkAlex}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Clint"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkClint}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Demetrius"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkDemetrius}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Elliott"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkElliott}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/George"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"I can't get out of this chair, and it's been so long since Evelyn did this for me. I wouldn't mind getting that Haley over here some time - she's such a tease.#$b#That's right, bend down and enjoy the taste. Bet you didn't expect to see such a large dick on a man in a wheelchair, huh? [{TempRefs.MilkGeorge}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Gil"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $"Huh? What's going on? *Snore* Why are my pants off?#$b#Guess I must be dreaming again...having a beautiful face looking up at me from between my knees.#$b#*You quickly get to work, licking his balls while his penis hardens. It doesn't get fully erect at first, but after several minutes of soft sucking, and flicking his tip, he starts moaning and getting ready to cum*#$b#*Gil shoots his load into your mouth, and you quickly spit it into a bottle, wiping off any that dribbled down your chin* [{TempRefs.MilkGil}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Gunther"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkGunther}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Gus"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkGus}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Harvey"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkHarv}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Kent"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkKent}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Lewis"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkLewis}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Linus"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkLinus}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Marlon"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkMarlon}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Morris"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkMorris}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Mr. Qi"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkQi}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Pierre"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkPierre}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Sam"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkSam}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Sebastian"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkSeb}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Shane"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkShane}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Willy"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkWilly}]";
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Wizard"))
            {
                data = (asset.AsDictionary<string, string>()).Data;
                data["milk_start"] = $" [{TempRefs.MilkWiz}]";
            }
            return;
        }
    }

    public class ItemEditor : IAssetEditor
    {
        public IDictionary<int, string> _objectData;

        public bool Contains(int ID)
        {
            return this._objectData.ContainsKey(ID);
        }

        public IDictionary<int, string> Report()
        {
            return this._objectData;
        }

        public void AddEntry(int ID, string itemString)
        {
            this._objectData[ID] = itemString;
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Data/ObjectInformation");
        }

        public void Edit<T>(IAssetData asset)
        {
            TempRefs.Monitor.Log("Adding in items", LogLevel.Trace);
            if (asset.AssetNameEquals("Data/ObjectInformation"))
                _objectData = asset.AsDictionary<int, string>().Data;
            if (!TempRefs.loaded)
                return;


            //milk items
            _objectData[TempRefs.MilkAbig] = $"Abigail's Milk/300/15/Basic {TempRefs.MilkType}/Abigail's Milk/A jug of Abigail's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkEmil] = $"Emily's Milk/300/15/Basic {TempRefs.MilkType}/Emily's Milk/A jug of Emily's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkHale] = $"Haley's Milk/300/15/Basic {TempRefs.MilkType}/Haley's Milk/A jug of Haley's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkLeah] = $"Leah's Milk/300/15/Basic {TempRefs.MilkType}/Leah's Milk/A jug of Leah's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkMaru] = $"Maru's Milk/300/15/Basic {TempRefs.MilkType}/Maru's Milk/A jug of Maru's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkPenn] = $"Penny's Milk/300/15/Basic {TempRefs.MilkType}/Penny's Milk/A jug of Penny's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkCaro] = $"Caroline's Milk/300/15/Basic {TempRefs.MilkType}/Caroline's Milk/A jug of Caroline's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkJodi] = $"Jodi's Milk/300/15/Basic {TempRefs.MilkType}/Jodi's Milk/A jug of Jodi's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkMarn] = $"Marnie's Milk/140/15/Basic {TempRefs.MilkType}/Marnie's Milk/A jug of Marnie's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkRobi] = $"Robin's Milk/300/15/Basic {TempRefs.MilkType}/Robin's Milk/A jug of Robin's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkPam] = $"Pam's Milk/90/15/Basic {TempRefs.MilkType}/Pam's Milk/A jug of Pam's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkSand] = $"Sandy's Milk/350/15/Basic {TempRefs.MilkType}/Sandy's Milk/A jug of Sandy's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkEvel] = $"Evelyn's Milk/50/15/Basic {TempRefs.MilkType}/Evelyn's Milk/A jug of Evelyn's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkGeneric] = $"Woman's Milk/50/15/Basic {TempRefs.MilkType}/Woman's Milk/A jug of woman's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

            //cum items
            _objectData[TempRefs.MilkSpecial] = $"Special milk/50/15/Basic {TempRefs.CumType}/'Special' Milk/A bottle of 'special' milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

            //recipes
            _objectData[TempRefs.ProteinShake] = $"Protein shake/50/15/Basic -6/'Protein' shake/Shake made with extra protein/Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkShake] = $"Milkshake/50/15/Basic -6/'Special' Milkshake/Extra milky milkshake./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
        }
    }

    internal enum QuestTypes
    {
        Basic,
        Building,
        Crafting,
        ItemDelivery,
        ItemHarvest,
        Location,
        LostItem,
        Monster,
        Social,
    }

    public class QuestEditor : IAssetEditor
    {
        public IDictionary<int, string> data;

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Data/Quests");
        }

        public void Edit<T>(IAssetData asset)
        {
            if (!((IAssetInfo)asset).AssetNameEquals("Data/Quests"))
                return;
            this.data = ((IAssetData<IDictionary<int, string>>)asset.AsDictionary<int, string>()).Data;
            this.data[TempRefs.QuestID1] = this.QuestString(QuestTypes.ItemDelivery, "Abigail's Eggplant", "Abigail needs an eggplant for her cam show. Make sure it's a good one", "Bring Abigail an eggplant.", "Abigail 272", string.Format("{0}", (object)TempRefs.QuestID2), "350", "-1", "true", "Wow, it's so big!. I'll be thinking of you tonight, @. Be sure to watch my show.");
            this.data[TempRefs.QuestID2] = this.QuestString(QuestTypes.ItemDelivery, "Abigail's carrot", "Abigail needs a cave carrot to scratch an itch. Bring her one to 'fill' a need", "Abigail 78", string.Format("{0}", (object)TempRefs.QuestID3), "410", "-1", "true", "I hope you washed it! Those caves are wonderful, but it needs to be SUPER clean before I can use it.");
            this.data[TempRefs.QuestID3] = this.QuestString(QuestTypes.ItemDelivery, "Abigail's radishes", "Abigail wants some radishes for a new idea she had", "Bring Abigail Radishes", "Abigail 264", string.Format("{0}", (object)TempRefs.QuestID4), "240", "-1", "true", "I'm gonna have so much fun with these! How many do you think I can fit?");
            this.data[TempRefs.QuestID4] = this.QuestString(QuestTypes.ItemDelivery, "Abigail's helping hand", "Abigail wants you to help her with her show tonight. Go visit her at her house after 7pm", "Go to Abigail's house tonight", "Abigail", "-1", "0", "", "true", "");
        }

        private string QuestString(
          QuestTypes Type,
          string Title,
          string Details,
          string Hint,
          string Solution,
          string NextQuest,
          string GoldReward,
          string RewardDescription,
          string Cancelable)
        {
            return string.Format("\"{0}\"/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}", (object)Type, (object)Title, (object)Details, (object)Hint, (object)Solution, (object)NextQuest, (object)GoldReward, (object)RewardDescription, (object)Cancelable);
        }

        private string QuestString(
          QuestTypes Type,
          string Title,
          string Details,
          string Hint,
          string Solution,
          string NextQuest,
          string GoldReward,
          string RewardDescription,
          string Cancelable,
          string ReactionText)
        {
            return string.Format("\"{0}\"/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}", (object)Type, (object)Title, (object)Details, (object)Hint, (object)Solution, (object)NextQuest, (object)GoldReward, (object)RewardDescription, (object)Cancelable, (object)ReactionText);
        }
    }

}
