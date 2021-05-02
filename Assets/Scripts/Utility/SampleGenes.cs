using System.Security;

namespace Organism {
    public class SampleGenes {
        public static Gene[] geneArray = new Gene[] {
            new Gene("mouse", OrganismType.ANIMAL, DietType.BOTH, new float[] {
                10f, 120f, 20f, 10f, 
                50f, 100f, 40f, 10f,
                1f, 4f, 50f, 1f, 
                0.2f, 10f, 1000f
            }),
            new Gene("lion", OrganismType.ANIMAL, DietType.ANIMAL, new float[] {
                15f, 120f, 25f, 10f, 
                150f, 100f, 40f, 10f,
                1f, 4f, 50f, 3f,
                0.4f, 4f, 1000f
            })
        };

        public static Gene[] mates = new Gene[] {
            new Gene("mouse", Gender.MALE, OrganismType.ANIMAL, DietType.BOTH, new float[] {
                10f, 120f, 20f, 10f, 
                50f, 100f, 40f, 10f,
                1f, 4f, 50f, 1f, 
                0.2f, 10f, 1000f
            }),
            new Gene("mouse", Gender.FEMALE, OrganismType.ANIMAL, DietType.BOTH, new float[] {
                10f, 120f, 20f, 10f, 
                50f, 100f, 40f, 10f,
                1f, 4f, 50f, 1f, 
                0.2f, 10f, 1000f
            }),
        };

        public static Gene plantGene = new Gene("plant1", OrganismType.PLANT, DietType.PLANT, new float[] {
                0f, 0f, 0f, 0f, 
                50f, 100f, 0f, 0f,
                0f, 0f, 1f, 1f,
                10f, 0f, (7f *24)     //2 days = (2*24)
        });
    }
}