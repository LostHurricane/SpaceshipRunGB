using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace SpaceshipRunGB.Lesson10
{
    [Serializable]
    public class Ingredient
    {
        public string Name;
        public int Amount = 1;
        public IngredientUnit Unit;
    }

    public enum IngredientUnit
    {
        Spoon,
        Cup,
        Bowl,
        Piece
    }

}