import { Component, OnInit } from '@angular/core';
import {FormGroup, FormControl, FormBuilder, ReactiveFormsModule, FormsModule, FormArray} from '@angular/forms';
import { NewRoundRequest } from '../../contract/new-round-request';
import { Player } from '../../contract/player';
import { GameService } from '../../game.service';
import { NewRoundResponse } from '../../contract/new-round-response';
import { DataSharingService } from '../../data-sharing-service';
import {stageEnum} from '../../models/round-status';

@Component({
  selector: 'app-new-game-form',
  templateUrl: './new-game-form.component.html',
  styleUrls: ['./new-game-form.component.css', '../../app.component.css']
})
export class NewGameFormComponent implements OnInit {
  isActive: boolean = true;
  
  form: FormGroup;

  player0Name: string = 'Player0';
  player1Name: string = 'Player1';
  player2Name: string = 'Player2';
  player3Name: string = 'Player3';
  player4Name: string = 'Player4';
  player5Name: string = 'Player5';

  player0StackSize: number = 10000;
  player1StackSize: number = 10000;
  player2StackSize: number = 10000;
  player3StackSize: number = 10000;
  player4StackSize: number = 10000;
  player5StackSize: number = 10000;

  smallBlindSizes = [
    {
        value: 5,
        description: '$5'
    },
    {
      value: 250,
        description: '$250'
    },
    {
      value: 1000,
        description: '$1000'
    },
    {
      value: 5000,
        description: '$5000'
    },
    {
      value: 25000,
        description: '$25000'
    },
    {
      value: 100000,
        description: '$100000'
    }
  ];

  seatIndexes = [
    {value: 0, description: '0'},
    {value: 1, description: '1'},
    {value: 2, description: '2'},
    {value: 3, description: '3'},
    {value: 4, description: '4'},
    {value: 5, description: '5'},
  ];

  smallBlindSize: number = this.smallBlindSizes[0].value;
  heroIndex: number = this.seatIndexes[5].value;
  buttonIndex: number = this.seatIndexes[0].value;

  constructor(fb: FormBuilder, private service: GameService, private dataSharingService: DataSharingService){
    this.form = fb.group({
      smallBlindSize: [],
      heroIndex: [],
      buttonIndex: [],
      players: fb.group({
        player0:[],
        player1:[],
        player2:[],
        player3:[],
        player4:[],
        player5:[],
      })
    });
  }

  ngOnInit(): void {
    this.dataSharingService.currentStatus.subscribe(status => {this.isActive = (status.stage == stageEnum.NotStarted);});
  }

  onSubmit(){
    this.useForm();

    let request = new NewRoundRequest();
    
    request.Players = new Array<Player>();
    request.Players.push(new Player(this.player0Name, +this.player0StackSize));
    request.Players.push(new Player(this.player1Name, +this.player1StackSize));
    request.Players.push(new Player(this.player2Name, +this.player2StackSize));
    request.Players.push(new Player(this.player3Name, +this.player3StackSize));
    request.Players.push(new Player(this.player4Name, +this.player4StackSize));
    request.Players.push(new Player(this.player5Name, +this.player5StackSize));

    request.HeroIndex = +this.heroIndex;
    request.ButtonIndex = +this.buttonIndex;
    request.SmallBlindSize = +this.smallBlindSize;
    request.BigBlindSize = +this.smallBlindSize * 2;

    this.service.startNewRound(request)
      .subscribe(data => {
        console.log(JSON.stringify(data));
        let response = (data as NewRoundResponse);
        this.dataSharingService.setup(request);
        this.dataSharingService.perceiveNewRoundResponse(response);
      });
  }

  useForm(){
    if (this.form.get("players.player0").touched)
    {
      this.player0Name = this.getPlayerNameFromInput("players.player0");
      this.player0StackSize = parseInt(this.getPlayerStackSizeFromInput("players.player0"), 10);
    }
    if (this.form.get("players.player1").touched)
    {
      this.player1Name = this.getPlayerNameFromInput("players.player1");
      this.player1StackSize = parseInt(this.getPlayerStackSizeFromInput("players.player1"), 10);
    }
    if (this.form.get("players.player2").touched)
    {  
      this.player2Name = this.getPlayerNameFromInput("players.player2");
      this.player2StackSize = parseInt(this.getPlayerStackSizeFromInput("players.player2"), 10);
    }
    if (this.form.get("players.player3").touched)
    {
      this.player3Name = this.getPlayerNameFromInput("players.player3");
      this.player3StackSize = parseInt(this.getPlayerStackSizeFromInput("players.player3"), 10);
    }
    if (this.form.get("players.player4").touched)
    {
      this.player4Name = this.getPlayerNameFromInput("players.player4");
      this.player4StackSize = parseInt(this.getPlayerStackSizeFromInput("players.player4"), 10);
    }
    if (this.form.get("players.player5").touched)
    { 
      this.player5Name = this.getPlayerNameFromInput("players.player5");
      this.player5StackSize = parseInt(this.getPlayerStackSizeFromInput("players.player5"), 10);
    }

  }

  getPlayerNameFromInput(inputName: string) : string{
    let player = this.form.get(inputName).value as string;
    let index = player.indexOf(' ');
    return player.substr(0, index);
  }

  getPlayerStackSizeFromInput(inputName: string) : string{
    let player = this.form.get(inputName).value as string;
    let index = player.indexOf(' ');
    return player.substr(index + 1);
  }

  onSmallBlindSizeChange(entry){
    this.smallBlindSize = entry.value;
  }

  onHeroIndexChange(entry){
    this.heroIndex = entry.value;
  }

  onButtonIndexChange(entry){
    this.buttonIndex = entry.value;
  }
}
