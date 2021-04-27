namespace Organism {

    public enum OrganismState {
        IDLE,
        SEEKING_FOOD,
        CHASING_FOOD,
        ATTACKING,
        //EATING,
        EVADING,
        SEARCHING_MATE,
        CHASING_MATE,
        FITNESS_CHECK,
        //MATING,
        REST,
        SLEEP, // Dont use for now
        DEATH,
    }
}