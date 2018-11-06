import {Card as CardContract} from "../contract/card";

export class Card {
    Suit: string;
    Rank: string;
    Folded: boolean = true;
    ImgFilePath: string;
    Selected: boolean = false;
    Used: boolean = false;
  
    toggleSelect(){
        if (!this.Selected && this.Used){
            throw Error("Cannot select a used card!");
        }
        this.Selected = !this.Selected;
    }
  
    getImgFilePath() : string {
        if (this.Folded)
            return "../../assets/cards/folded.png";
        else
            return "../../assets/cards/" + this.Rank + "_of_" + this.Suit + "s.png";
    }

    getTextImg(): string{
        if (this.Folded) {
            return '♚';
        }
        else {
            return this.getTextForSuit() + this.getTextForRank();
        }
    }

    getTextForSuit(): string{
        switch(this.Suit)
        {
            case 'heart':
                return '♥';
            case 'spade':
                return '♠';
            case 'diamond':
                return '♦';
            case 'club':
                return '♣';
            default:
                return 'error_suit';
        }
    }

    getTextForRank(): string{
        switch (this.Rank){
            case 'ace':
                return 'A';
            case 'king':
                return 'K';
            case 'queen':
                return 'Q';
            case 'jack':
                return 'J';
            case '10':
                return 'T';
            default:
                return this.Rank;
        }
    }
  
    isUsed(): boolean{
        return this.Used;
    }
  
    onConsoleClick() {
        if (this.Used){
            return;
        }
        this.Selected = !this.Selected;
    }
  
    setCard(card: CardContract){
        console.log('setting ' + JSON.stringify(card));
        this.Suit = card.Suit;
        this.Rank = card.Rank;    
        this.Folded = false;    
    }
}
