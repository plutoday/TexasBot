import { Decision } from "./decision";

export class NotifyDecisionRequest {
    Decision: Decision;
    PlayerName: string;
    RoundId: string;
}
