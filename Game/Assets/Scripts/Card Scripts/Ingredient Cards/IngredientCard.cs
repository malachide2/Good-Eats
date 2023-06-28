using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Ingredient Card")]
public class IngredientCard : ScriptableObject {
    public new string name;
    public Sprite artwork;
}