namespace Organism {

    public enum OrganismState {
        IDLE,
        SEEKING_FOOD,
        CHASING_FOOD,
        ATTACKING_FOOD,
        EATING,
        EVADING,
        SEARCHING_MATE,
        REST,
        SLEEP, // Dont use for now
        DEATH,
    }
}