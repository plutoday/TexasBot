import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { RoundStatus, stageEnum } from "./models/round-status";
import { Injectable } from "@angular/core";
import { Card as CardContract} from "./contract/card";
import {Card} from "./models/card";
import { NewRoundRequest } from "./contract/new-round-request";
import { PlayerStatus } from "./models/player-status";
import { NewRoundResponse } from "./contract/new-round-response";
import { HeroHolesResponse } from "./contract/hero-holes-response";
import { NotifyDecisionResponse } from "./contract/notify-decision-response";
import { GetDecisionResponse } from "./contract/get-decision-response";
import { ExpectedAction } from "./contract/expected-action";
import { FlopsResponse } from "./contract/flops-response";
import { TurnResponse } from "./contract/turn-response";
import { RiverResponse } from "./contract/river-response";
import { RoundSetup } from "./models/round-setup";
import { NotifyDecisionRequest } from "./contract/notify-decision-request";
import { DecisionTypeEnum } from "./contract/decision";
import { GameService } from "./game.service";
//import { ExpectedActionEnum } from "./contract/expected-action";

@Injectable()
export class DataSharingService {

    private roundStatusSource = new BehaviorSubject<RoundStatus>(new RoundStatus());
    private roundSetupSource = new BehaviorSubject<RoundSetup>(new RoundSetup());
  
    currentStatus = this.roundStatusSource.asObservable();
    roundSetup = this.roundSetupSource.asObservable();

    constructor(private gameService: GameService){        
    }

    setup(request: NewRoundRequest){
        let temp = this.roundStatusSource.getValue();
        temp.buttonIndex = request.ButtonIndex;

        temp.player0 = new PlayerStatus();
        temp.player0.set(request.Players[0], 0, request.ButtonIndex, request.HeroIndex);
        
        temp.player1 = new PlayerStatus();
        temp.player1.set(request.Players[1], 1, request.ButtonIndex, request.HeroIndex);
        
        temp.player2 = new PlayerStatus();
        temp.player2.set(request.Players[2], 2, request.ButtonIndex, request.HeroIndex);
        
        temp.player3 = new PlayerStatus();
        temp.player3.set(request.Players[3], 3, request.ButtonIndex, request.HeroIndex);
        
        temp.player4 = new PlayerStatus();
        temp.player4.set(request.Players[4], 4, request.ButtonIndex, request.HeroIndex);
        
        temp.player5 = new PlayerStatus();
        temp.player5.set(request.Players[5], 5, request.ButtonIndex, request.HeroIndex);
        
        this.roundStatusSource.next(temp);

        let tempSetup = this.roundSetupSource.getValue();
        tempSetup.heroIndex = request.HeroIndex;
        this.roundSetupSource.next(tempSetup);
    }

    perceiveNewRoundResponse(response: NewRoundResponse){
        switch(response.Action.Action){
            case 'HeroHoles':
                let temp = this.roundStatusSource.getValue();
                
                temp.roundId = response.RoundId;  
                temp.stage = stageEnum.Preflop;    
                temp.started = true;                 
                temp.streetRaised = true;
                this.roundStatusSource.next(temp);
                break;
            default:
                throw new Error('new round response should contain HeroHoles ');
        }
    }

    perceiveHeroHolesResponse(response: HeroHolesResponse){        
        switch(response.Action.Action){
            case 'Decision':
                this.perceiveExpectedAction(response.Action);
                break;
            default:
                throw new Error('hero holes response should contain only Decision');
        }
    }

    perceiveNotifyDecisionResponse(request: NotifyDecisionRequest, response: NotifyDecisionResponse){
        this.resetChipsDigit();

        if (request.Decision.DecisionType == 'AllIn' 
            || request.Decision.DecisionType == 'AllInRaise'
            || request.Decision.DecisionType == 'Raise'
            || request.Decision.DecisionType == 'Reraise'
            || request.Decision.DecisionType == 'Ante')
        {
            let temp = this.roundStatusSource.getValue();
            temp.streetRaised = true;
            this.roundStatusSource.next(temp);
        }

        switch(response.Action.Action){
            case 'Decision':
                this.perceiveExpectedAction(response.Action);
                break;
            case 'Flops':
            case 'Turn':
            case 'River':
                this.moveToNextStage();
                break;
            default:
                throw new Error('notify decision response should not contain' + response.Action.Action);
        }
    }

    perceiveGetDecisionResponse(response: GetDecisionResponse){
        let temp = this.roundStatusSource.getValue();

        temp.heroDecision = response.Decision;
        if (response.Decision.DecisionType == 'AllIn' 
            || response.Decision.DecisionType == 'AllInRaise'
            || response.Decision.DecisionType == 'Raise'
            || response.Decision.DecisionType == 'Reraise'
            || response.Decision.DecisionType == 'Ante'){
            temp.streetRaised = true;
        }

        this.roundStatusSource.next(temp);

        switch(response.Action.Action){
            case 'Decision':
                this.perceiveExpectedAction(response.Action);
                break;
            case 'Flops':
            case 'Turn':
            case 'River':
                this.moveToNextStage();
                break;
            default:
                throw new Error('notify decision response should not contain' + response.Action.Action);
        }
    }

    perceiveFlopsResponse(response: FlopsResponse){
        switch(response.Action.Action){
            case 'Decision':
                this.perceiveExpectedAction(response.Action);
                break;
            case 'Turn':
            case 'River':
                this.moveToNextStage();
                break;
            default:
                throw new Error('notify decision response should not contain' + response.Action.Action);
        }
    }

    perceiveTurnResponse(response: TurnResponse){
        switch(response.Action.Action){
            case 'Decision':
                this.perceiveExpectedAction(response.Action);
                break;
            case 'River':
                this.moveToNextStage();
                break;
            default:
                throw new Error('notify decision response should not contain' + response.Action.Action);
        }
    }

    perceiveRiverResponse(response: RiverResponse){
        switch(response.Action.Action){
            case 'Decision':
                this.perceiveExpectedAction(response.Action);
                break;
            default:
                throw new Error('notify decision response should not contain' + response.Action.Action);
        }
    }

    private perceiveExpectedAction(action: ExpectedAction){        
        let temp = this.roundStatusSource.getValue();
        let nextPlayer = this.getPlayerByName(temp, action.PlayerName);
        temp.currentPlayer = nextPlayer;
        this.roundStatusSource.next(temp);

        let setup = this.roundSetupSource.getValue();
        let heroName = setup.playerNames[setup.heroIndex];
        console.log('Comparing ' + action.PlayerName + ' with ' + heroName + ' ' + setup.heroIndex);
        if (action.PlayerName == heroName){
            this.gameService.getDecision(temp.roundId).subscribe(data => {
                let response = data as GetDecisionResponse;
                this.perceiveGetDecisionResponse(response);
            })
        }
    }

    addHeroHoles(hole1: CardContract, hole2: CardContract) {
        let temp = this.roundStatusSource.getValue();
        temp.heroHole1 = new Card();        
        temp.heroHole1.setCard(hole1);
        temp.heroHole2 = new Card();
        temp.heroHole2.setCard(hole2);
        this.roundStatusSource.next(temp);
    }

    addFlops(flop1: CardContract, flop2: CardContract, flop3: CardContract){
        let temp = this.roundStatusSource.getValue();
        temp.flop1 = new Card();        
        temp.flop1.setCard(flop1);
        temp.flop2 = new Card();        
        temp.flop2.setCard(flop2);
        temp.flop3 = new Card();        
        temp.flop3.setCard(flop3);
        this.roundStatusSource.next(temp);
    }

    addTurn(turn:  CardContract){
        let temp = this.roundStatusSource.getValue();
        temp.turn = new Card();        
        temp.turn.setCard(turn);
        this.roundStatusSource.next(temp);        
    }

    addRiver(river:  CardContract){
        let temp = this.roundStatusSource.getValue();
        temp.river = new Card();        
        temp.river.setCard(river);
        this.roundStatusSource.next(temp);        
    }

    private moveToNextStage(){
        let temp = this.roundStatusSource.getValue();
        temp.stage++;
        temp.streetRaised = false;
        this.roundStatusSource.next(temp);
    }

    private moveToNextPlayer(nextPlayerName: string){
        let temp = this.roundStatusSource.getValue();
        temp.currentPlayer = this.getPlayerByName(temp, nextPlayerName);
        this.roundStatusSource.next(temp);
    }

    setChipsDigit(baseIndex: number, value: number){
        let temp = this.roundStatusSource.getValue();
        temp.digits[baseIndex] = value;
        this.roundStatusSource.next(temp);
    }

    private resetChipsDigit(){
        let temp = this.roundStatusSource.getValue();
        temp.digits.forEach((digit, index) => {
            temp.digits[index] = 0;
        });
    }

    private getPlayerByName(temp: RoundStatus, playerName: string){
        switch(playerName){
            case temp.player0.name:
                return temp.player0;
            case temp.player1.name:
                return temp.player1;
            case temp.player2.name:
                return temp.player2;
            case temp.player3.name:
                return temp.player3;
            case temp.player4.name:
                return temp.player4;
            case temp.player5.name:
                return temp.player5;
        }

    }
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
