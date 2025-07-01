namespace Dajjsand.Controllers.Craft
{
    public class CraftController
    {
        // public bool TryCraft(List<string> inputResources, out string result)
        // {
        //     foreach (var recipe in _currentLevelRecipes.availableRecipes)
        //     {
        //         if (RecipeMatches(recipe, inputResources))
        //         {
        //             result = recipe.result;
        //             return true;
        //         }
        //     }
        //
        //     result = null;
        //     return false;
        // }
        //
        // private bool RecipeMatches(CraftRecipe recipe, List<string> inputResources)
        // {
        //     var required = recipe.ingredients
        //         .GroupBy(i => i.resourceName)
        //         .ToDictionary(g => g.Key, g => g.Sum(i => i.amount));
        //
        //     var inputGrouped = inputResources
        //         .GroupBy(r => r)
        //         .ToDictionary(g => g.Key, g => g.Count());
        //
        //     foreach (var req in required)
        //     {
        //         if (!inputGrouped.TryGetValue(req.Key, out var inputAmount) || inputAmount < req.Value)
        //             return false;
        //     }
        //
        //     return true;
        // }
    }
}