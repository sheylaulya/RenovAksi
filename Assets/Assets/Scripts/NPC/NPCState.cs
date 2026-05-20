
public enum NPCStates
{
    AtHome,
    Working,
    Praying,
    Sleeping,
    Patrolling
}


/// <summary>
/// Komponen AI utama tiap NPC.
/// Berlangganan ke DayTimeCycle.OnHourChanged dan
/// memperbarui state FSM setiap jam game berganti.
/// </summary>