export class ExpectedAction {
    Action: string;
    PlayerName: string;
}

export enum ExpectedActionEnum {
    StartNewRound,
    HeroHoles,
    Flops,
    Turn,
    River,
    Decision,
    VillainHoles,
}
