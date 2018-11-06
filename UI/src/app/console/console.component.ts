import { Component, OnInit } from '@angular/core';
import {CardComponent} from'../card/card.component';
import { Card } from '../models/card';
import {Card as CardContract } from '../contract/card';
import { forEach } from '@angular/router/src/utils/collection';
import { DataSharingService } from '../data-sharing-service';
import { stageEnum } from '../models/round-status';
import { GameService } from '../game.service';
import { HeroHolesRequest } from '../contract/hero-holes-request';
import { HeroHolesResponse } from '../contract/hero-holes-response';
import { PlayerStatus } from '../models/player-status';
import { FlopsRequest } from '../contract/flops-request';
import { FlopsResponse } from '../contract/flops-response';
import { TurnRequest } from '../contract/turn-request';
import { TurnResponse } from '../contract/turn-response';
import { RiverRequest } from '../contract/river-request';
import { RiverResponse } from '../contract/river-response';


@Component({
  selector: 'app-console',
  templateUrl: './console.component.html',
  styleUrls: ['./console.component.css', '../app.component.css']
})

export class ConsoleComponent implements OnInit{
  Cards : Card[][];

  selectedCards : Card[];

  isActive: boolean;
  isPreflop: boolean;
  isFlop: boolean;
  isTurn: boolean;
  isRiver: boolean;
  roundId: string;

  player0: PlayerStatus;
  player0Assigned: boolean = false;
  player1: PlayerStatus;
  player1Assigned: boolean = false;
  player2: PlayerStatus;
  player2Assigned: boolean = false;
  player3: PlayerStatus;
  player3Assigned: boolean = false;
  player4: PlayerStatus;
  player4Assigned: boolean = false;
  player5: PlayerStatus;
  player5Assigned: boolean = false;

  constructor(private gameService: GameService, private dataSharingService: DataSharingService){
    this.selectedCards = new Array<Card>();
    this.Cards = new Array<Array<Card>>();

    let heartCards = new Array<Card>();
    heartCards.push(this.generateUnfoldedCard('heart', 'ace', false, false));
    heartCards.push(this.generateUnfoldedCard('heart', 'king', false, false));
    heartCards.push(this.generateUnfoldedCard('heart', 'queen', false, false));
    heartCards.push(this.generateUnfoldedCard('heart', 'jack', false, false));
    heartCards.push(this.generateUnfoldedCard('heart', '10', false, false));
    heartCards.push(this.generateUnfoldedCard('heart', '9', false, false));
    heartCards.push(this.generateUnfoldedCard('heart', '8', false, false));
    heartCards.push(this.generateUnfoldedCard('heart', '7', false, false));
    heartCards.push(this.generateUnfoldedCard('heart', '6', false, false));
    heartCards.push(this.generateUnfoldedCard('heart', '5', false, false));
    heartCards.push(this.generateUnfoldedCard('heart', '4', false, false));
    heartCards.push(this.generateUnfoldedCard('heart', '3', false, false));
    heartCards.push(this.generateUnfoldedCard('heart', '2', false, false));
    this.Cards.push(heartCards);
    
    let spadeCards = new Array<Card>();
    spadeCards.push(this.generateUnfoldedCard('spade', 'ace', false, false));
    spadeCards.push(this.generateUnfoldedCard('spade', 'king', false, false));
    spadeCards.push(this.generateUnfoldedCard('spade', 'queen', false, false));
    spadeCards.push(this.generateUnfoldedCard('spade', 'jack', false, false));
    spadeCards.push(this.generateUnfoldedCard('spade', '10', false, false));
    spadeCards.push(this.generateUnfoldedCard('spade', '9', false, false));
    spadeCards.push(this.generateUnfoldedCard('spade', '8', false, false));
    spadeCards.push(this.generateUnfoldedCard('spade', '7', false, false));
    spadeCards.push(this.generateUnfoldedCard('spade', '6', false, false));
    spadeCards.push(this.generateUnfoldedCard('spade', '5', false, false));
    spadeCards.push(this.generateUnfoldedCard('spade', '4', false, false));
    spadeCards.push(this.generateUnfoldedCard('spade', '3', false, false));
    spadeCards.push(this.generateUnfoldedCard('spade', '2', false, false));
    this.Cards.push(spadeCards);
    
    let diamondCards = new Array<Card>();
    diamondCards.push(this.generateUnfoldedCard('diamond', 'ace', false, false));
    diamondCards.push(this.generateUnfoldedCard('diamond', 'king', false, false));
    diamondCards.push(this.generateUnfoldedCard('diamond', 'queen', false, false));
    diamondCards.push(this.generateUnfoldedCard('diamond', 'jack', false, false));
    diamondCards.push(this.generateUnfoldedCard('diamond', '10', false, false));
    diamondCards.push(this.generateUnfoldedCard('diamond', '9', false, false));
    diamondCards.push(this.generateUnfoldedCard('diamond', '8', false, false));
    diamondCards.push(this.generateUnfoldedCard('diamond', '7', false, false));
    diamondCards.push(this.generateUnfoldedCard('diamond', '6', false, false));
    diamondCards.push(this.generateUnfoldedCard('diamond', '5', false, false));
    diamondCards.push(this.generateUnfoldedCard('diamond', '4', false, false));
    diamondCards.push(this.generateUnfoldedCard('diamond', '3', false, false));
    diamondCards.push(this.generateUnfoldedCard('diamond', '2', false, false));
    this.Cards.push(diamondCards);
    
    let clubCards = new Array<Card>();
    clubCards.push(this.generateUnfoldedCard('club', 'ace', false, false));
    clubCards.push(this.generateUnfoldedCard('club', 'king', false, false));
    clubCards.push(this.generateUnfoldedCard('club', 'queen', false, false));
    clubCards.push(this.generateUnfoldedCard('club', 'jack', false, false));
    clubCards.push(this.generateUnfoldedCard('club', '10', false, false));
    clubCards.push(this.generateUnfoldedCard('club', '9', false, false));
    clubCards.push(this.generateUnfoldedCard('club', '8', false, false));
    clubCards.push(this.generateUnfoldedCard('club', '7', false, false));
    clubCards.push(this.generateUnfoldedCard('club', '6', false, false));
    clubCards.push(this.generateUnfoldedCard('club', '5', false, false));
    clubCards.push(this.generateUnfoldedCard('club', '4', false, false));
    clubCards.push(this.generateUnfoldedCard('club', '3', false, false));
    clubCards.push(this.generateUnfoldedCard('club', '2', false, false));
    this.Cards.push(clubCards);

    /*
    let aceCards = new Array<Card>();
    aceCards.push(this.generateUnfoldedCard('heart', 'ace', false, false));
    aceCards.push(this.generateUnfoldedCard('spade', 'ace', false, false));
    aceCards.push(this.generateUnfoldedCard('diamond', 'ace', false, false));
    aceCards.push(this.generateUnfoldedCard('club', 'ace', false, false));
    this.Cards.push(aceCards);

    let kingCards = new Array<Card>();
    kingCards.push(this.generateUnfoldedCard('heart', 'king', false, false));
    kingCards.push(this.generateUnfoldedCard('spade', 'king', false, false));
    kingCards.push(this.generateUnfoldedCard('diamond', 'king', false, false));
    kingCards.push(this.generateUnfoldedCard('club', 'king', false, false));
    this.Cards.push(kingCards);

    let queueCards = new Array<Card>();
    queueCards.push(this.generateUnfoldedCard('heart', 'queen', false, false));
    queueCards.push(this.generateUnfoldedCard('spade', 'queen', false, false));
    queueCards.push(this.generateUnfoldedCard('diamond', 'queen', false, false));
    queueCards.push(this.generateUnfoldedCard('club', 'queen', false, false));
    this.Cards.push(queueCards);

    let jackCards = new Array<Card>();
    jackCards.push(this.generateUnfoldedCard('heart', 'jack', false, false));
    jackCards.push(this.generateUnfoldedCard('spade', 'jack', false, false));
    jackCards.push(this.generateUnfoldedCard('diamond', 'jack', false, false));
    jackCards.push(this.generateUnfoldedCard('club', 'jack', false, false));
    this.Cards.push(jackCards);

    let tenCards = new Array<Card>();
    tenCards.push(this.generateUnfoldedCard('heart', '10', false, false));
    tenCards.push(this.generateUnfoldedCard('spade', '10', false, false));
    tenCards.push(this.generateUnfoldedCard('diamond', '10', false, false));
    tenCards.push(this.generateUnfoldedCard('club', '10', false, false));
    this.Cards.push(tenCards);

    let nineCards = new Array<Card>();
    nineCards.push(this.generateUnfoldedCard('heart', '9', false, false));
    nineCards.push(this.generateUnfoldedCard('spade', '9', false, false));
    nineCards.push(this.generateUnfoldedCard('diamond', '9', false, false));
    nineCards.push(this.generateUnfoldedCard('club', '9', false, false));
    this.Cards.push(nineCards);

    let eightCards = new Array<Card>();
    eightCards.push(this.generateUnfoldedCard('heart', '8', false, false));
    eightCards.push(this.generateUnfoldedCard('spade', '8', false, false));
    eightCards.push(this.generateUnfoldedCard('diamond', '8', false, false));
    eightCards.push(this.generateUnfoldedCard('club', '8', false, false));
    this.Cards.push(eightCards);

    let sevenCards = new Array<Card>();
    sevenCards.push(this.generateUnfoldedCard('heart', '7', false, false));
    sevenCards.push(this.generateUnfoldedCard('spade', '7', false, false));
    sevenCards.push(this.generateUnfoldedCard('diamond', '7', false, false));
    sevenCards.push(this.generateUnfoldedCard('club', '7', false, false));
    this.Cards.push(sevenCards);

    let sixCards = new Array<Card>();
    sixCards.push(this.generateUnfoldedCard('heart', '6', false, false));
    sixCards.push(this.generateUnfoldedCard('spade', '6', false, false));
    sixCards.push(this.generateUnfoldedCard('diamond', '6', false, false));
    sixCards.push(this.generateUnfoldedCard('club', '6', false, false));
    this.Cards.push(sixCards);

    let fiveCards = new Array<Card>();
    fiveCards.push(this.generateUnfoldedCard('heart', '5', false, false));
    fiveCards.push(this.generateUnfoldedCard('spade', '5', false, false));
    fiveCards.push(this.generateUnfoldedCard('diamond', '5', false, false));
    fiveCards.push(this.generateUnfoldedCard('club', '5', false, false));
    this.Cards.push(fiveCards);

    let fourCards = new Array<Card>();
    fourCards.push(this.generateUnfoldedCard('heart', '4', false, false));
    fourCards.push(this.generateUnfoldedCard('spade', '4', false, false));
    fourCards.push(this.generateUnfoldedCard('diamond', '4', false, false));
    fourCards.push(this.generateUnfoldedCard('club', '4', false, false));
    this.Cards.push(fourCards);

    let threeCards = new Array<Card>();
    threeCards.push(this.generateUnfoldedCard('heart', '3', false, false));
    threeCards.push(this.generateUnfoldedCard('spade', '3', false, false));
    threeCards.push(this.generateUnfoldedCard('diamond', '3', false, false));
    threeCards.push(this.generateUnfoldedCard('club', '3', false, false));
    this.Cards.push(threeCards);

    let twoCards = new Array<Card>();
    twoCards.push(this.generateUnfoldedCard('heart', '2', false, false));
    twoCards.push(this.generateUnfoldedCard('spade', '2', false, false));
    twoCards.push(this.generateUnfoldedCard('diamond', '2', false, false));
    twoCards.push(this.generateUnfoldedCard('club', '2', false, false));
    this.Cards.push(twoCards);
    */
  }

  ngOnInit(): void {
    this.dataSharingService.currentStatus.subscribe(status => {
      this.isActive = (status.stage == stageEnum.Preflop || status.stage == stageEnum.Flop || status.stage == stageEnum.Turn || status.stage == stageEnum.River);
      this.isPreflop = (status.stage == stageEnum.Preflop);
      this.isFlop = (status.stage == stageEnum.Flop);
      this.isTurn = (status.stage == stageEnum.Turn);
      this.isRiver = (status.stage == stageEnum.River);
      this.roundId = status.roundId;

      if (status.player0 != undefined && this.player0Assigned == false) {
        this.player0 = status.player0;
        this.player0Assigned = true;
        console.log('player0 assigned');
      }
      if (status.player1 != undefined && this.player1Assigned == false) {
        this.player1 = status.player1;
        this.player1Assigned = true;
        console.log('player1 assigned');
      }
      if (status.player2 != undefined && this.player2Assigned == false) {
        this.player2 = status.player2;
        this.player2Assigned = true;
        console.log('player2 assigned');
      }
      if (status.player3 != undefined && this.player3Assigned == false) {
        this.player3 = status.player3;
        this.player3Assigned = true;
        console.log('player3 assigned');
      }
      if (status.player4 != undefined && this.player4Assigned == false) {
        this.player4 = status.player4;
        this.player4Assigned = true;
        console.log('player4 assigned');
      }
      if (status.player5 != undefined && this.player5Assigned == false) {
        this.player5 = status.player5;
        this.player5Assigned = true;
        console.log('player5 assigned');
      }
    });
  }

  generateUnfoldedCard(suit: string, rank: string, used: boolean, selected: boolean) : Card {
    var card = new Card();
    card.Folded = false;
    card.Rank = rank;
    card.Suit = suit;
    card.Used = used;
    card.Selected = selected;

    return card;
  }

  onCardClicked(card: Card){
    if (card.Selected)
      this.selectedCards.push(card);
    else
    {
      let index = this.selectedCards.findIndex(c => c.Rank == card.Rank && c.Suit == card.Suit);
      this.selectedCards.splice(index, 1);
    }
  }

  heroHolesReady() : boolean{
    return this.selectedCards.length == 2;
  }

  flopsReady() : boolean{
    return this.selectedCards.length == 3;
  }
  
  turnReady() : boolean{
    return this.selectedCards.length == 1;
  }

  riverReady() : boolean{
    return this.selectedCards.length == 1;
  }

  notifyHeroHoles(){
    console.log("notifying hero holes");
    let request = new HeroHolesRequest();
    request.roundId = this.roundId;
    request.Holes = new Array<CardContract>();
    
    let hole1 = new CardContract();
    hole1.Suit = this.selectedCards[0].Suit;
    hole1.Rank = this.selectedCards[0].Rank;

    let hole2 = new CardContract();
    hole2.Suit = this.selectedCards[1].Suit;
    hole2.Rank = this.selectedCards[1].Rank;

    request.Holes.push(hole1);
    request.Holes.push(hole2);

    let response = this.gameService.notifyHeroHoles(request)
      .subscribe(data=>{
        console.log(JSON.stringify(data));
        let response = (data as HeroHolesResponse);
        this.dataSharingService.addHeroHoles(request.Holes[0], request.Holes[1]);
        this.dataSharingService.perceiveHeroHolesResponse(response);
      });

    this.selectedCards[0].Used = true;
    this.selectedCards[0].Selected = false;
    this.selectedCards[1].Used = true;
    this.selectedCards[1].Selected = false;

    for(let c of this.selectedCards){
      console.log("notifying " + c.Suit + c.Rank);
    }

    this.selectedCards.splice(0,2);
  }

  notifyFlops(){
    let request = new FlopsRequest();
    request.RoundId = this.roundId;
    request.Flops = new Array<CardContract>();
    
    let flop1 = new CardContract();
    flop1.Suit = this.selectedCards[0].Suit;
    flop1.Rank = this.selectedCards[0].Rank;

    let flop2 = new CardContract();
    flop2.Suit = this.selectedCards[1].Suit;
    flop2.Rank = this.selectedCards[1].Rank;

    let flop3 = new CardContract();
    flop3.Suit = this.selectedCards[2].Suit;
    flop3.Rank = this.selectedCards[2].Rank;

    request.Flops.push(flop1);
    request.Flops.push(flop2);
    request.Flops.push(flop3);

    let response = this.gameService.notifyFlops(request)
      .subscribe(data=>{
        console.log(JSON.stringify(data));
        let response = (data as FlopsResponse);
        this.dataSharingService.addFlops(request.Flops[0], request.Flops[1], request.Flops[2]);
        this.dataSharingService.perceiveFlopsResponse(response);
      });

    this.selectedCards[0].Used = true;
    this.selectedCards[0].Selected = false;
    this.selectedCards[1].Used = true;
    this.selectedCards[1].Selected = false;
    this.selectedCards[2].Used = true;
    this.selectedCards[2].Selected = false;

    this.selectedCards.splice(0, 3);
  }

  notifyTurn(){
    let request = new TurnRequest();
    request.RoundId = this.roundId;
    request.Turn = new CardContract();
    request.Turn.Suit = this.selectedCards[0].Suit;
    request.Turn.Rank = this.selectedCards[0].Rank;

    let response = this.gameService.notifyTurn(request)
      .subscribe(data=>{
        console.log(JSON.stringify(data));
        let response = (data as TurnResponse);
        this.dataSharingService.addTurn(request.Turn);
        this.dataSharingService.perceiveTurnResponse(response);
      });

    this.selectedCards[0].Used = true;
    this.selectedCards[0].Selected = false;
    this.selectedCards.splice(0, 1);
  }

  notifyRiver(){
    let request = new RiverRequest();
    request.RoundId = this.roundId;
    request.River = new CardContract();
    request.River.Suit = this.selectedCards[0].Suit;
    request.River.Rank = this.selectedCards[0].Rank;

    let response = this.gameService.notifyRiver(request)
      .subscribe(data=>{
        console.log(JSON.stringify(data));
        let response = (data as RiverResponse);
        this.dataSharingService.addRiver(request.River);
        this.dataSharingService.perceiveRiverResponse(response);
      });

    this.selectedCards[0].Used = true;
    this.selectedCards[0].Selected = false;
    this.selectedCards.splice(0, 1);
  }
}
