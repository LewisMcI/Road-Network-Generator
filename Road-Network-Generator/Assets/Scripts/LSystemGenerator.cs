using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LSystemGenerator : MonoBehaviour
{
    // Initial String
    public string axiom;

    // Active Rules
    public Rule[] rules;

    // No. of iterations for the axiom to go through.

    [Range(0,10)]
    public int iterations;

    // Chance to ignore a branch.
    [Range(0.0f, 1.0f)]
    public float ignoreBranchChance;

    public Visualizer visualizer;

    private void Awake()
    {
        // Display active rulesets in console.
        for (int i = 1; i <= rules.Length; i++)
        {
            Debug.Log("Rule " + i + ": " + rules[i - 1].ToString());
        }
        // Gets start time of the generator.
        DateTime startingTime = DateTime.Now;
        // Draws roads with value generated.
        visualizer.DrawRoads(GenerateValue(axiom));
        // Finds the duration at which the visualizer and LSystemGenerator took to process.
        TimeSpan duration = DateTime.Now.Subtract(startingTime);
        // Prints out time taken.
        Debug.Log(duration.Milliseconds + "ms");
    }

    private String GenerateValue(string axiom)
    {
        // Creates an oldWord and appends initial axiom.
        StringBuilder oldWord = new StringBuilder();
        oldWord.Append(axiom);

        // For every iteration
        for (int iterationIndex = 0; iterationIndex < iterations; iterationIndex++)
        {
            // Creates a newWord every iteration.
            StringBuilder newWord = new StringBuilder();

            // For each rule.
            foreach (var rule in rules)
            {
                // For each letter in the oldWord.
                for (int i = 0; i < oldWord.Length; i++)
                {
                    // If the current letter in the oldWord is in the current rule we append this to the neWord.
                    if ((oldWord[i] == rule.alphabet) && (UnityEngine.Random.Range(0, 100) / 100.0f > ignoreBranchChance))
                    {
                        newWord.Append(rule.GetValue());
                    }
                    // If it isn't, we append the letter from the oldWord.
                    else
                    {
                        newWord.Append(oldWord[i]);
                    }
                }
            }
            // The word we just generated is now the old word.
            oldWord = newWord;
        }
        // Returning the final value generated.
        return oldWord.ToString();
    } 
}
