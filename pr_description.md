Title: 🧹 [code health improvement] Refactor CraftingBench.cs to reduce code duplication

Description:
🎯 **What:** The code health issue addressed
Extracted duplicated crafting logic into a helper method `TryCraft(int woodCost, int stoneCost, string itemName)` in `CraftingBench.cs`.

💡 **Why:** How this improves maintainability
This reduces boilerplate code and makes the `CraftingBench` class easier to read and maintain. If the crafting logic changes in the future, it only needs to be updated in one place.

✅ **Verification:** How you confirmed the change is safe
Created a temporary .NET project with mock UnityEngine classes and successfully built `CraftingBench.cs` using `dotnet build` to ensure no syntax errors were introduced. Requested code review to verify functionality preservation.

✨ **Result:** The improvement achieved
Successfully removed duplicate blocks of code across `CraftPickaxe`, `CraftAxe`, `CraftSword`, and `CraftBow`, resulting in cleaner, more centralized crafting logic with preserved behavior.
