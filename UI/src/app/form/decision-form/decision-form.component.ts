import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { GameService } from '../../game.service';
import { NotifyDecisionRequest } from '../../contract/notify-decision-request';
import { Decision, DecisionTypeEnum } from '../../contract/decision';
import { NotifyDecisionResponse } from '../../contract/notify-decision-response';
import { DataSharingService } from '../../data-sharing-service';
import { GetDecisionResponse } from '../../contract/get-decision-response';

@Component({
  selector: 'app-decision',
  templateUrl: './decision-form.component.html',
  styleUrls: ['./decision-form.component.css', '../../app.component.css']
})
export class DecisionFormComponent implements OnInit {
  
  form: FormGroup;
  roundId: string;
  isActive: boolean;
  isHero: boolean;
  heroDecision: Decision;
  isPolled: boolean;
  isAllIn: boolean;
  isBetted: boolean;

  @Input('playerName') playerName: string;
  chips: number;
  stackSize: number;
  heroName: string;

  constructor(fb: FormBuilder, private gameService: GameService,
    private dataSharingService: DataSharingService) {
      this.form = fb.group({
            chips: []
    });}

  ngOnInit() {
    this.dataSharingService.roundSetup.subscribe(setup => {
      this.heroName = setup.playerNames[setup.heroIndex];
    });

    this.dataSharingService.currentStatus.subscribe(
      status => {
        this.roundId = status.roundId;
        if (status.currentPlayer != undefined){
          this.isActive = status.currentPlayer.name == this.playerName;
        }

        if (this.isHero == undefined && this.playerName != undefined){
          this.isHero = this.heroName == this.playerName;
        }

        if (this.isHero != undefined && this.isHero && status.heroDecision != undefined){
          this.heroDecision = status.heroDecision;
        }

        if (this.playerName != undefined){
          switch(this.playerName){
            case status.player0.name:
              this.stackSize = status.player0.stackSize;
              break;
            case status.player1.name:
              this.stackSize = status.player1.stackSize;
              break;
            case status.player0.name:
              this.stackSize = status.player0.stackSize;
              break;
            case status.player0.name:
              this.stackSize = status.player0.stackSize;
              break;
            case status.player0.name:
              this.stackSize = status.player0.stackSize;
              break;
            case status.player0.name:
              this.stackSize = status.player0.stackSize;
              break;
          }
        }
      }
    );
  }

  check(){
    let request = new NotifyDecisionRequest();
    request.RoundId = this.roundId;
    request.PlayerName = this.playerName;
    request.Decision = new Decision();
    request.Decision.DecisionType = 'Check';
    request.Decision.ChipsAdded = 0;
    this.gameService.notifyDecision(request).subscribe(data => {
      let response = (data as NotifyDecisionResponse);
      this.dataSharingService.perceiveNotifyDecisionResponse(request, response);
    });
  }

  raise(){
    let request = new NotifyDecisionRequest();
    request.RoundId = this.roundId;
    request.PlayerName = this.playerName;
    request.Decision = new Decision();
    request.Decision.DecisionType = 'Raise';
    request.Decision.ChipsAdded = (this.form.get('chips').value as number);
    this.gameService.notifyDecision(request).subscribe(data => {
      let response = (data as NotifyDecisionResponse);
      this.dataSharingService.perceiveNotifyDecisionResponse(request, response);
    });
  }

  call(){
    let request = new NotifyDecisionRequest();
    request.RoundId = this.roundId;
    request.PlayerName = this.playerName;
    request.Decision = new Decision();
    request.Decision.DecisionType = 'Call';
    request.Decision.ChipsAdded = 0;
    this.gameService.notifyDecision(request).subscribe(data => {
      let response = (data as NotifyDecisionResponse);
      this.dataSharingService.perceiveNotifyDecisionResponse(request, response);
    });

  }

  allIn(){
    let request = new NotifyDecisionRequest();
    request.RoundId = this.roundId;
    request.PlayerName = this.playerName;
    request.Decision = new Decision();
    request.Decision.DecisionType = 'AllIn';
    request.Decision.ChipsAdded = this.stackSize;
    this.gameService.notifyDecision(request).subscribe(data => {
      let response = (data as NotifyDecisionResponse);
      this.dataSharingService.perceiveNotifyDecisionResponse(request, response);
    });

  }

  fold(){
    let request = new NotifyDecisionRequest();
    request.RoundId = this.roundId;
    request.PlayerName = this.playerName;
    request.Decision = new Decision();
    request.Decision.DecisionType = 'Fold';
    request.Decision.ChipsAdded = 0;
    this.gameService.notifyDecision(request).subscribe(data => {
      let response = (data as NotifyDecisionResponse);
      this.dataSharingService.perceiveNotifyDecisionResponse(request, response);
    });
  }

  getDecision(){
    this.gameService.getDecision(this.roundId).subscribe(
      data => {
        let response = (data as GetDecisionResponse);
        this.dataSharingService.perceiveGetDecisionResponse(response);
      }
    );
  }
}
