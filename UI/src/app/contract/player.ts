export class Player {
    name: string;
    stackSize: number;
    sittingOut: boolean;

    constructor(name: string, stackSize: number, isSittingOut: boolean){
        this.name = name;
        this.stackSize = stackSize;
        this.sittingOut = isSittingOut;
    }
}
