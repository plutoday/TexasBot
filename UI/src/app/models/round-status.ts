import { PlayerStatus } from "./player-status";
import { Card } from "./card";
import { Decision } from "../contract/decision";

export class RoundStatus {
    started: boolean;
    player0: PlayerStatus;
    player1: PlayerStatus;
    player2: PlayerStatus;
    player3: PlayerStatus;
    player4: PlayerStatus;
    player5: PlayerStatus;
    roundId: string;
    heroHole1: Card;
    heroHole2: Card;
    flop1: Card;
    flop2: Card;
    flop3: Card;
    turn: Card;
    river: Card;
    stage: stageEnum;
    currentPlayer: PlayerStatus;
    buttonIndex: number;
    heroDecision: Decision;
    streetRaised: boolean;

    digits: Array<number>;

    constructor(){
        this.stage = stageEnum.NotStarted;
        this.digits = new Array<number>();
        this.digits.push(0);
        this.digits.push(0);
        this.digits.push(0);
        this.digits.push(0);
        this.digits.push(0);
        this.digits.push(0);
    }
}

export enum stageEnum{
    NotStarted,
    Preflop,
    Flop,
    Turn,
    River,
    Showhand,
}
