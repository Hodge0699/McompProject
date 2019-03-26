using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Difficulty
{
    public enum DifficultyLevel { EASY, MEDIUM, HARD };
    private static DifficultyLevel currentDifficulty = DifficultyLevel.MEDIUM;

    // Modifer to apply to enemy frequency, bullet damage, etc. depending on current difficulty level.
    private static Dictionary<DifficultyLevel, float> levelModifier = new Dictionary<DifficultyLevel, float>()
    { {DifficultyLevel.EASY, 0.75f  },
      {DifficultyLevel.MEDIUM, 1.0f },
      {DifficultyLevel.HARD, 1.5f   } };

    /// <summary>
    /// Sets the current difficulty
    /// </summary>
    /// <param name="difficulty">New difficulty</param>
    public static void setDifficulty(DifficultyLevel difficulty)
    { currentDifficulty = difficulty; }

    /// <summary>
    /// Gets the current difficulty
    /// </summary>
    public static DifficultyLevel getDifficulty()
    { return currentDifficulty; }

    /// <summary>
    /// Converts the currentDifficulty to a string
    /// </summary>
    /// <param name="convertFromUpper">Should this function convert the string from enum-style all uppercase.</param>
    /// <returns>currentDifficulty as a string</returns>
    public static string getDifficultyString(bool convertFromUpper = true)
    {
        char[] difficulty = currentDifficulty.ToString().ToCharArray();

        if (!convertFromUpper)
            return new string(difficulty);

        // Make every letter after first lower case
        for (int i = 1; i < difficulty.Length; i++)
            difficulty[i] = char.ToLower(difficulty[i]);

        return new string(difficulty);
    }

    /// <summary>
    /// Returns the current difficulty level modifer.
    /// </summary>
    /// <returns>Float multiplyer.</returns>
    public static float getModifier()
    { return levelModifier[currentDifficulty]; }

    /// <summary>
    /// Attempts to increase the difficulty by one step, loops back around to easiest if already at max
    /// </summary>
    /// <returns>The current difficulty</returns>
    public static DifficultyLevel stepDifficulty()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.EASY:
                currentDifficulty = DifficultyLevel.MEDIUM;
                break;
            case DifficultyLevel.MEDIUM:
                currentDifficulty = DifficultyLevel.HARD;
                break;
            case DifficultyLevel.HARD:
                currentDifficulty = DifficultyLevel.EASY;
                break;
            default:
                break;
        }

        return currentDifficulty;
    }
}