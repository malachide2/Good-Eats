using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card", menuName = "Card/Recipe Card")]
public class RecipeCard : ScriptableObject {
    public new string name;
    public Sprite artwork;

    public IngredientCard[] ingredientList;

    public int pointValue;
}