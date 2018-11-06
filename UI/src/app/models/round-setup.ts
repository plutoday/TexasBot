export class RoundSetup {
    heroIndex: number;
    playerNames: Array<string>;
    stackSizes: Array<number>;
    bigBlindSize: number;

    constructor(){
        this.bigBlindSize = 10;
        this.playerNames = new Array<string>();
        this.playerNames.push('Player0');
        this.playerNames.push('Player1');
        this.playerNames.push('Player2');
        this.playerNames.push('Player3');
        this.playerNames.push('Player4');
        this.playerNames.push('Player5');

        this.stackSizes = new Array<number>();
        this.stackSizes.push(10000);
        this.stackSizes.push(10000);
        this.stackSizes.push(10000);
        this.stackSizes.push(10000);
        this.stackSizes.push(10000);
        this.stackSizes.push(10000);
    }
}
