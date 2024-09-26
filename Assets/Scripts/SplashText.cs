using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SplashText : MonoBehaviour
{
    // If you, a player manages to find this... HOW?!
   
    private float timer;
    private float animationDuration = 3f; // Duration of the animation in seconds
    private float colorChangeDuration = 0.5f; // Duration for color change
    public TMP_Text splashText;
    // Array of splash text
    private string[] splashTexts = {
                "How's the weather", "Hey! Where's Perry?",
                "Too much benadryl and you'll be meeting the grayman", "Always piss in aisle 4... It's a security camera blindspot",
                "How much rain is too much", "What's 9 + 10", "Creeper... Aw man!", "*Windows 95 Startup Sound*",
                "The cake is a lie", "The lie is a cake", "The beans be crazy", "Roxanne Wolf is cute", "This RR Engine game is enhanced with URP",
                "Nuh uh", "I know what's wrong with it, ain't got no gas in it!", "GRAND DAD!", "Omae wa mu, shinderu", "I'm the trash man", "Can i offer you an egg in this trying time?",
                "Anyway, I started blastin", "This splash text be bussin", "Try and find the secret", "public bool MMRIsCool = true;", "This splash text is held together by duct tape and dreams",
                "Wanna play Animal Crossing?", "I'm a goofy goober!", "You're a goofy goober!", "We're all goofy goobers!", "Goofy goofy goofy goober!", "*GMod collision noises*", "Ah, Hello Gordon",
                "Major Magic's Replay is brought to you by players like you, Thank You", "Say goodbye to your knee caps chucklehead!", "The spy is a spy", "CEC Corp. is sus", "Sgt. Pepperoni was not the imposter", "Quokka's are adorable",
                "Heat from fire, fire from heat", "Mom said it's my turn to program the animatronics", "The only really good thing to come out of Security Breach is Roxanne Wolf",
                "Do a flip", "Skibidi Toilet. Is. Fucking. Dumb.", "youtube.com/watch?v=xconDALayoU", "Liminal spaces and vaporwave are two things that somehow work really well together",
                "Just click play", "The man is in the source", "When you're in, you're in for a real good time", "When you're here, you're family",
                "Keep your stick on the ice", "This week in Major Magic's Replay...", "This is a game, it can change, if it has to, I guess",
                "If at first you dont succeed, use more duct tape", "Quando Omni Flunkus Moritati",
                "If it ain't broke, you're not trying.",
                "The Rock and Roll Rebellion is the band of all time", "Where there's a will... there's probably not a way", "Where there's a will...", "If it's not working... try harder",
                "All toasters toast toast", "Sweep it under the rug... It's probably fine", "DO NOT THE COYOTE", "https://www.youtube.com/watch?v=GZ20x4qBJmM", "We should start the winning now",
                "Money will not wait forever", "You cannot hide from pink pony heavy", "It's Perfect", "Is good day for 6s", "You are best of best", "It is good day to be giant man",
                "We make good team", "Hm, is nice", "Sandvich make me strong", "Counter-terrorists win", "Are we rushing, or we going sneaky deaky like", "Easy peasy lemon squeazy", "Bingo bango bongo bish bash bosh",
                "", "", "", "", "", "", "", "", "", "", "", ""
};

    // Display Random splash text on start
    void Start()
    {
        SetRandomSplashText();
        splashText = GetComponent<TMP_Text>();
    }
    
    void Update()
    {
/*         
        // Update the timer
        timer += Time.deltaTime;

        // Animate scaling
        float scale = 1 + Mathf.Sin(timer * Mathf.PI * 2 / animationDuration) * 0.1f; // Scaling effect
        splashText.transform.localScale = new Vector3(scale, scale, 1);
*/        
    }
    
    void SetRandomSplashText()
    {
        int index = Random.Range(0, splashTexts.Length);
        splashText.text = splashTexts[index];
    }
}
