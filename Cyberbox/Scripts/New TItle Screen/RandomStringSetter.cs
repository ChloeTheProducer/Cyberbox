using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class RandomStringSetter : MonoBehaviour
{
    string[] stringArray = {
        "Did you know that Crusty the Cat is a cat? Me neither!",
        "Yeah, fun facts would be kinda neat to have.",
        "Praise our Lord and Savior Pasqually",
        "Did you know, I forgot.",
        "randombeans was here",
        "Did you know, Harmony Howlette is in my walls. Send help.",
        "The moon became the moon during Concept Unification.",
        "Did you know, settings hasn't worked for 10 versions!",
        "Showtape Central was originally called RTR+",
        "Showtape Central once had a stage with 15 TVs in it",
        "A few party stages were made in SC, but were later removed.",
        "This is the only mod to have ReminaProd's Full Cyberamics",
        "Where did Family Times go.",
        "Laughter.",
        "I bet the text above says 3-Stage",
        "The Moon is the only survivor of Concept Unification",
        "In Russia, Building is Building",
        "A man once said, 'Don't be racist, I am a building'",
        "Crusty the Cat was removed due to a large possession of Catnip",
        "A bear saved a dying rat",
        "BIRB",
        "Madame Oink was arrested for cannibalism",
        "SS was founded in October 2022",
        "If HedgeWedge sees this, I ate his sandwich",
        "Sweet, we have quotes now?",
        "TextMeshProUGUI component is not assigned.",
        "hey random - HedgeWedge",
        "ReminaProd is an awesome person",
        "String arrays are so satisfying",
        "Have you tried turning it off and on again?",
        "Is this the police? Then which country am I speaking to?",
        "Beep boop",
        "Munch Jr was found dead in Miami",
        "DON'T GO TO SWIFT'S AT 3AM (THE BOTS ATE MY PIZZA)",
        "A powerful rat, named Charles Entertainment Cheese",
        "Mooooo.",
        "Null",
        "OK.",
        "Pickles",
        "Chips!",
        "My spoon burnt down",
        "I am the reader.",
        "Don't overdo arm raises, please.",
        "Please wait. I said wait. WAIT.",
        "I heard this map is really neat.",
        "Did you know, the discord server decided all these..things.",
        "With assistance from Showtape Selection",
        "fluffernutter",
        "A ping a day keeps the randombeans away.",
        "He sleeps.",
        "The time is currently not 25:34",
        "This is a rare message",
        "Mueheheheheheheheheheh.",
        "Typo",
        "The65thGamer",
        "In Russia, the animatronics collect you.",
        "DON'T STEAL THE FAX MACHINE",
        "The Industrial Revolution was a period of global transi",
        
    };

    private void Start()
    {
        // Get a random index within the array bounds
        int randomIndex = UnityEngine.Random.Range(0, stringArray.Length);

        this.GetComponent<TMP_Text>().text = stringArray[randomIndex];
    }
}
