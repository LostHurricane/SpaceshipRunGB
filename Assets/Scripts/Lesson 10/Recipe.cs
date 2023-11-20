using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace SpaceshipRunGB.Lesson10
{
	public class Recipe : MonoBehaviour
	{
        public Ingredient PotionResult;
        public Ingredient[] PotionIngredients;

        [RangeAttribute(1, 6)]
        public int pp;

    }
}