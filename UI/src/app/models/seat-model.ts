import { Card } from "./card";
import { PlayerStatus } from "./player-status";

export class SeatModel {
    hole1: Card = new Card();
    hole2: Card = new Card();
    
    //before round starts
    isHero: boolean;
    isButton: boolean;
    isHisTurn: boolean;
    isSittingOut: boolean;

    playserStatus: PlayerStatus;

    constructor(){
        this.hole1 = new Card();
        this.hole2 = new Card();

        this.isHero = false;
        this.isButton = false;   
        this.isSittingOut = false;     
    }
}
