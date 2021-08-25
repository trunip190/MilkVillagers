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
                int id = TempRefs.MilkType;
                id = true ? TempRefs.MilkHale : TempRefs.MilkType;
                data = asset.AsDictionary<string, string>().Data;
                data["'Protein' Shake"] = $"{TempRefs.CumType} 1/10 10/{TempRefs.ProteinShake}/f Gus 8/'Protein' shake";
                data["Milkshake"] = $"{id} 1/10 10/{TempRefs.MilkShake}/f Emily 8/Milkshake";
            }
        }
    }

    public class DialogueEditor : IAssetEditor
    {
        public IDictionary<string, string> data;
        public bool ExtraContent = false;

        public bool CanEdit<T>(IAssetInfo asset)
        {
            bool result =
                   asset.AssetNameEquals("Characters/Dialogue/Abigail")
                || asset.AssetNameEquals("Characters/Dialogue/Alex")
                || asset.AssetNameEquals("Characters/Dialogue/Caroline")
                || asset.AssetNameEquals("Characters/Dialogue/Clint")
                || asset.AssetNameEquals("Characters/Dialogue/Demetrius")
                || asset.AssetNameEquals("Characters/Dialogue/Dwarf")
                || asset.AssetNameEquals("Characters/Dialogue/Elliott")
                || asset.AssetNameEquals("Characters/Dialogue/Emily")
                || asset.AssetNameEquals("Characters/Dialogue/Evelyn")
                || asset.AssetNameEquals("Characters/Dialogue/George")
                || asset.AssetNameEquals("Characters/Dialogue/Gil")
                || asset.AssetNameEquals("Characters/Dialogue/Gus")
                || asset.AssetNameEquals("Characters/Dialogue/Haley")
                || asset.AssetNameEquals("Characters/Dialogue/Harvey")
                || asset.AssetNameEquals("Characters/Dialogue/Jodi")
                || asset.AssetNameEquals("Characters/Dialogue/Kent")
                || asset.AssetNameEquals("Characters/Dialogue/Krobus")
                || asset.AssetNameEquals("Characters/Dialogue/Leah")
                || asset.AssetNameEquals("Characters/Dialogue/Lewis")
                || asset.AssetNameEquals("Characters/Dialogue/Linus")
                || asset.AssetNameEquals("Characters/Dialogue/Marnie")
                || asset.AssetNameEquals("Characters/Dialogue/Maru")
                || asset.AssetNameEquals("Characters/Dialogue/Mister Qi")
                || asset.AssetNameEquals("Characters/Dialogue/Pam")
                || asset.AssetNameEquals("Characters/Dialogue/Penny")
                || asset.AssetNameEquals("Characters/Dialogue/Pierre")
                || asset.AssetNameEquals("Characters/Dialogue/rainy")
                || asset.AssetNameEquals("Characters/Dialogue/Robin")
                || asset.AssetNameEquals("Characters/Dialogue/Sam")
                || asset.AssetNameEquals("Characters/Dialogue/Sandy")
                || asset.AssetNameEquals("Characters/Dialogue/Sebastian")
                || asset.AssetNameEquals("Characters/Dialogue/Shane")
                || asset.AssetNameEquals("Characters/Dialogue/Willy")
                || asset.AssetNameEquals("Characters/Dialogue/Wizard");

            return result;
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
            TempRefs.Monitor.Log($"Loading messages: {asset.AssetName}", LogLevel.Trace);
            data = asset.AsDictionary<string, string>().Data;

            #region Girls
            if (asset.AssetNameEquals("Characters/Dialogue/Abigail")) // ver 1.0
            {
                if (ExtraContent)
                {
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
                    data["panties_keep"] = "#$p 300131#What do you mean you're keeping them? Anyone could see up my skirt! /addObject 10 17 {{spacechase0.JsonAssets/ObjectId]=PantiesAbigail}}|I might taste awesome, but you have some making up to do next time. I might blindfold you and tease YOU instead.";
                    data["sex_no"] = "Not right now. Someone might see us";
                }
                if (!data.ContainsKey("milk_start"))
                {
                    data["milk_start"] = "Please be gentle, they are really sore today.#$b#*You sit down as she lies across your lap, letting her breast hang down. She gives you a bottle and you start kneeding her breasts as gently as you can*#$b#*Milk collects in the bottle as you expertly milk her, moving on to the second breast when the first runs dry*#$b#Thank you. It's so much more erotic when you do it." + $"#$b#Just think of this as taking care of one of your 'cows'. Here, you can keep this.";
                    //data["milk_start"] = "My breasts are so sore, I NEED someone to milk them.#$q 300006 milk_no#Will you help milk Abigail?#$r 300003 15 milk_yes#Milk her#$r 300003 -15 milk_no#Make her milk herself";
                    //data["milk_yes"] = "Please be gentle, they are really sore today.#$b#*You sit down as she lies across your lap, letting her breast hang down. She gives you a bottle and you start kneeding her breasts as gently as you can*#$b#*Milk collects in the bottle as you expertly milk her, moving on to the second breast when the first runs dry*#$b#Thank you. It's so much more erotic when you do it." + $"#$b#Just think of this as taking care of one of your 'cows'. Here, you can keep this. [{TempRefs.MilkAbig}]";
                    //data["milk_no"] = "But...I'm so sore. I'm going to have to try and suck the milk out myself now! $s#$b#Fine, then you have to watch me...as I lick my nipples, suck on them, feel the milk washing down my throat...#$b#*Abigail lifts her breast to her mouth and slowly circles the tip of her nipple. Milk starts leaking, and she carefully scoops it up with her tongue*#$b#*She starts sucking in ernest, and milk dribbles down her chin while she starts moaning softly. You move towards her and she puts a hand out to stop you*#$b#No! You made me do this, so you have to watch. *she switches to her other breast and takes big gulps while a river runs down her front and starts pooling on the floor*#$b#*She finally finishes, looks you in the eye, then turns around and leaves*";
                }
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Emily"))
            {
                data["milk_start"] = $"Oh! Did you know that human breast milk is a super food?#$b#It's way better for you than cows milk..." +
                    $"Not that your milk isn't great!";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Haley"))
            {
                if (!data.ContainsKey("milk_start"))
                {
                    data["milk_start"] = $"Everyone always said my boobs were great. I guess I shouldn't be surprised that you love them too.";
                }
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Leah"))
            {

                data["milk_start"] = $"I love the way your hands feel on me.#$b#I could get used to this...";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Maru"))
            {

                data["milk_start"] = $"I'm sure i could come up with a machine to help with this...#$b#I'm not sure it would be better than your touch though.";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Penny"))
            {

                data["milk_start"] = $"I'm always self conscious about the size of my breasts.#$b#But you make me feel like a woman every time.";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Caroline"))
            {

                data["milk_start"] = $"My breasts are so sore, I NEED someone to milk them.#$b#Aw, thanks honey. You're much firmer than Pierre. Sometimes he's so afraid of hurting me that he can't make me feel very good.#$b#Here's something for your trouble.";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Jodi"))
            {

                data["milk_start"] = $"I've been so lonely since Kent first went off to war. Now that Vincent is growing up so fast, I didn't think anyone would ever pay attention to me again.#$b#You make me feel so WANTED, even if it is only for my milk. I feel like I have a role to play again.#$b#*Your hands gently caress her breasts, and her nipples quickly get hard. Milk dribbles from them, and you angle them over a jar so that you can aim the stream better*";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Marnie"))
            {

                data["milk_start"] = $"I'm glad that Lewis isn't the only one to appreciate my big tits! He spends every moment he can in my cleavage, but he never thought to suck on them!#$b#*Marnie's milk quickly fills the jar, and she sighs contentedly as she rearranges her clothing*#$b#Make sure Lewis...I mean the Mayor...doesn't catch you! He might get jealous!";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Robin"))
            {

                data["milk_start"] = $"Demetrius is always so...clinical...when he talks about my breasts. I wish he was as romantic as you!#$b#Of course you can collect my milk! Just...don't be surprised if I leave a damp spot on the chair when you're done!#$b#*As you massage her breast with your hand, filling up the jar, you make sure to play with her other nipple.*#$b#*Robin snakes a hand down her jeans and starts playing with herself, moaning and whimpering as her milk fills the jar. You finish milking her, but wait for her until she clenches her legs tightly and throws her head back.*#$b#That was wonderful...Come back, any time.";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Pam"))
            {

                data["milk_start"] = $"Really? I...didn't know people were into that kind of thing. I guess it wouldn't hurt, but don't expect me too go 'moo'!#$b#*You give her nipple a quick flick with your tongue, and then suck on it to taste her milk. It's sourer then normal milk, but you dutifully kneed her breasts to fill a jar.*";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Sandy"))
            {

                data["milk_start"] = $"I knew you were too tempted to pass up this opportunity. I'd love to have you worship my breasts.#$b#*She quickly sheds her top, baring her beautiful breasts to the air.*#$b#*Her nipples are perky for their size, and you give them a quick suck to get the milk flowing. You collect a lot of her sweet milk in a jar.";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Evelyn"))
            {

                data["milk_start"] = $"*Evelyn sits down on a nearby chair and unbottons her blouse. She deftly unhooks her bra, and you tenderly hold her mature breasts in your hands.*#$b#*You aren't able to coax much milk out, but she sighs contentedly.*#$b#This brings back memories of when I was MUCH younger...and prettier.";
                return;
            }
            #endregion

            #region Guys
            if (asset.AssetNameEquals("Characters/Dialogue/Alex")) //Ver 1.0
            {
                if (TempRefs.thirdParty)
                {
                    // Iliress
                    data["milk_start"] = $"Ha! I knew you couldn't resist my dick. Sure you can have my cum. But I'm gonna need help, if you know what I mean.$h" +
                        $"#$b#%*Alex unzips his pants you you massage his cock. It swells under your touch. You bend down and begin to take in his member*" +
                        $"#$b#%*Slowly, moving back and forth until his tip is forcing its way further down your throat. You repress your gag reflex*" +
                        $"#$b#%*You use your tongue and swirl around the shaft, from base to tip, and Alex moans*" +
                        $"#$b#Almost there...little faster$l" +
                        $"#$b#%*Using your spit as lube, you work his shaft while artfully swirling the tip with your tongue." +
                        $"#$b#%Soon, hot salty fluid bursts into your mouth as Alex seizes, then lets out a sigh of relief." +
                        $"#$b#%*You spit your collection into a jar and cap it.*" +
                        $"#$b#Thanks, @. I can always count on you for a quick release.";
                }
                else
                {
                    // ver 1.0
                    data["milk_start"] = $"You think I'm hot? Well, I think you're really hot too, and if you want to take this further I'm sure we can find somewhere 'quiet' for a bit" +
                        $"#$b#%*He pulls you aside and unzips his pants as you drop to your knees. His dick is already rising, and he guides you with his hand on the back of your head.*" +
                        $"#$b#%*He cums rather quickly, and sags backwards as you fill your bottle with his semen, buttong up his pants and giving you a half dazed smile*";
                }
                return;

            }
            if (asset.AssetNameEquals("Characters/Dialogue/Clint")) // ver 1.0
            {
                data["milk_start"] = $"Wow, this must be my lucky day, @! I might be a little hot and sweaty down there. I've been busy all day and haven't had a chance to wash" +
                    $"#$b#*You pull his pants and underwear down and are lost in his thick, musky smell.* " +
                    $"#$b#*It makes you a little light headed, but when his prick bumps against your forehead you get to business*" +
                    $"#$b#Yeah, just like that, @. It feels really good when you, er, use your tongue like that. Oh! And you're sucking so hard I don't think I can last much longer!" +
                    $"#$b#*Clint cums down your throat";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Demetrius")) // ver 1.0
            {
                if (TempRefs.thirdParty)
                {
                    data["milk_start"] = "Hmm...I guess I could help you out. For scientific research purposes, of course. Just don't let this get back to Robin." +
                        $"#$b#%*Demetrius looks over his shoulder a few times before pulling down the zipper of his pants. You reach in, and removed a semi-erect cock." +
                        $"#$b#You place it in your mouth and it begins to expand to its full size as you move along, until his tip has reached your throat and then some." +
                        $"#$b#You continue a back and forth motion, picking up the pace until you feel Demetrius tense up*" +
                        $"#$b#Uh--!$s" +
                        $"#$b#%*Quickly, you pull out a bottle and collect the scientist's cum inside of it*" +
                        $"#$b#Whew. You know, orgasms are a great way to clear the mind. Now I feel like I can focus on my project more. Thank you, @.";
                }
                else
                {
                    data["milk_start"] = $"I'm not really comfortable but with this, but I guess if you're collecting samples for scientific study then Robin won't mind...much#$b#" +
                        $"I'm not as large as the popular media would have you think based on my background, but I do make up for it in, er, volume.#$b#" +
                        $"*Demetrius seems to be unused to this much attention, and is quickly on the verge of cumming. You back off for a bit, and tell him that you want to make sure you get a 'proper sample'#$b#" +
                        $"But I was almost there... *You pick up the pace again, and in no time he is on the edge.*#$b#" +
                        $"*Demetrius wasn't lying and you struggle to keep up with his flow, your mouth filling up and his semen spilling down your face. You bottle it up and look up at him, covered in his cum*";
                }
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Elliott")) // ver 1.0
            {
                if (TempRefs.thirdParty)
                {
                    data["milk_start"] = "I've been writing all day. This will be a perfect release, my love! I do however, have some carpal tunnel. Could you...?" +
                        $"#$b#%*You pull down Elliot's waistband and lather his cock in thick saliva, and begin working his shaft, starting with a loose grip at the base and firmer at the tip*" +
                        $"#$b#%*He moans with pleasure and smiles down at you before tilting his head back and closing his eyes. You quicken your hand movements, and flick the tip with your tongue*" +
                        $"#$b#Almost...there...@" +
                        $"#$b#%*Elliott shudders with pleasure, and you aim his cock at the opening of your jar. His cock spurts out waves of the thick, sticky, fluid*" +
                        $"#$b#Thank you, @. I always enjoy our little trysts. Do come back later if you need any more, I'm very happy to oblige, and I always feel reinvigorated.$h";
                }
                else
                {
                    data["milk_start"] = $"I could definitely use a break. I'm suffering from some dreadful writers block and this is exactly what I need to clear my head." +
                        $"#$b#If you could pretend to be a ship's boy then it would definitely help me with this part of my story I\"m working on." +
                        $"#$b#*You drop to your knees and pretend to be surprised by him pulling out his penis, faking a little bit of hesitancy as he encourages you*" +
                        $"#$b#*You pretend to be inexperienced at first, but once Elliott is into it you pick up the pace, and suck as much of his dick into your mouth as you can handle*" +
                        $"#$b#*As your eyes start to water, you try and swallow to massage his penis, and it's enough to send him over the edge*" +
                        $"#$b#@, that is incredible!";
                }
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/George")) // ver 1.0
            {

                data["milk_start"] = $"I can't get out of this chair, and it's been so long since Evelyn did this for me. I wouldn't mind getting that Haley over here some time - she's such a tease." +
                    $"#$b#That's right, bend down and enjoy the taste. Bet you didn't expect to see such a large dick on a man in a wheelchair, huh?";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Gil")) // ver 1.0
            {

                data["milk_start"] = $"Huh? What's going on? *Snore* Why are my pants off?#$b#Guess I must be dreaming again...having a beautiful face looking up at me from between my knees.#$b#*You quickly get to work, licking his balls while his penis hardens. It doesn't get fully erect at first, but after several minutes of soft sucking, and flicking his tip, he starts moaning and getting ready to cum*#$b#*Gil shoots his load into your mouth, and you quickly spit it into a bottle, wiping off any that dribbled down your chin*";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Harvey")) // ver 1.0
            {
                if (TempRefs.thirdParty)
                {
                    data["milk_start"] = "Well, a good orgasm IS good for your health. You know, it decrease blood pressure, helps with stress and sleep, and lowers your risk for health disease?" +
                        $"#$b#In fact, there was a study done by Philip Haake that analyzed the effects of the male orgasm on limphocyte subset circulation and cytokine production..." +
                        $"#$b#%*Before Harvey can continue, you unzip his pants and immediately place your mouth on his cock. He stops mid sentence in both surprise and pleasure." +
                        $"#$b#%*You slurp the whole thing in, and he all but quivers as you begin to move back and forth; slowly at first, but soon beginning to pick up the pace." +
                        $"#$b#%*Your tongue glides along his shaft; the faster you go, the less Harvey can maintain his composure. He begins to tremble, his knees threatening to buckle*" +
                        $"#$b#Keep...going...almost...there..." +
                        $"#$b#%*Harvey braces himself on you, and the grip on your shoulder becomes vice-like as he finishes. You don't have time to take out your bottle and take the full load into your mouth." +
                        $"#$b#%*You stand up, and as gracefully as you can, spit the contents in the container. Harvey is sweaty and weak*" +
                        $"#$b#That...took a lot more out of me than I expected. You're too skilled for my own good, @.";
                }
                else
                {
                    data["milk_start"] = $"It's very important to have a healthy sexual life, but have you have you been making sure to keep track of your sexual partners?" +
                        $"#$b#Perfect, then can I recommend you pay special attention to the underside of my glans? It's very sensitive." +
                        $"#$b#Yes...right there. Make sure you like just inside the tip every so often, but don't press too hard. Here we go, my orgasm is imminent." +
                        "#$b#*You grab a flask and hold it over the tip of his penis as he starts to ejaculate, collecting most in it, and giving his dick a quick suck to get the last little bit*";
                }
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Sam")) // ver 1.0
            {
                data["milk_start"] = $"Sure, that sounds great! I love it when you wrap your sexy lips around my shaft" +
                    $"#$b#*You pull his pants down below his cute, white butt and give it a squeeze, then fondle his balls to get the blood flowing*" +
                    $"#$b#*This really gets him going and he's standing to attention in no time, with a slight deodorant smell tinging the air and masking his natural smell*" +
                    $"#$b#*You hold on tight to his behind as your head bobs back and forth, coating his cock with your saliva, and wringing the occasional gasp and moan from him*" +
                    $"#$b#*His hands rest lightly on your head, giving you encouragement, and you pick up the pace until his legs start to shake and he holds your head against his crotch hair*" +
                    $"#$b#*He explodes in your mouth and you pull back just in time to avoid having to swallow his precious load, getting the bottle in the way of a white shower to the face*";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Sebastian")) // ver 1.0
            {
                data["milk_start"] = $"Have you ever thought about doing this with anyone else? Like, at the same time? I'm sure Abigail or Sam would be down if you ever want to?" +
                    $"#$b#Not that I'm saying this isn't great. You're definitely really skilled...at this; your mouth is heavenly and so warm." +
                    $"#$b#*You and Sebastian spend several minutes locked together, mouth to groin, without only your sucking sounds being heard*" +
                    $"#$b#Here it comes! I'm going to cum in your mouth!" +
                    $"#$b#*You try and keep all of it in your mouth, but a lot of it explodes out of the side of your mouth and leaves you scraping the cum up with the bottle*";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Shane")) // ver 1.0
            {
                data["milk_start"] = $"Are you joking? I don't know why you would offer that to me, it's not like I deserve it or anything. Are you sure?" +
                    $"#$b#Oh, ok then, just don't expect anything special from me." +
                    $"#$b#*You pull down Shane's jeans and free his member from his underwear, taking a moment to admire it.* " +
                    $"#$b#*He gets a little embarrassed, but when you lunge forward and take the whole thing in your mouth his insecurities vanish and he focusses on the sensations you are giving him*" +
                    $"#$b#*He gets lost in a world consisting solely of you, himself and the wonderful things your tongue and lips are doing to him, and you see a slightly dazed smile form on his face*" +
                    $"#$b#That feel amazing @, I can't believe you can give me such pleasure. I'm going to cum soon - get your bottle ready!";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Pierre")) // ver 1.0
            {
                if (TempRefs.thirdParty)
                {
                    data["milk_start"] = "Honestly, when you come into the store to buy seeds from me, I sometimes stare at you, imagining you on your knees, giving me all the pleasure in the world. It's nice to see I was right." +
                        $"#$b#I knew you were secretly into me. Don't worry, Caroline doesn't have to know. We don't do much together these days anyway. Well, not with each other.$s" +
                        $"#$b#%*Pierre fidgets the the button and zipper of his pants before pulling out his dick. It's not that impressive, but nothing to laugh at. It looks slightly sad. " +
                        $"#$b#%*You reach down and begin to preform a familiar back and forth motion. He smiles contently and leans his head back." +
                        $"#$b#%*As you start to increase the rhythm, Pierre immediately jerks his head back, his eyes wide*" +
                        $"#$b#I'm close, @!" +
                        $"#$b#%*You bring out the bottle but he finishes before you can get everything in. Only half of the white material manages to get into the jar*" +
                        $"#$b#Sorry, @. I'm kind of sensitive and over eager. Since I don't get much action with Caroline, I don't exactly have any stamina built up, but we can go again tomorrow?";
                }
                else
                {
                    data["milk_start"] = $"What? Who told you I was into that? Does Caroline know...?" +
                        $"#$b#*You quickly calm down Pierre and kneel in front of him, waiting expectantly. He looks around and pulls out his hardening dick, accidentally poking you in the face*" +
                        $"#$b#*You open your mouth and use your tongue to guide it inside, licking the head as you do so and eliciting a small groan of pleasure from him.*" +
                        $"#$b#That's so good @. I had no idea you were such a great little cocksucker. I hope you're ready for this to become a regular thing because Caroline never does this for me." +
                        $"#$b#*It seems that Pierre likes talking dirty, so you lock eyes with him and use your hands to fondle his balls while he gets ready to ejaculate.*" +
                        $"#$b#*At the last minute you pull a bottle out of your bag and start pumping his cock to make sure you get everything out of him.*";
                }
                return;
            }
            //if (asset.AssetNameEquals("Characters/Dialogue/Gunther"))
            //{
            //    data["milk_start"] = $"" +
            //        $"#$b#" +
            //        $"#$b#" +
            //        $"#$b#";
            //    return;
            //}
            if (asset.AssetNameEquals("Characters/Dialogue/Gus")) // Iliress dialogue
            {
                data["milk_start"] = "Honestly, @, this is unexpected. I didn't know you thought of me like that. You are an attractive person, so if you're up for it, I am too.$h" +
                    $"#$b#%*Gus undoes his belt and lowers his pants. You reach down and grasp his cock, and begin to move your hand steadily back and forth, but you struggle to get a rhythm going." +
                    $"#$b#%*Gus grabs some truffle oil he keeps nearby for cooking and pours some on your hands as they grip his cock, making your job much easier. As your hand movements quicken, so does his breathing*" +
                    $"#$b#I'm about to...c..c..$l" +
                    $"#$b#%*Before he can complete his word, he starts squirting on your face. You clumsily hold the jar with slippery hands and collect the rest. For some reason, his semen smells citrusy*" +
                    $"#$b#Always feels good to be touched like that.$h";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Kent")) // Iliress dialogue
            {
                data["milk_start"] = "Jodi and I...we haven't been connecting on that level lately. I'll admit, I've been seeing you around town and have fantasized about you approaching me like this." +
                     $"#$b#Didn't expect it to actually happen, though. Do you mind keeping this quiet from my wife? I don't think she'd be so understanding. Life is short, but I don't want to hurt her." +
                     $"#$b#%*Kent unbuckles his belt and removes his dick, already fully erect. He looks at you, a mixture of trepidation and eagerness in his eyes. You take the initiative and gently grasp it*" +
                     $"#$b#You make slow, deliberate movements, looking straight at him. Kent closes his eyes and breathes deeply, his muscular chest rising and falling in line with the movements of your hand." +
                     $"#$b#You kneel down and take him inside your mouth, and he trembles as he feels the soft warmth. As you accelerate your motions, his breathing becomes more and more irregular*" +
                     $"#$b#@...$h" +
                     $"#$b#%*Kent tenses up and holds your head in place, almost forcing you to take the entirety of his load in your mouth. You deposit the substance in your bottle, capping it*" +
                     $"#$b#Sorry...uh...got carried away at the end there. Hope you didn't mind. Stop by if you want to do this again sometime.";

                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Lewis"))
            {
                data["milk_start"] = $"Ah, @. This isn't one of the funny games you kids play, is it?$a" +
                    $"#$b#I've seen you walking around town, causing all kinds of problems and flaunting that cute little ass in everyone's faces." +
                    $"#$b#On your knees then, like the cockslut you are - I'm sure this isn't the first time you've done this, and you look so good down there." +
                    $"#$b#%*Lewis undoes his belt, and drops his pants and underwear to his ankles. He looks at you hungrily and gestures impatiently to his crotch*" +
                    $"#$b#%*You hesitantly lean forward, contemplating whether this was a mistake, when Lewis grabs your head and plunges forward until his hair tickles your nose*" +
                    $"#$b#%*You gag a little, but he lets up and you get to work sucking and slurping away, while Lewis looks down at you smugly*" +
                    $"#$b#I bet you get off on this kind of thing, @? Being controlled by an older man and used as a recepticle for their semen? Well here is comes.$h" +
                    $"#$b#%*He explodes inside your mouth, and you choke a little until he lets go of your head and you can dribble his load into your jar, coughing all the time*";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Linus"))// Iliress dialogue
            {
                data["milk_start"] = "Pleasure is but a natural part of life. We are but humans, after all. Society puts too large of a stigma on such a thing." +
                    $"#$b#%*Linus casually lifts his outfit revealing an unsurprising lack of undergarments. He smiles kindly and gestures to his organ." +
                    $"#$b#%*For someone who lives in the mountains he smells surprisingly little, with a nice, manly mush. You shrug, lick your hand, and start to work his member." +
                    $"#$b#%*Linus relaxes to your touch. As you increase your handwork, he makes no indication of losing control, remaining completely calm*" +
                    $"#$b#I would get your bottle ready. I'm going to ejaculate soon." +
                    $"#$b#%*You do as you are told, and Linus releases himself in the glass container, maintaining a calm, almost serene composure. You cap the bottle*" +
                    $"#$b#It's nice to help one another out. Be one with each other as well as nature. Thank you for having such an open mind.";
                return;
            }
            //if (asset.AssetNameEquals("Characters/Dialogue/Marlon"))
            //{
            //    data["milk_start"] = $""; return;
            //}
            if (asset.AssetNameEquals("Characters/Dialogue/Morris"))// Iliress dialogue
            {
                data["milk_start"] = "Okay, fine, but wear this JojaCorp uniform. It's the only way I can get off, and after my last store they won't let me do this with an employee...$s" +
                    $"#$b#%*You put on the blue outfit and Morris takes out his chode of a cock. He looks at you, but doesn't let you actually touch him." +
                    $"#$b#%*Using his thumb and forefinger, because his hand is too big, he begins to pleasure himself. His hand speeds up*" +
                    $"#$b#J..j...joja..." +
                    $"#$b#%*He finishes onto the floor and hands you a squeegee and a pan to clean it up yourself*" +
                    $"#$b#Have a good day. Cleanup in aisle 2, %rival. Er...@.$a";
                return;
            }
            if (asset.AssetNameEquals("Characters/Dialogue/Mister Qi"))// Iliress dialogue
            {
                data["milk_start"] = "I knew you'd come to me for that one day. Your curiosity is very predictable. Be warned, @, this will not be like your experience you've had with others in town." +
                    $"#$b#%*Mr. Qi lifts up his shirt and begins to lower his waistband. But where any genitalia would be is instead a blinding light." +
                    $"#$b#%*Once your eyes adjust you see what may as well be a portal to some otherwordly beyond. You reach your hand towards it, checking his face to see if this is safe, and he nods." +
                    $"#$b#%*When your hand touches the vortex, visions and sensations you've never imagined and could never fully describe crash into your mind. You can see time, you can smell space." +
                    $"#$b#%*It's as if you've been brought into the fourth, fifth, sixth and seventh dimension all at once. You feel a great understanding of life itself*" +
                    $"#$b#That will do." +
                    $"#$b#%*As soon as it had started it was gone. You are left with spots in your vision and after images of some immense epiphany that is slowly fading from your mind." +
                    $"#$b#%*In your hand is a bottle containing what is possibly a rip in space-time itself. You do not recall ever taking out the container*" +
                    $"#$b#See you around, my dear @.";
                return;
            }
            //if (asset.AssetNameEquals("Characters/Dialogue/Willy"))
            //{
            //    data["milk_start"] = $" "; return;
            //}
            //if (asset.AssetNameEquals("Characters/Dialogue/Wizard"))
            //{
            //    data["milk_start"] = $" "; return;
            //}
            #endregion

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
            _objectData[TempRefs.MilkAlex] = $"Alex's Cum/300/15/Basic {TempRefs.CumType}/Alex's Cum /A bottle of Alex's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkClint] = $"Clint's Cum/300/15/Basic {TempRefs.CumType}/Clint's Cum/A bottle of Clint's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkDemetrius] = $"Demetrius's Cum/300/15/Basic {TempRefs.CumType}/Demetrius's Cum/A bottle of Demetrius's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkElliott] = $"Elliott's Cum/300/15/Basic {TempRefs.CumType}/Elliott's Cum/A bottle of Elliott's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkGeorge] = $"George's Cum/300/15/Basic {TempRefs.CumType}/George's Cum /A bottle of George's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkGil] = $"Gil's Cum/300/15/Basic {TempRefs.CumType}/Gil's Cum/A bottle of Gil's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkGunther] = $"Gunther's Cum/300/15/Basic {TempRefs.CumType}/Gunther's Cum/A bottle of Gunther's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkGus] = $"Gus's Cum/300/15/Basic {TempRefs.CumType}/Gus's Cum/A bottle of Gus's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkHarv] = $"Harvey's Cum/300/15/Basic {TempRefs.CumType}/Harvey's Cum /A bottle of Harvey's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkKent] = $"Kent's Cum/300/15/Basic {TempRefs.CumType}/Kent's Cum /A bottle of Kent's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkLewis] = $"Lewis's Cum/300/15/Basic {TempRefs.CumType}/Lewis's Cum/A bottle of Lewis's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkLinus] = $"Linus's Cum/300/15/Basic {TempRefs.CumType}/Linus's Cum/A bottle of Linus's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkMarlon] = $"Marlon's Cum/300/15/Basic {TempRefs.CumType}/Marlon's Cum /A bottle of Marlon's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkMorris] = $"Morris's Cum/300/15/Basic {TempRefs.CumType}/Morris's Cum /A bottle of Morris's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkQi] = $"Mr. Qi's Cum/300/15/Basic {TempRefs.CumType}/Mr. Qi's Cum /A bottle of Mr. Qi's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkPierre] = $"Pierre's Cum/300/15/Basic {TempRefs.CumType}/Pierre's Cum /A bottle of Pierre's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkSam] = $"Sam's Cum/300/15/Basic {TempRefs.CumType}/Sam's Cum/A bottle of Sam's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkSeb] = $"Sebastian's Cum/300/15/Basic {TempRefs.CumType}/Sebastian's Cum/A bottle of Sebastian's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkShane] = $"Shane's Cum/300/15/Basic {TempRefs.CumType}/Shane's Cum/A bottle of Shane's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkWilly] = $"Willy's Cum/300/15/Basic {TempRefs.CumType}/Willy's Cum/A bottle of Willy's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkWiz] = $"Wizard's Cum/300/15/Basic {TempRefs.CumType}/Wizard's Cum /A bottle of Wizard's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

            //recipes
            _objectData[TempRefs.ProteinShake] = $"Protein shake/50/15/Cooking -7/'Protein' shake/Shake made with extra protein/Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkShake] = $"Milkshake/50/15/Cooking -7/'Special' Milkshake/Extra milky milkshake./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
        }
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
            if (!asset.AssetNameEquals("Data/Quests"))
                return;

            data = asset.AsDictionary<int, string>().Data;
            //data[ID] = $"Type/Name/Description/Hint/Condition/Next Quest/Gold/Reward Description/Cancellable/Completion Text";
            data[TempRefs.QuestID1] = $"ItemDelivery/Abigail's Eggplant/Abigail needs an eggplant for her cam show. Make sure it's a good one/Bring Abigail an eggplant./Abigail 272/h{TempRefs.QuestIDWait}/350/-1/false/Wow, it's so big!. I'll be thinking of you tonight, @. Be sure to watch my show.";
            data[TempRefs.QuestID2] = $"ItemDelivery/Abigail's Carrot/Abigail needs a cave carrot to scratch an itch. Bring her one to 'fill' a need/Bring Abigail a cave carrot/Abigail 78/h{TempRefs.QuestIDWait}/410/-1/false/I hope you washed it! Those caves are wonderful, but the cave carrot needs to be SUPER clean before its going anywhere near my ass.";
            data[TempRefs.QuestID3] = $"ItemDelivery/Abigail's Radishes/Abigail wants some radishes for a new idea she had/Bring Abigail Radishes/Abigail 264/h{TempRefs.QuestIDWait}/240/-1/false/I'm gonna have so much fun with these! How many do you think I can fit?";
            data[TempRefs.QuestID4] = $"Location/Abigail's 'helping hand'/Abigail wants you to help her with her show tonight. Go visit her at her house, and bring her an amethyst./Go to Abigail's house with an Amethyst/SeedShop/{TempRefs.QuestIDWait}/500/-1/false/I'm so glad you could come over and give me a helping 'hand'. My viewers are going to appreciate it as well...";
            data[TempRefs.QuestIDWait] = $"Basic/Wait for Abigail/Give Abigail some time to do her show and contact you for the next request/Wait for Abigail/594800/-1/500/-1/false";
            
            

        }
    }

}
