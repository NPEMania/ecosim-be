using System;
using Organism;

class UtilityMethods {

    public static bool IsEdible(Gene consumer, Gene food) {
        // TODO: can check predator prey in gene
        if (consumer.dietType == DietType.BOTH) {
            return true;
        } else if (consumer.dietType == DietType.ANIMAL && food.organismType == OrganismType.ANIMAL) {
            return true;
        } else if (consumer.dietType == DietType.PLANT && food.organismType == OrganismType.PLANT) {
            return true;
        }
        return false;
    }
}