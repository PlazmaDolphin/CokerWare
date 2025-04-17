using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SentenceGenerator : MonoBehaviour{
    private static System.Random random = new System.Random();
    private const int MAXCHARS = 140;

    //For sentence generation, please modify for funny results
    string[] starts = {"Hey dummy, ", "You better ", "You need to ", "Sir, please "};
    string[] negationWords = {"not ", "never ", "avoid ", "skip ", "ignore ", "refrain from ", "steer clear of ", "forgo ", "disregard "};
    string[] negationsIng = {"not ", "never ", "avoiding ", "skipping ", "ignoring ", "refraining from ", "steering clear of ", "forgoing ", "disregarding "};
    string[] filler = {"just ", "simply ", "perfectly ", "quickly ", "properly ", "immediately ", "actually "};
    string[] ends = {" or you'll die!", " to defuse the bomb!", " to save everyone!", ". Hurry up!", ". Go ahead, Cut!"};
    string[] endsNeg = {" or you'll live!", " to detonate the bomb!", " to kill everyone!"};
    public string ConstructCommand(int negations, string theWire)
    {
        /*
        RULE: DO NOT EXCEED 120 Characters!
        1. Core Phrase (16-18)
        1. Choose Ending (9-21)
        2. Choose Start (9-12)
        3. Choose Negations (4-18)
        4. Filler (5-10)*/
        int charsLeft = MAXCHARS;
        string core = "cutting the " + theWire + " wire";
        string start = starts[random.Next(starts.Length)];
        string end = "";
        if (UnityEngine.Random.value < 0.3f){
          end = endsNeg[random.Next(endsNeg.Length)]; //30% chance of negative ending
          negations -= 1; //counts as a negative phrase
        } else {
          end = ends[random.Next(ends.Length)];  
        }
        charsLeft -= core.Length + start.Length + end.Length;
        //Force first word to be a verb to eliminate grammar BS
        string firstNeg = negations==0 ? "" : negationWords[random.Next(2, negationWords.Length)];
        negations -= 1;
        //Choose combinations of negations, keep going until it fits in character limit or 50 tries
        List<string> negs = new List<string>();
        for (int i=0; i<50; i++){
          negs.Clear();
          for (int j=0; j<negations; j++){
            int r = random.Next(negationWords.Length);
            negs.Add(negationsIng[r]);
          }
          if (negs.Sum(s => s.Length) <= charsLeft){
            break;
          }
        }
        charsLeft -= negs.Sum(s => s.Length);
        List<string> fill = new List<string>();
        //Add filler until running out of space, then delete last filler
        for (int i=0; i<negations+2; i++){
          int r = random.Next(filler.Length);
          fill.Add(filler[r]);
          if (fill.Sum(s => s.Length) <= charsLeft){
            //delete last filler
            //fill.RemoveAt(fill.Count-1);
            break;
          }
        }
        //Randomly choose strings from the lists
        List<string> all = new List<string>();
        all.AddRange(negs);
        all.AddRange(fill);
        string theMeat = string.Join("", all.OrderBy(x => Guid.NewGuid()));
        //Put it all together :)
        return start + firstNeg + theMeat + core + end;
    }
}