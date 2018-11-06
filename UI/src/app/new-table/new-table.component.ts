import { Component, OnInit } from '@angular/core';
import { SeatModel } from '../models/seat-model';
import { forEach } from '@angular/router/src/utils/collection';
import { DataSharingService } from '../data-sharing-service';
import { GameService } from '../game.service';
import { NewRoundRequest } from '../contract/new-round-request';
import { Player } from '../contract/player';
import { NewRoundResponse } from '../contract/new-round-response';

@Component({
  selector: 'app-new-table',
  templateUrl: './new-table.component.html',
  styleUrls: ['./new-table.component.css']
})

export class NewTableComponent implements OnInit {
  seats: Array<SeatModel>;

  newRoundButtonClickedDown: boolean;

  bigBlindSize: number = 10;
  heroIndex: number;
  buttonIndex: number;
  playerNames: Array<string>;
  stackSizes: Array<number>;

  beforeRound: boolean;

  streetRaised: boolean;

  constructor(private service: GameService, private dataSharingService: DataSharingService) { }

  ngOnInit() {
    this.seats = new Array<SeatModel>();
    this.seats.push(new SeatModel());
    this.seats.push(new SeatModel());
    this.seats.push(new SeatModel());
    this.seats.push(new SeatModel());
    this.seats.push(new SeatModel());
    this.seats.push(new SeatModel());

    this.dataSharingService.roundSetup.subscribe(setup =>{
      this.bigBlindSize = setup.bigBlindSize;
      this.heroIndex = setup.heroIndex;
      this.playerNames = setup.playerNames;
      this.stackSizes = setup.stackSizes;
    });

    this.dataSharingService.currentStatus.subscribe(status => {
      this.buttonIndex = status.buttonIndex;
      this.beforeRound = !status.started;
      this.seats[0].playserStatus = status.player0;
      this.seats[1].playserStatus = status.player1;
      this.seats[2].playserStatus = status.player2;
      this.seats[3].playserStatus = status.player3;
      this.seats[4].playserStatus = status.player4;
      this.seats[5].playserStatus = status.player5;

      this.streetRaised = status.streetRaised;

      if (status.currentPlayer != null){
        for(let seat of this.seats){
          seat.isHisTurn = seat.playserStatus.name == status.currentPlayer.name;        
        }
      }
    });
  }

  toggleNewRoundButtonStatus(){
    if (this.isReadyToStartNewRound() == false){
      return;
    }
    this.newRoundButtonClickedDown = !this.newRoundButtonClickedDown;
    console.log('new round status changed' + this.newRoundButtonClickedDown);
    if (this.newRoundButtonClickedDown == false){
      //mouse up
      //send request
      this.sendStartNewRoundRequest();
    }
  }

  sendStartNewRoundRequest(){
    let request: NewRoundRequest = new NewRoundRequest();
    request.HeroIndex = this.getHeroIndex();
    request.ButtonIndex = this.getButtonIndex();
    request.BigBlindSize = this.bigBlindSize;
    request.SmallBlindSize = this.bigBlindSize/2;
    request.Players = new Array<Player>();
    
    this.playerNames.forEach((name, index) => {
      request.Players.push(new Player(this.playerNames[index], this.stackSizes[index], this.seats[index].isSittingOut));
    });

    console.log('before calling service, the heroIndex in request is ' + request.HeroIndex);
    this.service.startNewRound(request)
    .subscribe(data => {
      console.log('new round response is back ' + JSON.stringify(data));
      let response = (data as NewRoundResponse);
      this.dataSharingService.setup(request);
      this.dataSharingService.perceiveNewRoundResponse(response);
    });;
  }

  isNewRoundButtonClickedDown(): boolean{    
    return this.newRoundButtonClickedDown;
  }

  getHeroIndex(): number{
    let heroIndex = -1;
    this.seats.forEach((seat, index) => {
      if (seat.isHero){ 
        heroIndex = index;
      }
    });
    return heroIndex;
  }

  getButtonIndex(): number{
    let buttonIndex = -1;
    this.seats.forEach((seat, index) => {
      if (seat.isButton){
        buttonIndex = index;
      }
    });
  return buttonIndex;
  }

  isReadyToStartNewRound(): boolean{
    let numberOfHeros: number = 0;
    let numberOfButtons: number = 0;

    for(let seat of this.seats){
      if (seat.isHero){
        numberOfHeros++;
      }

      if (seat.isButton){
        numberOfButtons++;
      }
    }

    return numberOfHeros == 1 && numberOfButtons == 1;
  }
}
