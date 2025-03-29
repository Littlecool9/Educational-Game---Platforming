using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Sentence", menuName = "EducationalGame/Sentence")]
public class Sentence : ScriptableObject
{
    public List<string> corpus = new List<string>();
    public string sentence;

    public List<string> GetLastWords(int count)
    {
        var words = sentence.Split(' ');
        int i = Math.Min(count, words.Length);
        return words.Skip(words.Length - i).ToList();
    }
}
