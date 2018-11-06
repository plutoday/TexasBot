import { Player } from "../contract/player";

export class PlayerStatus {
    name: string;
    stackSize: number;
    folded: boolean;
    allIn: boolean;
    polled: boolean;
    absent: boolean;
    position: string;
    isHero: boolean;

    set(player: Player, myIndex: number, buttonIndex: number, heroIndex: number) {
        this.name = player.name;
        this.stackSize = player.stackSize;
        this.folded = false;
        this.polled = false;
        this.allIn = false;
        this.absent = false;
        this.position = this.getPosition(myIndex, buttonIndex);
        this.isHero = (myIndex == heroIndex);
        this.absent = player.sittingOut;
    }

    getPosition(myIndex: number, buttonIndex: number) : string {
        let diff = (myIndex + 6 - buttonIndex) % 6;
        switch(diff){
            case 0:
                return 'BTN';
            case 1:
                return 'SB';
            case 2:
                return 'BB';
            case 3:
                return '';
            case 4:
                return '';
            case 5:
                return '';
        }
    }
}

enum positionEnum{
    SmallBlind,
    BigBlind,
    UnderTheGun,
    MiddlePosition,
    CuttingOff,
    Button,
}
