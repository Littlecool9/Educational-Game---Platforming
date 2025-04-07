using System;
using System.Collections.Generic;
using System.Linq;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "Sentence", menuName = "EducationalGame/Sentence")]
public class SentenceObject : ScriptableObject, IBridge
{
    [Tooltip("The Complete sentence as ANSWER")]
    public string sentence;

    public List<string> corpus = new List<string>();
    public int missingWordCount;

    public List<string> GetMissingWords()
    {
        var words = sentence.Split(' ');
        int i = Math.Min(missingWordCount, words.Length);
        return words.Skip(words.Length - i).ToList();
    }

    public string GetMissingSentence()
    {
        var words = sentence.Split(' ');
        int i = Math.Min(missingWordCount, words.Length);
        return string.Join(" ", words.Skip(words.Length - i));
    }

    public string GetIncompleteSentence()
    {
        var words = sentence.Split(' ');
        int i = Math.Min(missingWordCount, words.Length);
        return string.Join(" ", words.Take(words.Length - i)) + " ";
    }

    public void LinkEntity(IComponent component)
    {
        if (component is IncompleteSentenceComponent) { LinkEntity(component as IncompleteSentenceComponent); }
        else throw new Exception("Invalid Component Type");
    }

    private void LinkEntity(IncompleteSentenceComponent component)
    {
        component.corpus = corpus;
        component.missingWordCount = missingWordCount;
        component.fullSentence = sentence;
        component.incompleteSentence = "(missing "+ missingWordCount + " words)\n" + GetIncompleteSentence();
        component.answer = GetMissingSentence();
    }
}
