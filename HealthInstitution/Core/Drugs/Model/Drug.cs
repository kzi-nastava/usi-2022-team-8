using HealthInstitution.Core.Ingredients.Model;

namespace HealthInstitution.Core.Drugs.Model;

public class Drug
{
    public int Id { get; set; }    
    public String Name { get; set; }
    public DrugState State { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public String RejectionReason { get; set; }

    public Drug(int id, string name, DrugState state, List<Ingredient> ingredients, string rejectionReason = "")
    {
        this.Id = id;
        this.Name = name;
        this.State = state;
        this.Ingredients = ingredients;
        this.RejectionReason = rejectionReason;
    }

    public override string ToString()
    {
        return this.Name;
    }

    public bool ContainsIngredient(Ingredient ingredient)
    {
        return Ingredients.Contains(ingredient);
    }
}

public enum DrugState
{
    Created,
    Accepted,
    Rejected
}