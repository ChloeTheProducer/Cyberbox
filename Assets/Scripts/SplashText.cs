using UnityEngine;
using TMPro;
public class SplashText : MonoBehaviour
{
    // If you, a player manages to find this... HOW?!
    public TMP_Text splashText;
    // Array of splash text
    private string[] splashTexts =
    {
        "How's the weather", "Hey! Where's Perry?",
        "Too much benadryl and you'll be meeting the grayman", "Always piss in aisle 4... It's a security camera blindspot",
        "How much rain is too much", "What's 9 + 10", "Creeper... Aw man!", "*Windows 95 Startup Sound*", "*Windows XP Startup Sound*",
        "The cake is a lie", "The lie is a cake", "The beans be crazy", "Roxanne Wolf is cute", "This software is enhanced with HDRP",
        "Nuh uh", "I know what's wrong with it, ain't got no gas in it!", "GRAND DAD!", "Omae wa mu, shinderu", "I'm the trash man", "Can I offer you an egg in this trying time?",
        "Anyway, I started blastin", "This splash text be bussin'", "Try and find the secret", "public bool CyberboxIsCool = true;", "This splash text is held together by duct tape and dreams",
        "Wanna play Showtape Central?", "I'm a goofy goober!", "You're a goofy goober!", "We're all goofy goobers!", "Goofy goofy goofy goober!", "*GMod collision noises*", "Ah, Hello Gordon",
        "Cyberbox is brought to you by animatronic enthusiasts like you, Thank You", "Say goodbye to your knee caps chucklehead!", "The spy is a spy", "CEC Corp. is sus", "Chuck E. Cheese was not the imposter", "Quokka's are adorable",
        "Heat from fire, fire from heat", "Mom said it's my turn to program the animatronics", "The only really good thing to come out of Security Breach is Roxanne Wolf",
        "Do a flip", "Skibidi Toilet. Is. Fucking. Dumb.", "Liminal spaces and vaporwave are two things that somehow work really well together",
        "Just click play", "The man is in the source", "When you're in, you're in for a real good time", "When you're here, you're family",
        "Keep your stick on the ice", "This is software, it can change, if it has to, I guess",
        "If at first you dont succeed, use more duct tape", "Quando Omni Flunkus Moritati",
        "If it ain't broke, you're not trying.",
        "The Rock and Roll Rebellion is the band of all time", "Where there's a will... there's probably not a way", "Where there's a will...", "If it's not working... try harder",
        "All toasters toast toast", "Sweep it under the rug... It's probably fine", "DO NOT THE COYOTE", "We should start the winning now",
        "Money will not wait forever", "You cannot hide from pink pony heavy", "It's Perfect", "Is good day for 6s", "You are best of best", "It is good day to be giant man",
        "We make good team", "Hm, is nice", "Sandvich make me strong", "Counter-terrorists win", "Are we rushing, or we going sneaky deaky like", "Easy peasy lemon squeazy", "Bingo bango bongo bish bash bosh",
        "Don’t Smoke Crayolas!", "That medic is jschlatt", "Femboys??? Would", "F1nn5ter??? Would", "What the flip", "swing, your, arms, from side to side, and now you're doin the mario", 
        "Would ya turn off the dog... TURN OFF THE FRUITCAKE DOG... THE BUTTON ON THE LEFT", "makin pancakes, makin bacon pancakes, take some bacon and I put it in a pancake, bacon pancakes thats what its gonna make, baacooon pancaaaaaakes.", 
        "Connection Terminated", "Have a day", "He's here, he's there, he's everywhere. who you gonna call? Psychic friend Fredbear", "Teehee nyaa~ :3"
    };

    // Display Random splash text on start
    void Start()
    {
        SetRandomSplashText();
        splashText = GetComponent<TMP_Text>();
    }
    
    void SetRandomSplashText()
    {
        int index = Random.Range(0, splashTexts.Length);
        splashText.text = splashTexts[index];
    }
}
