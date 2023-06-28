using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour {
    public IngredientCard[] ingredientCard;
    public RecipeCard[] recipeCard;

    public byte FindIngredientCard(IngredientCard ingredientCardToFind) {
        for (int possibleCardIndex = 0; possibleCardIndex < ingredientCard.Length; possibleCardIndex++) {
            if (ingredientCardToFind == ingredientCard[possibleCardIndex]) {
                return (byte)possibleCardIndex;
            }
        }

        return 0;
    }

    public byte FindRecipeCard(RecipeCard recipeCardToFind) {
        for (int possibleCardIndex = 0; possibleCardIndex < recipeCard.Length; possibleCardIndex++) {
            if (recipeCardToFind == recipeCard[possibleCardIndex]) {
                return (byte)possibleCardIndex;
            }
        }

        return 0;
    }
}