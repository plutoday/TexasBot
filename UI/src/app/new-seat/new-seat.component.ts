import { Component, OnInit, Input } from '@angular/core';
import { Card } from '../models/card';
import { SeatModel } from '../models/seat-model';
import { DataSharingService } from '../data-sharing-service';
import { GameService } from '../game.service';
import { NotifyDecisionRequest } from '../contract/notify-decision-request';
import { Decision, DecisionTypeEnum } from '../contract/decision';
import { NotifyDecisionResponse } from '../contract/notify-decision-response';

@Component({
  selector: 'app-new-seat',
  templateUrl: './new-seat.component.html',
  styleUrls: ['./new-seat.component.css']
})
export class NewSeatComponent implements OnInit {

  @Input('seat') seat: SeatModel;

  @Input('beforeRound') beforeRound: boolean;

  @Input('raised') raised: boolean; 

  //before new round submitted
  heroSelected: boolean;
  buttonSelected: boolean;

  isActionAllowed(): boolean{
    return this.seat.isHisTurn 
      && this.seat.playserStatus.absent == false
      && this.seat.playserStatus.folded == false
      && this.seat.playserStatus.allIn == false;
  }

  //action buttons
  //check
  isCheckAllowed(): boolean{
    return this.isActionAllowed()
      && this.raised == false;
  }

  isCheckButtonDown: boolean;

  toggleCheckButtonStatus(){
    if (this.isCheckAllowed()) {
      this.isCheckButtonDown = !this.isCheckButtonDown;
      if (this.isCheckButtonDown == false){
        let request = new NotifyDecisionRequest();
        request.RoundId = this.roundId;
        request.PlayerName = this.seat.playserStatus.name;
        request.Decision = new Decision();
        request.Decision.DecisionType = 'Check';
        this.gameService.notifyDecision(request).subscribe(data => {
          let response = data as NotifyDecisionResponse;
          this.dataSharingService.perceiveNotifyDecisionResponse(request, response);
        });
      }
    }
  }

  //call
  isCallAllowed(): boolean{
    return this.isActionAllowed()
      && this.raised == true;
  }

  isCallButtonDown: boolean;

  toggleCallButtonStatus(){
    if (this.isCallAllowed()) {
      this.isCallButtonDown = !this.isCallButtonDown;
      if (this.isCallButtonDown == false){
        let request = new NotifyDecisionRequest();
        request.RoundId = this.roundId;
        request.PlayerName = this.seat.playserStatus.name;
        request.Decision = new Decision();
        request.Decision.DecisionType = 'Call';
        //let the server take care of the #chips
        this.gameService.notifyDecision(request).subscribe(data => {
          let response = data as NotifyDecisionResponse;
          this.dataSharingService.perceiveNotifyDecisionResponse(request, response);
        });
      }
    }
  }

  //raise
  isRaiseAllowed(): boolean{
    return this.isActionAllowed();
  }

  isRaiseButtonDown: boolean;

  //subscribe from dataSharingService.consoleStatus
  customChips: number;

  toggleRaiseButtonStatus(){
    if (this.isRaiseAllowed()){
      this.isRaiseButtonDown = !this.isRaiseButtonDown;
      if (this.isRaiseButtonDown == false){
        let request = new NotifyDecisionRequest();
        request.RoundId = this.roundId;
        request.PlayerName = this.seat.playserStatus.name;
        request.Decision = new Decision();
        request.Decision.DecisionType = 'Raise';
        request.Decision.ChipsAdded = this.customChips;
        this.gameService.notifyDecision(request).subscribe(data => {
          let response = data as NotifyDecisionResponse;
          this.dataSharingService.perceiveNotifyDecisionResponse(request, response);          
        });
      }
    }
  }

  //all in
  isAllInAllowed(): boolean{
    return this.isActionAllowed();
  }

  isAllInButtonDown: boolean;

  toggleAllInButtonStatus(){
    if (this.isAllInAllowed()){
      this.isAllInButtonDown = !this.isAllInButtonDown;
      if (this.isAllInButtonDown == false){
        let request = new NotifyDecisionRequest();
        request.RoundId = this.roundId;
        request.PlayerName = this.seat.playserStatus.name;
        request.Decision = new Decision();
        request.Decision.DecisionType = 'AllIn';
        request.Decision.ChipsAdded = this.customChips;
        this.gameService.notifyDecision(request).subscribe(data => {
          let response = data as NotifyDecisionResponse;
          this.dataSharingService.perceiveNotifyDecisionResponse(request, response);
        });
        this.seat.playserStatus.allIn = true;
      }
    }
  }

  //fold
  isFoldAllowed(): boolean{
    return this.isActionAllowed();
  }

  isFoldButtonDown: boolean;

  toggleFoldButtonStatus(){
    if (this.isFoldAllowed()){
      this.isFoldButtonDown = !this.isFoldButtonDown;
      if (this.isFoldButtonDown == false){
        let request = new NotifyDecisionRequest();
        request.RoundId = this.roundId;
        request.PlayerName = this.seat.playserStatus.name;
        request.Decision = new Decision();
        request.Decision.DecisionType = 'Fold';
        this.gameService.notifyDecision(request).subscribe(data => {
          let response = data as NotifyDecisionResponse;
          this.dataSharingService.perceiveNotifyDecisionResponse(request, response);
        });
        this.seat.playserStatus.folded = true;
      }
    }
  }

  constructor(private dataSharingService: DataSharingService, private gameService: GameService) { 

  }

  roundId: string;
  heroDecisionType: string;
  heroDecisionChips: number;

  ngOnInit() {
    this.dataSharingService.currentStatus.subscribe(status => {
      if (status.roundId != undefined){
        this.roundId = status.roundId;
      }
      if (status.heroDecision != undefined){
        this.heroDecisionType = status.heroDecision.DecisionType;
        this.heroDecisionChips = status.heroDecision.ChipsAdded;
      }
      this.customChips = this.getCustomChips(status.digits);            
    });
  }

  private getCustomChips(digits: Array<number>): number{
    let result: number = 0;
    digits.forEach(digit => {
      result *= 10;
      result += digit;
    });

    return result;
  }

  onClickHero(){
    this.seat.isHero = !this.seat.isHero;
  }

  onClickButton(){
    this.seat.isButton = !this.seat.isButton;
  }

  onClickSittingOut(){
    this.seat.isSittingOut = !this.seat.isSittingOut;
  }

  isHeroSelected(): boolean{
    return this.seat.isHero;
  }

  isButtonSelected(): boolean{
    return this.seat.isButton;
  }

  isSittingOutSelected(): boolean{
    return this.seat.isSittingOut;
  }

  getHole1(): Card{
    return this.seat.hole1;
  }

  getHole2(): Card{
    return this.seat.hole2;
  }

  getPosition(): string{
    return this.seat.playserStatus.position;
  }

  hasFolded(): boolean{
    if (this.beforeRound){
      return false;
    }
    else {
      return this.seat.playserStatus.folded;
    }
  }

  hasAllIned(): boolean{
    if (this.beforeRound){
      return false;
    }
    else {
      return this.seat.playserStatus.allIn;
    }
  }

  isHisTurn(): boolean{
    if (this.beforeRound){
      return false;
    }
    else {
      return this.seat.isHisTurn;
    }
  }

  isHero(): boolean{
    if (this.beforeRound){
      return false;
    }
    else{
      return this.seat.isHero;
    }
  }

  heroDecisionIsFold(): boolean{
    return this.heroDecisionType == 'Fold';
  }

  heroDecisionIsCheck(): boolean{
    return this.heroDecisionType == 'Check';
  }

  heroDecisionIsCall(): boolean{
    return this.heroDecisionType == 'Call';
  }

  heroDecisionIsRaise(): boolean{
    console.log('herodecisiontype == ' + this.heroDecisionType);
    return this.heroDecisionType == 'Raise' || this.heroDecisionType == 'Reraise';
  }

  heroDecisionIsAllIn(): boolean{
    return this.heroDecisionType == 'AllIn' || this.heroDecisionType == 'AllInRaise';
  }
}
