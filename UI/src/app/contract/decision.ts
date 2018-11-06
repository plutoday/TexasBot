export class Decision {
    DecisionType: string;
    ChipsAdded: number;
}

export enum DecisionTypeEnum {
        Undefined,
        Ante,
        Check,
        Fold,
        Raise,
        Reraise,
        Call,
        AllIn,
        AllInRaise,
    }