import {Player} from './player';

export class NewRoundRequest {
    Players: Array<Player>;
    HeroIndex: number;
    ButtonIndex: number;
    BigBlindSize: number;
    SmallBlindSize: number;
}
