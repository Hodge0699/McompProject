using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Difficulty
{

    public enum DifficultyLevel { EASY, MEDIUM, HARD };
    private static DifficultyLevel currentLevel = DifficultyLevel.MEDIUM;

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
    { currentLevel = difficulty; }

    /// <summary>
    /// Gets the current difficulty
    /// </summary>
    public static DifficultyLevel GetDifficulty()
    { return currentLevel; }

    /// <summary>
    /// Returns the current difficulty level modifer.
    /// </summary>
    /// <returns>Float multiplyer.</returns>
    public static float getModifier()
    { return levelModifier[currentLevel]; }
}